using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Game.LevelSystem;
using Game.LevelSystem.BallPacks;
using Game.PlatformSystem.Base;
using Helpers;
using UnityEngine;

namespace Game.Managers
{
    public class AssetManager : GenericSingleton<AssetManager>
    {
        private const string LEVEL_PATH = "Levels/Level";
        private const string BALLPACK_PATH = "BallPacks";
        private const string PLATFORM_PATH = "PlatformPrefabs";

        private List<PlatformBase> _platformBases;
        private List<BallPackBase> _ballPackBases;
        
        public Material GroundMaterial;
        public Material PickerMaterial;
        
        public LevelData LoadLevel(int levelindex)
        {
            return Resources.Load<LevelData>(LEVEL_PATH + levelindex);
        }
    }
}
