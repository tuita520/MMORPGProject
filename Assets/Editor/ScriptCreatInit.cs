using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEditor;

/*
 替换代码注释
     */
public class ScriptCreatInit : UnityEditor.AssetModificationProcessor {

    // 添加脚本注释模板
    private static string str =
    "// ========================================================\r\n"
    + "// 描 述：\r\n"
    + "// 作 者：#AuthorName# \r\n"
    + "// 创建时间：#CreateTime#\r\n"
    + "// 版 本：v 1.0\r\n"
    + "// ========================================================\r\n";

    private static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        if (path.EndsWith(".cs"))
        {
            string strContent = str;
            strContent += File.ReadAllText(path);
            strContent = strContent.Replace("#AuthorName#", "牛水鱼").Replace("#CreateTime#", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            File.WriteAllText(path, strContent);
            AssetDatabase.Refresh();
        }
    }
}
