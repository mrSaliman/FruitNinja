﻿using System.Collections.Generic;
using App.GameScene.Blocks;
using App.GameScene.Gameplay_Management.Input_Management;
using App.GameScene.Gameplay_Management.UI_Management;
using App.GameScene.Settings;
using App.GameScene.Visualization;
using DG.Tweening;
using UnityEngine;

namespace App.GameScene.Gameplay_Management.Block_Management.Block_Interaction
{
    public class BlockInteractionController : BaseController
    {
        private readonly List<Block> _blocks = new();
        
        private CameraInfoProvider _cameraInfoProvider;
        private Rect _cameraSize;

        [SerializeField] private BlockInteractionControllerSettings settings;
        private float _maxThrowawaySpeed;

        private ScoreController _scoreController;
        private HealthController _healthController;
        
        [SerializeField] private Part partPrefab;
        [SerializeField] private Transform effectsFolder;

        public override void Init()
        {
            _scoreController = ControllerLocator.Instance.GetController<ScoreController>();
            _healthController = ControllerLocator.Instance.GetController<HealthController>();
            _cameraInfoProvider = ControllerLocator.Instance.GetController<CameraInfoProvider>();
            _cameraSize = _cameraInfoProvider.CameraRect;
            _maxThrowawaySpeed = settings.MaxThrowawaySpeed;
        }

        public void AddBlock(Block block)
        {
            _blocks.Add(block);
            block.transform.parent = transform;
            SubscribeBlock(block);
        }

        private void SubscribeBlock(Block block)
        {
            block.OnBlockHit += () => _scoreController.HandleBlockHit(block);
            block.OnBlockMiss += () => _healthController.HandleBlockMiss(block);
        }

        private void DeleteBlock(Block block)
        {
            block.transform.DOKill();
            Destroy(block.gameObject);
            _blocks.Remove(block);
        }

        private void Update()
        {
            _cameraSize = _cameraInfoProvider.CameraRect;
            for (var i = 0; i < _blocks.Count; i++)
            {
                var block = _blocks[i];
                if (block.transform.position.y + block.Radius >= _cameraSize.height / 2f &&
                    block.physicsObject.velocity.y > 0) block.physicsObject.velocity.y *= -1f;

                if (!(block.physicsObject.velocity.y < 0) ||
                    _cameraSize.Overlaps(
                        new Rect((Vector2)block.transform.position - new Vector2(block.Radius, block.Radius),
                            new Vector2(block.Radius * 2, block.Radius * 2)))) continue;
                
                block.OnMiss();
                DeleteBlock(block);
                i--;
            }
        }

        public void HandleDeathLine(DeathLine deathLine)
        {
            for (var i = 0; i < _blocks.Count; i++)
            {
                var block = _blocks[i];
                if (!block.IsInteractable) continue;
                if (!(DistanceToSegment(deathLine, block) <= block.Radius)) continue;

                if (block.IsHalfable)
                {
                    CreateAndSetupParts(deathLine, block);
                }
                
                block.OnHit();
                HandleAfterBlockAnimations(block, deathLine);
                DeleteBlock(block);
                i--;
            }
        }

        private void HandleAfterBlockAnimations(Block block, DeathLine deathLine)
        {
            SpawnAnimatedSplash(block);
            var deathLineDirection = (deathLine.To - deathLine.From).normalized;
            SpawnSplashParticle(block, deathLineDirection);
        }
        
        private void SpawnAnimatedSplash(Block block)
        {
            if (block.disappearingSprite is null || block.splash is null) return;
            var disappearingSpriteInstance = 
                Instantiate(block.disappearingSprite, block.transform.position, Quaternion.identity, effectsFolder);
            disappearingSpriteInstance.Setup(block.splash, 1);
        }

        private void SpawnSplashParticle(Block block, Vector2 direction)
        {
            if (block.splashParticle is null) return;
            
            var angleInDegrees = -Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            var particles = 
                Instantiate(block.splashParticle,
                    block.transform.position,
                    Quaternion.Euler(angleInDegrees, 90, 0),
                    effectsFolder);
            
            var main = particles.main;
            main.startColor = block.particleColor;
            main.startRotation = angleInDegrees * Mathf.Deg2Rad;
        }

        private void CreateAndSetupParts(DeathLine deathLine, Block block)
        {
            Vector2 blockPosition = block.transform.position;
            
            var sprite = block.spriteRenderer.sprite;
            var pixelSpriteSize = sprite.rect.size;
            var spriteSize = sprite.bounds.size;
                    
            var deathLineDirection = (deathLine.To - deathLine.From).normalized;
            var perp = Vector2.Perpendicular(deathLineDirection);
            
            var rotatedDeathLineDirection = Quaternion.Euler(0, 0, 90) * deathLineDirection;
            var rotation = Quaternion.LookRotation(Vector3.forward, rotatedDeathLineDirection);

            var firstPartPosition = blockPosition + spriteSize.y / 4f * perp;
            var secondPartPosition = blockPosition - spriteSize.y / 4f * perp;
                    
            var firstPart = Instantiate(partPrefab, firstPartPosition, rotation);
            var secondPart = Instantiate(partPrefab, secondPartPosition, rotation);
            
            var firstPartRect = new Rect(0, pixelSpriteSize.y / 2f, pixelSpriteSize.x, pixelSpriteSize.y / 2f);
            var secondPartRect = new Rect(0, 0, pixelSpriteSize.x, pixelSpriteSize.y / 2f);
                    
            firstPart.SetSprite(Sprite.Create(sprite.texture,
                firstPartRect,
                new Vector2(0.5f, 0.5f)));
                    
            secondPart.SetSprite(Sprite.Create(sprite.texture,
                secondPartRect, 
                new Vector2(0.5f, 0.5f)));
                    
                    
            firstPart.ThrowItself(firstPartPosition,  Mathf.Min(deathLine.Speed, _maxThrowawaySpeed) * (deathLineDirection / 5f + perp / 10f));
            secondPart.ThrowItself(secondPartPosition, Mathf.Min(deathLine.Speed, _maxThrowawaySpeed) * (deathLineDirection / 5f - perp / 10f));

            firstPart.transform.DORotate(rotation.eulerAngles + new Vector3(0, 0, 360), 5, RotateMode.FastBeyond360).SetLoops(-1);
            secondPart.transform.DORotate(rotation.eulerAngles + new Vector3(0, 0, -360), 5, RotateMode.FastBeyond360).SetLoops(-1);
                    
            AddBlock(firstPart);
            AddBlock(secondPart);
        }
        
        private static float DistanceToSegment(DeathLine deathLine, Component block)
        {
            var v = deathLine.To - deathLine.From;
            var w = (Vector2)block.transform.position - deathLine.From;

            var c1 = Vector2.Dot(w, v);
            if (c1 <= 0)
                return Vector2.Distance(block.transform.position, deathLine.From) - deathLine.Thickness / 2f;
            
            var c2 = Vector2.Dot(v, v);
            if (c2 <= c1)
                return Vector2.Distance(block.transform.position, deathLine.To) - deathLine.Thickness / 2f;

            var b = c1 / c2;

            var pb = deathLine.From + v * b;

            return Vector2.Distance(block.transform.position, pb) - deathLine.Thickness / 2f;
        }
    }
}