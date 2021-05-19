using System;
using System.Collections;
using System.Collections.Generic;

namespace Framework
{
    public class Co
    {
        private static int IdSeed = 0;
        public int Id { get; private set; }

        public Queue<IEnumerator> Cos { get; private set; }

        public bool IsFree { get { return Cos != null && Cos.Count > 0; } }

        public Co()
        {
            Id = ++IdSeed;
            Cos = new Queue<IEnumerator>();
            CoroutineManager.Instance.StartCoroutine(StartCo());
        }

        public IEnumerator StartCo()
        {
            while (Cos.Count <= 0)
            {
                yield return null;
            }
            while (Cos.Count > 0)
            {
                IEnumerator co = Cos.Dequeue();
                yield return co;
            }
            CoroutineManager.Instance.StartCoroutine(StartCo());
        }

        public void AddCo(IEnumerator co)
        {
            Cos.Enqueue(co);
        }
    }

    public class CoroutineManager : MonoSingleton<CoroutineManager>
    {
        private List<Co> cos = null;

        protected override void OnInit()
        {
            cos = new List<Co>();
        }

        public Co GetCo(int coId)
        {
            if (cos != null && cos.Count > 0)
            {
                for (int i = 0; i < cos.Count; i++)
                {
                    Co co = cos[i];
                    if (co.Id == coId)
                    {
                        return co;
                    }
                }
            }
            return null;
        }
        public void AddCo(int coId, IEnumerator e)
        {
            Co co = GetCo(coId);
            if (co != null)
            {
                co.AddCo(e);
                cos.Add(co);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cos"></param>
        internal void AddCos(int coId, params IEnumerator[] cos)
        {
            if (cos == null || cos.Length <= 0)
            {
                return;
            }
            for (int i = 0; i < cos.Length; i++)
            {
                IEnumerator e = cos[i];
                AddCo(coId, e);
            }
        }

        public int AddCo(IEnumerator e)
        {
            Co coFree = GetFreeCo();
            coFree.AddCo(e);
            cos.Add(coFree);
            return coFree.Id;
        }
        internal Co GetFreeCo()
        {
            if (cos != null && cos.Count > 0)
            {
                for (int i = 0; i < cos.Count; i++)
                {
                    Co co = cos[i];
                    if (co.IsFree)
                    {
                        return co;
                    }
                }
            }
            return new Co();
        }

        /// <summary>
        /// 新启动一个协程，串行一组协程方法
        /// </summary>
        /// <param name="cos"></param>
        /// <returns></returns>
        internal int StartSerialCoroutines(params IEnumerator[] cos)
        {
            if (cos == null || cos.Length <= 0)
            {
                return 0;
            }
            int coId = 0;
            for (int i = 0; i < cos.Length; i++)
            {
                IEnumerator e = cos[i];
                if (i == 0)
                {
                    coId = AddCo(e);
                }
                else
                {
                    AddCo(coId, e);
                }
            }
            return coId;
        }
    }
}