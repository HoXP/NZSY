using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RepeatPressEventTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public float interval = 0.1f; //回调触发间隔时间;

    public float delay = 1.0f; //延迟时间;

    public UnityEvent onLongPress = new UnityEvent();

    private bool isPointDown = false;
    private float lastInvokeTime;

    private float m_Delay = 0f;

    void Start()
    {
        m_Delay = delay;
    }

    void Update()
    {
        if (isPointDown)
        {
            if ((m_Delay -= Time.deltaTime) > 0f)
            {
                return;
            }

            if (Time.time - lastInvokeTime > interval)
            {//触发点击;
                onLongPress.Invoke();
                lastInvokeTime = Time.time;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointDown = true;
        m_Delay = delay;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointDown = false;
        m_Delay = delay;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointDown = false;
        m_Delay = delay;
    }
}