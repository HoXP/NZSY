using UnityEngine;
using UnityEngine.UI;

public static class StaticExt
{
    //Screen坐标值适配Canvas画布
    public static float GetFixed(this float value, Canvas canvas)
    {
        var cs = canvas.GetComponent<CanvasScaler>();
        if (cs.matchWidthOrHeight == 0)
        {
            return value * cs.referenceResolution.x / Screen.width; //匹配宽度时仅按照宽度计算
        }
        else
        {
            return value * cs.referenceResolution.y / Screen.height; //匹配高度时仅按照高度计算
        }
    }
}