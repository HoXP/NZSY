using Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GoogoFSM
{
    public class GStateMachine
    {
        public int Id { get; set; }
        private Dictionary<string, int> dictStateEnumKV = new Dictionary<string, int>(); //该状态机的状态枚举的枚举名-枚举值映射字典
        private Dictionary<int, GState> dictState = new Dictionary<int, GState>();
        private List<GState> states = new List<GState>();
        private GState _curState = null;
        private int coId = 0;

        private GStateMachine() { }
        public static GStateMachine CreateInstance(int fsmId)
        {
            GStateMachine sm = new GStateMachine();
            sm.Id = fsmId;
            return sm;
        }

        public void Init(int stateEnum)
        {
            if (!dictState.ContainsKey(stateEnum))
            {
                throw new Exception($"StateMachine does not contain state [{stateEnum}].");
            }
            GState currState = dictState[stateEnum];
            if (currState == null)
            {
                Debug.LogError($"No state [{stateEnum}].");
                return;
            }
            _curState = currState;
            coId = CoroutineManager.Instance.AddCo(_curState.OnEnter());
        }

        public void ChangeState(int stateEnum)
        {
            if (!dictState.ContainsKey(stateEnum))
            {
                throw new Exception($"StateMachine does not contain state [{stateEnum}].");
            }
            GState currState = _curState;
            GState nextState = dictState[stateEnum];
            if (nextState == null)
            {
                Debug.LogError($"No next state [{stateEnum}].");
                return;
            }
            if (currState == nextState)
            {
                Debug.LogError($"AIready in state [{stateEnum}].");
                return;
            }
            Debug.Log($"ChangeState [{stateEnum}].");
            _curState = nextState;
            Co co = CoroutineManager.Instance.GetCo(coId);
            if (co != null)
            {
                CoroutineManager.Instance.AddCo(coId, currState.OnEixt());
            }
            else
            {
                coId = CoroutineManager.Instance.AddCo(nextState.OnEnter());
            }
            CoroutineManager.Instance.AddCo(coId, nextState.OnEnter());
        }

        public void ChangeState<T>(T stateEnum)
        {
            GStateMachineManager.Instance.ChangeState(Id, stateEnum);
        }
        public void Destroy()
        {
            if (dictState != null)
            {
                dictState = null;
            }
            if (states != null)
            {
                for (int i = 0; i < states.Count; i++)
                {
                    states[i].Destroy();
                    states[i] = null;
                }
                states = null;
            }
            _curState = null;
        }

        private GState GetState(int enumValue)
        {
            if (dictState != null && dictState.ContainsKey(enumValue))
            {
                return dictState[enumValue];
            }
            return null;
        }

        public bool AddState(GState pState)
        {
            if (pState == null)
            {
                return false;
            }
            int stateEnum = pState.EnumValue;
            GState state = GetState(stateEnum);
            if (state != null)
            {
                Debug.LogError($"State [{stateEnum}] already exist.");
                return false;
            }
            dictState.Add(stateEnum, pState);
            states.Add(pState);
            return true;
        }

        public void AddStateEnumKV(string key, int value)
        {
            if (dictStateEnumKV != null && !dictStateEnumKV.ContainsKey(key))
            {
                dictStateEnumKV.Add(key, value);
            }
        }
        public int GetStateEnum(string key)
        {
            if (dictStateEnumKV != null && dictStateEnumKV.ContainsKey(key))
            {
                return dictStateEnumKV[key];
            }
            return 0;
        }
    }
}