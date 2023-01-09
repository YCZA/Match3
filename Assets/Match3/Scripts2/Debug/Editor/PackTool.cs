using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Match3.Scripts1.Puzzletown.Build;
using Match3.Scripts2.Env;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Match3.Scripts2.SRDebug.Editor
{
    public class PackTool
    {
        public static string envConfigPath = "Assets/New/Resources/config/build/env.txt";
        public static string versionNamePath = "Assets/New/Resources/config/build/version.txt";
        public static string buildVersionPath = "Assets/Scripts/Puzzletown/Build/BuildVersion.cs";
        
        public static string firebaseConfigTargetPath = "Assets/New/Config/Firebase/google-services.json";
        public static string firebaseConfigSourcePath_hw = "myfile/config/hw/google-services.json";
        public static string firebaseConfigSourcePath_samsung = "myfile/config/samsung/google-services.json";

        [MenuItem("Tools/❗‼仅程序人员使用‼❗/导出安卓工程/华为海外/用于测试")]
        public static void Build_HW_test()
        {
            string envId = new GameEnvironmentHWAbroad().GetId(GameEnvironment.Environment.CI);
            Build(ScriptingImplementation.Mono2x, true, envId, "myfile/build/hw_test", GameEnvironment.Platform.HW_Abroad);
        }
        [MenuItem("Tools/❗‼仅程序人员使用‼❗/导出安卓工程/华为海外/用于上线")]
        public static void Build_HW_Production()
        {
            string envId = new GameEnvironmentHWAbroad().GetId(GameEnvironment.Environment.PRODUCTION);
            Build(ScriptingImplementation.IL2CPP, false, envId, "myfile/build/hw_production", GameEnvironment.Platform.HW_Abroad);
        }
        [MenuItem("Tools/❗‼仅程序人员使用‼❗/导出安卓工程/三星海外/用于测试")]
        public static void Build_Samsung_test()
        {
            string envId = new GameEnvironmentSamsungAbroad().GetId(GameEnvironment.Environment.CI);
            Build(ScriptingImplementation.Mono2x, true, envId, "myfile/build/samsung_test", GameEnvironment.Platform.Samsung_Abroad);
        }
        [MenuItem("Tools/❗‼仅程序人员使用‼❗/导出安卓工程/三星海外/用于上线")]
        public static void Build_Samsung_Production()
        {
            string envId = new GameEnvironmentSamsungAbroad().GetId(GameEnvironment.Environment.PRODUCTION);
            Build(ScriptingImplementation.IL2CPP, false, envId, "myfile/build/samsung_production", GameEnvironment.Platform.Samsung_Abroad);
        }

        [MenuItem("Tools/❗‼仅程序人员使用‼❗/版本/版本号+1")]
        public static void InternalVersionAdd1()
        {
            var path = Path.Combine(Environment.CurrentDirectory, buildVersionPath);
            var code = File.ReadAllText(path);
            var lines = code.Split(new string[]{"\r\n", "\n"}, StringSplitOptions.None).ToList();
            lines.RemoveAt(lines.Count - 1);    // 最后一行是空行，移除
            var prefix = "public const int INTERNAL_VERSION = ";
            int oldVersion = -1;
            
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Trim().StartsWith(prefix))
                {
                    oldVersion = int.Parse(lines[i].Trim().Replace(prefix, "").Replace(";", ""));
                    var newVersion = oldVersion + 1;
                    lines[i] = lines[i].Replace(oldVersion.ToString(), newVersion.ToString());
                    File.WriteAllLines(path, lines);
                    AssetDatabase.Refresh();
                    UnityEngine.Debug.Log($"版本号之前为: {oldVersion}, 现在: {newVersion}");
                    break;
                }
            }

            if (oldVersion == -1)
            {
                UnityEngine.Debug.Log("修改版本号失败");
            }
        }
        
        [MenuItem("Tools/❗‼仅程序人员使用‼❗/版本/版本名+0.0.1")]
        public static void VersionNameAdd001()
        {
            var path = Path.Combine(Environment.CurrentDirectory, versionNamePath);
            var oldVersionName = File.ReadAllText(path);
            var versionNameArr = oldVersionName.Split('.');
            var newVersionName = $"{versionNameArr[0]}.{versionNameArr[1]}.{int.Parse(versionNameArr[2]) + 1}";
            File.WriteAllText(path, newVersionName);
            AssetDatabase.Refresh();
            UnityEngine.Debug.Log($"版本名称之前为: {oldVersionName}, 现在: {newVersionName}");
        }

        private static void Build(ScriptingImplementation backend, bool isDev, string envId, string buildPath, GameEnvironment.Platform platform)
        {
            // 设置firebase和appsflyer
            string targetPath = Path.Combine(Environment.CurrentDirectory, firebaseConfigTargetPath);
            if (platform == GameEnvironment.Platform.HW_Abroad)
            {
                UnityEngine.Debug.Log("<color=green>1. 平台:华为海外</color>");
                string sourcePath = Path.Combine(Environment.CurrentDirectory, firebaseConfigSourcePath_hw);
                File.WriteAllText(targetPath, File.ReadAllText(sourcePath));
            }
            else if (platform == GameEnvironment.Platform.Samsung_Abroad)
            {
                UnityEngine.Debug.Log("<color=green>1. 平台:三星海外</color>");
                string sourcePath = Path.Combine(Environment.CurrentDirectory, firebaseConfigSourcePath_samsung);
                File.WriteAllText(targetPath, File.ReadAllText(sourcePath));
            }
            
            // PlayerSetting设置
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, backend);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
            if (backend == ScriptingImplementation.IL2CPP)
            {
                PlayerSettings.Android.targetArchitectures |= AndroidArchitecture.ARM64;
            }
            UnityEngine.Debug.Log("<color=green>2. 脚本后端: </color>" + backend);

            // EditorUserBuildSettings设置
            EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
            // EditorUserBuildSettings.development = isDev;    // 不需要
            
            // 设置平台环境id
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, envConfigPath), envId);
            UnityEngine.Debug.Log("<color=green>3. 环境id: </color>" + envId);
            
            // 路径
            var timeStr = DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_");
            var filePath = Path.Combine(Environment.CurrentDirectory, buildPath);
            var internalVersion = BuildVersion.INTERNAL_VERSION;
            var versionName = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, versionNamePath));
            var fileName = $"AndroidProject_{internalVersion}v{versionName}_{timeStr}";
            UnityEngine.Debug.Log($"<color=green>4. 版本号: {internalVersion} 版本名: {versionName}</color>");
        
            // 添加场景
            List<string> scenePathList = new List<string>();
            int i = 0;
            while(true)
            {
                var path = SceneUtility.GetScenePathByBuildIndex(i);

                if (string.IsNullOrEmpty(path))
                {
                    break;
                }
                scenePathList.Add(path);
                i++;
            }
            UnityEngine.Debug.Log($"<color=green>5. 打包场景数: {scenePathList.Count}</color>");
            
            // build设置
            var buildOptions = new BuildPlayerOptions();
            buildOptions.scenes = scenePathList.ToArray();
            buildOptions.locationPathName = Path.Combine(filePath, fileName);
            buildOptions.target = BuildTarget.Android;
            buildOptions.options = BuildOptions.CompressWithLz4;
            if (isDev)
            {
                buildOptions.options |= BuildOptions.Development;
            }
            UnityEngine.Debug.Log($"<color=green>6. 打测试包: {isDev}</color>");
            
            AssetDatabase.Refresh();
            BuildPipeline.BuildPlayer(buildOptions);
            
            // 打包结束之后
            Application.OpenURL(filePath);
            EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
            EditorUserBuildSettings.development = false;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;
            
            envId = new GameEnvironmentHWAbroad().GetId(GameEnvironment.Environment.CI);
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, envConfigPath), envId);
            UnityEngine.Debug.Log("<color=green>7. 结束</color>");
        }
    }
}