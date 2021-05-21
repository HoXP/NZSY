using System.Collections;
using DG.Tweening;
using GoogoFSM;
using UnityEngine;

public class StateGameInit : GState
{
    private string strStick1 = "pCylinder7";
    private string strBreakStick1 = "pCylinder1";
    private string strBreakStick2 = "pCylinder2";
    private string strTightStick = "banzi";

    public override IEnumerator OnEnter()
    {
        UIManager.Instance.Show("UIMain");
        UIManager.Instance.Hide("UIChart");
        UIManager.Instance.Hide("UIAttention");
        UIManager.Instance.Hide("UIMachine");

        MachineManager.Instance.ShowHideGameObject(strStick1, true);
        GameObject stick1 = MachineManager.Instance.GetGOByName(strStick1);
        Waypoints waypointsStick1 = MachineManager.Instance.GetWaypointsByName($"{strStick1}_up");
        stick1.transform.DOPath(MachineManager.Instance.GetReverseArray(waypointsStick1.Positions), 0, PathType.Linear, PathMode.Full3D, 10, Color.red);
        MachineManager.Instance.ShowHideGameObject(strBreakStick1, false);
        MachineManager.Instance.ShowHideGameObject(strBreakStick2, false);

        MachineManager.Instance.SetOnClick(strStick1, string.Empty, null);
        MachineManager.Instance.SetOnClick(strTightStick, string.Empty, null);

        GStateMachineManager.Instance.ChangeState(FsmId, StateGame.Key90);
        yield break;
    }
    public override IEnumerator OnEixt()
    {
        yield break;
    }
}