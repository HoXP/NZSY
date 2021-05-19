using System.Collections;
using Framework;
using GoogoFSM;

public class StateGameSelectSpeed : GState
{
    public override IEnumerator OnEnter()
    {
        UIManager.Instance.FlushHint("选择转速");
        ClientEventManager.Instance.PushEvent(EventId.OnRegisterBtnSpeedsCallback, false, 0, true);
        ClientEventManager.Instance.RegisterEvent(EventId.OnClickBtnRotate, StartRotate);
        return base.OnEnter();
    }

    public override IEnumerator OnEixt()
    {
        return base.OnEixt();
    }

    private void StartRotate(ClientEvent eve)
    {
        ClientEventManager.Instance.PushEvent(EventId.OnRegisterBtnSpeedsCallback, false, 0, false);
        ClientEventManager.Instance.UnRegisterEvent(EventId.OnClickBtnRotate, StartRotate);

        GStateMachineManager.Instance.ChangeState(FsmId, StateGame.Rotate);
    }
}