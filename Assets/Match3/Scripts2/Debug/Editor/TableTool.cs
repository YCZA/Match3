using System;
using System.IO;
using System.Linq;
using Match3.Scripts1;
using Match3.Scripts1.Localization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using UnityEditor;
using UnityEngine;
using Wooga.Foundation.Json;

namespace Match3.Scripts2.SRDebug.Editor
{
    public class TableTool
    {
        private static string path_LanScriptableObject = "localization/";
        private static string path_LanExcelFile = "myfile/gamedata/language/localization.xlsx";
        private static string path_MapConfigExcelFile = "myfile/gamedata/map/mapconfig.xlsx";
        private static string path_MapConfigBinaryFile = "Assets/Bundles/scene_townenvironment_0/Texts/mapConfigBinary.bytes";

        [MenuItem("Tools/导表/打开语言表", false,41)]
        private static void OpenLanTable()
        {
            var path = Path.Combine(System.Environment.CurrentDirectory, path_LanExcelFile);
            Application.OpenURL(path);
        }

        [MenuItem("Tools/导表/导出语言表--->", validate = true )]
        private static bool ExportLanTableValidate()
        {
            return false;
        }
        
        [MenuItem("Tools/导表/导出语言表--->", false, 60)]
        private static void ExportLanTable()
        {
            return;
            LocaleWrapper[] scriptableObjectList = Resources.LoadAll<LocaleWrapper>(path_LanScriptableObject);
            LocaleWrapper scriptableObject_en =
                Resources.Load<LocaleWrapper>(Path.Combine(path_LanScriptableObject, "en"));
            int rowCount = scriptableObject_en.locale.Entries.Length + 3;
            int colCount = scriptableObjectList.Length + 1;
            object[,] array = new object[rowCount, colCount];
            //---
            int curRowIndex = 1;
            int curColIndex = 1;
            foreach (var so in scriptableObjectList)
            {
                array[curRowIndex, curColIndex] = "string";
                array[curRowIndex + 1, curColIndex] = so.name;
                curColIndex++;
            }
            curRowIndex = 3;
            foreach (var entry in scriptableObject_en.locale.Entries)
            {
                curColIndex = 0;
                array[curRowIndex, curColIndex] = entry.key;
                foreach (var so in scriptableObjectList)
                {
                    curColIndex++;
                    array[curRowIndex, curColIndex] = so.locale.Entries.First(e => e.key == entry.key).val;
                }
                curRowIndex++;
            }
            //--- 
            var path = Path.Combine(System.Environment.CurrentDirectory, path_LanExcelFile);
            ArrayToExcel(array, path);
        }
        
