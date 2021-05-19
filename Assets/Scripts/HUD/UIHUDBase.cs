using UnityEngine;

public class UIHUDBase : MonoBehaviour
{
    /// <summary>
    /// 是否可以显示
    /// </summary>
    internal virtual bool CanShow { get; }
    //protected RectTransform _tranOffset = null;

    private RectTransform _tran = null;
    internal RectTransform Tran
    {
        get
        {
            if (_tran == null)
            {
                _tran = transform as RectTransform;
            }
            return _tran;
        }
    }

    private void Awake()
    {
        //_tranOffset = transform.Find("offset") as RectTransform;
    }

    //internal void SetOffset(Vector2 delta)
    //{
    //    _tranOffset.anchoredPosition = _tranOffset.anchoredPosition + delta;
    //}

    internal void ShowHide(bool isShow)
    {
        gameObject.SetActive(isShow);
    }
}