using System.Collections;
using Framework;
using GoogoFSM;
using UnityEngine;

public class StateGameSwitch : GState
{
    private string strSwitch = "polySurface3";

    public override IEnumerator OnEnter()
    {
        UIManager.Instance.FlushHint("打开试验机电源开关");
        MachineManager.Instance.SetOnClick(strSwitch, OnClick);
        return base.OnEnter();
    }
    public override IEnumerator OnEixt()
    {
        return base.OnEixt();
    }
    
    internal void OnClick(GameObject go)
    {
        MachineManager.Instance.SetOnClick(strSwitch, null);

        UIManager.Instance.Show("UIMachine");
        ClientEventManager.Instance.PushEvent(EventId.OnInitUIMachine);
        GStateMachineManager.Instance.ChangeState(FsmId, StateGame.Measure0Up);
    }
}