        [MenuItem("Tools/导表/导入语言表<---", false, 80)]
        private static void ImportLanTable()
        {
            var xlsx_path = Path.Combine(System.Environment.CurrentDirectory, path_LanExcelFile);
            
            object[,] array = ExcelToArray(xlsx_path);
            int keyNum = array.GetLength(0) - 3;

            for (int col = 1; col < array.GetLength(1); col++)
            {
                string lanName = array[2, col].ToString();
                LocaleWrapper curLocale = Resources.Load<LocaleWrapper>(Path.Combine(path_LanScriptableObject, lanName));
                if (curLocale == null)
                {
                    // 读取或创建asset
                    curLocale = ScriptableObject.CreateInstance<LocaleWrapper>();
                    AssetDatabase.CreateAsset(curLocale, "Assets/Resources/"+ path_LanScriptableObject + "/" + lanName +".asset");
                }

                curLocale.locale.Entries = new Locale.Entry[keyNum];
                for (int row = 3; row < array.GetLength(0); row++)
                {
                    curLocale.locale.Entries[row - 3].key = array[row, 0].ToString();
                    curLocale.locale.Entries[row - 3].val = array[row, col]?.ToString();
                }
		        EditorUtility.SetDirty(curLocale);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            UnityEngine.Debug.Log("导入完成。");
        }

        [MenuItem("Tools/导表/导入地图表<---", false, 80)]
        private static void ImportMapTable()
        {
            byte[] bytes = new byte[128 * 128];
            var xlsx_path = Path.Combine(System.Environment.CurrentDirectory, path_MapConfigExcelFile);
            var array = ExcelToArray(xlsx_path, 128, 128);

            for (int i = 0; i < 128; i++)
            {
                for (int j = 0; j < 128; j++)
                {
                    bytes[i * 128 + j] = Convert.ToByte(array[j, i]); // 行列的表示方式和坐标的表示方式正好相反
                }
            }
            
            File.WriteAllBytes(Path.Combine(System.Environment.CurrentDirectory, path_MapConfigBinaryFile), bytes);
            UnityEngine.Debug.Log("导入完成");
            AssetDatabase.Refresh();
        }

        // [MenuItem("工具/导表/导出建筑bundle配置表--->")]
        private static void ExportBuildingTable()
        {
            TextAsset json = Resources.Load<TextAsset>("puzzletown/config/buildingtobundlemap.json");
			BuildingToBundleMap buildingList = JSON.Deserialize<BuildingToBundleMap>(json.text);
            
            // 声明数组大小
            int rowCount = buildingList.entries.Count + 3;
            int colCount = 7 + 1;
            object[,] array = new object[rowCount, colCount];
            // 表头
            array[1, 0] = "string";
            array[2, 0] = "id";
            array[1, 1] = "string";
            array[2, 1] = "prefabBundle";
            array[1, 2] = "string";
            array[2, 2] = "iconBundle";
            array[1, 3] = "string";
            array[2, 3] = "destroyedBundle";
            array[1, 4] = "string";
            array[2, 4] = "path";
            array[1, 5] = "string";
            array[2, 5] = "icon";
            array[1, 6] = "string";
            array[2, 6] = "destroyedPath";
            // 内容
            int curRowIndex = 3;
            foreach (var entry in buildingList.entries)
            {
                array[curRowIndex, 0] = entry.id;
                array[curRowIndex, 1] = entry.prefabBundle;
                array[curRowIndex, 2] = entry.iconBundle;
                array[curRowIndex, 3] = entry.destroyedBundle;
                array[curRowIndex, 4] = entry.path;
                array[curRowIndex, 5] = entry.icon;
                array[curRowIndex, 6] = entry.destroyedPath;
                curRowIndex++;
            }
            //--- 
            var path = Path.Combine(System.Environment.CurrentDirectory, "myfile/gamedata/buildingtobundlemap.xlsx");
            ArrayToExcel(array, path);
        }
        
        // [MenuItem("工具/导表/导出地图初始建筑配置表--->")]
        private static void ExportStartBuildingTable()
        {
            TextAsset json = Resources.Load<TextAsset>("puzzletown/config/json/startbuildingconfig.json");
			StartBuildingConfig startBuildingConfig = JsonUtility.FromJson<StartBuildingConfig>(json.text);
            
            // 声明数组大小
            int rowCount = startBuildingConfig.GetAll().Count() + 3;
            int colCount = 6;
            object[,] array = new object[rowCount, colCount];
            // 表头
            array[1, 0] = "int";
            array[2, 0] = "area";
            array[1, 1] = "int";
            array[2, 1] = "pos_x";
            array[1, 2] = "int";
            array[2, 2] = "pos_y";
            array[1, 3] = "bool";
            array[2, 3] = "destroyed";
            array[1, 4] = "string";
            array[2, 4] = "building_id";
            array[1, 5] = "string";
            array[2, 5] = "deco_set";
            // 内容
            int curRowIndex = 3;
            foreach (var entry in startBuildingConfig.GetAll())
            {
                array[curRowIndex, 0] = entry.area;
                array[curRowIndex, 1] = entry.pos_x;
                array[curRowIndex, 2] = entry.pos_y;
                array[curRowIndex, 3] = entry.destroyed;
                array[curRowIndex, 4] = entry.building_id;
                array[curRowIndex, 5] = entry.deco_set;
                curRowIndex++;
            }
            //--- 
            var path = Path.Combine(System.Environment.CurrentDirectory, "myfile/gamedata/startbuildingconfig.xlsx");
            ArrayToExcel(array, path);
        }
        
        // [MenuItem("工具/导表/导出建筑配置表--->")]
        private static void ExportBuildingConfigTable()
        {
            return;
            TextAsset json = Resources.Load<TextAsset>("puzzletown/config/json/buildingconfiglist.json");
			BuildingConfigList buildingConfigList = JSON.Deserialize<BuildingConfigList>(json.text);
            
            // 声明数组大小
            int rowCount = buildingConfigList.buildings.Length + 3;
            int colCount = 27;
            object[,] array = new object[rowCount, colCount];
            // 表头
            array[1, 0] = "string";
            array[2, 0] = "name";
            array[1, 1] = "int";
            array[2, 1] = "chapter_id";
            array[1, 2] = "int";
            array[2, 2] = "type";
            array[1, 3] = "string";
            array[2, 3] = "tag";
            array[1, 4] = "string";
            array[2, 4] = "category";
            
            array[2, 5] = "art_override";
            array[2, 6] = "island_id";
            array[2, 7] = "max_number";
            array[2, 8] = "unlock_level";
            array[2, 9] = "harmony";
            array[2, 10] = "harmony_seasonals_v3";
            array[2, 11] = "season_currency";
            array[2, 12] = "unlock_resource";
            array[2, 13] = "unlock_resource_amount";
            array[2, 14] = "size";
            array[2, 15] = "season";
            array[2, 16] = "harvest_resource";
            array[2, 17] = "harvest_timer";
            array[2, 18] = "harvest_minimum";
            array[2, 19] = "harvest_maximum";
            array[2, 20] = "rubble_id";
            array[2, 21] = "batch_mode";
            array[2, 22] = "challenge_set";
            array[2, 23] = "blueprints";
            array[2, 24] = "shared_storage";
            array[2, 25] = "costs";
            
            // 内容
            int curRowIndex = 3;
            foreach (var entry in buildingConfigList.buildings)
            {
                array[curRowIndex, 0] = entry.name;
                array[curRowIndex, 1] = entry.chapter_id;
                array[curRowIndex, 2] = entry.type;
                array[curRowIndex, 3] = entry.tag;
                array[curRowIndex, 4] = entry.category;
                array[curRowIndex, 5] = entry.art_override;
                array[curRowIndex, 6] = entry.island_id;
                array[curRowIndex, 7] = entry.max_number;
                array[curRowIndex, 8] = entry.unlock_level;
                array[curRowIndex, 9] = entry.harmony;
                array[curRowIndex, 10] = entry.harmony_seasonals_v3;
                array[curRowIndex, 11] = entry.season_currency;
                array[curRowIndex, 12] = entry.unlock_resource;
                array[curRowIndex, 13] = entry.unlock_resource_amount;
                array[curRowIndex, 14] = entry.size;
                array[curRowIndex, 15] = entry.season;
                array[curRowIndex, 16] = entry.harvest_resource;
                array[curRowIndex, 17] = entry.harvest_timer;
                array[curRowIndex, 18] = entry.harvest_minimum;
                array[curRowIndex, 19] = entry.harvest_maximum;
                array[curRowIndex, 20] = entry.rubble_id;
                array[curRowIndex, 21] = entry.batch_mode;
                array[curRowIndex, 22] = entry.challenge_set;
                array[curRowIndex, 23] = entry.blueprints;
                array[curRowIndex, 24] = entry.shared_storage;
                string costsString = "";
                foreach (var cost in entry.costs)
                {
                    costsString += cost.type + " " + cost.amount + " ";
                }
                array[curRowIndex, 25] = costsString;
                curRowIndex++;
            }
            //--- 
            var path = Path.Combine(System.Environment.CurrentDirectory, "myfile/gamedata/buildingconfiglist.xlsx");
            ArrayToExcel(array, path);
        }
        
        // ------

        // 前三行：别名，类型，字段名
        private static void ArrayToExcel(object[,] array, string excelFilePath)
        {
            FileInfo fi = new FileInfo(excelFilePath);
            if (fi.Exists)
            {
                fi.Delete();
            }

            using (ExcelPackage ep = new ExcelPackage(fi))
            {
                ExcelWorksheet mainSheet = ep.Workbook.Worksheets.Add("sheet1");
                for (int row = 1; row <= array.GetLength(0); row++)
                {
                    for (int col = 1; col <= array.GetLength(1); col++)
                    {
                        mainSheet.Cells[row, col].Value = array[row - 1, col - 1];
                    }
                }
                
                using (var range = mainSheet.Cells[1, 1, 3, array.GetLength(1)])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(255, 0, 0, 0);
                    range.Style.Font.Color.SetColor(255, 255, 255, 255);
                }
                
                // 保存
                ep.Save();
                UnityEngine.Debug.Log(string.Format("已保存文件：{0}", fi.FullName));
                Application.OpenURL(fi.FullName);
            }
        }
        
