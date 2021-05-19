using System.Collections;
using DG.Tweening;
using GoogoFSM;
using UnityEngine;

public class StateGameSwitchClose : GState
{
    private string strKey = "polySurface1";
    private string strSwitch = "polySurface3";

    public override IEnumerator OnEnter()
    {
        UIManager.Instance.FlushHint("关闭开关");
        MachineManager.Instance.SetOnClick(strSwitch, OnSwitchClick);
        return base.OnEnter();
    }
    public override IEnumerator OnEixt()
    {
        return base.OnEixt();
    }
    
    internal void OnSwitchClick(GameObject go)
    {
        MachineManager.Instance.SetOnClick(strSwitch, null);
        UIManager.Instance.Hide("UIChart");
        UIManager.Instance.Hide("UIMachine");

        GameObject goKey = MachineManager.Instance.GetGOByName(strKey);
        goKey.transform.parent.DOLocalRotate(new Vector3(goKey.transform.parent.localEulerAngles.x, 0, 0), 1).OnComplete(() =>
        {
            GStateMachineManager.Instance.ChangeState(FsmId, StateGame.Exit);
        });
    }
}