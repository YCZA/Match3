using System.Collections.Generic;
using OfficeOpenXml;

// 1. 方法1：每个格子只关心自己下一步怎么走
// 2. 方法2：通过某种算法，把格子按顺序读出来(好像只要按从左到右，左上到下的顺序读就好了)
namespace Match3.Scripts2.SRDebug.Editor.TableToolPlus.Structure
{
    public class StructureTree
    {
        public ExcelWorksheet sheet;
        public List<Node> nodeList = new List<Node>();

        public StructureTree(ExcelWorksheet sheet)
        {
            this.sheet = sheet;
            ReadAllNode();
        }

        public Node GetRootNode()
        {
            string root = sheet.Cells[1, 1].Value.ToString();
            return new Node(root, 1, 1);
        }

        private void ReadAllNode()
        {
            // 简化处理，就当结构树最深为32层, 最高为128层
            int curRow = 1;
            int curCol = 1;
            while (curRow < 129)
            {
                var curCell = sheet.Cells[curRow, curCol].Value?.ToString();
                if (!string.IsNullOrEmpty(curCell))
                {
                    // 添加节点
                    var node = new Node(curCell, curRow, curCol);
                    if (!string.IsNullOrEmpty(node.GetNodeType()))
                    {
                        nodeList.Add(node);
                        // 添加父节点
                        for (int i = nodeList.Count - 1; i > 0; i--)
                        {
                            if (nodeList[i].col == curCol - 1)
                            {
                                node.parentNode = nodeList[i];
                                break;
                            }
                        }
                    }
                }
                
                if (curCol < 32)
                {
                    curCol++;
                }
                else
                {
                    curCol = 1;
                    curRow++;
                }
            }
        }

        #region oldCode
        // private bool isWalkToEnd;
        // public Node curNode
        // curNode = GetRootNode();
        // public bool IsWalkToEnd()
        // {
        //     // var rightCell = sheet.Cells[curNode.row, curNode.col + 1].Value;
        //     // var downCell = sheet.Cells[curNode.row + 1, curNode.col].Value;
        //     // return string.IsNullOrEmpty(rightCell.ToString()) && string.IsNullOrEmpty(downCell.ToString());
        //     return isWalkToEnd;
        // }
        
        // public Node WalkToNextTailNode()
        // {
        //     
        // }

        // public Node WalkToNextNode()
        // {
        //     var rightCell = sheet.Cells[curNode.row, curNode.col + 1].Value;
        //     var downCell = sheet.Cells[curNode.row + 1, curNode.col].Value;
        //
        //     if (string.IsNullOrEmpty(rightCell.ToString()) && string.IsNullOrEmpty(downCell.ToString()))
        //     {
        //         // 回头
        //         if (curNode.row == 1 && curNode.col == 1)
        //         {
        //             isWalkToEnd = true;
        //             return null;
        //         }
        //
        //         int curRow = curNode.row;
        //         while (string.IsNullOrEmpty((string) sheet.Cells[curRow, curNode.col - 1].Value))
        //         {
        //             
        //         }
        //         
        //         if (curRow == 1 && curNode.col - 1 == 1)
        //         {
        //             isWalkToEnd = true;
        //             return null;
        //         }
        //         
        //         
        //     }
        //     else if (string.IsNullOrEmpty(rightCell.ToString()) && !string.IsNullOrEmpty(downCell.ToString()))
        //     {
        //        // 向下走 
        //        curNode = new Node(downCell.ToString(), curNode.row + 1, curNode.col);
        //     }
        //     else
        //     {
        //         // 向右走
        //        curNode = new Node(rightCell.ToString(), curNode.row, curNode.col + 1);
        //     }
        //
        //     return curNode;
        // }

        // private void WalkToNextRightNode()
        // {
        //     var rightCell = sheet.Cells[curNode.row, curNode.col + 1].Value;
        //     if (string.IsNullOrEmpty(rightCell.ToString()))
        //     {
        //         WalkToNextDownNode();
        //     }
        //     curNode = new Node(rightCell.ToString(), curNode.row + 1, curNode.col);
        //     WalkToNextRightNode();   
        // }

        // private void WalkToNextDownNode()
        // {
        //     var downCell = sheet.Cells[curNode.row + 1, curNode.col].Value;
        //     var rightDownCell = sheet.Cells[curNode.row + 1, curNode.col + 1].Value;
        //     
        //     // 如果当前cell不为空，则执行walkToNextRightNode
        //     if (!string.IsNullOrEmpty(downCell.ToString()))
        //     {
        //         WalkToNextRightNode();
        //     }
        //     // 如果当前cell为空，但右边不为空，则继续向下(可以用递归，也可以用while, 用递归吧)
        //     else if (string.IsNullOrEmpty(downCell.ToString()) && !string.IsNullOrEmpty(rightDownCell.ToString()))
        //     {
        //         WalkToNextDownNode();
        //     }
        //     // 如果当前cell为空，右边也为空，则返回父节点
        //     else if (string.IsNullOrEmpty(downCell.ToString()) && string.IsNullOrEmpty(rightDownCell.ToString()))
        //     {
        //         WalkToParentNode();
        //     }
        // }

        // private Node WalkToParentNode()
        // {
        //     // 判断是不是root, 不是的话，向下
        //     // 是的话，结束
        //     
        // }
        #endregion
    }
}