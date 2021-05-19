using System;
using UnityEngine;

/// <summary>
/// 用于设置点击3D物体时的回调
/// </summary>
public class ClickHandler : MonoBehaviour
{
    protected Action<GameObject> _ua = null;

    internal virtual void SetOnClick(Action<GameObject> ua)
    {
        _ua = ua;
    }
    
    protected virtual void OnMouseDown()
    {
        _ua?.Invoke(gameObject);
    }
}