using Framework;
using System;
using System.Collections.Generic;

namespace GoogoFSM
{
    public class GStateMachineManager : ASingleton<GStateMachineManager>
    {
        static private int idSeedStateMachine = 1;
        private Dictionary<int, GStateMachine> _stateMachineDict = new Dictionary<int, GStateMachine>();
        private List<GStateMachine> _stateMachineList = new List<GStateMachine>();

        private GStateMachineManager() { }

        /// <summary>
        /// 创建状态机
        /// </summary>
        /// <typeparam name="T">状态枚举</typeparam>
        /// <param name="stateId">初始状态</param>
        /// <returns>状态机</returns>
        public GStateMachine CreateStateMachine<T>() where T : struct
        {
            int id = idSeedStateMachine++;
            GStateMachine sm = GStateMachine.CreateInstance(id);
            Type enumType = typeof(T);
            string[] names = Enum.GetNames(typeof(T)) as string[];
            for (int i = 0; i < names.Length; i++)
            {
                string strEnumKey = names[i];
                string name = $"{enumType.Name}{strEnumKey}";
                GState state = GState.CreateInstance(name);
                if (state != null)
                {
                    int intValue = (int)Enum.Parse(typeof(T), strEnumKey);
                    //Log.Fatal($"### {typeof(T)} - {intValue}");
                    state.Init(intValue, id);
                    sm.AddStateEnumKV(strEnumKey, state.EnumValue);
                    sm.AddState(state);
                }
                else
                {
                    throw new Exception($"### no class named {name}.");
                }
            }
            _stateMachineDict.Add(id, sm);
            _stateMachineList.Add(sm);
            return sm;
        }
        public void DestroyStateMachine(int fsmId)
        {
            GStateMachine sm = null;
            if (_stateMachineDict != null)
            {
                if (_stateMachineDict.ContainsKey(fsmId))
                {
                    sm = _stateMachineDict[fsmId];
                    _stateMachineDict.Remove(fsmId);
                }
            }
            if (_stateMachineList != null)
            {
                _stateMachineList.Remove(sm);
            }
            if (sm != null)
            {
                sm.Destroy();
                sm = null;
            }
        }

        public GStateMachine GetStateMachine(int fsmId)
        {
            if (_stateMachineDict != null)
            {
                if (_stateMachineDict.ContainsKey(fsmId))
                {
                    GStateMachine sm = _stateMachineDict[fsmId];
                    return sm;
                }
            }
            return null;
        }

        public void AddState(int fsmId, GState state)
        {
            GStateMachine sm = GetStateMachine(fsmId);
            if (sm != null)
            {
                sm.AddState(state);
            }
        }

        /// <summary>
        /// 初始化状态机
        /// </summary>
        /// <typeparam name="T">int型的状态枚举</typeparam>
        /// <param name="fsmId">状态机Id</param>
        /// <param name="stateId">状态枚举</param>
        public void Init<T>(int fsmId, T stateId)
        {
            GStateMachine sm = GetStateMachine(fsmId);
            if (sm != null)
            {
                int state = sm.GetStateEnum(stateId.ToString());
                sm.Init(state);
            }
        }

        /// <summary>
        /// 更改状态
        /// </summary>
        /// <typeparam name="T">int型的状态枚举</typeparam>
        /// <param name="fsmId">状态机Id</param>
        /// <param name="stateId">状态枚举</param>
        public void ChangeState<T>(int fsmId, T stateId)
        {
            GStateMachine sm = GetStateMachine(fsmId);
            if (sm != null)
            {
                int state = sm.GetStateEnum(stateId.ToString());
                sm.ChangeState(state);
            }
        }
    }
}