        // 前三行：别名，类型，字段名
        private static object[,] ExcelToArray(string excelFilePath, int rowNum = 0, int colNum = 0)
        {
            FileInfo fi = new FileInfo(excelFilePath);
            using (ExcelPackage ep = new ExcelPackage(fi))
            {
                ExcelWorksheet mainSheet = ep.Workbook.Worksheets[1];
                int rows = 3;
                int cols = 0;
                // 根据类型行确定列数，根据第一列确定行数
                if (rowNum == 0)
                {
                    for (int row = 4; row <= mainSheet.Cells.Rows; row++)
                    {
                        if (mainSheet.Cells[row, 1].Value != null)
                        {
                            rows++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    rows = rowNum;
                }

                if (colNum == 0)
                {
                    for (int col = 1; col <= mainSheet.Cells.Columns; col++)
                    {
                        if (mainSheet.Cells[2, col].Value != null)
                        {
                            cols++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    cols = colNum;
                }
                
                // 写入数组
                var result = new object[rows, cols];
                for (int row = 1; row <= rows; row++)
                {
                    for (int col = 1; col <= cols; col++)
                    {
                        var value = mainSheet.Cells[row, col].Value;
                        result[row - 1, col - 1] = value;
                    }
                }

                return result;
            }
        }
    }
}