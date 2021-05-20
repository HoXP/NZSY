using Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MachineManager : MonoSingleton<MachineManager>
{
    private GameObject _goMachine = null;
    internal GameObject goMachine
    {
        get
        {
            if (_goMachine == null)
            {
                _goMachine = GameObject.Find("Machine");
            }
            return _goMachine;
        }
    }

    /// <summary>
    /// AirSwitch Switch Holder1 Holder2
    /// </summary>
    private Dictionary<string, ClickHandler> _dictClickHandler = null;
    /// <summary>
    /// 
    /// </summary>
    private Dictionary<string, DataTips> _dictDataTips = null;

    protected override void OnInit()
    {
        _dictClickHandler = new Dictionary<string, ClickHandler>();
        ClickHandler[] handlers = goMachine.GetComponentsInChildren<ClickHandler>();
        if (handlers == null)
        {
            throw new Exception($"### no ClickHandler under Machine.");
        }
        for (int i = 0; i < handlers.Length; i++)
        {
            ClickHandler handler = handlers[i];
            _dictClickHandler.Add(handler.name, handler);
        }

        _dictDataTips = new Dictionary<string, DataTips>();
        DataTips[] tips = goMachine.GetComponentsInChildren<DataTips>();
        if (tips == null || tips.Length <= 0)
        {
            throw new Exception($"### no DataTips under Machine.");
        }
        for (int i = 0; i < tips.Length; i++)
        {
            DataTips tip = tips[i];
            _dictDataTips.Add(tip.name, tip);
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
    }

    /// <summary>
    /// 为名字为name的物体加点击回调
    /// </summary>
    /// <param name="name"></param>
    /// <param name="ua"></param>
    internal void SetOnClick(string name, Action<GameObject> ua)
    {
        if (_dictClickHandler == null)
        {
            return;
        }
        if (_dictClickHandler.ContainsKey(name))
        {
            _dictClickHandler[name].SetOnClick(ua);
        }
    }
    /// <summary>
    /// 为名字为name的物体加带HUD按钮的点击回调
    /// </summary>
    /// <param name="name"></param>
    /// <param name="operation"></param>
    /// <param name="ua"></param>
    internal void SetOnClick(string name, string operation, Action<GameObject> ua)
    {
        if (_dictClickHandler == null)
        {
            return;
        }
        if (_dictClickHandler.ContainsKey(name))
        {
            ClickHandlerWithHudBtn hud = _dictClickHandler[name] as ClickHandlerWithHudBtn;
            if(hud == null)
            {
                throw new Exception($"### no ClickHandlerWithHudBtn on {name}.");
            }
            hud.Flush(operation);
            hud.SetOnClick(ua);
        }
    }

    private Dictionary<string, Collider> _dict3DGO = null;
    internal Dictionary<string, Collider> Dict3DGO
    {
        get
        {
            if (_dict3DGO == null)
            {
                Collider[] colliders = goMachine.GetComponentsInChildren<Collider>();
                if(colliders == null)
                {
                    throw new Exception($"### no Collider under Machine.");
                }
                _dict3DGO = new Dictionary<string, Collider>();
                for (int i = 0; i < colliders.Length; i++)
                {
                    Collider collider = colliders[i];
                    _dict3DGO.Add(collider.name, collider);
                }
            }
            return _dict3DGO;
        }
    }

    public object Tran { get; private set; }

    /// <summary>
    /// 根据名字获取go
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    internal GameObject GetGOByName(string name)
    {
        if (Dict3DGO != null && Dict3DGO.ContainsKey(name))
        {
            return Dict3DGO[name].gameObject;
        }
        throw new Exception($"### no Collider named {name}.");
    }

    /// <summary>
    /// 显隐名字为name的物体的HUD
    /// </summary>
    /// <param name="name"></param>
    /// <param name="isShow"></param>
    internal void ShowHideHUDData(string name, bool isShow)
    {
        if (_dictDataTips == null)
        {
            return;
        }
        if (_dictDataTips.ContainsKey(name))
        {
            DataTips hud = _dictDataTips[name] as DataTips;
            if (hud == null)
            {
                throw new Exception($"### no DataTips on {name}.");
            }
            hud.HUD.ShowHide(isShow);
        }
    }
    /// <summary>
    /// 显示名字为name的物体的HUD信息
    /// </summary>
    /// <param name="name"></param>
    /// <param name="operation"></param>
    /// <param name="ua"></param>
    internal void SetHUDData(string name, string data)
    {
        if (_dictDataTips == null)
        {
            return;
        }
        if (_dictDataTips.ContainsKey(name))
        {
            DataTips hud = _dictDataTips[name] as DataTips;
            if (hud == null)
            {
                throw new Exception($"### no DataTips on {name}.");
            }
            GameObject go = GetGOByName(name);
            hud.Flush(data);
        }
    }

    internal void RotateGameObject(GameObject go, Vector3 axis, float delta)
    {
        Quaternion q =  Quaternion.Euler(axis * delta) * go.transform.localRotation;
        go.transform.localRotation = q;
    }

    internal void ShowHideGameObject(string name, bool isShow)
    {
        GameObject go = GetGOByName(name);
        go.SetActive(isShow);
    }

    #region 转子
    private float _curSpeedOrigin = 0; //转速设定，°/min
    internal float CurSpeedOrigin
    {
        get
        {
            return _curSpeedOrigin;
        }
        set
        {
            _curSpeedOrigin = value;
            _delta = _curSpeedOrigin / (60 * Application.targetFrameRate); //计算每帧转速
        }
    }
    private float _delta = 0; //扭角每帧delta值
    private float _curTwistAngle = 0; //当前扭角
    private float _maxTwistAngle = 9; //最大扭角，达到这个最大扭角，工件就会断掉
    internal bool IsClockwise { get; set; } = false; //正转还是反转
    private Vector3 _axis = Vector3.right;
    private int _curIndex = 0;

    private float[] _dividingPoints = new float[] { 1, 3 }; //曲线分解点（直线、折线、曲线）
    private float _k = 5; //斜率
    private float _amplitude = 3; //折线振幅
    private float _p = 200; //焦准距
    private List<Vector2> _twistAngle2Torque = null; //曲线列表，x-扭角y扭矩
    internal List<Vector2> TwistAngle2Torque
    {
        get
        {
            if (_twistAngle2Torque == null)
            {
                _twistAngle2Torque = new List<Vector2>();
            }
            return _twistAngle2Torque;
        }
    }
    internal void InitRotor()
    {
        _curIndex = 0;
        CalcCurve(); //转之前先把曲线算好
        ClientEventManager.Instance.PushEvent(EventId.OnRotate, true, 0, 0, 0);
    }
    /// <summary>
    /// 计算扭矩-扭角曲线
    /// </summary>
    private void CalcCurve()
    {
        TwistAngle2Torque.Clear();
        float curTwistAngle = 0;
        float dividingPoints12 = _dividingPoints[0];
        float dividingPoints23 = _dividingPoints[1];
        float endValueOfLine = _k * dividingPoints12;
        while (curTwistAngle <= _maxTwistAngle)
        {
            float yValue = 0;
            //曲线公式
            if (0 <= curTwistAngle && curTwistAngle <= dividingPoints12)
            {//直线
                yValue = _k * curTwistAngle;
            }
            else if (dividingPoints12 < curTwistAngle && curTwistAngle <= dividingPoints23)
            {//折线
                yValue = UnityEngine.Random.Range(endValueOfLine - _amplitude, endValueOfLine + _amplitude);
            }
            else
            {//曲线 抛物线 [x^2 = 2py] => [y = sqrt(2px)]
                yValue = Mathf.Sqrt(2 * _p * (curTwistAngle - dividingPoints23)) + endValueOfLine;
            }
            TwistAngle2Torque.Add(new Vector2(curTwistAngle, yValue)); //扭角-扭矩对应值存储于List<Vector2>中
            curTwistAngle = curTwistAngle + _delta;
        }
    }
    internal void RotateRotor()
    {
        if (0 <= _curIndex && _curIndex < TwistAngle2Torque.Count)
        {
            Vector2 v2 = TwistAngle2Torque[_curIndex];
            float twistAngle = v2.x;
            float torque = v2.y;
            GameObject go = GetGOByName("rotor");
            RotateGameObject(go, _axis, IsClockwise ? _delta : -_delta);
            ClientEventManager.Instance.PushEvent(EventId.OnRotate, true, 0, twistAngle, torque, _curIndex);
            _curIndex++;
        }
        else
        {
            if (_curIndex >= TwistAngle2Torque.Count)
            {//断裂
                ClientEventManager.Instance.PushEvent(EventId.OnBreak);
                return;
            }
            Debug.LogError($"### _curIndex invalid {_curIndex}");
        }
    }
    #endregion

    #region Waypoints
    private GameObject _goWaypoints = null;
    internal GameObject goWaypoints
    {
        get
        {
            if (_goWaypoints == null)
            {
                _goWaypoints = GameObject.Find("Waypoints");
            }
            return _goWaypoints;
        }
    }
    /// <summary>
    /// 根据Waypoints结点的GameObject名获取Waypoints
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    internal Waypoints GetWaypointsByName(string name)
    {
        Transform t = goWaypoints.transform.Find($"Waypoint_{name}");
        if(t != null)
        {
            Waypoints w = t.GetComponent<Waypoints>();
            return w;
        }
        return null;
    }
    #endregion

    internal Vector3[] GetReverseArray(Vector3[] arr)
    {
        Vector3[] arrRet = arr.Clone() as Vector3[];
        Array.Reverse(arrRet);
        return arrRet;
    }
}