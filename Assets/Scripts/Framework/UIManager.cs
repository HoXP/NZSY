using Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    private Camera _uiCamera = null;
    internal Camera UICamera
    {
        get
        {
            if (_uiCamera == null)
            {
                GameObject go = GameObject.Find("UICamera");
                if (go == null)
                {
                    throw new Exception("can not find UICamera.");
                }
                _uiCamera = go.transform.GetComponent<Camera>();
            }
            return _uiCamera;
        }
    }

    private GameObject _goUIRoot = null;
    internal GameObject goUIRoot
    {
        get
        {
            if (_goUIRoot == null)
            {
                _goUIRoot = GameObject.Find("UIRoot");
            }
            return _goUIRoot;
        }
    }
    private GameObject _goUIRootMachine = null;
    internal GameObject goUIRootMachine
    {
        get
        {
            if (_goUIRootMachine == null)
            {
                _goUIRootMachine = GameObject.Find("Machine/offset/uiOffset/UIRoot");
            }
            return _goUIRootMachine;
        }
    }

    private Dictionary<string, UIBase> _dictUI = null;

    protected override void OnInit()
    {
        _dictUI = new Dictionary<string, UIBase>();
        UIBase[] uiBases = goUIRoot.GetComponentsInChildren<UIBase>();
        if (uiBases == null)
        {
            throw new Exception($"### no UIBase under UIRoot.");
        }
        UIBase[] uiBasesMachine = goUIRootMachine.GetComponentsInChildren<UIBase>();
        if (uiBasesMachine == null)
        {
            throw new Exception($"### no UIBase under UIRootMachine.");
        }
        for (int i = 0; i < uiBases.Length; i++)
        {
            UIBase ui = uiBases[i];
            _dictUI.Add(ui.name, ui);
        }
        for (int i = 0; i < uiBasesMachine.Length; i++)
        {
            UIBase ui = uiBasesMachine[i];
            _dictUI.Add(ui.name, ui);
        }
    }

    public override void OnUpdate(float deltaTime)
    {
    }

    internal void Show(string uiName, params object[] ps)
    {
        if(_dictUI == null)
        {
            return;
        }
        if(_dictUI.ContainsKey(uiName))
        {
            _dictUI[uiName].Show(ps);
        }
    }
    internal void Hide(string uiName)
    {
        if (_dictUI == null)
        {
            return;
        }
        if (_dictUI.ContainsKey(uiName))
        {
            _dictUI[uiName].Hide();
        }
    }

    internal void FlushHint(string hint)
    {
        if (_dictUI == null)
        {
            return;
        }
        string s = "UIMain";
        if (_dictUI.ContainsKey(s))
        {
            UIBase ui = _dictUI[s];
            if (!(ui is UIMain))
            {
                throw new Exception($"### ui {ui.name} is not UIMain");
            }
            UIMain uiMain = ui as UIMain;
            uiMain.FlushHint(hint);
        }
    }
}