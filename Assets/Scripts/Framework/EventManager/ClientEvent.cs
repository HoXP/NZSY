namespace Framework
{
    public class ClientEvent
    {
        private object[] m_EventParams = null;//动态参数;

        internal float timelock { get; set; } = 0;

        public int eventId { get; private set; } = -1;

        internal void Reset()
        {
            m_EventParams = null;
            eventId = -1;
            timelock = 0f;
        }

        internal void SetEvent(int eventId, float timeLock, params object[] arms)
        {
            this.eventId = eventId;
            timelock = timeLock;
            m_EventParams = arms;
        }

        internal void SetEvent(int eventId, float timeLock = 0f)
        {
            this.eventId = eventId;
            timelock = timeLock;
            m_EventParams = null;
        }

        public T GetParameter<T>(int index)
        {
            if (m_EventParams == null || index >= m_EventParams.Length)
            {
                return default;
            }
            if (m_EventParams[index] != null && !(m_EventParams[index] is T))
            {
                return default;
            }
            return (T)m_EventParams[index];
        }

        public bool GetParameter<T>(int index, ref T value)
        {
            if (m_EventParams == null || index >= m_EventParams.Length)
            {
                value = default;
                return false;
            }

            if (m_EventParams[index] != null && !(m_EventParams[index] is T))
            {
                value = default;
                return false;
            }
            value = (T)m_EventParams[index];
            return true;
        }
    }
}