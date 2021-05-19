using Framework;
using System;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

/// <summary>
/// 第一人称管理器单例
/// </summary>
public class FPSManager : MonoSingleton<FPSManager>
{
    private FirstPersonController _fpsController = null;
    public FirstPersonController FPSController
    {
        get
        {
            if (_fpsController == null)
            {
                GameObject go = GameObject.Find("FPSController");
                if(go == null)
                {
                    throw new Exception($"### no GameObject named FPSController in scene.");
                }
                _fpsController = go.GetComponent<FirstPersonController>();
                if (_fpsController == null)
                {
                    throw new Exception($"### no component [FirstPersonController] on FPSController.");
                }
            }
            return _fpsController;
        }
    }
}