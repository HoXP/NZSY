using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace GoogoFSM
{
    public interface IState
    {
        int EnumValue { get; set; } //对应状态枚举的值
        int FsmId { get; set; } //状态机Id

        void Init(int enumValue, int fsmId);
    }

    public class GState : IState
    {
        private List<GTransition> transitions = null;

        public int EnumValue { get; set; }
        public int FsmId { get; set; }

        protected GState()
        {
        }

        public void Init(int enumValue, int fsmId)
        {
            EnumValue = enumValue;
            FsmId = fsmId;
        }

        public virtual IEnumerator OnEnter()
        {
            yield return null;
        }
        public virtual IEnumerator OnEixt()
        {
            yield return null;
        }
        public virtual void Destroy()
        {

        }

        /// <summary>
        /// 根据子类类名实例化子类实例
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static GState CreateInstance(string className)
        {
            GState s = null;
            Type tt = Type.GetType(className);
            if (tt == null)
            {
                throw new Exception($"### no class named {className}.");
            }
            ConstructorInfo ci = tt.GetConstructor(Type.EmptyTypes);
            s = ci.Invoke(null) as GState;
            return s;
        }
    }
}