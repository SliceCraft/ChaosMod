using ChaosMod.Patches;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static ChaosMod.Activator.AllEffects;

namespace ChaosMod.Activator
{
    internal class TimerSystem
    {
        private static Activator activator = null;
        private static List<Effect> activeEffects = new List<Effect>();
        private static List<Effect> effectHistory = new List<Effect>();
        private static long lastTimerRun = 0;
        private static bool enabled = false;
        private static PositionTracker positionTracker = null;
        private static TextMeshProUGUI effectText = null;
        private static Camera secondDisplayCamera = null;
        private static bool halfEffectWaitTime = false;

        public static void Update()
        {
            if (!enabled || activator == null || positionTracker == null) return;
            positionTracker.Update();
            List<Effect> removeEffects = new List<Effect>();
            foreach(Effect effect in activeEffects)
            {
                try
                {
                    effect.UpdateEffect();
                }
                catch (Exception ex)
                {
                    ChaosMod.getInstance().logsource.LogError(ex);
                }
                if(effect.GetEffectLength() + effect.GetEffectStartTime() < DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
                {
                    try
                    {
                        effect.StopEffect();
                    }
                    catch (Exception ex)
                    {
                        ChaosMod.getInstance().logsource.LogError(ex);
                    }
                    removeEffects.Add(effect);
                }
            }
            foreach(Effect effect in removeEffects)
            {
                activeEffects.Remove(effect);
            }
            if (lastTimerRun + (halfEffectWaitTime ? ChaosMod.ConfigTimeBetweenEffects.Value / 2 * 1000 : ChaosMod.ConfigTimeBetweenEffects.Value * 1000) < DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
            {
                ChaosMod.getInstance().logsource.LogInfo("Choosing effect");
                Effect chosenEffect = activator.ChooseEffect();
                bool alreadyActive = false;
                foreach(Effect effect in activeEffects)
                {
                    if(effect.GetEffectName() ==  chosenEffect.GetEffectName())
                    {
                        alreadyActive = true;
                        effect.ResetEffectStartTime();
                        break;
                    }
                }
                if (!alreadyActive)
                {
                    try
                    {
                        chosenEffect.ResetEffectStartTime();
                        chosenEffect.StartEffect();
                    }
                    catch (Exception ex)
                    {
                        ChaosMod.getInstance().logsource.LogError(ex);
                    }
                    effectHistory.Add(chosenEffect);
                    if (chosenEffect.IsTimedEffect()) activeEffects.Add(chosenEffect);
                }
                lastTimerRun = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }
            UpdateEffectList();
        }

        public static void Enable()
        {
            if (enabled)
            {
                Disable();
            }

            if(effectText == null) AddTextElement();

            ChaosMod.getInstance().logsource.LogInfo("Enabling the mod");
            if (!AllActivators.GetActivators().ContainsKey(ChaosMod.ConfigActivator.Value))
            {
                HUDManager.Instance.DisplayTip("Chaos Mod", "An invalid activator was specified in the config. Chaos Mod has been disabled!", true);
                return;
            }
            activator = AllActivators.GetActivators()[ChaosMod.ConfigActivator.Value];
            lastTimerRun = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            positionTracker = new PositionTracker();
            enabled = true;
            activator.Start();
        }

        public static void Disable()
        {
            if (!enabled) return;
            ChaosMod.getInstance().logsource.LogInfo("Disabling the mod");
            if (ChaosMod.getInstance().twitchIRCClient != null)
            {
                ChaosMod.getInstance().twitchIRCClient.Disconnect();
                ChaosMod.getInstance().twitchIRCClient = null;
            }
            foreach (Effect effect in activeEffects)
            {
                try
                {
                    effect.StopEffect();
                }
                catch (Exception ex)
                {
                    ChaosMod.getInstance().logsource.LogError(ex);
                }
            }
            effectText.text = "";
            activator.Stop();
            activator = null;
            positionTracker = null;
            lastTimerRun = 0;
            activeEffects = new List<Effect>();
            effectHistory = new List<Effect>();
            PlayerControllerBPatch.setOneHitExplode(false);
            PlayerControllerBPatch.setInfiniteSprint(false);
            halfEffectWaitTime = false;
            enabled = false;
        }

        private static void AddTextElement()
        {
            if(ChaosMod.ConfigTwitchOptionsShowcase.Value == "newwindow")
            {
                HUDManager.Instance.DisplayTip("Chaos Mod", "You are currently using the newwindow option. This feature is unstable and not properly tested, use with caution.", true);
                GameObject cameraObject = new GameObject();
                cameraObject.name = "ChaosModSecondDisplayCamera";
                secondDisplayCamera = cameraObject.AddComponent<Camera>();
                secondDisplayCamera.targetDisplay = 1;
                
                GameObject go = new GameObject();
                go.name = "CanvasDD";
                Canvas canvas = go.AddComponent<Canvas>();

                GameObject panelgo = new GameObject();
                panelgo.name = "CanvasDDPanel";
                panelgo.transform.parent = go.transform;

                GameObject textElement2 = new GameObject();
                textElement2.transform.parent = go.transform;
                textElement2.name = "CanvasDDText";
                Vector3 scale2 = textElement2.transform.localScale;
                scale2.x = 1F;
                scale2.y = 1F;
                scale2.z = 1F;
                textElement2.transform.localScale = scale2;
                TextMeshProUGUI effectText = textElement2.AddComponent<TextMeshProUGUI>();
                effectText.text = "AAAAAAAAAAAA";
                effectText.fontSize = 15;
                effectText.lineSpacing = 30;

                canvas.worldCamera = secondDisplayCamera;
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.targetDisplay = 1;
            }

            GameObject textElement = new GameObject();
            textElement.transform.parent = HUDManager.Instance.HUDContainer.transform;
            textElement.name = "ChaosModEffectHistory";
            Vector3 scale = textElement.transform.localScale;
            scale.x = 1F;
            scale.y = 1F;
            scale.z = 1F;
            textElement.transform.localScale = scale;
            Vector3 position = textElement.transform.localPosition;
            position.x = -330F;
            position.y = 40F;
            position.z = 0F;
            textElement.transform.localPosition = position;
            effectText = textElement.AddComponent<TextMeshProUGUI>();
            effectText.text = "";
            effectText.fontSize = 15;
            effectText.lineSpacing = 30;
        }

        private static void UpdateEffectList()
        {
            if (effectHistory.Count == 0) return;
            string text = "";
            foreach (Effect effect in activeEffects)
            {
                if (effect.HideEffectTimer()) continue;
                text += "(" + (effect.GetEffectStartTime() + effect.GetEffectLength() - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()) / 1000 + " sec) " + effect.GetEffectName() + "\n";
            }
            Effect newestEffect = effectHistory[effectHistory.Count - 1];
            if(!newestEffect.IsTimedEffect() || newestEffect.HideEffectTimer())
            {
                text += newestEffect.GetEffectName() + "\n";
            }
            effectText.text = text;
        }

        // This effect list update system was supposed to be more advanced but is not working in an intended way.
        // The code is still here in the case I want to use this in the future but for now it's not intended to use this function.
        public static void UpdateEffectListAdvanced()
        {
            // This could probably be better optimized
            Effect newestEffect = effectHistory[effectHistory.Count - 1];
            if ((activeEffects.Count > 5 && newestEffect.IsTimedEffect()) || (activeEffects.Count > 4 && !newestEffect.IsTimedEffect()))
            {
                string text = "";
                foreach (Effect effect in activeEffects)
                {
                    text += (effect.IsTimedEffect() && (effect.GetEffectStartTime() + effect.GetEffectLength() - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 1000) / 1000 >= 1 ? "(" + (effect.GetEffectStartTime() + effect.GetEffectLength() - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()) / 1000 + " sec) " : "") + effect.GetEffectName() + "\n";
                }
                if (!newestEffect.IsTimedEffect())
                {
                    text += (newestEffect.IsTimedEffect() && (newestEffect.GetEffectStartTime() + newestEffect.GetEffectLength() - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 1000) / 1000 >= 1 ? "(" + (newestEffect.GetEffectStartTime() + newestEffect.GetEffectLength() - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()) / 1000 + " sec) " : "") + newestEffect.GetEffectName() + "\n";
                }
                else
                {

                }
                effectText.text = text;
            }
            else
            {
                List<Effect> mostRecentEffects = new List<Effect>();
                // Add the 5 most recent effects
                for (int i = effectHistory.Count - 6 < 0 ? 0 : effectHistory.Count - 6; i < effectHistory.Count; i++)
                {
                    Effect effect = effectHistory[i];
                    mostRecentEffects.Add(effect);
                }
                // Go from oldest active effect to the newest
                for(int i = activeEffects.Count - 1; i >= 0; i--)
                {
                    // If the oldest one is in the effect list then all the other active effects have to be in there aswell
                    if (mostRecentEffects.Contains(activeEffects[i])) break;
                    // Go from oldest recent effects and replace it with an active effect
                    for (int j = mostRecentEffects.Count - 1; j > mostRecentEffects.Count - 6; j--)
                    {
                        Effect effect = mostRecentEffects[j];
                        // If the selected effect is a timed effect then the active effect needs to be pushed down into a spot of a non active effect
                        if (effect.IsTimedEffect()) PushEffectDownIntoNonTimedAdvanced(mostRecentEffects, effect);
                        mostRecentEffects[j] = activeEffects[i];
                        break;
                    }
                }
                string text = "";
                foreach (Effect effect in mostRecentEffects)
                {
                    text += (effect.IsTimedEffect() && (effect.GetEffectStartTime() + effect.GetEffectLength() - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 1000) / 1000 >= 1 ? "(" + (effect.GetEffectStartTime() + effect.GetEffectLength() - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()) / 1000 + " sec) " : "") + effect.GetEffectName() + "\n";
                }
                effectText.text = text;
            }
        }

        private static void PushEffectDownIntoNonTimedAdvanced(List<Effect> effectList, Effect effect)
        {
            bool foundEffectPos = false;
            for (int i = effectList.Count - 1; i > 0; i--)
            {
                if(foundEffectPos)
                {
                    if (effectList[i].IsTimedEffect()) PushEffectDownIntoNonTimedAdvanced(effectList, effectList[i]);
                    effectList[i] = effect;
                    break;
                }
                if (effectList[i] == effect)
                {
                    foundEffectPos = true;
                    continue;
                }
            }
        }

        public static long GetLastTimerRun()
        {
            return lastTimerRun;
        }

        public static Activator GetActivator()
        {
            return activator;
        }
        public static List<Effect> GetActiveEffects()
        {
            return activeEffects;
        }

        public static PositionTracker GetPositionTracker()
        {
            return positionTracker;
        }

        public static bool GetHalfEffectTime()
        {
            return halfEffectWaitTime;
        }
        public static void SetHalfEffectTime(bool set)
        {
            halfEffectWaitTime = set;
        }

        public static bool GetEnabled()
        {
            return enabled;
        }
    }
}
