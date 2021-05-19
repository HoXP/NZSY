using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Framework
{
    public interface ISingleton
    {
        bool DestroySingleton();
    }
    public abstract class ASingleton<T> : ISingleton where T : ASingleton<T>
    {
        protected static T instance = null;
        public static bool HasInstance => instance != null;
#if USE_MULTITHREAD_SINGLETON
        public static T Instance { get { lock (instanceLock) { return HasInstance ? instance : instance = SingletonManager.CreateSingleton<T>(); } } }
        private static readonly object instanceLock = new object();
#else
        private static readonly int mainThreadId = Thread.CurrentThread.ManagedThreadId;
        private static bool isPlaying { get { return Thread.CurrentThread.ManagedThreadId == mainThreadId ? Application.isPlaying : SingletonManager.isPlaying; } }

        public static T Instance
        {
            get
            {
                return HasInstance ? instance : instance = SingletonManager.CreateSingleton<T>();
            }
        }
#endif

        protected ASingleton() { }

        public bool DestroySingleton()
        {
            if (!SingletonManager.DestroySingleton<T>()) return false;
            instance = null;
            return true;
        }
    }
    public static class SingletonManager
    {
        private static LinkedList<ISingleton> singletons = new LinkedList<ISingleton>();
        public static bool isPlaying { get; private set; }
        public static int SingletonsCount => singletons.Count;

        public static T TryGetSingleton<T>() where T : ASingleton<T>
        {
            return ASingleton<T>.HasInstance ? GetSingleton<T>() : null;
        }
        public static T GetSingleton<T>() where T : ASingleton<T>
        {
            return ASingleton<T>.Instance;
        }
        public static T CreateSingleton<T>() where T : ASingleton<T>
        {
            lock (singletons)
            {
                T singleton = TryGetSingleton<T>();
                if (singleton == null)
                {
                    singleton = NonPublicCtor<T>.Ctor();
                    singletons.AddLast(singleton);
                }
                return singleton;
            }
        }
        public static bool DestroySingleton<T>() where T : ASingleton<T>
        {
            T singleton = TryGetSingleton<T>();
            if (singleton == null || !singletons.Remove(singleton)) return false;
            singleton.DestroySingleton();
            return true;
        }
        public static void OnApplicationStart()
        {
            isPlaying = true;
        }
        public static void OnApplicationQuit()
        {
            DestroyAllSingletons();
            isPlaying = false;
        }
        public static void DestroyAllSingletons()
        {
            while (SingletonsCount > 0)
            {
                var singleton = singletons.Last.Value;
                singleton.DestroySingleton();
            }
        }
    }
}