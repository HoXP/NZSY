using System.Collections;
using Framework;
using GoogoFSM;
using UnityEngine;

public class StateGameMeasure0Down : GState
{
    private string strStick1 = "pCylinder7";

    public override IEnumerator OnEnter()
    {
        UIManager.Instance.FlushHint("测量0°方向试件下段");
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
        GStateMachineManager.Instance.ChangeState(FsmId, StateGame.Rotate90);
    }
}