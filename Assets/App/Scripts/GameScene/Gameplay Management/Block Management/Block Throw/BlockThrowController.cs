﻿using System.Collections.Generic;
using System.Linq;
using App.GameScene.Blocks;
using App.GameScene.Blocks.SpecialBlocks;
using App.GameScene.Gameplay_Management.Block_Management.Block_Assignment;
using App.GameScene.Gameplay_Management.Block_Management.Block_Interaction;
using App.GameScene.Gameplay_Management.State;
using App.GameScene.Physics;
using App.GameScene.Settings;
using App.GameScene.Visualization;
using App.GameScene.Visualization.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace App.GameScene.Gameplay_Management.Block_Management.Block_Throw
{
    public class BlockThrowController : BaseController
    {
        private CameraInfoProvider _cameraInfoProvider;
        private Rect _cameraSize;
        
        private BlockInteractionController _blockInteractionController;
        private TimeController _timeController;
        
        private RandomBlockThrower _randomBlockThrower;
        
        [SerializeField] private BlockThrowControllerSettings settings;
        private float _throwPackDelay,
            _throwBlockDelay,
            _difficultyFactor,
            _maxDifficulty,
            _minScoreBlockPercent,
            _stringBagImmortalityTime,
            _samuraiThrowPackDelayMultiplier,
            _samuraiModeTime,
            _mimicChance;

        private int _samuraiPackSizeMultiplier;
        private Vector2Int _stringBagSizeRange;
        private ParticleSystem _mimicParticlePrefab;
        
        public BlockAssignmentsContainer blockAssignmentsContainer;
        
        [HideInInspector] public List<ThrowZone> throwZones;

        [SerializeField] private TimerLabel timerLabel;
        [SerializeField] private Image samuraiBg;

        private readonly List<Block> _currentPack = new();
        private ThrowZone _currentThrowZone;

        private float _blockTimer,
            _packTimer,
            _difficulty,
            _totalThrowZonesProbability,
            _totalBlockProbability,
            _samuraiModeTimer;

        private int _blockIndex, _scoreBlockId;

        private bool _samuraiMode;
        
        public override void Init()
        {
            _currentPack.Clear();
            _timeController = ControllerLocator.Instance.GetController<TimeController>();
            _cameraInfoProvider = ControllerLocator.Instance.GetController<CameraInfoProvider>();
            _blockInteractionController = ControllerLocator.Instance.GetController<BlockInteractionController>();
            _randomBlockThrower = new RandomBlockThrower();
            UnpackSettings();
            StartThrowingLoop();
        }

        private void UnpackSettings()
        {
            _throwPackDelay = settings.BaseThrowPackDelay;
            _throwBlockDelay = settings.BaseThrowBlockDelay;
            _difficultyFactor = settings.DifficultyFactor;
            _maxDifficulty = settings.MaxDifficulty;
            _minScoreBlockPercent = settings.MinScoreBlockPercent;
            _stringBagSizeRange = settings.StringBagSizeRange;
            _stringBagImmortalityTime = settings.StringBagImmortalityTime;
            _samuraiThrowPackDelayMultiplier = settings.SamuraiThrowPackDelayMultiplier;
            _samuraiModeTime = settings.SamuraiModeTime;
            _samuraiPackSizeMultiplier = settings.SamuraiPackSizeMultiplier;
            _mimicChance = settings.MimicChance;
            _mimicParticlePrefab = settings.MimicParticlePrefab;
        }
        
        private void StartThrowingLoop()
        {
            _packTimer = _throwPackDelay;
            _blockTimer = _throwBlockDelay;
            _blockIndex = 0;

            _samuraiModeTimer = 0;
            _samuraiMode = false;
            timerLabel.ClearValue();
            var samuraiBgColor = samuraiBg.color;
            samuraiBgColor.a = 0;
            samuraiBg.color = samuraiBgColor;
            
            _difficulty = 1f;
            
            _cameraSize = _cameraInfoProvider.CameraRect;

            _scoreBlockId = blockAssignmentsContainer.ScoreBlockAssignmentId;

            _totalThrowZonesProbability = throwZones.Sum(throwZone => throwZone.probability);
            _totalBlockProbability =
                blockAssignmentsContainer.BlockAssignments.Sum(blockAssignment => blockAssignment.probability);
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
            _blockTimer -= _timeController.DeltaTime;
            _packTimer -= _timeController.DeltaTime;
            if (_samuraiModeTimer > 0)
            {
                _samuraiModeTimer -= _timeController.AbsoluteDeltaTime;
                timerLabel.SetValue((int)_samuraiModeTimer);
            }
            if (_samuraiModeTimer < 0)
            {
                _samuraiModeTimer = 0;
                _samuraiMode = false;
                timerLabel.ClearValue();
                samuraiBg.DOFade(0, 1);
            }
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
                
                if (_currentPack.Count - _blockIndex == 1)
                    _packTimer = _throwPackDelay * _difficulty * (_samuraiMode ? _samuraiThrowPackDelayMultiplier : 1);
            }
            else if (_packTimer <= 0)
            {
                GenerateRandomPack(settings.PackSizeRange);
                if (_difficulty > _maxDifficulty) _difficulty *= _difficultyFactor;
            }
        }
        
        private void GenerateRandomPack(Vector2Int packSizeRange)
        {
            _blockIndex = 0;
            _currentPack.Clear();
            System.Random random = new();
            
            _cameraSize = _cameraInfoProvider.CameraRect;
            _randomBlockThrower.CameraSize = _cameraSize;
            
            var size = random.Next(packSizeRange.x, packSizeRange.y + 1);
            var scoreBlockCount = 0;
            var requiredScoreBlockAmount = Mathf.CeilToInt(_minScoreBlockPercent * size);
            if (_samuraiMode)
            {
                size *= _samuraiPackSizeMultiplier;
                requiredScoreBlockAmount = size;
            }
            
            for (var i = 0; i < size; i++)
            {

                var blockTypeId = GetRandomAssignmentId(random);

                if (blockAssignmentsContainer.BlockAssignments[blockTypeId].blockSettings.blockType is BlockType.Brick &&
                    _blockInteractionController.BrickQuantity > 0) blockTypeId = _scoreBlockId;
                if (blockTypeId == _scoreBlockId) scoreBlockCount++;
                else if (requiredScoreBlockAmount - scoreBlockCount == size - i)
                {
                    blockTypeId = _scoreBlockId;
                    scoreBlockCount++;
                }
                var blockAssignment = blockAssignmentsContainer.BlockAssignments[blockTypeId];
                var block = SetupBlock(blockAssignment, _cameraSize.size, random);
                if (_samuraiMode) block.isMissable = false;
                else if (random.NextDouble() < _mimicChance)
                {
                    block.isMimic = true;
                    block.mimicParticle = Instantiate(_mimicParticlePrefab, block.transform);
                }

                _currentPack.Add(block);
                _blockInteractionController.AddBlock(block);
            }
        }

        public int GetRandomAssignmentId(System.Random random)
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

            return blockTypeId;
        }

        private Block SetupBlock(BlockAssignment blockAssignment, Vector3 blockPosition, System.Random random)
        {
            var block = Instantiate(blockAssignmentsContainer.BlockPrefab, blockPosition, Quaternion.identity);
            block.SetSettings(blockAssignment.blockSettings);
            if (blockAssignment.blockSettings.hasShadow)
            {
                block.shadowController = Instantiate(blockAssignmentsContainer.ShadowPrefab, block.transform);
            }
            var sspAssignment =
                blockAssignment.sspAssignments[random.Next(blockAssignment.sspAssignments.Count)];
            block.SetSprite(sspAssignment.sprite);
            block.splash = sspAssignment.splash;
            block.particleColor = sspAssignment.particleColor;
            return block;
        }

        public void HandleBlockHit(Block block)
        {
            switch (block.blockType)
            {
                case BlockType.StringBag:
                {
                    System.Random random = new();
                
                    var size = random.Next(_stringBagSizeRange.x, _stringBagSizeRange.y + 1);

                    for (var i = 0; i < size; i++)
                    {
                        var sbBlock = SetupBlock(blockAssignmentsContainer.BlockAssignments[_scoreBlockId],
                            block.transform.position, random);
                        sbBlock.isInteractable = false;
                        sbBlock.immortalityTimer = _stringBagImmortalityTime;
                        sbBlock.transform.localScale = block.transform.localScale;
                        _blockInteractionController.AddBlock(sbBlock);

                        sbBlock.physicsObject.velocity = new Vector2(((float)random.NextDouble() * 2 - 1) * 5, 5);
                        sbBlock.physicsObject.isFrozen = false;
                    }

                    break;
                }
                case BlockType.SamuraiBlock:
                {
                    _samuraiMode = true;
                    _samuraiModeTimer = _samuraiModeTime;
                    timerLabel.SetValue((int)_samuraiModeTimer);
                    samuraiBg.DOFade(1, 1);
                    break;
                }
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