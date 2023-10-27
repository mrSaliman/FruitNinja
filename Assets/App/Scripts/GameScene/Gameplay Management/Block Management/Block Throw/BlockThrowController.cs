﻿using System.Collections.Generic;
using System.Linq;
using App.GameScene.Blocks;
using App.GameScene.Blocks.SpecialBlocks;
using App.GameScene.Gameplay_Management.Block_Management.Block_Assignment;
using App.GameScene.Gameplay_Management.Block_Management.Block_Interaction;
using App.GameScene.Gameplay_Management.State;
using App.GameScene.Settings;
using App.GameScene.Visualization;
using UnityEngine;
using Random = UnityEngine.Random;

namespace App.GameScene.Gameplay_Management.Block_Management.Block_Throw
{
    public class BlockThrowController : BaseController
    {
        private CameraInfoProvider _cameraInfoProvider;
        private Rect _cameraSize;
        
        private BlockInteractionController _blockInteractionController;
        
        private RandomBlockThrower _randomBlockThrower;
        
        [SerializeField] private BlockThrowControllerSettings settings;
        [SerializeField] private BlockAssignmentsContainer blockAssignmentsContainer;
        
        [HideInInspector] public List<ThrowZone> throwZones;


        private readonly List<Block> _currentPack = new();
        private ThrowZone _currentThrowZone;

        private float _blockTimer,
            _packTimer,
            _difficulty,
            _throwPackDelay,
            _throwBlockDelay,
            _difficultyFactor,
            _maxDifficulty,
            _minScoreBlockPercent,
            _totalThrowZonesProbability,
            _totalBlockProbability;

        private int _blockIndex, _scoreBlockId;
        
        public override void Init()
        {
            _currentPack.Clear();
            _cameraInfoProvider = ControllerLocator.Instance.GetController<CameraInfoProvider>();
            _blockInteractionController = ControllerLocator.Instance.GetController<BlockInteractionController>();
            _randomBlockThrower = new RandomBlockThrower();
            StartThrowingLoop();
        }

        private void Update()
        {
            if (CurrentGameState is GameState.Paused) return;
            if (CurrentGameState is GameState.GameOver)
            {
                if (_currentPack.Count - _blockIndex <= 0) return;
                for (var i = _blockIndex; i < _currentPack.Count; i++)
                {
                    _blockInteractionController.DeleteBlock(_currentPack[i]);
                }
                _currentPack.Clear();
                return;
            }
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
            _throwPackDelay = settings.BaseThrowPackDelay;
            _throwBlockDelay = settings.BaseThrowBlockDelay;
            
            _packTimer = _throwPackDelay;
            _blockTimer = _throwBlockDelay;
            _blockIndex = 0;
            
            _difficulty = 1f;
            _difficultyFactor = settings.DifficultyFactor;
            _maxDifficulty = settings.MaxDifficulty;
            
            _cameraSize = _cameraInfoProvider.CameraRect;

            _scoreBlockId = blockAssignmentsContainer.ScoreBlockAssignmentId;
            _minScoreBlockPercent = settings.MinScoreBlockPercent;

            _totalThrowZonesProbability = throwZones.Sum(throwZone => throwZone.probability);
            _totalBlockProbability =
                blockAssignmentsContainer.BlockAssignments.Sum(blockAssignment => blockAssignment.probability);
        }

        private void GeneratePack(Vector2Int packSizeRange)
        {
            _blockIndex = 0;
            _currentPack.Clear();
            System.Random random = new();
            
            _cameraSize = _cameraInfoProvider.CameraRect;
            _randomBlockThrower.CameraSize = _cameraSize;
            
            var size = random.Next(packSizeRange.x, packSizeRange.y + 1);
            var scoreBlockCount = 0;
            var requiredScoreBlockAmount = Mathf.CeilToInt(_minScoreBlockPercent * size);
            
            for (var i = 0; i < size; i++)
            {
                var randomValue = random.NextDouble() * _totalBlockProbability;
                var blockTypeId = _scoreBlockId;
                for (var j = 0; j < blockAssignmentsContainer.BlockAssignments.Count; j++)
                {
                    var assignment = blockAssignmentsContainer.BlockAssignments[j];
                    if (randomValue < assignment.probability)
                    {
                        blockTypeId = j;
                        break;
                    }

                    randomValue -= assignment.probability;
                }

                if (blockAssignmentsContainer.BlockAssignments[blockTypeId].blockPrefab is Brick &&
                    _blockInteractionController.BrickQuantity > 0) blockTypeId = _scoreBlockId;
                if (blockTypeId == _scoreBlockId) scoreBlockCount++;
                else if (requiredScoreBlockAmount - scoreBlockCount == size - i)
                {
                    blockTypeId = _scoreBlockId;
                    scoreBlockCount++;
                }
                var blockAssignment = blockAssignmentsContainer.BlockAssignments[blockTypeId];
                var block = Instantiate(blockAssignment.blockPrefab, _cameraSize.size, Quaternion.identity);
                var sspAssignment =
                    blockAssignment.sspAssignments[random.Next(blockAssignment.sspAssignments.Count)];
                block.SetSprite(sspAssignment.sprite);
                block.splash = sspAssignment.splash;
                block.particleColor = sspAssignment.particleColor;
                _currentPack.Add(block);
                _blockInteractionController.AddBlock(block);
            }
        }
        
        private ThrowZone GetRandomThrowZone()
        {
            if (throwZones is null || throwZones.Count == 0) return null;
            var randomValue = Random.value * _totalThrowZonesProbability;
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

#if UNITY_EDITOR
        [SerializeField] private CameraInfoProvider camEditorInfo;
        
        private void OnDrawGizmos()
        {
            GizmosDrawer.DrawThrowZones(throwZones, camEditorInfo);
        }
#endif
    }
}