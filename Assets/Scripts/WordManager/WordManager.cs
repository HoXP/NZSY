using Framework;
using NPOI.XWPF.UserModel;
using System.IO;
using UnityEngine;

public class WordManager : MonoSingleton<WordManager>
{
    private string path;
    private XWPFDocument doc = new XWPFDocument();

    void Start()
    {
        path = Path.Combine(@"C:/Users/Administrator/Desktop", "david.docx");
    }

    private void CreateParagraph(ParagraphAlignment _alignment, int _fontSize, string _color, string _content)
    {
        XWPFParagraph paragraph = doc.CreateParagraph();
        paragraph.Alignment = _alignment;
        XWPFRun run = paragraph.CreateRun();
        run.FontSize = _fontSize;
        run.SetColor(_color);
        run.FontFamily = "宋体";
        run.SetText(_content);
        FileStream fs = new FileStream(path, FileMode.Create);
        doc.Write(fs);
        fs.Close();
        fs.Dispose();
        Debug.Log("写入成功");
    }
}