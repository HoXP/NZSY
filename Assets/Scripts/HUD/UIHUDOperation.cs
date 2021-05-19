using System;
using UnityEngine;
using UnityEngine.UI;

public class UIHUDOperation : UIHUDBase
{
    public Button _btn = null;
    public Text _txt = null;

    internal override bool CanShow
    {
        get
        {
            return !string.IsNullOrEmpty(_txt.text);
        }
    }

    private void OnEnable()
    {
        Tran.position = HUDManager.Instance.MousePos2WorldPos();
    }

    internal void Flush(string str)
    {
        _txt.text = str;
    }
    /// <summary>
    /// 点击HUD按钮回调
    /// </summary>
    /// <param name="func"></param>
    /// <param name="go">传所点击的3D物体</param>
    internal void SetOnClick(Action<GameObject> func, GameObject go)
    {
        _btn.onClick.RemoveAllListeners();
        if(func == null)
        {
            return;
        }
        _btn.onClick.AddListener(() => {
            ShowHide(false);
            func(go);
        }); //使Button的点击回调可以带参数
    }
}