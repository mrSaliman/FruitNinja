using System.Collections.Generic;
using System.Linq;
using App.GameScene.Blocks;
using App.GameScene.Visualization;
using UnityEngine;
using Random = UnityEngine.Random;

namespace App.GameScene.Gameplay_Management.Block_Management
{
    public class BlockThrowManager : Manager
    {
        [SerializeReference] private CameraManager cameraManager;
        private Rect _cameraSize;
        
        [SerializeReference] private BlockInteractionManager blockInteractionManager;
        
        private RandomBlockThrower _randomBlockThrower;
        
        [SerializeField] private BlockThrowManagerSettings settings;

        [SerializeField] private List<Sprite> sprites;
        [SerializeField] private List<Block> prefabs;

        [HideInInspector] public List<ThrowZone> throwZones;


        private readonly List<Block> _currentPack = new();
        private ThrowZone _currentThrowZone;

        private float _blockTimer,
            _packTimer,
            _difficulty,
            _throwPackDelay,
            _throwBlockDelay,
            _difficultyFactor,
            _maxDifficulty;
        private bool _stop = true;
        private int _blockIndex;
        
        public override void Init()
        {
            _randomBlockThrower = new RandomBlockThrower();
            StartThrowingLoop();
        }

        private void Update()
        {
            if (_stop) return;
            _blockTimer -= Time.deltaTime;
            _packTimer -= Time.deltaTime;
            ThrowingLoop();
        }

        private void ThrowingLoop()
        {
            if (throwZones is null || throwZones.Count == 0) return;
            if (_currentPack.Count - _blockIndex > 0)
            {
                if (!(_blockTimer <= 0)) return;
                _currentThrowZone = GetRandomThrowZone();
                _randomBlockThrower.Throw(_currentPack[_blockIndex], _currentThrowZone);
                _blockIndex++;
                _blockTimer = _throwBlockDelay * _difficulty;
                
                if (_currentPack.Count - _blockIndex == 1) _packTimer = _throwPackDelay * _difficulty;
            }
            else if (_packTimer <= 0)
            {
                GeneratePack(settings.PackSizeRange);
                if (_difficulty > _maxDifficulty) _difficulty *= _difficultyFactor;
            }
        }

        private void StartThrowingLoop()
        {
            _stop = false;
            
            _throwPackDelay = settings.BaseThrowPackDelay;
            _throwBlockDelay = settings.BaseThrowBlockDelay;
            
            _packTimer = _throwPackDelay;
            _blockTimer = _throwBlockDelay;
            _blockIndex = 0;
            
            _difficulty = 1f;
            _difficultyFactor = settings.DifficultyFactor;
            _maxDifficulty = settings.MaxDifficulty;
            
            _cameraSize = cameraManager.CameraRect;
        }

        private void GeneratePack(Vector2Int packSizeRange)
        {
            _blockIndex = 0;
            _currentPack.Clear();
            System.Random random = new();
            
            _cameraSize = cameraManager.CameraRect;
            _randomBlockThrower.CameraSize = _cameraSize;
            
            var size = random.Next(packSizeRange.x, packSizeRange.y + 1);
            
            for (var i = 0; i < size; i++)
            {
                var block = Instantiate(prefabs[random.Next(prefabs.Count)], _cameraSize.size, Quaternion.identity);
                block.SetSprite(sprites[random.Next(sprites.Count)]);
                _currentPack.Add(block);
                blockInteractionManager.AddBlock(block);
            }
        }
        
        private ThrowZone GetRandomThrowZone()
        {
            if (throwZones is null || throwZones.Count == 0) return null;
            var totalProbability = throwZones.Sum(throwZone => throwZone.probability);
            var randomValue = Random.value * totalProbability;
            foreach (var throwZone in throwZones)
            {
                if (randomValue < throwZone.probability)
                {
                    return throwZone;
                }
                randomValue -= throwZone.probability;
            }
            
            return throwZones[^1];
        }

        private void OnDrawGizmos()
        {
            GizmosDrawer.DrawThrowZones(throwZones, cameraManager);
        }
    }
}