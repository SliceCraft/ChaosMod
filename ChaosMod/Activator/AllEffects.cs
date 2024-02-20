using ChaosMod.Effects;

namespace ChaosMod.Activator
{
    internal class AllEffects
    {
        public enum Effects
        {
            //LockAllDoors,
            InfiniteSprint,
            SpawnEnemy,
            DamageRoulette,
            Heal,
            RandomTeleport,
            YIPPEEEEE,
            //RemoveHoldingItems,
            DropEverything,
            UnlockUnlockable,
            FasterEffects,
            OneHitExplosions,
            AttractivePlayer,
            WarpSpeed,
            DisableControls
        }

        public static Effect InstantiateEffect(Effects effect)
        {
            System.Random rnd = new System.Random();
            int nm = rnd.Next(3);
            if (nm == 0) return new InfiniteSprintEffect();
            else if(nm == 1) return new SpawnEnemyEffect();
            else return new YIPPEEEEEffect();
            switch (effect)
            {
                default: return null;
                //case Effects.LockAllDoors: return new LockAllDoorsEffect(); // This effect is currently broken, it works kinda but not fully as expected
                case Effects.InfiniteSprint: return new InfiniteSprintEffect();
                case Effects.SpawnEnemy: return new SpawnEnemyEffect();
                case Effects.DamageRoulette: return new DamageRouletteEffect();
                case Effects.Heal: return new HealEffect();
                case Effects.RandomTeleport: return new RandomTeleportEffect();
                case Effects.YIPPEEEEE: return new YIPPEEEEEffect();
                //case Effects.RemoveHoldingItems: return new RemoveHolidingItemsEffect(); // This effect sometimes causes some weird issues, needs more investigation
                case Effects.DropEverything: return new DropEverythingEffect();
                case Effects.UnlockUnlockable: return new UnlockUnlockableEffect();
                case Effects.FasterEffects: return new FasterEffectsEffect();
                case Effects.OneHitExplosions: return new OneHitExplosionsEffect();
                case Effects.AttractivePlayer: return new AttractivePlayerEffect();
                case Effects.WarpSpeed: return new WarpSpeedEffect();
                case Effects.DisableControls: return new DisableControlsEffect();
            }
        }
    }
}
