using Game.LevelSystem;
using Game.LevelSystem.BallPacks;
using Game.PlatformSystem.Base;
using Picker.Game.Editor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace Picker.Game.Editor
{
    public class LevelEditorUtility
    {
        public const string LOAD_LEVEL_SO_PATH = "Levels";
        public const string SAVE_LEVEL_SO_PATH = "Resources/Levels";
        private const string PLATFORM_PREFABS_PATH = "PlatformPrefabs";
        private const string BALLPACK_PREFABS_PATH = "BallPacks";

        private List<PlatformBase> platformPrefabs;
        private List<BallPackBase> ballPackPrefabs;

        private PlatformBase[] itemsToSavePlatforms;
        private BallPackBase[] itemsToSaveBallPacks;

        private EditorLevelBuilder levelBuilder;

        public LevelEditorUtility(EditorLevelBuilder levelBuilder)
        {
            this.levelBuilder = levelBuilder;

            platformPrefabs = Resources.LoadAll<PlatformBase>(PLATFORM_PREFABS_PATH).ToList();
            ballPackPrefabs = Resources.LoadAll<BallPackBase>(BALLPACK_PREFABS_PATH).ToList();
        }

        // First we put platforms and ballpacks on scene properly,
        // then this method find and order them.
        public void SaveLevel(string levelName)
        {
            FindBallPacks();

            AlignPlatforms();

            CreateLevel(levelName);
        }

        private void FindBallPacks()
        {
            itemsToSaveBallPacks = Object.FindObjectsOfType<BallPackBase>().OrderBy(x => x.transform.position.z).ToArray();
        }

        private void FindPlatforms()
        {
            // Ordering By position.z improve the readibility of LevelDataScriptableObject on inspector.
            // Which platform or ballpack come first, which one is ahead of other, we can easily view on inspector by order.
            // Besides, ordering is necessary for AlignPlatforms() method, current platform , previous platform can be known by this way.
            itemsToSavePlatforms = Object.FindObjectsOfType<PlatformBase>().OrderBy(x => x.transform.position.z).ToArray();
        }

        // Align platforms on the scene, by this way no space will occur between platforms, one platform begining starts from the previous platform end.
        public void AlignPlatforms()
        {
            FindPlatforms();

            for (int i = 1; i < itemsToSavePlatforms.Length; i++)
            {
                float z;

                if (itemsToSavePlatforms[i - 1].PlatformType == PlatformType.CHECKPOINT)
                {
                    z = itemsToSavePlatforms[i - 1].transform.Find("BottomPlatform").GetComponent<Renderer>().bounds.size.z;
                }
                else
                {
                    z = itemsToSavePlatforms[i - 1].transform.Find("Mesh").GetComponent<Renderer>().bounds.size.z;
                }

                itemsToSavePlatforms[i].transform.position = itemsToSavePlatforms[i - 1].transform.position + Vector3.forward * z;
            }
        }

        private void CreateLevel(string levelName)
        {
            LevelData levelData = ScriptableObject.CreateInstance<LevelData>();

            int checkPointCounter = 0;

            foreach (var item in itemsToSavePlatforms)
            {
                PlatformData platformData = new PlatformData();

                platformData.Position = item.transform.position;
                platformData.PlatformType = item.PlatformType;
                platformData.CheckPointCount = item is CheckPoint ? levelBuilder.CheckPointCounts[checkPointCounter++] : 0;

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

        public void LoadLevel(string fileName)
        {
            var filePath = Path.Combine(LOAD_LEVEL_SO_PATH, fileName);
            var levelData = Resources.Load<LevelData>(filePath);
            
            Selection.activeObject = levelData;

            // Debug.Log($"Resources.Load<LevelData>() levelPath: {filePath} fileName: {fileName} ");

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

            var list = levelData.PlatformDatas.Where(x => x.PlatformType == PlatformType.CHECKPOINT).Select(x => x.CheckPointCount).ToList();

            levelBuilder.LoadCheckPoints(list);

        }

        private PlatformBase InstantiatePlatform(PlatformType platformType)
        {
            var platform = platformPrefabs.FirstOrDefault(x => x.PlatformType == platformType);
            platform = Object.Instantiate(platform);
            return platform;
        }

        private BallPackBase InstantiateBallPack(BallPackType ballPackType)
        {
            var ballPack = ballPackPrefabs.FirstOrDefault(x => x.BallPackType == ballPackType);
            ballPack = Object.Instantiate(ballPack);
            return ballPack;
        }

        public void ClearScene()
        {
            var levelPlatformItems = Object.FindObjectsOfType<PlatformBase>();
            foreach (var platformItem in levelPlatformItems)
                Object.DestroyImmediate(platformItem.gameObject);

            var levelBallPackItems = Object.FindObjectsOfType<BallPackBase>();
            foreach (var ballPackItem in levelBallPackItems)
                Object.DestroyImmediate(ballPackItem.gameObject);
        }
    }
}

#endif