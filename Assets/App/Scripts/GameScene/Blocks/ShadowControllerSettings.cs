using UnityEngine;

namespace App.GameScene.Blocks
{
    [CreateAssetMenu(menuName = "Settings/ShadowControllerSettings", fileName = "New ShadowControllerSettings")]
    public class ShadowControllerSettings : ScriptableObject
    {
        [SerializeField] private float positionChangeFactor;
        [SerializeField] private float alphaChangeFactor;
        [SerializeField] private Vector2 shadowOffset;

        public Vector2 ShadowOffset => shadowOffset;
        public float PositionChangeFactor => positionChangeFactor;
        public float AlphaChangeFactor => alphaChangeFactor;
    }
}