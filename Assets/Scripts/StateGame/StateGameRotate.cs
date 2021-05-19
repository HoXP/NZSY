using System.Collections;
using Framework;
using GoogoFSM;

public class StateGameRotate : GState
{
    private bool _isRotate = false;

    public override IEnumerator OnEnter()
    {
        UIManager.Instance.FlushHint("转动");
        ClientEventManager.Instance.RegisterEvent(EventId.OnBreak, BreakCallback);
        MachineManager.Instance.InitRotor();
        UIManager.Instance.Show("UIChart");
        _isRotate = true;
        CoroutineManager.Instance.StartCoroutine(RotateStick());
        return base.OnEnter();
    }
    public override IEnumerator OnEixt()
    {
        CoroutineManager.Instance.StopCoroutine(RotateStick());
        return base.OnEixt();
    }

    private IEnumerator RotateStick()
    {
        while (_isRotate)
        {
            MachineManager.Instance.RotateRotor();
            yield return null; //等一帧
        }
    }

    private void BreakCallback(ClientEvent eve)
    {
        _isRotate = false;
        GStateMachineManager.Instance.ChangeState(FsmId, StateGame.StickDown);
    }
}