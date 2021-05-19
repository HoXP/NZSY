using UnityEngine.UI;

public class UIHUDData : UIHUDBase
{
    public Text _txt = null;

    private void OnEnable()
    {
        Tran.position = HUDManager.Instance.MousePos2WorldPos();
    }

    internal void Flush(string str)
    {
        _txt.text = str;
    }
}