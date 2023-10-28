using UnityEngine;

namespace App.GameScene.Blocks.SpecialBlocks
{
    public class Part : Block
    {
        public override void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
            SetupShadow();
        }
    }
}