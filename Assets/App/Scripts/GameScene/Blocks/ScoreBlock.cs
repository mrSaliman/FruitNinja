using System.Collections.Generic;
using UnityEngine;

namespace App.GameScene.Blocks
{
    public class ScoreBlock : Block
    {
        [SerializeField] private DisappearingSprite disappearingSprite;
        [SerializeField] private List<Sprite> splashes;
        
        private void Awake()
        {
            IsHalfable = true;
        }

        public override void OnHit()
        {
            base.OnHit();
            SpawnAnimatedSplash(transform.position);
        }

        private void SpawnAnimatedSplash(Vector3 position)
        {
            System.Random random = new();
            
            var disappearingSpriteInstance = Instantiate(disappearingSprite, position, Quaternion.identity);
            disappearingSpriteInstance.Setup(splashes[random.Next(splashes.Count)], 1);
        }
    }
}