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

namespace Picker.Game.Editor
{

#if UNITY_EDITOR
    public class EditorLevelBuilder : EditorWindow
    {
        private List<string> _savedLevelNames = new List<string>();
        private string NewLevelName = string.Empty;
        private const string LOAD_LEVEL_SO_PATH = "NewLevels";
        private const string SAVE_LEVEL_SO_PATH = "Resources/NewLevels";
        private const string PLATFORM_PREFABS_PATH = "PlatformPrefabs";
        private const string BALLPACK_PREFABS_PATH = "BallPacks";
        #region Variables

        List<PlatformBase> platformPrefabs;
        List<BallPackBase> ballPackPrefabs;
        #endregion

        [MenuItem("Tools/EditorLevelBuilder")]
        private static void Init()
        {
            EditorLevelBuilder window = (EditorLevelBuilder)GetWindow(typeof(EditorLevelBuilder));
            window.Show();
        }

        private void OnEnable()
        {
            platformPrefabs = Resources.LoadAll<PlatformBase>(PLATFORM_PREFABS_PATH).ToList();
            ballPackPrefabs = Resources.LoadAll<BallPackBase>(BALLPACK_PREFABS_PATH).ToList();
        }

        private void OnGUI()
        {
            NewLevelName = GUI.TextField(new Rect(10, 10, position.width, 20), NewLevelName, 25);

            if (GUI.Button(new Rect(10, 40, position.width, 20), "Save Level"))
            {
                if (!string.IsNullOrEmpty(NewLevelName)
                    && NewLevelName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0
                    && !File.Exists(Path.Combine(Application.dataPath, SAVE_LEVEL_SO_PATH, NewLevelName + ".asset")))
                {
                    SaveLevelAsScriptableObject(NewLevelName);
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

            if (GUI.Button(new Rect(10, 70, position.width, 20), "Show Saved Levels"))
            {
                _savedLevelNames = GetLevelNames();
            }

            GUILayout.BeginArea(new Rect(10, 150, position.width, position.height));

            foreach (var t in _savedLevelNames)
            {
                if (GUILayout.Button(t))
                {
                    LoadLevelFromScriptableObject(t);
                }
            }

            GUILayout.Space(50);

            if (GUILayout.Button("Clear Scene"))
            {
                ClearScene();
            }

            GUILayout.EndArea();


        }

        private void SaveLevelAsScriptableObject(string levelName)
        {
            // Ordering By position.z improve the readibility of LevelDataScriptableObject on inspector.
            // Which platform or ballpack come first, which one is ahead of other, we can easily view on inspector by order.
            var itemsToSavePlatforms = FindObjectsOfType<PlatformBase>().OrderBy(x => x.transform.position.z).ToArray();
            var itemsToSaveBallPacks = FindObjectsOfType<BallPackBase>().OrderBy(x => x.transform.position.z).ToArray();

            AlignPlatforms(itemsToSavePlatforms);
            CreateScriptableObject(levelName, itemsToSavePlatforms, itemsToSaveBallPacks);
        }

        // Align platforms on the scene, by this way no space will occur between platforms, one platform begining starts from the previous platform end.
        private void AlignPlatforms(PlatformBase[] platformsToAlign)
        {
            for (int i = 0; i < platformsToAlign.Length; i++)
            {
                if (i == 0)
                    continue;

                float z = 0;
                
                if (platformsToAlign[i - 1].PlatformType == PlatformType.CHECKPOINT)
                {
                    z = platformsToAlign[i - 1].transform.Find("BottomPlatform").GetComponent<Renderer>().bounds.size.z;
                }
                else
                {
                    z = platformsToAlign[i - 1].transform.Find("Mesh").GetComponent<Renderer>().bounds.size.z;
                }

                platformsToAlign[i].transform.position = platformsToAlign[i-1].transform.position + Vector3.forward * z;
            }
        }

        private void CreateScriptableObject(string levelName, PlatformBase[] itemsToSavePlatforms, BallPackBase[] itemsToSaveBallPacks)
        {
            LevelData levelData = ScriptableObject.CreateInstance<LevelData>();

            foreach (var item in itemsToSavePlatforms)
            {
                PlatformData platformData = new PlatformData();

                platformData.Position = item.transform.position;
                platformData.PlatformType = item.PlatformType;
                platformData.CheckPointCount = 0;

                levelData.PlatformDatas.Add(platformData);
            }

            foreach (var item in itemsToSaveBallPacks)
            {
                BallPackData ballPackData = new BallPackData();

                ballPackData.Position = item.transform.position;
                ballPackData.BallPackType = item.BallPackType;

                levelData.BallPackDatas.Add(ballPackData);
            }

            var savePath = Path.Combine("Assets", SAVE_LEVEL_SO_PATH, levelName + ".asset");
            Debug.Log("AssetDatabase.CreateAsset level scriptable object save path: " + savePath);

            AssetDatabase.CreateAsset(levelData, savePath);
            AssetDatabase.Refresh();

            Selection.activeObject = levelData;
        }

        private void LoadLevelFromScriptableObject(string fileName)
        {
            var filePath = Path.Combine(LOAD_LEVEL_SO_PATH, fileName);
            var levelData = Resources.Load<LevelData>(filePath);

            Selection.activeObject = levelData;
            Debug.Log($"Resources.Load<LevelData>() levelPath: {filePath} fileName: {fileName} ");
            LoadScene(levelData);
        }

        private void LoadScene(LevelData levelData)
        {
            ClearScene();

            foreach (var platformItem in levelData.PlatformDatas)
            {
                var platform = InstantiatePlatform(platformItem.PlatformType);
                platform.transform.position = platformItem.Position;
            }

            foreach (var ballPackItem in levelData.BallPackDatas)
            {
                var ballPack = InstantiateBallPack(ballPackItem.BallPackType);
                ballPack.transform.position = ballPackItem.Position;
            }
        }

        private PlatformBase InstantiatePlatform(PlatformType platformType)
        {
            var platform = platformPrefabs.FirstOrDefault(x => x.PlatformType == platformType);
            platform = Instantiate(platform);
            return platform;
        }

        private BallPackBase InstantiateBallPack(BallPackType ballPackType)
        {
            var ballPack = ballPackPrefabs.FirstOrDefault(x => x.BallPackType == ballPackType);
            ballPack = Instantiate(ballPack);
            return ballPack;
        }

        private void ClearScene()
        {
            var levelPlatformItems = FindObjectsOfType<PlatformBase>();
            foreach (var platformItem in levelPlatformItems)
                DestroyImmediate(platformItem.gameObject);

            var levelBallPackItems = FindObjectsOfType<BallPackBase>();
            foreach (var ballPackItem in levelBallPackItems)
                DestroyImmediate(ballPackItem.gameObject);
        }

        private List<string> GetLevelNames()
        {
            string partialName = string.Empty;

            DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(Path.Combine(Application.dataPath, SAVE_LEVEL_SO_PATH));
            FileSystemInfo[] filesAndDirs = hdDirectoryInWhichToSearch.GetFileSystemInfos("*" + partialName + "*.asset");

            Debug.Log("[EditorLevelBuilder] Application.dataPath: " + Application.dataPath);

            return filesAndDirs.Select(foundFile => Path.GetFileNameWithoutExtension(foundFile.Name)).ToList();
        }

    }
}
#endif