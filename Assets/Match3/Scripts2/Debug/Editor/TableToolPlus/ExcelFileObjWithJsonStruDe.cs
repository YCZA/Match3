using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using Match3.Scripts2.SRDebug.Editor.TableToolPlus.Structure;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Match3.Scripts2.SRDebug.Editor.TableToolPlus
{
    public class ExcelFileObjWithJsonStruDe
    {
        public StructureTree structureTree = null;
        public JsonData jsonData = null;
        
        private string jsonFileFullPath;
        private ExcelPackage ep;

        private List<string> baseTypeList = new List<string>()
        {
            "string", "int", "double", "boolean", "long"
        };
        
        public ExcelFileObjWithJsonStruDe(string path)
        {
            if (!File.Exists(path))
            {
                UnityEngine.Debug.Log("文件不存在！路径: " + path);
                return;
            }

            ep = new ExcelPackage(new FileInfo(path));
            foreach (var workbookWorksheet in ep.Workbook.Worksheets)
            {
                if (workbookWorksheet.Name == "jsonstructure")
                {
                    structureTree = new StructureTree(workbookWorksheet);
                }
            }
        }

        public void SetJsonData(string jsonFileFullPath)
        {
            this.jsonFileFullPath = jsonFileFullPath;
            JsonData jsonData = JsonMapper.ToObject(File.ReadAllText(jsonFileFullPath));
            this.jsonData = jsonData;
        }
        
        public bool IsValid()
        {
            return structureTree != null;
        }

        public void Json2Excel()
        {
            foreach (var node in structureTree.nodeList)
            {
                // 基本类型的节点
                if (baseTypeList.Contains(node.GetNodeType()))
                {
                    string nodeValue = GetNodeValueFromJson(node).ToString();
                    structureTree.sheet.Cells[node.row, node.col + 1].Value = nodeValue;
                }
                // 数组节点
                else if (node.GetNodeType() == "array" && structureTree.sheet.Cells[node.row, node.col + 1].Value == null)
                {
                    ExcelWorksheet sheet = null;
                    string sheetName = string.IsNullOrEmpty(node.GetNodeAlias())
                        ? node.GetNodeName()
                        : node.GetNodeAlias();
                    foreach (var workbookWorksheet in ep.Workbook.Worksheets)
                    {
                        if (workbookWorksheet.Name == sheetName)
                        {
                            sheet = workbookWorksheet;
                        }
                    }

                    if (sheet == null)
                    {
                        sheet = ep.Workbook.Worksheets.Add(sheetName);
                    }
                    
                    JsonArray2ExcelSheet(node, sheet);
                }
            }
            
            ep.Save();
        }

        public void Excel2Json()
        {
            foreach (var node in structureTree.nodeList)
            {
                // 基本类型的节点
                if (baseTypeList.Contains(node.GetNodeType()))
                {
                    string value = GetNodeValueFromExcel(node);
                    switch (node.GetNodeType())
                    {
                        case "string":
                            GetJsonDataOfNodeParent(node)[node.GetNodeName()] = value;
                            break;
                        case "int":
                            GetJsonDataOfNodeParent(node)[node.GetNodeName()] = int.Parse(value);
                            break;
                        case "double":
                            GetJsonDataOfNodeParent(node)[node.GetNodeName()] = double.Parse(value);
                            break;
                        case "long":
                            GetJsonDataOfNodeParent(node)[node.GetNodeName()] = long.Parse(value);
                            break;
                        case "boolean":
                            GetJsonDataOfNodeParent(node)[node.GetNodeName()] = bool.Parse(value);
                            break;
                    }
                }
                // 数组节点
                else if (node.GetNodeType() == "array" && structureTree.sheet.Cells[node.row, node.col + 1].Value == null)
                {
                    ExcelWorksheet sheet = null;
                    string sheetName = string.IsNullOrEmpty(node.GetNodeAlias())
                        ? node.GetNodeName()
                        : node.GetNodeAlias();
                    foreach (var workbookWorksheet in ep.Workbook.Worksheets)
                    {
                        if (workbookWorksheet.Name == sheetName)
                        {
                            sheet = workbookWorksheet;
                        }
                    }
                    
                    if (sheet == null)
                    {
                        continue;
                    }

                    var array = ExcelSheet2JsonArray(sheet);
                    if (array.GetJsonType() == JsonType.Array && array.Count > 0)
                    {
                        // 如果array.count为0，则不能直接赋值，不然转出来的json格式不对，会变成{xxx:}
                        GetJsonDataOfNodeParent(node)[node.GetNodeName()] = array;
                    }
                    else
                    {
                        GetJsonDataOfNodeParent(node)[node.GetNodeName()].SetJsonType(JsonType.Array);
                    }
                }
            }

            var sb = new StringBuilder();
            var jsonWriter = new JsonWriter(sb);
            jsonWriter.PrettyPrint = true;
            JsonMapper.ToJson(jsonData, jsonWriter);
            File.WriteAllText(jsonFileFullPath, sb.ToString());
        }

        private JsonData GetJsonDataOfNodeParent(Node node)
        {
            Node curNode = node;
            Stack<string> parentStack = new Stack<string>();
            while (curNode.parentNode != null)
            {
                parentStack.Push(curNode.parentNode.GetNodeName());
                curNode = curNode.parentNode;
            }

            JsonData curJsonData = jsonData;
            while (parentStack.Count > 0)
            {
                curJsonData = curJsonData[parentStack.Pop()];
            }

            return curJsonData;
        }

        // 不能传入根节点
        private JsonData GetNodeValueFromJson(Node node)
        {
            return GetJsonDataOfNodeParent(node)[node.GetNodeName()];
        }
        
        private string GetNodeValueFromExcel(Node node)
        {
            return structureTree.sheet.Cells[node.row, node.col + 1].Value.ToString();
        }

        // 简便起见，最多处理2层Array
        private void JsonArray2ExcelSheet(Node node, ExcelWorksheet sheet)
        {
            // 获取dataList
            var dataList = new List<JsonData>();
            JsonData parent = GetJsonDataOfNodeParent(node);
            if (parent.IsArray)
            {
                foreach (JsonData data1 in parent)
                {
                    foreach (JsonData data2 in data1[node.GetNodeName()])
                    {
                        dataList.Add(data2);
                    }
                }
            }
            else
            {
                JsonData self = GetNodeValueFromJson(node);
                foreach (JsonData data in self)
                {
                    dataList.Add(data);
                }
            }
            
            // 写入excel表
            if (dataList.Count <= 0)
            {
                return;
            }

            int curCol = 1;
            List<string> allFields = new List<string>();
            foreach (var data in dataList)
            {
                foreach (var key in data.Keys)
                {
                    if (!allFields.Contains(key))
                    {
                        allFields.Add(key);
                        sheet.Cells[2, curCol].Value = data[key].GetJsonType().ToString().ToLower();
                        sheet.Cells[3, curCol].Value = key;
                        curCol++;
                    }
                }
            }
            
            using (var range = sheet.Cells[1, 1, 3, curCol - 1])
            {
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(255, 0, 0, 0);
                range.Style.Font.Color.SetColor(255, 255, 255, 255);
            }
            
            // 写入数据
            int curRow = 4;
            foreach (JsonData data in dataList)
            {
                curCol = 1;
                foreach (string key in allFields)
                {
                    if (!data.ContainsKey(key))
                    {
                        sheet.Cells[curRow, curCol].Value = null;
                        curCol++;
                        continue;
                    }
                    if (data[key].IsArray || data[key].IsObject)
                    {
                        sheet.Cells[curRow, curCol].Value = data[key].ToJson();
                    }
                    else
                    {
                        sheet.Cells[curRow, curCol].Value = data[key].ToString();
                    }
                    curCol++;
                }
                curRow++;
            }
        }
        
        // 简单起见，不支持多层数组
        private JsonData ExcelSheet2JsonArray(ExcelWorksheet sheet)
        {
            int colNum = 0;
            // 确定列数
            for (int col = 1; col <= sheet.Cells.Columns; col++)
            {
                if (sheet.Cells[2, col].Value != null)
                {
                    colNum++;
                }
                else
                {
                    break;
                }
            }

            // 写入jsonData
            JsonData result = new JsonData();
            int curRow = 4;
            bool isEnd = false;
            while (!isEnd)
            {
                isEnd = true;
                JsonData item = new JsonData();
                for (int i = 1; i <= colNum; i++)
                {
                    string key = sheet.Cells[3, i].Value.ToString();
                    string type = sheet.Cells[2, i].Value.ToString();
                    if (sheet.Cells[curRow, i].Value != null)
                    {
                        isEnd = false;
                        try
                        {
                            switch (type)
                            {
                                case "string":
                                    item[key] = (string)sheet.Cells[curRow, i].Value;
                                    break;
                                case "int":
                                    item[key] = int.Parse(sheet.Cells[curRow, i].Value.ToString());
                                    break;
                                case "boolean":
                                    item[key] = bool.Parse(sheet.Cells[curRow, i].Value.ToString());
                                    break;
                                case "double":
                                    item[key] = double.Parse(sheet.Cells[curRow, i].Value.ToString());
                                    break;
                                case "long":
                                    item[key] = long.Parse(sheet.Cells[curRow, i].Value.ToString());
                                    break;
                                default:
                                    item[key] = JsonMapper.ToObject((string) sheet.Cells[curRow, i].Value);
                                    break;
                            }
                        }
                        catch (Exception e)
                        {
                            UnityEngine.Debug.LogError($"导表出错位置: {sheet.Name}表 {curRow}行 {i}列");
                            throw;
                        }
                    }
                }

                if (item.Keys.Count > 0)
                {
                    result.Add(item);
                }
                curRow++;
            }

            return result;
        }
    }
}