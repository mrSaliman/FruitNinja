using System.Collections.Generic;
using App.GameScene.Blocks;
using App.GameScene.Gameplay_Management.Input_Management;
using App.GameScene.Gameplay_Management.UI_Management;
using App.GameScene.Visualization;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.GameScene.Gameplay_Management.Block_Management
{
    public class BlockInteractionController : BaseController
    {
        private readonly List<Block> _blocks = new();
        
        private CameraInfoProvider _cameraInfoProvider;
        private Rect _cameraSize;

        private ScoreController _scoreController;
        
        [SerializeField] private Part partPrefab;

        public override void Init()
        {
            _scoreController = ControllerLocator.Instance.GetController<ScoreController>();
            _cameraInfoProvider = ControllerLocator.Instance.GetController<CameraInfoProvider>();
            _cameraSize = _cameraInfoProvider.CameraRect;
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
                DeleteBlock(block);
                i--;
            }
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
                    
                    
            firstPart.ThrowItself(firstPartPosition, deathLine.Speed * deathLineDirection / 5f + perp * deathLine.Speed / 10f);
            secondPart.ThrowItself(secondPartPosition, deathLine.Speed * deathLineDirection / 5f - perp * deathLine.Speed / 10f);

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