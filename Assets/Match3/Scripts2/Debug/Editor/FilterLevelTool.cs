using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Match3.Scripts1.Puzzletown.Match3;
using UnityEditor;
using UnityEngine;
using Wooga.Foundation.Json;

namespace Match3.Scripts2.SRDebug.Editor
{
    public class FilterLevelTool : EditorWindow
    {
        [MenuItem("Tools/打开\"筛选关卡\"窗口")]
        private static void Open()
        {
            var window = GetWindow<FilterLevelTool>();
            window.titleContent.text = "筛选关卡";
            window.position = new Rect(Screen.currentResolution.width / 2f - 300, Screen.currentResolution.height / 2f - 200, 600, 400);
            window.Show();
        }

        private static List<string> GetLevelsPath()
        {
            var pathList = new List<string>();
            for (int i = 1; i <= 29; i++)
            {
                pathList.Add(@"puzzletown\match3\levels\area "+i);
            }
            return pathList;
        }

        private string textFieldValue1 = "0 1 2 3 4 5 6 8 9 10 11";
        private string textFieldValue2;
        private string textFieldValue3;
        private string textFieldValue4;
        private string textFieldValue5;
        private string textFieldValue6;
        private string textFieldValue7;
        private void OnGUI()
        {
            GUILayout.Space(12);
            
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("只包含这些GemColor类型,(且不包含轮盘, 蛋, 变色龙，收集容器, 贝壳, 且目标不含乌龟)");
                textFieldValue1 = GUILayout.TextField(textFieldValue1);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("开始筛选"))
                {
                    Filter((config, args) =>
                    {
                        bool isOk = true;
                        foreach (var layoutGemColor in config.layout.gemColors)
                        {
                            if (!args.Contains(layoutGemColor))
                            {
                                isOk = false;
                            }
                        }

                        foreach (var stone in config.layout.stones)
                        {
                            if (stone > 5)
                            {
                                isOk = false;
                            }
                        }
                        
                        foreach (var type in config.layout.gemTypes)
                        {
                            if (type > 9)
                            {
                                isOk = false;
                            }
                        }
                        
                        foreach (var type in config.layout.spawning)
                        {
                            if (type > 1)
                            {
                                isOk = false;
                            }
                        }

                        foreach (var obj in config.data.objectives)
                        {
                            if (obj.type == "droppable")
                            {
                                isOk = false;
                            }
                        }

                        // 去掉沙土(去除寄居蟹时应该就已经把沙土去掉了)
                        // if (config.layout.gemModifier != null)
                        // {
                        //     foreach (var type in config.layout.gemModifier)
                        //     {
                        //         if (type > 3)
                        //         {
                        //             isOk = false;
                        //         }
                        //     }
                        // }
                        
                        return isOk;
                    }, textFieldValue1);
                }
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("包含这些spawning类型的关卡（2：贝壳，3：变色龙）");
                textFieldValue2 = GUILayout.TextField(textFieldValue2);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("开始筛选"))
                {
                    Filter((config, args) =>
                    {
                        bool isOk = true;

                        foreach (var arg in args)
                        {
                            if (!config.layout.spawning.Contains(arg))
                            {
                                isOk = false;
                            }
                        }

                        return isOk;
                    }, textFieldValue2);
                }
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("包含这些GemColor类型的关卡");
                textFieldValue3 = GUILayout.TextField(textFieldValue3);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("开始筛选"))
                {
                    Filter((config, args) =>
                    {
                        bool isOk = true;

                        foreach (var arg in args)
                        {
                            if (!config.layout.gemColors.Contains(arg))
                            {
                                isOk = false;
                            }
                        }

                        return isOk;
                    }, textFieldValue3);
                }
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("包含这些GemType类型的关卡");
                textFieldValue4 = GUILayout.TextField(textFieldValue4);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("开始筛选"))
                {
                    Filter((config, args) =>
                    {
                        bool isOk = true;

                        foreach (var arg in args)
                        {
                            if (!config.layout.gemTypes.Contains(arg))
                            {
                                isOk = false;
                            }
                        }

                        return isOk;
                    }, textFieldValue4);
                }
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("包含这些GemModifier类型的关卡");
                textFieldValue5 = GUILayout.TextField(textFieldValue5);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("开始筛选"))
                {
                    Filter((config, args) =>
                    {
                        bool isOk = true;

                        foreach (var arg in args)
                        {
                            if (config.layout.gemModifier != null)
                            {
                                if (!config.layout.gemModifier.Contains(arg))
                                {
                                    isOk = false;
                                }
                            }
                            else
                            {
                                isOk = false;
                            }
                        }

                        return isOk;
                    }, textFieldValue5);
                }
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("包含这些fields类型的关卡");
                textFieldValue6 = GUILayout.TextField(textFieldValue6);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("开始筛选"))
                {
                    Filter((config, args) =>
                    {
                        bool isOk = true;

                        foreach (var arg in args)
                        {
                            if (!config.layout.fields.Contains(arg))
                            {
                                isOk = false;
                            }
                        }

                        return isOk;
                    }, textFieldValue6);
                }
            }
            GUILayout.EndHorizontal();
            
            
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("包含这些tiles类型的关卡");
                textFieldValue7 = GUILayout.TextField(textFieldValue7);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("开始筛选"))
                {
                    Filter((config, args) =>
                    {
                        bool isOk = true;

                        foreach (var arg in args)
                        {
                            if (!config.layout.tiles.Contains(arg))
                            {
                                isOk = false;
                            }
                        }

                        return isOk;
                    }, textFieldValue7);
                }
            }
            GUILayout.EndHorizontal();
        }

        // 筛选只包含指定gemColor的关卡
        private void Filter(Func<LevelConfig, List<int>, bool> filterFun, string textFieldValue)
        {
            StringBuilder result = new StringBuilder(1000);
            Dictionary<string, int> countLevelDic = new Dictionary<string, int>();
            int resultCount = 0;
            
            var args = textFieldValue.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries)
                .Select((s)=>int.Parse(s)).ToList();
            
            foreach (var levelPath in GetLevelsPath())
            {
                var allFile = Resources.LoadAll<TextAsset>(levelPath);
                foreach (var file in allFile)
                {
                    LevelConfig config = JSON.Deserialize<LevelConfig>(file.text);
                    bool isOk = filterFun(config, args);

                    if (isOk)
                    {
                        result.Append(file.name).Append(" ");
                        string levelId = file.name.Substring(0, 4);
                        if (!countLevelDic.ContainsKey(levelId))
                        {
                            countLevelDic[levelId] = 0;
                        }

                        if (file.name.EndsWith("a"))
                        {
                            countLevelDic[levelId] += 4;
                        }
                        else if (file.name.EndsWith("b"))
                        {
                            countLevelDic[levelId] += 2;
                        }
                        else if (file.name.EndsWith("c"))
                        {
                            countLevelDic[levelId] += 1;
                        }
                        resultCount++;
                    }
                }
            }
            
            UnityEngine.Debug.Log("-----初步筛选-----");
            UnityEngine.Debug.Log("结果：" + result);
            UnityEngine.Debug.Log("结果总数：" + resultCount);
            UnityEngine.Debug.Log("关卡数：" + countLevelDic.Count);

            // ---2次筛选---
            UnityEngine.Debug.Log("-----进一步筛选, 从上面结果中选出 abc 3个难度都有的关卡-----");
            StringBuilder result2 = new StringBuilder(1000);
            int result2Count = 0;
            foreach (KeyValuePair<string, int> kv in countLevelDic)
            {
                if (kv.Value == 4 + 2 + 1)
                {
                    result2.Append(kv.Key).Append(" ");
                    result2Count++;
                }
            }
            UnityEngine.Debug.Log("结果：" + result2);
            UnityEngine.Debug.Log("关卡数：" + result2Count);
        }
    }
}