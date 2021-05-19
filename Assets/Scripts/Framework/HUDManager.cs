using Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HUD UI类型
/// </summary>
public enum HUDType
{
    Name, //移上去就显示的Tips
    Data, //弹出的显示一段时间的信息
    Operation //点击弹出的按钮
}
public class HUDManager : MonoSingleton<HUDManager>
{
    private GameObject _goHUDRoot = null;
    internal GameObject goHUDRoot
    {
        get
        {
            if(_goHUDRoot == null)
            {
                _goHUDRoot = GameObject.Find("HUDRoot");
            }
            return _goHUDRoot;
        }
    }

    private Canvas _hudCanvas = null;
    internal Canvas HUDCanvas
    {
        get
        {
            if (_hudCanvas == null)
            {
                Transform tran = goHUDRoot.transform.Find("HUDCanvas");
                if (tran == null)
                {
                    throw new Exception("no Canvas component on HUDCanvas game object.");
                }
                _hudCanvas = tran.GetComponent<Canvas>();
            }
            return _hudCanvas;
        }
    }

    private Dictionary<HUDType, UIHUDBase> dictHUDTemplate = null;
    private List<UIHUDBase> _goInitlizedList = null;

    protected override void OnInit()
    {
        dictHUDTemplate = new Dictionary<HUDType, UIHUDBase>();
        UIHUDBase hudName = Resources.Load<UIHUDBase>("Prefabs/HUD/UIHUDName");
        UIHUDBase hudData = Resources.Load<UIHUDBase>("Prefabs/HUD/UIHUDData");
        UIHUDBase hudOperation = Resources.Load<UIHUDBase>("Prefabs/HUD/UIHUDOperation");
        hudName.gameObject.SetActive(false);
        hudData.gameObject.SetActive(false);
        hudOperation.gameObject.SetActive(false);
        dictHUDTemplate.Add(HUDType.Name, hudName);
        dictHUDTemplate.Add(HUDType.Data, hudData);
        dictHUDTemplate.Add(HUDType.Operation, hudOperation);

        _goInitlizedList = new List<UIHUDBase>();
    }

    public override void OnUpdate(float deltaTime)
    {
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _goInitlizedList.Count; i++)
        {
            UIHUDBase go = _goInitlizedList[i];
            Destroy(go);
        }
        _goInitlizedList.Clear();
        _goInitlizedList = null;

        dictHUDTemplate.Clear();
        dictHUDTemplate = null;
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }

    /// <summary>
    /// 鼠标指针位置 -> 世界坐标
    /// </summary>
    internal Vector3 MousePos2WorldPos()
    {
        Vector2 posMouse = Input.mousePosition;
        Vector3 posWorld = Vector3.zero;
        RectTransform rt = HUDCanvas.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, posMouse, HUDCanvas.worldCamera, out posWorld);
        return posWorld;
    }
    /// <summary>
    /// 鼠标指针位置 -> 世界坐标
    /// </summary>
    internal Vector3 WorldPosToUIPos(GameObject go)
    {
        //var viewPos = HUDCanvas.worldCamera.WorldToViewportPoint(go.transform.position); //摄像机空间值域[0,1]，z轴值代表深度
        var viewPos = Camera.main.WorldToViewportPoint(go.transform.position); //摄像机空间值域[0,1]，z轴值代表深度
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1) //按照值域进行裁剪
        {
            float swidth = viewPos.x * Screen.width; //屏幕空间宽度值
            float sheight = viewPos.y * Screen.height; //屏幕空间高度值
            return new Vector2(swidth.GetFixed(HUDCanvas), sheight.GetFixed(HUDCanvas)); //适配转化
        }
        return -Vector2.one; //返回一个固定值-1代表不在屏幕当中
    }

    internal UIHUDBase InitializeHUD(HUDType hudType)
    {
        if(dictHUDTemplate == null)
        {
            return null;
        }
        UIHUDBase goTemplate = dictHUDTemplate[hudType];
        if (goTemplate == null)
        {
            return null;
        }
        UIHUDBase go = Instantiate(goTemplate);
        Transform tran = go.transform;
        tran.SetParent(HUDCanvas.transform);
        tran.localPosition = Vector3.zero;
        tran.localRotation = Quaternion.identity;
        tran.localScale = Vector3.one;
        _goInitlizedList.Add(go);
        return go;
    }

    /// <summary>
    /// 仅显示一个HUD按钮，隐藏其他
    /// </summary>
    /// <param name="name">HUD按钮名</param>
    internal void ShowHUDOperationOnly(string name)
    {
        for (int i = 0; i < _goInitlizedList.Count; i++)
        {
            UIHUDBase hud = _goInitlizedList[i];
            if (hud is UIHUDOperation)
            {
                UIHUDOperation tmp = hud as UIHUDOperation;
                if (tmp == null)
                {
                    throw new Exception($"### {name} is not UIHUDOperation.");
                }
                tmp.ShowHide(tmp.name == name);
            }
        }
    }
}