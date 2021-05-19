using System;
using UnityEngine;

[DisallowMultipleComponent]
public class DataTips : MonoBehaviour
{
    private UIHUDData _hud = null;
    internal UIHUDData HUD
    {
        get
        {
            if(_hud == null)
            {
                UIHUDBase hud = HUDManager.Instance.InitializeHUD(HUDType.Data);
                if (hud == null)
                {
                    throw new Exception($"### HUDManager.Instance.InitializeHUD HUDType.Name failed.");
                }
                hud.name = $"HUDData{gameObject.name}";
                _hud = hud as UIHUDData;
            }
            return _hud;
        }
    }

    internal void Flush(string data)
    {
        HUD.Flush(data);
    }
}