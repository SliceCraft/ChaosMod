using ChaosMod.Effects;

namespace ChaosMod.Activator
{
    internal class AllEffects
    {
        public enum Effects
        {
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
            DisableControls,
            DropHelpfulItemEffect,
            DropScrapEffect,
            SpinningEnemiesEffect,
            //RandomOutfitEffect
            RandomEffectEffect,
            TeleportToShipEffect,
            NoStaminaEffect,
            EnemiesVoteToLeaveEffect,
            TeleportToHeavenEffect,
            UTurnEffect,
            InvisibleEnemiesEffect
        }

        public static Effect InstantiateEffect(Effects effect)
        {
            switch (effect)
            {
                default: return null;
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
                case Effects.DropHelpfulItemEffect: return new DropHelpfulItemEffect();
                case Effects.DropScrapEffect: return new DropScrapEffect();
                case Effects.SpinningEnemiesEffect: return new SpinningEnemiesEffect();
                //case Effects.RandomOutfitEffect: return new RandomOutfitEffect();
                case Effects.RandomEffectEffect: return new RandomEffectEffect();
                case Effects.TeleportToShipEffect: return new TeleportToShipEffect();
                case Effects.NoStaminaEffect: return new NoStaminaEffect();
                case Effects.EnemiesVoteToLeaveEffect: return new EnemiesVoteToLeaveEffect();
                case Effects.TeleportToHeavenEffect: return new TeleportToHeavenEffect();
                case Effects.UTurnEffect: return new UTurnEffect();
                case Effects.InvisibleEnemiesEffect: return new InvisibleEnemiesEffect();
            }
        }
    }
}
