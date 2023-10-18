using DG.Tweening;
using UnityEngine;

namespace App.GameScene.Blocks
{
    public class DisappearingSprite : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        public void Setup(Sprite sprite, float time)
        {
            spriteRenderer.sprite = sprite;
            
            spriteRenderer.DOFade(0, time).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }
    }
}