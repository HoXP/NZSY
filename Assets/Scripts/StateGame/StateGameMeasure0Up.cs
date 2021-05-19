using System.Collections;
using DG.Tweening;
using Framework;
using GoogoFSM;
using UnityEngine;

public class StateGameMeasure0Up : GState
{
    private string strVernierCaliper = "kachi";
    private string strStick1 = "pCylinder7";

    public override IEnumerator OnEnter()
    {
        UIManager.Instance.FlushHint("测量0°方向试件上段");
        MachineManager.Instance.SetOnClick(strStick1, "测量直径", OnStick1Click);
        return base.OnEnter();
    }
    public override IEnumerator OnEixt()
    {
        return base.OnEixt();
    }

    /// <summary>
    /// 点击试件按钮
    /// </summary>
    private void OnStick1Click(GameObject go)
    {
        MachineManager.Instance.SetOnClick(strStick1, string.Empty, null);
        PlayFlyUp();
    }

    private void PlayFlyUp()
    {
        Sequence s = DOTween.Sequence();
        GameObject vernierCaliper = MachineManager.Instance.GetGOByName(strVernierCaliper);
        GameObject stick1 = MachineManager.Instance.GetGOByName(strStick1);
        Waypoints waypointsVernierCaliper = MachineManager.Instance.GetWaypointsByName(strVernierCaliper);
        Waypoints waypointsStick1 = MachineManager.Instance.GetWaypointsByName(strStick1);
        Tween t1 = vernierCaliper.transform.DOPath(waypointsVernierCaliper.Positions, 2, PathType.Linear, PathMode.Full3D, 10, Color.red);
        Tween r1 = vernierCaliper.transform.DOLocalRotate(new Vector3(90, 0, 0), 2, RotateMode.Fast);
        Tween t2 = stick1.transform.DOPath(waypointsStick1.Positions, 2, PathType.Linear, PathMode.Full3D, 10, Color.red);
        Tween r2 = stick1.transform.DOLocalRotate(new Vector3(0, -90, 0), 2, RotateMode.Fast);
        s.Append(t1);
        s.Append(r1);
        s.Append(t2);
        s.Append(r2);
        s.OnComplete(() => { PrepareMessure(); });
    }

    private void PrepareMessure()
    {
        MachineManager.Instance.SetOnClick(strStick1, "上段", Measure0Up);
    }

    private void Measure0Up(GameObject obj)
    {
        MachineManager.Instance.SetOnClick(strStick1, string.Empty, null);
        CoroutineManager.Instance.StartCoroutine(ShowHUDData());
    }

    private IEnumerator ShowHUDData()
    {
        MachineManager.Instance.ShowHideHUDData(strStick1, true);
        MachineManager.Instance.SetHUDData(strStick1, $"9.6mm");
        yield return new WaitForSeconds(Const.HUDDataWaitSeconds);
        MachineManager.Instance.ShowHideHUDData(strStick1, false);
        GStateMachineManager.Instance.ChangeState(FsmId, StateGame.Measure0Middle);
    }
}