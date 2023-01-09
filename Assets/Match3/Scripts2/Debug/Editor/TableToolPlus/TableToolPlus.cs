using System;
using System.IO;
using UnityEditor;

// 没有实现创建json的功能，只能在现有json上进行修改(问题不大)

namespace Match3.Scripts2.SRDebug.Editor.TableToolPlus
{
    public class TableToolPlus
    {
        // jsonFile不能重名
        private static string[] jsonFilePaths = new[]
        {
            "Assets/New/Resources/puzzletown/config/buildingtobundlemap.json.txt",
            
            "Assets/New/Resources/puzzletown/config/json/buildingconfiglist.json.txt",
            "Assets/New/Resources/puzzletown/config/json/areasconfig.json.txt",
            "Assets/New/Resources/puzzletown/config/json/startbuildingconfig.json.txt",
            "Assets/New/Resources/puzzletown/config/json/storydialogueconfig.json.txt",
            "Assets/New/Resources/puzzletown/config/json/villagerconfig.json.txt",
            "Assets/New/Resources/puzzletown/config/json/villagerbehaviorconfig.json.txt",
            "Assets/New/Resources/puzzletown/config/json/iapconfigdatalist.json.txt",
            "Assets/New/Resources/puzzletown/config/json/genericdialoguesconfig.json.txt",
            "Assets/New/Resources/puzzletown/config/json/generalconfig.json.txt",
            "Assets/New/Resources/puzzletown/config/json/chapterconfig.json.txt",
            "Assets/New/Resources/puzzletown/config/json/forceupdateconfig.json.txt",
            
            "Assets/Resources/sbsconfigservice/questconfig.txt",
            "Assets/Resources/sbsconfigservice/challenges.txt",
            "Assets/Resources/sbsconfigservice/islandareaconfig.txt",
            "Assets/Resources/sbsconfigservice/levelforeshadowingconfig.txt",
            "Assets/Resources/sbsconfigservice/villagerankconfig.txt",
            "Assets/Resources/sbsconfigservice/dailygifts.txt"
        };
        private static string excelExportPath = "myfile/gamedata/tempfile"; // 导出时用到的文件夹, 需要把json结构定义文件放这里
        private static string excelFolderPath = "myfile/gamedata";

        [MenuItem("Tools/❗‼仅程序人员使用‼❗/导表/导出所有表(不含语言表, 地图表)--->", false, 10000)]
        public static void ExportAllTable()
        {
            OperatorAllTable(excelExportPath, (e)=>e.Json2Excel());
            UnityEngine.Debug.Log("所有json表已导出");
        }
        
        [MenuItem("Tools/导表/导入所有表(不含语言表, 地图表)<---", false, 80)]
        public static void ImportAllTable()
        {
            OperatorAllTable(excelFolderPath, (e)=>e.Excel2Json());
            UnityEngine.Debug.Log("所有excel表已导入");
        }

        private static void OperatorAllTable(string excelFolderPath, Action<ExcelFileObjWithJsonStruDe> action)
        {
            // 读取所有json结构
            string excelFolderFullPath = Path.Combine(System.Environment.CurrentDirectory, excelFolderPath);
            DirectoryInfo excelFolder = new DirectoryInfo(excelFolderFullPath);
            int fileCount = 0;
            foreach (var file in excelFolder.GetFiles())
            {
                UnityEngine.Debug.Log("正在操作表：" + file.Name);
                var excelFileObj = CreateExcelFileObj(file);
                if(excelFileObj == null) continue;

                action(excelFileObj);
                fileCount++;
            }
            UnityEngine.Debug.Log($"共操作{fileCount}张表");
        }

        private static bool IsExcelFile(FileInfo file)
        {
            return file.Name.EndsWith("xlsx") && !file.Name.StartsWith("~$");
        }

        private static string GetJsonFilePath(string jsonFileName)
        {
            foreach (var path in jsonFilePaths)
            {
                if (path.EndsWith("/" + jsonFileName))
                {
                    return path;
                }
            }

            return "";
        }

        private static ExcelFileObjWithJsonStruDe CreateExcelFileObj(FileInfo file)
        {
             // 判断文件是否有效
             if (!IsExcelFile(file)) return null;
             
             var excelFileObj = new ExcelFileObjWithJsonStruDe(file.FullName);
             if (!excelFileObj.IsValid()) return null;
             
             // 获取json路径
             string jsonFilePath = GetJsonFilePath(excelFileObj.structureTree.GetRootNode().GetNodeName());
             if (string.IsNullOrEmpty(jsonFilePath))
             {
                 UnityEngine.Debug.LogError("未找到json文件：" + excelFileObj.structureTree.GetRootNode().GetNodeName());
                 return null;
             }
             string jsonFileFullPath = Path.Combine(System.Environment.CurrentDirectory, jsonFilePath);
             
             // 设置jsonData
             excelFileObj.SetJsonData(jsonFileFullPath);
             
             return excelFileObj;
        }
    }
}