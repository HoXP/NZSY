using System.Collections;
using DG.Tweening;
using GoogoFSM;
using UnityEngine;

public class StateGameStickUp : GState
{
    private string strCar = "car";
    private string strStick1 = "pCylinder7";

    public override IEnumerator OnEnter()
    {
        UIManager.Instance.FlushHint("安装低碳钢试件");
        MachineManager.Instance.SetOnClick(strStick1, "安装试件", OnStick1Click);
        return base.OnEnter();
    }
    public override IEnumerator OnEixt()
    {
        return base.OnEixt();
    }

    internal void OnStick1Click(GameObject go)
    {
        MachineManager.Instance.SetOnClick(strStick1, string.Empty, null);

        GameObject car = MachineManager.Instance.GetGOByName(strCar);
        GameObject stick1 = MachineManager.Instance.GetGOByName(strStick1);

        Sequence s = DOTween.Sequence();
        Waypoints waypointsCar = MachineManager.Instance.GetWaypointsByName(strCar);
        Waypoints waypointsStick1 = MachineManager.Instance.GetWaypointsByName($"{strStick1}_up");
        Tween t1 = car.transform.DOPath(waypointsCar.Positions, 1, PathType.Linear, PathMode.Full3D, 10, Color.red);
        Tween t2 = stick1.transform.DOPath(waypointsStick1.Positions, 2, PathType.Linear, PathMode.Full3D, 10, Color.red);
        Tween t3 = car.transform.DOPath(MachineManager.Instance.GetReverseArray(waypointsCar.Positions), 1, PathType.Linear, PathMode.Full3D, 10, Color.red);
        s.Append(t1);
        s.Append(t2);
        s.Append(t3);
        s.OnComplete(() => { GStateMachineManager.Instance.ChangeState(FsmId, StateGame.Tighten); });
    }
}