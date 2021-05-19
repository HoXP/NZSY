using System;
using UnityEngine;

/// <summary>
/// 挂到3D物体上，使鼠标hover时显示tips
/// </summary>
[DisallowMultipleComponent]
public class HoverTips : MonoBehaviour
{
    public string Tips = "--";
    private UIHUDName _hud = null;

    private void Awake()
    {
        UIHUDBase hud = HUDManager.Instance.InitializeHUD(HUDType.Name);
        if(hud == null)
        {
            throw new Exception($"### HUDManager.Instance.InitializeHUD HUDType.Name failed.");
        }
        hud.name = $"HUDName{gameObject.name}";
        _hud = hud as UIHUDName;
        _hud.Flush(Tips);
    }

    void OnMouseEnter()
    {
        if (_hud == null)
        {
            return;
        }
        _hud.ShowHide(true);
    }
    void OnMouseExit()
    {
        if (_hud == null)
        {
            return;
        }
        _hud.ShowHide(false);
    }
}