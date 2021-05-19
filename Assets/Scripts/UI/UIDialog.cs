using UnityEngine.UI;

/// <summary>
/// 对话框UI类
/// </summary>
public class UIDialog : UIBase
{
    public Button _btnClose = null;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        _btnClose.onClick.AddListener(Hide);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _btnClose.onClick.RemoveAllListeners();
    }
}