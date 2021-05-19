using Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChart : UIDialog
{
    public Text _txtTwistAngle = null;
    public Text _txtTorque = null;

    public WMG_Axis_Graph _graph = null;
    public WMG_Series _series = null;

    protected override void OnShow(params object[] ps)
    {
        List<Vector2> list = MachineManager.Instance.TwistAngle2Torque;
        if(list != null && list.Count > 0)
        {
            int index = list.Count - 1;
            float twistAngle = list[index].x;
            float torque = list[index].y;
            FlushTwistAngle(twistAngle, torque, index);
        }
        else
        {
            FlushTwistAngle(0, 0, 0);
        }
    }
    protected override void OnHide()
    {
    }

    protected override void OnRegisterEvent()
    {
        ClientEventManager.Instance.RegisterEvent(EventId.OnRotate, OnRotate);
    }
    protected override void OnUnRegisterEvent()
    {
        ClientEventManager.Instance.UnRegisterEvent(EventId.OnRotate, OnRotate);
    }

    private void OnRotate(ClientEvent eve)
    {
        float twistAngle = eve.GetParameter<float>(0);
        float torque = eve.GetParameter<float>(1);
        int index = eve.GetParameter<int>(2);
        FlushTwistAngle(twistAngle, torque, index);
    }
    /// <summary>
    /// 更新扭角
    /// </summary>
    private void FlushTwistAngle(float twistAngle, float torque, int index)
    {
        #region 求X轴最大值
        FlushXAxisMaxValue(twistAngle); //X轴最大值就是当前扭角值
        #endregion

        List<Vector2> list = MachineManager.Instance.TwistAngle2Torque;

        #region 求Y轴最大值
        float max = 0;
        for (int i = 0; i < index; i++)
        {
            if (max < list[i].y)
            {
                max = list[i].y;
            }
        }
        FlushYAxisMaxValue(max);
        #endregion

        #region 设置 DataSource
        List<Vector2> tmpV2 = new List<Vector2>();
        for (int i = 0; i < index; i++)
        {
            tmpV2.Add(list[i]);
        }
        FlushSerials(tmpV2.ToArray());
        #endregion

        _txtTwistAngle.text = $"{twistAngle}";
        _txtTorque.text = $"{torque}";
    }
    
    /// <summary>
    /// 更新X轴最大值
    /// </summary>
    /// <param name="max"></param>
    private void FlushXAxisMaxValue(float max)
    {
        _graph.xAxis.AxisMaxValue = max;
    }
    /// <summary>
    /// 更新Y轴最大值
    /// </summary>
    /// <param name="max"></param>
    private void FlushYAxisMaxValue(float max)
    {
        _graph.yAxis.AxisMaxValue = max;
    }
    /// <summary>
    /// 更新DataSource
    /// </summary>
    /// <param name="vs"></param>
    private void FlushSerials(Vector2[] vs)
    {
        _series.pointValues.SetList(vs);
    }
}