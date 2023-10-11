using System.Collections.Generic;
using App.GameScene.Blocks;
using App.GameScene.User_Input;
using App.GameScene.Visualization;
using UnityEngine;

namespace App.GameScene.Gameplay_Management.Block_Management
{
    public class BlockInteractionManager : Manager
    {
        private readonly List<Block> _blocks = new List<Block>();
        [SerializeReference] private CameraManager cameraManager;
        
        private Rect _cameraSize;

        public override void Init()
        {
            _cameraSize = cameraManager.CameraRect;
        }

        public void AddBlock(Block block)
        {
            _blocks.Add(block);
            block.transform.parent = transform;
        }

        private void DeleteBlock(int index)
        {
            if (index < 0 || index >= _blocks.Count) return;
            Destroy(_blocks[index].gameObject);
            _blocks.RemoveAt(index);
        }

        private void Update()
        {
            for (var i = 0; i < _blocks.Count; i++)
            {
                var block = _blocks[i];
                if (block.transform.position.y + block.Radius >= _cameraSize.height / 2f &&
                    block.physicsObject.velocity.y > 0) block.physicsObject.velocity.y *= -1f;

                if (block.physicsObject.velocity.y < 0 &&
                    !_cameraSize.Overlaps(
                        new Rect(block.transform.position,
                            new Vector2(block.Radius * 2, block.Radius * 2))))
                {
                    //block.OnMiss();
                    DeleteBlock(i);
                    i--;
                    continue;
                }
                
                foreach (var deathLine in TouchHandler.DeathLines)
                {
                    if (deathLine.Active == false) break;

                    if (!(DistanceToSegment(deathLine, block) <= block.Radius)) continue;
                    //block.OnHit();
                    DeleteBlock(i);
                    i--;
                }
            }
        }
        
        private static float DistanceToSegment(DeathLine deathLine, Component block)
        {
            var v = deathLine.To - deathLine.From;
            var w = (Vector2)block.transform.position - deathLine.From;

            var c1 = Vector2.Dot(w, v);
            if (c1 <= 0)
                return Vector2.Distance(block.transform.position, deathLine.From);
            
            var c2 = Vector2.Dot(v, v);
            if (c2 <= c1)
                return Vector2.Distance(block.transform.position, deathLine.To);

            var b = c1 / c2;

            var pb = deathLine.From + v * b;

            return Vector2.Distance(block.transform.position, pb);
        }
    }
}