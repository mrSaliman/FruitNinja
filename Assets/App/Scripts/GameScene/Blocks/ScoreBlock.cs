using System.Collections.Generic;
using UnityEngine;

namespace App.GameScene.Blocks
{
    public class ScoreBlock : Block
    {
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
            if (disappearingSprite is null || splash is null) return;
            var disappearingSpriteInstance = Instantiate(disappearingSprite, position, Quaternion.identity);
            disappearingSpriteInstance.Setup(splash, 1);
        }
    }
}