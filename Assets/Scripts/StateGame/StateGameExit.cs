using System.Collections;
using GoogoFSM;

public class StateGameExit : GState
{
    public override IEnumerator OnEnter()
    {
        GStateMachineManager.Instance.ChangeState(FsmId, StateGame.Init);
        return base.OnEnter();
    }
    public override IEnumerator OnEixt()
    {
        return base.OnEixt();
    }
}