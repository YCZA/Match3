using Match3.Scripts1;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Match3.Scripts2.Home.HomeMap.Editor
{
    [CustomEditor(typeof(TerrainGenerator))]
    public class TerrainGeneratorInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("生成"))
            {
                (target as TerrainGenerator).Recalculate();
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                UnityEngine.Debug.Log("地形生成完成");
            }
        }
    }
}
