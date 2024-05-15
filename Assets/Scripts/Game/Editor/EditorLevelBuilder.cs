using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using UnityEditor;
using Game.PlatformSystem.Base;
using Game.LevelSystem.BallPacks;
using Game.LevelSystem;
using UnityEngine.UIElements;

#if UNITY_EDITOR

namespace Picker.Game.Editor
{

    public class EditorLevelBuilder : EditorWindow
    {
        private List<string> _savedLevelNames = new List<string>();
        private string NewLevelName = string.Empty;
        private List<int> checkPointCounts = new List<int>();
        public List<int> CheckPointCounts => checkPointCounts;

        private LevelEditorUtility levelUtility;

        [MenuItem("Tools/EditorLevelBuilder")]
        private static void Init()
        {
            EditorLevelBuilder window = (EditorLevelBuilder)GetWindow(typeof(EditorLevelBuilder));
            window.Show();
        }

        private void OnEnable()
        {
            levelUtility = new LevelEditorUtility(this);
            Debug.Log("[EditorLevelBuilder] OnEnable() called...");
            ResetCheckPointCounts();
        }

        // It is useful to refresh checkpoints when an gameObject added or removed from scene, it might me CheckPoint, we are checking that.
        private void OnHierarchyChange()
        {
            Debug.Log("OnHierarchyChanged() called...");

            IsCheckPointCountChanged();
        }

        private void OnGUI()
        {
            for (int i = 0; i < checkPointCounts.Count; i++)
            {
                checkPointCounts[i] = EditorGUILayout.IntField($"CheckPoints ({i}) Count: ", checkPointCounts[i]);
            }

            NewLevelName = EditorGUILayout.TextField("Level Name : ", NewLevelName);

            if (GUILayout.Button("Save Level"))
            {
                // Check if Level Name is invalid, empty or there is a filename with the same name in the path.
                if (!string.IsNullOrEmpty(NewLevelName)
                    && NewLevelName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0
                    && !File.Exists(Path.Combine(Application.dataPath, LevelEditorUtility.SAVE_LEVEL_SO_PATH, NewLevelName + ".asset")))
                {
                    levelUtility.SaveLevel(NewLevelName);
                }
                else
                {
                    var message = $"New Level Name \"{NewLevelName}\" is either invalid, empty or there is a file with that name already!!!";
                    // rootVisualElement.Add(new HelpBox(message, HelpBoxMessageType.Warning));
                    // EditorGUILayout.HelpBox(message, MessageType.Warning, true);
                    EditorUtility.DisplayDialog("Save Level Warning", message, "Ok");
                    Debug.LogWarning("[EditorLevelBuilder] " + message);
                }
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Show Saved Levels"))
            {
                _savedLevelNames = GetLevelNames();
            }

            GUILayout.Space(25);

            if (GUILayout.Button("Align Platforms"))
            {
                
                levelUtility.AlignPlatforms();
            }

            GUILayout.Space(30);

            foreach (var t in _savedLevelNames)
            {
                if (GUILayout.Button(t))
                {
                    levelUtility.LoadLevel(t);
                    NewLevelName = t;
                }
            }

            GUILayout.Space(50);

            if (GUILayout.Button("Clear Scene"))
            {
                levelUtility.ClearScene();
            }
        }

        // Is CheckPoint Platform Count Changed On Scene?
        private void IsCheckPointCountChanged()
        {
            // CheckPointsCount on scene before OnHierarchyChange is equal to checkpoints count on OnHierarchyChange?
            // This means, is there a new CheckPoint added or removed on Scene? If equal, it means there is no change, so no need to ResetCheckPointCounts().
            if (checkPointCounts.Count == GetCheckPointsCountOnScene())
                return;

            ResetCheckPointCounts();
        }

        // Finds the CheckPoints platforms count on scene.
        private int GetCheckPointsCountOnScene()
        {
            return FindObjectsOfType<CheckPoint>().Length;
        }

        // Clear and Add checkPointcounts list because we cant show enough IntField and save them on OnGUI otherwise.
        private void ResetCheckPointCounts()
        {
            checkPointCounts.Clear();

            var count = GetCheckPointsCountOnScene();

            for (int i = 0; i < count; i++)
            {
                checkPointCounts.Add(0);
            }
        }

        public void LoadCheckPoints(List<int> loadedCheckPoints)
        {
            checkPointCounts.Clear();
            checkPointCounts.AddRange(loadedCheckPoints);
        }

        private List<string> GetLevelNames()
        {
            string partialName = string.Empty;

            DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(Path.Combine(Application.dataPath, LevelEditorUtility.SAVE_LEVEL_SO_PATH));
            FileSystemInfo[] filesAndDirs = hdDirectoryInWhichToSearch.GetFileSystemInfos("*" + partialName + "*.asset");

            // Debug.Log("[EditorLevelBuilder] Application.dataPath: " + Application.dataPath);

            return filesAndDirs.Select(foundFile => Path.GetFileNameWithoutExtension(foundFile.Name)).ToList();
        }
    }
}
#endif