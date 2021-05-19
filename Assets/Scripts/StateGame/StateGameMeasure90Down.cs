using System;
using System.Collections;
using DG.Tweening;
using Framework;
using GoogoFSM;
using UnityEngine;

public class StateGameMeasure90Down : GState
{
    private string strVernierCaliper = "kachi";
    private string strStick1 = "pCylinder7";

    public override IEnumerator OnEnter()
    {
        UIManager.Instance.FlushHint("测量90°方向试件下段");
        MachineManager.Instance.SetOnClick(strStick1, "下段", OnStick1Click);
        return base.OnEnter();
    }
    public override IEnumerator OnEixt()
    {
        return base.OnEixt();
    }

    internal void OnStick1Click(GameObject go)
    {
        MachineManager.Instance.SetOnClick(strStick1, string.Empty, null);
        CoroutineManager.Instance.StartCoroutine(ShowHUDData());
    }

    private IEnumerator ShowHUDData()
    {
        MachineManager.Instance.ShowHideHUDData(strStick1, true);
        MachineManager.Instance.SetHUDData(strStick1, $"10.4mm");
        yield return new WaitForSeconds(Const.HUDDataWaitSeconds);
        MachineManager.Instance.ShowHideHUDData(strStick1, false);
        PlayFlayBack();
    }

    private void PlayFlayBack()
    {
        Sequence s = DOTween.Sequence();
        GameObject vernierCaliper = MachineManager.Instance.GetGOByName(strVernierCaliper);
        GameObject stick1 = MachineManager.Instance.GetGOByName(strStick1);
        Waypoints waypointsVernierCaliper = MachineManager.Instance.GetWaypointsByName(strVernierCaliper);
        Waypoints waypointsStick1 = MachineManager.Instance.GetWaypointsByName($"{strStick1}");
        Tween t1 = vernierCaliper.transform.DOPath(MachineManager.Instance.GetReverseArray(waypointsVernierCaliper.Positions), 2, PathType.Linear, PathMode.Full3D, 10, Color.red);
        Tween r1 = vernierCaliper.transform.DOLocalRotate(new Vector3(0, 0, 0), 2, RotateMode.Fast);
        Tween t2 = stick1.transform.DOPath(MachineManager.Instance.GetReverseArray(waypointsStick1.Positions), 2, PathType.Linear, PathMode.Full3D, 10, Color.red);
        Tween r2 = stick1.transform.DOLocalRotate(new Vector3(0, 0, 0), 2, RotateMode.Fast);
        s.Append(r2);
        s.Append(t2);
        s.Append(r1);
        s.Append(t1);
        s.OnComplete(() => { GStateMachineManager.Instance.ChangeState(FsmId, StateGame.StickUp); });
    }
}