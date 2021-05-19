using System.Collections;
using DG.Tweening;
using GoogoFSM;
using UnityEngine;

public class StateGameStickDown : GState
{
    private string strCar = "car";
    private string strBreakStick1 = "pCylinder1";
    private string strBreakStick2 = "pCylinder2";
    private string strStick1 = "pCylinder7";

    public override IEnumerator OnEnter()
    {
        UIManager.Instance.FlushHint("取下低碳钢试件");
        MachineManager.Instance.ShowHideGameObject(strBreakStick1, true);
        MachineManager.Instance.ShowHideGameObject(strBreakStick2, true);
        MachineManager.Instance.ShowHideGameObject(strStick1, false);
        MachineManager.Instance.SetOnClick(strBreakStick1, "取下试件", OnStick1Click);
        MachineManager.Instance.SetOnClick(strBreakStick2, "取下试件", OnStick1Click);
        return base.OnEnter();
    }
    public override IEnumerator OnEixt()
    {
        return base.OnEixt();
    }

    internal void OnStick1Click(GameObject go)
    {
        MachineManager.Instance.SetOnClick(strBreakStick1, string.Empty, null);
        MachineManager.Instance.SetOnClick(strBreakStick2, string.Empty, null);

        Sequence s = DOTween.Sequence();
        GameObject car = MachineManager.Instance.GetGOByName(strCar);
        GameObject breakStick1 = MachineManager.Instance.GetGOByName(strBreakStick1);
        GameObject breakStick2 = MachineManager.Instance.GetGOByName(strBreakStick2);
        Waypoints waypointsCar = MachineManager.Instance.GetWaypointsByName(strCar);
        Waypoints waypointsStick1 = MachineManager.Instance.GetWaypointsByName(strBreakStick1);
        Waypoints waypointsStick2 = MachineManager.Instance.GetWaypointsByName(strBreakStick2);
        Tween t1 = car.transform.DOPath(waypointsCar.Positions, 1, PathType.Linear, PathMode.Full3D, 10, Color.red);
        Tween t2 = breakStick1.transform.DOPath(waypointsStick1.Positions, 2, PathType.Linear, PathMode.Full3D, 10, Color.red);
        Tween t3 = breakStick2.transform.DOPath(waypointsStick2.Positions, 2, PathType.Linear, PathMode.Full3D, 10, Color.red);
        s.Append(t1);
        s.Append(t2);
        s.Append(t3);
        s.OnComplete(() => { GStateMachineManager.Instance.ChangeState(FsmId, StateGame.SwitchClose); });
    }
}