using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Match3.Scripts2.TempCode.Editor
{
    public class TempTools
    {
        [MenuItem("TempTools/删除缺失的组件(未实现)", true)]
        public static bool DelMissionComponents_Switch()
        {
            return false;
        }
        [MenuItem("TempTools/删除缺失的组件(未实现)")]
        public static void DelMissionComponents()
        {
        
        }
    
        [MenuItem("TempTools/列出挂载的脚本")]
        public static void ListScript()
        {
            var goes = Selection.transforms;

            List<Transform> list = new List<Transform>(); 
            list.Add(goes[0]);
            list.AddRange(GetChild(goes[0]));
        
            Dictionary<string, MonoBehaviour> dic = new Dictionary<string, MonoBehaviour>();
            foreach (var tran in list)
            {
                foreach (var mono in tran.GetComponents<MonoBehaviour>())
                {
                    dic[mono.GetType().ToString()] = mono;
                }
            }

            string result = "";
            foreach (var kv in dic)
            {
                result += kv.Key + "\n";
            }
            Debug.LogError(result);
        }

        private static List<Transform> GetChild(Transform go)
        {
            var list = new List<Transform>();
            foreach (Transform child in go)
            {
                list.Add(child);
                if (child.childCount != 0)
                {
                    list.AddRange(GetChild(child));
                }
            }

            return list;
        }
    
        // [MenuItem("TempTools/反查资源依赖(大约需要运行1分钟)")]
        public static void FindRequest()
        {
            string[] GUIDs = Selection.assetGUIDs;
            if (GUIDs.Length <= 0)
            {
                Debug.Log("您没有选中任何资源");
            }

            string selectedGUID = GUIDs[0];
            var selectedGO = Selection.activeObject;
            Debug.Log($"您选中的资源: {selectedGO.name}, 这个资源的GUID为: {selectedGUID}");

            Debug.Log("依赖此资源的资源有:");
            var list =AssetDatabase.GetAllAssetPaths();
        
            foreach (var item in list)
            {
                string fullPath = Path.Combine(Environment.CurrentDirectory, item);
                // Debug.Log("<color=green>正在检查：</color>" + fullPath);
                if (Directory.Exists(fullPath))
                {
                    // 跳过目录
                    continue;
                }

                var depeList = AssetDatabase.GetDependencies(item);
                foreach (var depe in depeList)
                {
                    if (AssetDatabase.AssetPathToGUID(depe) == selectedGUID)
                    {
                        Debug.Log(item);
                    }
                }
            }
            Debug.Log("结束");
        }

        [MenuItem("TempTools/检查选中资源是否被依赖, 并选中其中的孤儿资源(大约需要10s)")]
        public static void RequiredBy()
        {
            string[] GUIDs = Selection.assetGUIDs;
            if (GUIDs.Length <= 0)
            {
                Debug.Log("您没有选中任何资源");
                return;
            }

            var depeDic = GetDepeDic();
            List<UnityEngine.Object> orphanRes = new List<UnityEngine.Object>();
            foreach (var guid in GUIDs)
            {
                string selectedPath = AssetDatabase.GUIDToAssetPath(guid);
                bool hasRequired = CheckRequired(depeDic, selectedPath);
                if (! hasRequired)
                {
                    orphanRes.Add(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(selectedPath));
                }
            }

            Selection.objects = orphanRes.ToArray();
        }

        private static Dictionary<string, Dictionary<string, int>> GetDepeDic()
        {
            var assetList =AssetDatabase.GetAllAssetPaths();
            var allDepeDic = new Dictionary<string, Dictionary<string, int>>();

            // 遍历所有资源
            foreach (var assetPath in assetList)
            {
                var depeList = AssetDatabase.GetDependencies(assetPath);
                foreach (var depe in depeList)
                {
                    if (!allDepeDic.ContainsKey(depe))
                    {
                        allDepeDic[depe] = new Dictionary<string, int>();
                    }

                    allDepeDic[depe][assetPath] = 1;
                }
            }

            return allDepeDic;
        }

        private static bool CheckRequired(Dictionary<string, Dictionary<string, int>> depeDic, string selectedPath)
        {
            Debug.Log("<color=green>正在检查资源：</color>" + selectedPath);
            depeDic[selectedPath].Remove(selectedPath);
            if (depeDic[selectedPath].Keys.Count == 0)
            {
                Debug.Log("<color=red>该资源不被任何资源所依赖</color>");
                return false;
            }
            else
            {
                Debug.Log("被依赖次数:" + depeDic[selectedPath].Keys.Count);
                Debug.Log("依赖该此资源的资源有：");
                foreach (var kv in depeDic[selectedPath])
                {
                    Debug.Log(kv.Key);
                }

                return true;
            }
        }

        public static List<string> mDepeList = new List<string>();
        [MenuItem("TempTools/列出选中资源的所有依赖")]
        public static void ListDepe()
        {
            string[] GUIDs = Selection.assetGUIDs;
            if (GUIDs.Length <= 0)
            {
                Debug.Log("您没有选中任何资源");
                return;
            }
            string selectedPath = AssetDatabase.GUIDToAssetPath(GUIDs[0]);
            mDepeList.Clear();
            ListDepe(selectedPath);
            Debug.LogError("依赖数量:" + mDepeList.Count);
            // var depeList = AssetDatabase.GetDependencies(selectedPath);

            // string result = "";
            // foreach (var depe in depeList)
            // {
            // result += depe + "\n";
            // }
            // Debug.Log("该资源的依赖有:" + "\n" + result);
        }

        public static void ListDepe(string path, int depth = 1)
        {
            if (depth > 1)
            {
                return;
            }
            foreach (var depe in AssetDatabase.GetDependencies(path))
            {
                if (path == depe)
                {
                    continue;
                }
                Debug.Log(new string('-', depth - 1) + depe);
                if (!mDepeList.Contains(depe))
                {
                    mDepeList.Add(depe);
                }
                ListDepe(depe, depth + 1);
            }
        }

        // [MenuItem("TempTools/复制刚才列出的依赖")]
        // public static void CopyDepe()
        // {
        //     foreach (var depe in mDepeList)
        //     {
        //         if (!depe.StartsWith("Assets"))
        //         {
        //             continue;
        //         }
        //         Debug.Log("正在复制: " + depe);
        //         string path1 = Path.Combine(Environment.CurrentDirectory, depe);
        //         string path2 = path1 + ".meta";
        //         new FileInfo(path1).CopyTo(@"E:\temp\tc_m3_level\Assets\MyFile\Level\M3_Options\" + new FileInfo(path1).Name);
        //         new FileInfo(path2).CopyTo(@"E:\temp\tc_m3_level\Assets\MyFile\Level\M3_Options\" + new FileInfo(path2).Name);
        //     }
        // }

        // public static List<string> GetDepe(string path)
        // {
        //     List<string> result = new List<string>();
        //     var depeList = AssetDatabase.GetDependencies(path);
        //     foreach (var depe in depeList)
        //     {
        //         if (path == depe)
        //         {
        //             continue;
        //         }
        //         if (AssetDatabase.GetDependencies(depe).Length > 0)
        //         {
        //             result.AddRange(GetDepe(depe));
        //         }
        //     }
        //
        //     return result;
        // }
    }
}
