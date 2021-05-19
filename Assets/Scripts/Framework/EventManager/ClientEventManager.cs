using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class ClientEventManager : MonoSingleton<ClientEventManager>
    {
        public delegate void EventDelegate(ClientEvent eve);

        protected Queue<ClientEvent> m_EventCache = new Queue<ClientEvent>();
        protected List<ClientEvent> m_ProcessEvents = new List<ClientEvent>();

        protected Dictionary<int, List<EventDelegate>> m_FunMap = new Dictionary<int, List<EventDelegate>>();   // 回调函数的处理列表;
        protected Dictionary<int, List<EventDelegate>> m_TempFunMap = new Dictionary<int, List<EventDelegate>>();// 临时event列表，可以检查错误:处理event过程中增加、删除同一个event;

        protected List<int> m_FrameCacheIDs = new List<int>();

        public void PushEvent(int eventId, bool isImmediately = true, float timeLock = 0f)
        {
            // 如果非立即执行，且当帧已经创建过，直接跳过;
            if (!isImmediately && m_FrameCacheIDs.Contains(eventId))
            {
                return;
            }

            ClientEvent eve = GetEventFromCache();
            if (eve == null)
            {
                return;
            }
            eve.SetEvent(eventId, timeLock);
            if (isImmediately)
            {
                ActionEvent(eve);
                return;
            }
            m_ProcessEvents.Add(eve);
            m_FrameCacheIDs.Add(eventId);
        }

        /// <summary>
        /// 发送一个消息;
        /// </summary>
        /// <param name="eventId">消息ID</param>
        /// <param name="isImmediately">是否立即发送</param>
        /// <param name="timeLock">延迟时间</param>
        /// <param name="arms">动态参数</param>
        public void PushEvent(int eventId, bool isImmediately, float timeLock, params object[] arms)
        {
            ClientEvent eve = GetEventFromCache();
            if (eve == null)
            {
                return;
            }
            eve.SetEvent(eventId, timeLock, arms);
            if (isImmediately)
            {
                ActionEvent(eve);
                return;
            }
            m_ProcessEvents.Add(eve);
        }

        public void RegisterEvent(int eventId, EventDelegate fun)
        {
            if (fun == null)
            {
                return;
            }
            List<EventDelegate> funList = GetFunList(eventId);
            if (funList != null && !funList.Contains(fun))
            {
                if (Application.isEditor)
                {
                    List<EventDelegate> tempList = GetTempFunList(eventId);
                    if (tempList != null && tempList.Count > 0)
                    {
                        Debug.LogWarning("Please don't register event when processing it: " + eventId);
                    }
                }
                funList.Add(fun);
            }
        }

        public void UnRegisterEvent(int eventId, EventDelegate fun, bool check = true)
        {
            if (fun == null)
            {
                return;
            }
            List<EventDelegate> funList = GetFunList(eventId);
            if (funList != null && funList.Contains(fun))
            {
                if (check && Application.isEditor)
                {
                    List<EventDelegate> tempList = GetTempFunList(eventId);
                    if (tempList != null && tempList.Count > 0)
                    {
                        //Debug.LogError("Please don't unregister event when processing it: " + eventId);
                    }
                }
                funList.Remove(fun);
            }
        }

        public void Update()
        {
            if (m_FrameCacheIDs.Count > 0)
            {
                m_FrameCacheIDs.Clear();
            }
            if (m_ProcessEvents == null || m_ProcessEvents.Count < 1)
            {
                return;
            }

            for (int i = 0; i < m_ProcessEvents.Count;)
            {
                ClientEvent eve = m_ProcessEvents[i];
                if (eve == null)
                {
                    m_ProcessEvents.RemoveAt(i);
                    continue;
                }
                eve.timelock -= Time.deltaTime;
                if (eve.timelock > 0)
                {
                    i++;
                    continue;
                }

                ActionEvent(eve);
                m_ProcessEvents.RemoveAt(i);
            }
        }

        protected void ActionEvent(ClientEvent eve)
        {
            if (eve == null)
            {
                return;
            }

            List<EventDelegate> tempList = GetTempFunList(eve.eventId);
            tempList.Clear();
            tempList.AddRange(GetFunList(eve.eventId));
            for (int i = 0; i < tempList.Count; ++i)
            {
                EventDelegate fun = tempList[i];
                if (fun == null)
                {
                    continue;
                }
                try
                {
                    fun(eve);
                }
                catch (Exception ex)
                {
                    Debug.LogError(string.Format("{0} --- {1} --- {2} --- {3}",
                        eve.eventId, fun.Target, fun.Method != null ? fun.Method.Name : "NULL", ex.ToString()));
                }
            }

            AddEvent2Cache(eve);
            tempList.Clear();
        }

        protected void AddEvent2Cache(ClientEvent eve)
        {
            if (eve == null)
            {
                return;
            }

            eve.Reset();
            m_EventCache.Enqueue(eve);
            //m_EventCache.Add(eve);
        }

        protected ClientEvent GetEventFromCache()
        {
            ClientEvent eve = null;

            if (m_EventCache.Count > 0)
            {
                eve = m_EventCache.Dequeue();
            }
            else
            {
                eve = new ClientEvent();
            }

            return eve;
        }

        protected List<EventDelegate> GetFunList(int eventId)
        {
            List<EventDelegate> funList = null;

            if (!m_FunMap.TryGetValue(eventId, out funList))
            {
                funList = new List<EventDelegate>();
                m_FunMap.Add(eventId, funList);
            }

            return funList;
        }

        protected List<EventDelegate> GetTempFunList(int eventId)
        {
            List<EventDelegate> funList = null;
            if (!m_TempFunMap.TryGetValue(eventId, out funList))
            {
                funList = new List<EventDelegate>();
                m_TempFunMap.Add(eventId, funList);
            }
            return funList;
        }
    }
}