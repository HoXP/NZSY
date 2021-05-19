using Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 对话框UI类
/// </summary>
public class UIMachine : UIBase
{
    public Text _txtTorque = null; //扭矩
    public Text _txtTorsionalAngle = null; //扭角
    public Text _txtDeformation = null; //变形
    public Text _txtTwistAngle = null; //转角
    public Text _txtSpeed = null; //转速

    public Button _btnClearForce = null;
    public Button _btnClearTorsionalAngle = null;
    public Button _btnClearTwistAngle = null;

    public Text _txtSpeedOrigin = null;
    private float[] _speeds = new float[] { 0.036f, 0.36f, 3.6f, 30, 36, 45, 60, 90, 180, 360, 720, 1080 }; //转速列表，长度与_btnSpeeds一致
    public Button[] _btnSpeeds = null;

    public Button _btnCW = null;
    public Button _btnStop = null;
    public Button _btnCCW = null;

    public Button _btnSetting = null;
    public Button _btnAbout = null;

    private string zero = "0";

    protected override void OnEnable()
    {
        base.OnEnable();
        RegisterBtnCallback(true);
        StartCoroutine(FlushDateTime());
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        RegisterBtnCallback(false);
        StopCoroutine(FlushDateTime());
    }

    #region RegisterBtnCallback
    private void RegisterBtnCallback(bool isRegister)
    {
        if(isRegister)
        {
            _btnClearForce.onClick.AddListener(OnClickBtnClearForce);
            _btnClearTorsionalAngle.onClick.AddListener(OnClickBtnClearTorsionalAngle);
            _btnClearTwistAngle.onClick.AddListener(OnClickBtnClearTwistAngle);
            _btnCW.onClick.AddListener(OnClickBtnCW);
            _btnStop.onClick.AddListener(OnClickBtnStop);
            _btnCCW.onClick.AddListener(OnClickBtnCCW);
            _btnSetting.onClick.AddListener(OnClickBtnSetting);
            _btnAbout.onClick.AddListener(OnClickBtnAbout);
        }
        else
        {
            _btnClearForce.onClick.RemoveAllListeners();
            _btnClearTorsionalAngle.onClick.RemoveAllListeners();
            _btnClearTwistAngle.onClick.RemoveAllListeners();
            _btnCW.onClick.RemoveAllListeners();
            _btnStop.onClick.RemoveAllListeners();
            _btnCCW.onClick.RemoveAllListeners();
            _btnSetting.onClick.RemoveAllListeners();
            _btnAbout.onClick.RemoveAllListeners();
        }
    }
    private void RegisterBtnSpeedsCallback(ClientEvent eve)
    {
        bool isRegister = eve.GetParameter<bool>(0);
        RegisterBtnSpeedsCallback(isRegister);
    }
    private void RegisterBtnSpeedsCallback(bool isRegister)
    {
        if (isRegister)
        {
            for (int i = 0; i < _btnSpeeds.Length; i++)
            {
                int index = i;
                _btnSpeeds[i].onClick.AddListener(() => { OnClickBtnSpeed(index); });
            }
        }
        else
        {
            for (int i = 0; i < _btnSpeeds.Length; i++)
            {
                _btnSpeeds[i].onClick.RemoveAllListeners();
            }
        }
    }
    #endregion

    #region Event
    protected override void OnRegisterEvent()
    {
        ClientEventManager.Instance.RegisterEvent(EventId.OnInitUIMachine, Init);
        ClientEventManager.Instance.RegisterEvent(EventId.OnRegisterBtnSpeedsCallback, RegisterBtnSpeedsCallback);
        ClientEventManager.Instance.RegisterEvent(EventId.OnRotate, OnRotate);
    }
    protected override void OnUnRegisterEvent()
    {
        ClientEventManager.Instance.UnRegisterEvent(EventId.OnInitUIMachine, Init);
        ClientEventManager.Instance.UnRegisterEvent(EventId.OnRegisterBtnSpeedsCallback, RegisterBtnSpeedsCallback);
        ClientEventManager.Instance.UnRegisterEvent(EventId.OnRotate, OnRotate);
    }
    #endregion

    #region _txtDateTime
    public Text _txtDateTime = null;
    private IEnumerator FlushDateTime()
    {
        _txtDateTime.text = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
        yield return new WaitForSeconds(1);
    }
    #endregion

    private void Init(ClientEvent eve)
    {
        FlushTorqueTorsionalAngle(0, 0);
        _txtDeformation.text = zero;
        OnClickBtnClearTwistAngle();
        _txtSpeed.text = zero;

        _txtSpeedOrigin.text = zero;
    }
    private void OnRotate(ClientEvent eve)
    {
        float twistAngle = eve.GetParameter<float>(0);
        float torque = eve.GetParameter<float>(1);
        FlushTorqueTorsionalAngle(twistAngle, torque);
    }
    /// <summary>
    /// 刷新扭矩扭角
    /// </summary>
    /// <param name="torque"></param>
    /// <param name="torsionalAngle"></param>
    private void FlushTorqueTorsionalAngle(float twistAngle, float torque)
    {
        //_txtTorsionalAngle.text = twistAngle.ToString();
        _txtTwistAngle.text = twistAngle.ToString();
        _txtTorque.text = torque.ToString();
    }

    #region OnClick
    private void OnClickBtnClearForce()
    {
        _txtTorque.text = zero;
    }
    private void OnClickBtnClearTorsionalAngle()
    {
        _txtTorsionalAngle.text = zero;
    }
    private void OnClickBtnClearTwistAngle()
    {
        _txtTwistAngle.text = zero;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="i">_btnSpeeds索引</param>
    private void OnClickBtnSpeed(int i)
    {
        MachineManager.Instance.CurSpeedOrigin = _speeds[i];
        _txtSpeedOrigin.text = $"{MachineManager.Instance.CurSpeedOrigin}";
    }
    private void OnClickBtnCW()
    {
        if (MachineManager.Instance.CurSpeedOrigin <= 0)
        {
            return;
        }
        MachineManager.Instance.IsClockwise = true;
        ClientEventManager.Instance.PushEvent(EventId.OnClickBtnRotate);
    }
    private void OnClickBtnStop()
    {
    }
    private void OnClickBtnCCW()
    {
        if (MachineManager.Instance.CurSpeedOrigin <= 0)
        {
            return;
        }
        MachineManager.Instance.IsClockwise = false;
        ClientEventManager.Instance.PushEvent(EventId.OnClickBtnRotate);
    }
    private void OnClickBtnSetting()
    {
    }
    private void OnClickBtnAbout()
    {
    }
    #endregion
}