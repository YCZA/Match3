using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3.Scripts2.SRDebug.Editor
{
    public class Tools 
    {
        [MenuItem("Tools/清除存档/确定?", false, 21)]
        public static void ClearArchives()
        {
            PlayerPrefs.DeleteAll();
            var dir = new DirectoryInfo(Application.persistentDataPath);
            dir.Delete(true);
            UnityEngine.Debug.Log("存档已清除");
        }
        [MenuItem("Tools/清除存档/取消", false, 21)]
        public static void CancelClearArchives()
        {
            UnityEngine.Debug.Log("已取消");
        }
        
        // [MenuItem("Tools/版本切换(已弃用)/正常版本")]
        // public static void SwitchToNormalVersion()
        // {
        //     PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "");
        //     UnityEngine.Debug.Log("已切换至普通版本");
        // }
        // [MenuItem("Tools/版本切换(已弃用)/审核版本")]
        // public static void SwitchToReviewVersion()
        // {
        //     PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "REVIEW_VERSION");
        //     UnityEngine.Debug.Log("已切换至审核版本");
        // }

        [MenuItem("Tools/打开存档目录", false, 20)]
        public static void OpenPersistentDataPath()
        {
            // EditorUtility.RevealInFinder(Application.persistentDataPath);
            Application.OpenURL(Application.persistentDataPath);
        }
        
        [MenuItem("Tools/✨✨✨打开myfile目录 #E", false, 40)]
        public static void OpenMyFilePath()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "myfile");
            // EditorUtility.RevealInFinder(path);
            Application.OpenURL(path);
            
        }
        
        [MenuItem("Tools/快速定位可点击的UI #S", false, 0)]
        public static void QuickPositioning()
        {
            // 方法一
            if (Application.isPlaying == false)
            {
                return;
            }
            
            //使焦点移动到Game视图
            Type gameViewType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GameView");
            EditorWindow window = EditorWindow.GetWindow(gameViewType);
            window.Focus();
            
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            
            //获取鼠标位置所有碰撞对象
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);
            
            if (raycastResults.Count > 0)
            {
                //选择第一个对象
                Selection.activeGameObject = raycastResults[0].gameObject;
            
                EditorGUIUtility.PingObject(raycastResults[0].gameObject);
            }
            
            // 方法二
            // Event e = Event.current;
            // var pos = e.mousePosition;
            //
            // Debug.LogError(pos);
            // Debug.LogError(Input.mousePosition);
        }
    }
}