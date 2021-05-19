using GoogoFSM;
using UnityEngine;

public class Game : MonoBehaviour
{
    private GStateMachine fsmGame = null;

    private void Awake()
    {
        fsmGame = GStateMachineManager.Instance.CreateStateMachine<StateGame>();
        GStateMachineManager.Instance.Init(fsmGame.Id, StateGame.Init);

        Application.targetFrameRate = 60; //锁定帧率60
    }
}

public enum StateGame : int
{
    Init, //游戏初始化
    Key90, //开启（钥匙顺时针90°）
    Switch, //开关

    Measure0Up, //测量0°方向试件上段
    Measure0Middle, //测量0°方向试件中段
    Measure0Down, //测量0°方向试件下段

    Rotate90, //试件旋转90°

    Measure90Up, //测量90°方向试件上段
    Measure90Middle, //测量90°方向试件中段
    Measure90Down, //测量90°方向试件下段

    StickUp, //安装低碳钢试件
    Tighten, //紧固

    SelectSpeed, //选择转速
    Rotate, //点击正转反转按钮，（让）试件断裂
    StickDown, //取下低碳钢试件
    SwitchClose, //关闭空气开关

    Exit //退出
}