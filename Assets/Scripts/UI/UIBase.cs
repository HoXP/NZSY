using UnityEngine;

[DisallowMultipleComponent]
public class UIBase : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        OnInitComponent();
        OnRegisterEvent();
    }
    protected virtual void OnDisable()
    {
        OnResetComponent();
        OnUnRegisterEvent();
    }

    private void Update()
    {
        OnUpdate();
    }

    internal void Show(params object[] ps)
    {
        gameObject.SetActive(true);
        OnShow(ps);
    }
    internal void Hide()
    {
        gameObject.SetActive(false);
        OnHide();
    }

    protected virtual void OnShow(params object[] ps)
    {
    }
    protected virtual void OnHide()
    {
    }
    protected virtual void OnUpdate()
    {
    }
    protected virtual void OnInitComponent()
    {
    }
    protected virtual void OnResetComponent()
    {
    }
    protected virtual void OnRegisterEvent()
    {
    }
    protected virtual void OnUnRegisterEvent()
    {
    }
}