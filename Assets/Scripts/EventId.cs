public class EventId
{
    private static int EventIdStart = 0;

    public static int OnRotate = 1;
    public static int OnBreak = 2;
    public static int OnInitUIMachine = 4;
    public static int OnRegisterBtnSpeedsCallback = 5;
    public static int OnClickBtnRotate = 6; //点击转动按钮，如btnCW或btnCCW

    private static int EventIdEnd = int.MaxValue;
}