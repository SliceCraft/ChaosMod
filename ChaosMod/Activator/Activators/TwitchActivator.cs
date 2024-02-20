using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ChaosMod.Activator.Activators
{
    internal class TwitchActivator : Activator
    {
        private List<Effect> chosenEffects = new List<Effect>();
        private List<string> votedPlayers = new List<string>();
        private List<int> votedIndexes = new List<int>();
        // When running chooseNewEffect() it'll invert highNumbers, it should start with the numbers 1 to 3 and then later on move to 5 to 6
        public bool highNumbers = true;
        private static TextMeshProUGUI twitchText = null;

        public string getName()
        {
            return "twitch";
        }

        public void Start()
        {
            if (ChaosMod.getInstance().twitchIRCClient == null)
            {
                ChaosMod.getInstance().twitchIRCClient = new Twitch.TwitchIRCClient();
                ChaosMod.getInstance().twitchIRCClient.ConnectToTwitch();
            }
            // When running chooseNewEffect() it'll invert highNumbers, it should start with the numbers 1 to 3 and then later on move to 5 to 6
            highNumbers = true;
            if (ChaosMod.ConfigTwitchOptionsShowcase.Value != "chatmessage" && twitchText == null)
            {
                AddTextElement();
            }
            ChooseNewEffectList();
        }

        public void Stop()
        {
            if(twitchText != null) twitchText.text = "";
        }

        public Effect ChooseEffect()
        {
            System.Random rnd = new System.Random();
            int effectIndex;
            if (votedIndexes.Count > 0) effectIndex = votedIndexes[rnd.Next(votedIndexes.Count)];
            else effectIndex = rnd.Next(chosenEffects.Count);
            Effect chosenEffect = chosenEffects[effectIndex];
            ChooseNewEffectList();
            return chosenEffect;
        }

        private void ChooseNewEffectList()
        {
            highNumbers = !highNumbers;
            votedIndexes = new List<int>();
            votedPlayers = new List<string>();
            System.Random rnd = new System.Random();
            Array effects = Enum.GetValues(typeof(AllEffects.Effects));
            chosenEffects = new List<Effect>();
            while (chosenEffects.Count < 3)
            {
                int a = rnd.Next(effects.Length);
                AllEffects.Effects test = (AllEffects.Effects)effects.GetValue(a);
                Effect effect = AllEffects.InstantiateEffect(test);
                bool effectAlreadyChosen = false;
                foreach (Effect previouslyChosenEffect in chosenEffects)
                {
                    // If effect == null it should just pick a new effect which will be done if effectAlreadyChosen is set to true
                    if (effect == null || previouslyChosenEffect.GetEffectName() == effect.GetEffectName())
                    {
                        effectAlreadyChosen = true;
                    }
                }
                if (!effectAlreadyChosen && effect.IsAllowedToRun())
                {
                    chosenEffects.Add(effect);
                }
            }
            if (ChaosMod.ConfigTwitchOptionsShowcase.Value == "chatmessage")
            {
                string twitchChatMessage = "";
                for(int i = 0; i < chosenEffects.Count; i++)
                {
                    twitchChatMessage += (!highNumbers ? i + 1 : (i + 4)) + ". " + chosenEffects[i].GetEffectName() + ". ";
                }
                twitchChatMessage += "Vote by typing the number in chat";
                ChaosMod.getInstance().twitchIRCClient.sendMessage(twitchChatMessage);
            }
            else
            {
                RefreshEffectText();
            }
        }

        private int GetAmountOfEffectVotes(int index)
        {
            int amount = 0;
            for (int i = 0; i < votedIndexes.Count; i++)
            {
                if (votedIndexes[i] == index) amount++;
            }
            return amount;
        }

        public void VoteEffect(string sender, int vote)
        {
            for (int i = 0; i < votedPlayers.Count; i++)
            {
                if (sender.Equals(votedPlayers[i])) return;
            }
            votedPlayers.Add(sender);
            votedIndexes.Add(vote);
            RefreshEffectText();
        }

        public void RefreshEffectText()
        {
            if (ChaosMod.ConfigTwitchOptionsShowcase.Value == "chatmessage") return;
            twitchText.text = "";
            for (int i = 0; i < chosenEffects.Count; i++)
            {
                twitchText.text += (!highNumbers ? (i + 1) : (i + 4)) + ". " + chosenEffects[i].GetEffectName() + " (" + GetAmountOfEffectVotes(i) + " votes)\n";
            }
            twitchText.text += "Vote by typing the number in chat\nTime left: " + (TimerSystem.GetLastTimerRun() + (TimerSystem.GetHalfEffectTime() ? ChaosMod.ConfigTimeBetweenEffects.Value / 2 * 1000 : ChaosMod.ConfigTimeBetweenEffects.Value * 1000) + 1000 - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()) / 1000 + " seconds";
        }

        private void AddTextElement()
        {
            GameObject textElement = new GameObject();
            textElement.transform.parent = HUDManager.Instance.HUDContainer.transform;
            textElement.name = "ChaosModTwitchText";
            Vector3 scale = textElement.transform.localScale;
            scale.x = 1F;
            scale.y = 1F;
            scale.z = 1F;
            textElement.transform.localScale = scale;
            Vector3 position = textElement.transform.localPosition;
            position.x = 350F;
            position.y = 220F;
            position.z = 0F;
            textElement.transform.localPosition = position;
            twitchText = textElement.AddComponent<TextMeshProUGUI>();
            twitchText.text = "";
            twitchText.fontSize = 13;
            twitchText.lineSpacing = 30;
        }
    }
}
