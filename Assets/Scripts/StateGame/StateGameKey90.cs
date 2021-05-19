using System.Collections;
using DG.Tweening;
using Framework;
using GoogoFSM;
using UnityEngine;

public class StateGameKey90 : GState
{
    private string strKey = "polySurface1";

    public override IEnumerator OnEnter()
    {
        UIManager.Instance.FlushHint("点击钥匙");
        MachineManager.Instance.SetOnClick(strKey, OnClick);
        return base.OnEnter();
    }
    public override IEnumerator OnEixt()
    {
        return base.OnEixt();
    }
    
    internal void OnClick(GameObject go)
    {
        MachineManager.Instance.SetOnClick(strKey, null);

        GameObject goKey = MachineManager.Instance.GetGOByName(strKey);
        goKey.transform.parent.DOLocalRotate(new Vector3(goKey.transform.parent.localEulerAngles.x, 0, 90), 1).OnComplete(() =>
        {
            GStateMachineManager.Instance.ChangeState(FsmId, StateGame.Switch);
        });
    }
}