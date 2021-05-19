using UnityEngine.UI;

public class UIHUDName : UIHUDBase
{
    public Text _txtName = null;
    
    internal void Flush(string str)
    {
        _txtName.text = str;
    }

    private void Update()
    {
        Tran.position = HUDManager.Instance.MousePos2WorldPos();
    }
}