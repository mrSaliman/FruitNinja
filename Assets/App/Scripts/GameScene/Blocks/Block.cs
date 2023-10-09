using App.GameScene.Physics;
using App.GameScene.User_Input;
using UnityEngine;

namespace App.GameScene.Blocks
{
    public abstract class Block : MonoBehaviour
    {
        [SerializeReference]
        private PhysicsObject2D physicsObject;
        
        [SerializeReference]
        private SpriteRenderer spriteRenderer;

        private float _radius;
        
        [HideInInspector] public float cameraHeight;
        [HideInInspector] public float cameraWidth;

        protected abstract void OnHit();

        protected abstract void OnMiss();

        private void Update()
        {
            if (transform.position.y + _radius >= cameraHeight / 2f &&
                physicsObject.velocity.y > 0) physicsObject.velocity.y *= -1f;

            foreach (var deathLine in TouchHandler.DeathLines)
            {
                if (deathLine.Active == false) break;

                if (DistanceToSegment(deathLine) <= _radius)
                {
                    //OnHit();
                    Destroy(gameObject);
                }
            }

            if (physicsObject.velocity.y < 0 &&
                (transform.position.y + _radius < -cameraHeight / 2f ||
                 transform.position.x + _radius < -cameraWidth / 2f ||
                 transform.position.x - _radius > cameraWidth / 2f))
            {
                //OnMiss();
                Destroy(gameObject);
            }
        }

        private float DistanceToSegment(DeathLine deathLine)
        {
            var v = deathLine.To - deathLine.From;
            var w = (Vector2)transform.position - deathLine.From;

            var c1 = Vector2.Dot(w, v);
            if (c1 <= 0)
                return Vector2.Distance(transform.position, deathLine.From);
            
            var c2 = Vector2.Dot(v, v);
            if (c2 <= c1)
                return Vector2.Distance(transform.position, deathLine.To);

            var b = c1 / c2;

            var pb = deathLine.From + v * b;

            return Vector2.Distance(transform.position, pb);
        }

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
            Vector2 spriteSize = spriteRenderer.bounds.size;
            _radius = Mathf.Max(spriteSize.x, spriteSize.y) / 2f;
        }

        public void ThrowItself(Vector3 position, Vector2 velocity, float angularVelocity)
        {
            transform.position = position;
            physicsObject.SetMainAndAngularVelocity(velocity, angularVelocity);
            physicsObject.isFrozen = false;
        }
    }
}