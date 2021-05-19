using UnityEngine;

namespace Framework
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T _instance;

        public static T GetInstance()
        {
            if (_instance == null)
            {
                GameObject gObj = new GameObject(typeof(T).Name);
                DontDestroyOnLoad(gObj);

                Transform tran = gObj.transform;
                tran.parent = Parent();

                tran.localPosition = Vector3.zero;
                tran.localEulerAngles = Vector3.zero;
                tran.localScale = Vector3.one;

                _instance = gObj.AddComponent<T>();
            }
            return _instance;
        }

        public static T Instance
        {
            get
            {
                return GetInstance();
            }
        }

        private void Awake()
        {
            OnInit();
        }

        private void Update()
        {
            OnUpdate(Time.deltaTime);
        }

        private void OnApplicationQuit()
        {
        }

        private static Transform _singletonRoot = null;
        private static Transform Parent()
        {
            if (_singletonRoot == null)
            {
                GameObject go = new GameObject("SingletonRoot");
                DontDestroyOnLoad(go);
                _singletonRoot = go.transform;
            }
            return _singletonRoot;
        }

        protected virtual void OnInit() { }
        public virtual void OnUpdate(float deltaTime) { }
        public virtual void OnReConnect() { }
        public virtual void Clear() { }
    }
}