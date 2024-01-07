namespace RespawnTimer.API.Features
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using UnityEngine;

    public static class RueiHelper
    {
        private const string RUEINAME = "RueI";
        private const string RUEIMAIN = "RueI.RueIMain";
        private const string ENSUREINIT = "EnsureInit";
        private const string REFLECTIONHELPERS = "RueI.Extensions.ReflectionHelpers";
        private const string GETELEMENTSHOWER = "GetElementShower";

        public static bool IsActive { get; private set; } = false;


        public static void Show(ReferenceHub hub, string content, TimeSpan span)
        {
            shower(hub, content, 0, span);
        }

        internal static void Refresh()
        {
            shower = null;
            IsActive = false;

            IEnumerable<Assembly> assemblies =
#if EXILED
            Exiled.Loader.Loader.Dependencies;
#else
            PluginAPI.Loader.AssemblyLoader.Dependencies;
#endif

            Assembly assembly = assemblies.FirstOrDefault(x => x.GetName().Name == RUEINAME);
            if (assembly == null)
            {
                return;
            }

            MethodInfo elementShower = assembly.GetType(REFLECTIONHELPERS)?.GetMethod(GETELEMENTSHOWER);
            var result = elementShower?.Invoke(null, new object[] { });
            if (result is not Action<ReferenceHub, string, float, TimeSpan> elemShower)
            {
                return;
            }

            MethodInfo init = assembly.GetType(RUEIMAIN)?.GetMethod(ENSUREINIT);
            if (init == null)
            {
                return;
            }

            init.Invoke(null, new object[] { });
            shower = elemShower;
            IsActive = true;
        }

        private static Action<ReferenceHub, string, float, TimeSpan> shower;
    }
}