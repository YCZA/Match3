using System.Collections;
using Match3.Scripts1;
using Match3.Scripts1.Shared.DataStructures;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Match3.Scripts2.Home.HomeMap.Editor
{
    [CustomEditor(typeof(TownPathfinding))]
    public class TownPathfindingInspector : UnityEditor.Editor
    {
        private TownPathfinding self;
        private const int mapWidth = 128;

        private void OnEnable()
        {
            self = target as TownPathfinding;
        }

        public override void OnInspectorGUI()
        {
            // areaid从1开始
            // mapData里的数据从0开始
            base.OnInspectorGUI();
            if (GUILayout.Button("生成网格"))
            {
                GenerateGrid();
                GenerateCloudByAreaInfos();

                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                UnityEngine.Debug.Log("网格生成完成");
            }
        }

        private void GenerateGrid()
        {
            Map1d<byte> mapData = new Map1d<byte>(mapWidth);
            mapData.Fill(self.mapConfigBinary.bytes);
            int maxAreaId = 0;
            foreach (var id in self.mapConfigBinary.bytes)
            {
                if (id > maxAreaId)
                {
                    maxAreaId = id;
                }
            }

            self.areaInfos = new AreaInfo[maxAreaId];
            for (int i = 0; i < self.areaInfos.Length; i++)
            {
                self.areaInfos[i] = new AreaInfo();
            }

            for (int areaId = 1; areaId <= maxAreaId; areaId++)
            {
                var areaMesh = self.BuildMeshForArea(areaId, (i, j) =>
                {
                    byte value = mapData[new IntVector2(i, j)];
                    if (value == areaId)
                    {
                        // 构建mesh的同时向tiles中添加数据
                        if (!self.areaInfos[areaId - 1].tiles.Contains(new IntVector2(i, j)))
                            self.areaInfos[areaId - 1].tiles.Add(new IntVector2(i, j));
                        return true;
                    }

                    return false;
                });
                // cloudMesh比AreaMesh大一圈
                var id = areaId;
                var cloudMesh = self.BuildMeshForArea(areaId, (i, j) =>
                {
                    byte value = mapData[new IntVector2(i, j)];
                    if (value == id)
                    {
                        return true;
                    }else
                    {
                        int boardWidth = 0;
                        boardWidth += 1;
                        // 构建cloudMesh
                        if (mapData[new IntVector2(i + boardWidth, j)] == id) return true;
                        if (mapData[new IntVector2(i, j + boardWidth)] == id) return true;
                        if (mapData[new IntVector2(i - boardWidth, j)] == id) return true;
                        if (mapData[new IntVector2(i, j - boardWidth)] == id) return true;
                        if (mapData[new IntVector2(i + boardWidth, j + boardWidth)] == id) return true;
                        if (mapData[new IntVector2(i - boardWidth, j + boardWidth)] == id) return true;
                        if (mapData[new IntVector2(i + boardWidth, j - boardWidth)] == id) return true;
                        if (mapData[new IntVector2(i - boardWidth, j - boardWidth)] == id) return true;
                    }

                    return false;
                });
                self.areaInfos[areaId - 1].areaMesh = areaMesh;
                self.areaInfos[areaId - 1].cloudMesh = cloudMesh;
            }
        }

        // cloudMeshMask图要比cloudMesh大一圈, 相当于cloudMeshMask图比AreaMesh大两圈
        private void GenerateCloudByAreaInfos()
        {
            int i = 1;
            foreach (var area in self.areaInfos)
            {
                Texture2D texture = new Texture2D(mapWidth, mapWidth);
                Color[] colorArray = new Color[mapWidth * mapWidth];
                ArrayList.Repeat(new Color(0,0,0,0), mapWidth * mapWidth).CopyTo(colorArray);
                texture.SetPixels(0, 0, mapWidth, mapWidth, colorArray);
                
                foreach (var tile in area.tiles)
                {
                    texture.SetPixel(tile.y, tile.x, Color.yellow);
                    int boardWidth = 0;
                    // 向外多画一圈
                    boardWidth += 1;
                    texture.SetPixel(tile.y + boardWidth, tile.x, Color.yellow);
                    texture.SetPixel(tile.y, tile.x + boardWidth, Color.yellow);
                    texture.SetPixel(tile.y + boardWidth, tile.x + boardWidth, Color.yellow);
                    texture.SetPixel(tile.y + boardWidth, tile.x - boardWidth, Color.yellow);
                    texture.SetPixel(tile.y - boardWidth, tile.x + boardWidth, Color.yellow);
                    texture.SetPixel(tile.y - boardWidth, tile.x - boardWidth, Color.yellow);
                    texture.SetPixel(tile.y - boardWidth, tile.x, Color.yellow);
                    texture.SetPixel(tile.y, tile.x - boardWidth, Color.yellow);
                    
                    // 向外多画两圈
                    boardWidth += 1;
                    texture.SetPixel(tile.y + boardWidth, tile.x, Color.yellow);
                    texture.SetPixel(tile.y, tile.x + boardWidth, Color.yellow);
                    texture.SetPixel(tile.y + boardWidth, tile.x + boardWidth, Color.yellow);
                    texture.SetPixel(tile.y + boardWidth, tile.x - boardWidth, Color.yellow);
                    texture.SetPixel(tile.y - boardWidth, tile.x + boardWidth, Color.yellow);
                    texture.SetPixel(tile.y - boardWidth, tile.x - boardWidth, Color.yellow);
                    texture.SetPixel(tile.y - boardWidth, tile.x, Color.yellow);
                    texture.SetPixel(tile.y, tile.x - boardWidth, Color.yellow);
                }
                
                texture.Apply();
                
                // 保存texture
                // File.WriteAllBytes( Path.Combine(Environment.CurrentDirectory, "Assets/New/Generation/Cloud/OriginalImage") + "/cloud" + i + ".png", texture.EncodeToPNG());

                texture = self.BlurMapTexture(texture, i);
                area.cloudTexture = texture;

                i++;
            }
        }
    }
}
