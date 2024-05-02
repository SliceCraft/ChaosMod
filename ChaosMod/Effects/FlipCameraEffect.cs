using ChaosMod.Activator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects
{
    internal class FlipCameraEffect : Effect
    {
        private bool horizontalFlip;

        public FlipCameraEffect() : base()
        {
            System.Random random = new System.Random();
            horizontalFlip = random.Next(2) == 0;
        }

        public override string GetEffectName()
        {
            return "Flip Camera " + (horizontalFlip ? "Horizontally" : "Vertically");
        }

        public override void StartEffect()
        {
            FlipScreen();
        }

        public override void StopEffect()
        {
            FlipScreen();
        }

        private void FlipScreen()
        {
            float x = HUDManager.Instance.HUDContainer.transform.parent.parent.localScale.x;
            float y = HUDManager.Instance.HUDContainer.transform.parent.parent.localScale.y;
            float z = HUDManager.Instance.HUDContainer.transform.parent.parent.localScale.z;
            if (horizontalFlip) x = -x;
            else y = -y;
            Vector3 newScale = new Vector3(x, y, z);
            HUDManager.Instance.HUDContainer.transform.parent.parent.localScale = newScale;
        }

        public override bool IsTimedEffect()
        {
            return true;
        }

        public override long GetEffectLength()
        {
            return 15000;
        }
    }
}
