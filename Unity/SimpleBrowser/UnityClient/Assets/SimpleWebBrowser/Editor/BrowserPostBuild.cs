using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.Linq;

public class BrowserPostBuild
{
    [PostProcessBuildAttribute(1)]
    public static void OnPostprocessBuild(BuildTarget target, string path)
    {
        string dstPath = Path.GetDirectoryName(path);
        
        Debug.Log("Post Build:"+ dstPath);       
                
        switch (target)
        {
            case BuildTarget.StandaloneWindows64:
            case BuildTarget.StandaloneWindows:
            {                   
                string dstPluginPath = Path.Combine(Path.Combine(dstPath, Path.GetFileNameWithoutExtension(path) + "_Data"), "Plugins");
                string pluginPath = GetTargetPluginPath(target);
                string fullPath = Application.dataPath.Replace("Assets", pluginPath);
                
                string[] files=Directory.GetFiles(fullPath);
                foreach (var file in files)
                {
                    if (!file.Contains("meta"))
                    {
                        if(!File.Exists(Path.Combine(dstPluginPath, Path.GetFileName(file))))
                            FileUtil.CopyFileOrDirectory(file, Path.Combine(dstPluginPath, Path.GetFileName(file)));
                    }
                }

                string dstLocalePath = Path.Combine(dstPluginPath, "locales");
                Directory.CreateDirectory(dstLocalePath);
                files = Directory.GetFiles(Path.Combine(fullPath, "locales"));
                foreach (var file in files)
                {
                    if (!file.Contains("meta"))
                    {
                        if (!File.Exists(Path.Combine(dstLocalePath, Path.GetFileName(file))))
                            FileUtil.CopyFileOrDirectory(file, Path.Combine(dstLocalePath,Path.GetFileName(file)));
                    }
                }

                break;
            }            
            default:
                Debug.LogError("Web browser is not supported on this platform!");
                break;
        }
    }

    private static string GetTargetPluginPath(BuildTarget target)
    {   
        string pluginPath = "";
        var assets = AssetDatabase.FindAssets("Xilium.CefGlue");
        foreach (var item in assets)
        {
            string path = AssetDatabase.GUIDToAssetPath(item);
            string dirFullPath = Path.GetDirectoryName(path);
            string dirName = Path.GetDirectoryName(path);

            PluginImporter importer = (PluginImporter)AssetImporter.GetAtPath(path);

            if ( importer.GetCompatibleWithPlatform(target) )
            {
                pluginPath = dirFullPath;
                break;
            }
        }

        return pluginPath;
    }
}
