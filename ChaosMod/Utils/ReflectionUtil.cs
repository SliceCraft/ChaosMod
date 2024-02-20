using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMod.Utils
{
    internal class ReflectionUtil
    {
        public static void CallFunction(object o, string methodName, params object[] args)
        {
            var method = o.GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method?.Invoke(o, args);
        }
    }
}
