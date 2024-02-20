using ChaosMod.Activator.Activators;
using System.Collections.Generic;

namespace ChaosMod.Activator
{
    internal class AllActivators
    {
        private static Dictionary<string, Activator> activators = new Dictionary<string, Activator>();

        static AllActivators()
        {
            activators.Add("random", new RandomActivator());
            activators.Add("twitch", new TwitchActivator());
        }

        public static Dictionary<string, Activator> GetActivators()
        {
            return activators;
        }
    }
}
