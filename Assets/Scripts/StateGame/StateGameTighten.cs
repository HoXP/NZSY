using System;
using System.Collections;
using DG.Tweening;
using Framework;
using GoogoFSM;
using UnityEngine;

public class StateGameTighten : GState
{
    private string strBanZi = "banzi";
    private Vector3 v3BanZi = new Vector3(0, -90, -33);
    private Vector3 v3AxisBanZi = Vector3.zero;

    public override IEnumerator OnEnter()
    {
        v3AxisBanZi = Quaternion.Euler(v3BanZi) * Vector3.up;
        UIManager.Instance.FlushHint("紧固工件");
        MachineManager.Instance.SetOnClick(strBanZi, "紧固工件", OnBanZiClick);
        yield break;
    }
    public override IEnumerator OnEixt()
    {
        yield break;
    }

    private void OnBanZiClick(GameObject go)
    {
        MachineManager.Instance.SetOnClick(strBanZi, string.Empty, null);

        Sequence s = DOTween.Sequence();
        GameObject goBanZi = MachineManager.Instance.GetGOByName(strBanZi);
        Waypoints waypointsBanZi = MachineManager.Instance.GetWaypointsByName(strBanZi);
        Tween r1 = goBanZi.transform.DOLocalRotate(v3BanZi, 2, RotateMode.Fast);
        Tween t1 = goBanZi.transform.DOPath(waypointsBanZi.Positions, 2, PathType.Linear, PathMode.Full3D, 10, Color.red);
        s.Append(r1);
        s.Append(t1);
        s.OnComplete(() => { CoroutineManager.Instance.StartCoroutine(DoRotateAxis(v3AxisBanZi, 2, Tight2)); });
    }

    private IEnumerator DoRotateAxis(Vector3 up, float duration, Action callback)
    {
        float f = duration;
        while (f > 0)
        {
            GameObject goBanZi = MachineManager.Instance.GetGOByName(strBanZi);
            MachineManager.Instance.RotateGameObject(goBanZi, up, 1);
            f = f - Time.deltaTime;
            yield return null;
        }
        callback?.Invoke();
        yield break;
    }

    private void Tight2()
    {
        Sequence s = DOTween.Sequence();
        GameObject goBanZi = MachineManager.Instance.GetGOByName(strBanZi);
        Waypoints waypointsBanZi = MachineManager.Instance.GetWaypointsByName(string.Format($"{strBanZi}2"));
        Tween t1 = goBanZi.transform.DOPath(waypointsBanZi.Positions, 2, PathType.Linear, PathMode.Full3D, 10, Color.red);
        s.Append(t1);
        s.OnComplete(() => { CoroutineManager.Instance.StartCoroutine(DoRotateAxis(v3AxisBanZi, 2, FlayBack)); });
    }

    private void FlayBack()
    {
        Sequence s = DOTween.Sequence();
        GameObject goBanZi = MachineManager.Instance.GetGOByName(strBanZi);
        Waypoints waypointsBanZi = MachineManager.Instance.GetWaypointsByName(string.Format($"{strBanZi}3"));
        Tween t1 = goBanZi.transform.DOPath(waypointsBanZi.Positions, 2, PathType.Linear, PathMode.Full3D, 10, Color.red);
        Tween r1 = goBanZi.transform.DOLocalRotate(new Vector3(0, -90, 90), 2, RotateMode.Fast);
        s.Append(t1);
        s.Append(r1);
        s.OnComplete(() =>
        {
            GStateMachineManager.Instance.ChangeState(FsmId, StateGame.SelectSpeed);
        });
    }
}