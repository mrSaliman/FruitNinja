using UnityEngine;

namespace App.GameScene.Blocks
{
    public class ShadowController : MonoBehaviour
    {
        public SpriteRenderer mainSpriteRenderer;
        public Block parent;

        [SerializeField] private ShadowControllerSettings settings;
        
        private Vector2 _shadowOffset;
        private float _positionChangeFactor;
        private float _alphaChangeFactor;

        private void Awake()
        {
            _shadowOffset = settings.ShadowOffset;
            _positionChangeFactor = settings.PositionChangeFactor;
            _alphaChangeFactor = settings.AlphaChangeFactor;
        }

        private void Update()
        {
            var parentTransform = parent.transform;
            var mainTransform = transform;
            var parentScale = parentTransform.localScale;
            
            mainTransform.position = parentTransform.position + (Vector3)_shadowOffset * (parentScale.x * parentScale.x * _positionChangeFactor);
            
            var alpha = 1 / parentScale.x * _alphaChangeFactor;
            mainSpriteRenderer.color = new Color(0, 0, 0, Mathf.Max(Mathf.Min(1f, alpha), 0f));
        }
    }
}