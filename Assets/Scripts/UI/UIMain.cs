using Framework;
using UnityEngine.UI;

public class UIMain : UIBase
{
    public Button _btnIntro = null;
    public Button _btnRecord = null;
    public Button _btnChart = null;
    public Button _btnAttention = null;
    public Button _btnExit = null;
    public Text _txtHint = null;
    public Text _txtTwistAngle = null;
    public Text _txtTorque = null;

    protected override void OnShow(params object[] ps)
    {
        base.OnShow(ps);
        FlushTwistAngleTorque(0, 0);
    }

    protected override void OnInitComponent()
    {
        base.OnInitComponent();
        _btnChart.onClick.AddListener(() => {
            UIManager.Instance.Show("UIChart");
        });
        _btnAttention.onClick.AddListener(OpenAttention);
    }
    protected override void OnResetComponent()
    {
        base.OnResetComponent();
        _btnChart.onClick.RemoveAllListeners();
        _btnAttention.onClick.RemoveAllListeners();
    }

    protected override void OnRegisterEvent()
    {
        ClientEventManager.Instance.RegisterEvent(EventId.OnRotate, OnRotate);
    }
    protected override void OnUnRegisterEvent()
    {
        ClientEventManager.Instance.UnRegisterEvent(EventId.OnRotate, OnRotate);
    }

    internal void FlushHint(string hint)
    {
        _txtHint.text = $"{hint}";
    }

    private void OnRotate(ClientEvent eve)
    {
        float twistAngle = eve.GetParameter<float>(0);
        float torque = eve.GetParameter<float>(1);
        FlushTwistAngleTorque(twistAngle, torque);
    }
    internal void FlushTwistAngleTorque(float twistAngle, float torque)
    {
        _txtTwistAngle.text = $"{twistAngle}";
        _txtTorque.text = $"{torque}";
    }

    private void OpenAttention()
    {
        UIManager.Instance.Show("UIAttention");
    }
}