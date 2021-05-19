using System.Collections;
using DG.Tweening;
using GoogoFSM;
using UnityEngine;

public class StateGameRotate90 : GState
{
    private string strStick1 = "pCylinder7";

    public override IEnumerator OnEnter()
    {
        UIManager.Instance.FlushHint("试件旋转90°");
        MachineManager.Instance.SetOnClick(strStick1, "旋转90°", OnStick1Click);
        return base.OnEnter();
    }
    public override IEnumerator OnEixt()
    {
        return base.OnEixt();
    }
    
    internal void OnStick1Click(GameObject go)
    {
        MachineManager.Instance.SetOnClick(strStick1, string.Empty, null);
        GameObject stick1 = MachineManager.Instance.GetGOByName(strStick1);
        Vector3 toEuler = stick1.transform.localRotation.eulerAngles;
        toEuler.Set(-90, toEuler.y, 0);
        Tween r2 = stick1.transform.DOLocalRotate(toEuler, 2, RotateMode.Fast);
        r2.OnComplete(() => { GStateMachineManager.Instance.ChangeState(FsmId, StateGame.Measure90Up); });
    }
}