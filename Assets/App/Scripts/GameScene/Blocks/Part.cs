using UnityEngine;

namespace App.GameScene.Blocks
{
    public class Part : Block
    {
        private void Awake()
        {
            IsInteractable = false;
        }

        public override void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
            SetupShadow();
        }
    }
}