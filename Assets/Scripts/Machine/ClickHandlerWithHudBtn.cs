using System;
using UnityEngine;

/// <summary>
/// 点击3D物体弹出HUD按钮，点击按钮操作
/// </summary>
public class ClickHandlerWithHudBtn : ClickHandler
{
    private UIHUDOperation _hudOperation = null;
    internal UIHUDOperation HUDOperation
    {
        get
        {
            if (_hudOperation == null)
            {
                UIHUDBase hud = HUDManager.Instance.InitializeHUD(HUDType.Operation);
                if (hud == null)
                {
                    throw new Exception($"### create HUDType.Operation failed.");
                }
                hud.name = $"HUDOperation{gameObject.name}";
                hud.ShowHide(false);
                _hudOperation = hud as UIHUDOperation;
            }
            return _hudOperation;
        }
    }

    internal void Flush(string str)
    {
        HUDOperation.Flush(str);
    }

    /// <summary>
    /// 回调在点击按钮时执行，而非点击3D物体时执行
    /// </summary>
    /// <param name="ua"></param>
    internal override void SetOnClick(Action<GameObject> ua)
    {
        HUDOperation.SetOnClick(ua, gameObject);
    }

    protected override void OnMouseDown()
    {
        if(HUDOperation.CanShow)
        {
            HUDManager.Instance.ShowHUDOperationOnly(HUDOperationName());
        }
    }

    /// <summary>
    /// 对应的HUD按钮名字
    /// </summary>
    /// <returns></returns>
    internal string HUDOperationName()
    {
        return HUDOperation.name;
    }
}