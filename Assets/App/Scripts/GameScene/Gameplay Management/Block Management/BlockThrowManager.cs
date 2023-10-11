using System.Collections;
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
        private CameraManager _cameraManager;
        private Rect _cameraSize;

        private Thrower _thrower;

        [SerializeReference] private BlockInteractionManager blockInteractionManager;

        [SerializeField] private BlockThrowManagerSettings settings;

        [SerializeField] private List<Sprite> sprites;
        [SerializeField] private List<Block> prefabs;

        public List<ThrowZone> ThrowZones = new List<ThrowZone>();

        private readonly List<Block> _currentPack = new();

        private bool _stop;
        
        public override void Init(CameraManager cameraManager)
        {
            _cameraManager = cameraManager;
            _thrower = new Thrower(cameraManager);
            StartCoroutine(StartThrowingLoop());
        }

        public IEnumerator StartThrowingLoop()
        {
            _cameraSize = _cameraManager.CameraRect;
            var throwPackDelay = settings.BaseThrowPackDelay;
            var throwBlockDelay = settings.BaseThrowBlockDelay;
            var difficultyFactor = settings.DifficultyFactor;
            var maxDifficulty = settings.MaxDifficulty;
            var difficulty = 1f;
            
            do
            {
                GeneratePack(settings.PackSizeRange);
                yield return StartCoroutine(ThrowPack(_currentPack, throwBlockDelay * difficulty));
                if (difficulty > maxDifficulty) difficulty *= difficultyFactor;
                yield return new WaitForSeconds(throwPackDelay);
            } while (!_stop);
        }

        private void GeneratePack(Vector2Int packSizeRange)
        {
            _currentPack.Clear();
            System.Random random = new();
            var size = random.Next(packSizeRange.x, packSizeRange.y + 1);
            
            for (var i = 0; i < size; i++)
            {
                var block = Instantiate(prefabs[random.Next(prefabs.Count)], _cameraSize.size, Quaternion.identity);
                block.SetSprite(sprites[random.Next(sprites.Count)]);
                _currentPack.Add(block);
                blockInteractionManager.AddBlock(block);
            }
        }

        private IEnumerator ThrowPack(List<Block> blocks, float delay)
        {
            var throwZone = GetRandomThrowZone();
            
            foreach (var block in blocks)
            {
                _thrower.Throw(block, throwZone);
                yield return new WaitForSeconds(delay);
            }
            
        }
        
        private ThrowZone GetRandomThrowZone()
        {
            if (ThrowZones.Count == 0) return null;
            var totalProbability = ThrowZones.Sum(throwZone => throwZone.Probability);
            var randomValue = Random.value * totalProbability;
            foreach (var throwZone in ThrowZones)
            {
                if (randomValue < throwZone.Probability)
                {
                    return throwZone;
                }
                randomValue -= throwZone.Probability;
            }
            
            return ThrowZones[^1];
        }

        private void OnDrawGizmos()
        {
            GizmosDrawer.DrawThrowZones(ThrowZones, _cameraManager);
        }
    }
}