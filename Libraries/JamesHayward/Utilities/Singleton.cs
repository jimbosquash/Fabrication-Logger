using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public abstract class Singleton<T> where T : Singleton<T>
    {
        private static readonly Lazy<T> lazy =
                            new Lazy<T>(() => Activator.CreateInstance(typeof(T), true) as T);

        public static T Instance { get { return lazy.Value; } }

        public static void Initialize()
        {
            var I = Instance;
        }
    }
}
