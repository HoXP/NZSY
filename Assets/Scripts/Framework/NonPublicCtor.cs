using System;
using System.Reflection;

namespace Framework
{
    public static class NonPublicCtor<T> where T : class
    {
        private static ConstructorInfo ctor = null;
        public static T Ctor()
        {
            if (ctor == null)
            {
                var type = typeof(T);
                var ctorsPublic = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
                if (ctorsPublic.Length > 0)
                {//不允许有public的构造函数
                    throw new Exception($"find {type} public constructor");
                }
                var ctorsNonPublic = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                ctor = Array.Find(ctorsNonPublic, c => c.GetParameters().Length == 0);
                if (ctor == null)
                {
                    throw new Exception($"not find {type} non-public default constructor");
                }
            }
            return ctor.Invoke(null) as T;
        }
    }
}