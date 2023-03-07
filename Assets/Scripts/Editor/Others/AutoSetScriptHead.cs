using System.IO;

public class AutoSetScriptHead : UnityEditor.AssetModificationProcessor
{
    //导入资源创建资源时候调用
    public static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        if (!path.EndsWith(".cs"))
        {
            return;
        }
        string time = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string showMes = File.ReadAllText(path);
        showMes = " /** \n*    Class Description: XXX\n*\n*    CreateTime: " + time + "\n*\n*    Author : Ankh\n*\n*/\n\n" + showMes;

        File.WriteAllText(path, showMes);
 
    }
}