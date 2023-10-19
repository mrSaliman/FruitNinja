using UnityEngine;

namespace App.GameScene.Gameplay_Management.Block_Management
{
    [CreateAssetMenu(menuName = "Settings/BlockThrowManagerSettings", fileName = "New BlockThrowManagerSettings")]
    public class BlockThrowControllerSettings : ScriptableObject
    {
        [SerializeField] private float baseThrowBlockDelay;
        [SerializeField] private float baseThrowPackDelay;
        [SerializeField] private float difficultyFactor;
        [SerializeField] private float maxDifficulty;
        [SerializeField] private Vector2Int packSizeRange;
        
        public float MaxDifficulty => maxDifficulty;
        public Vector2Int PackSizeRange => packSizeRange;
        public float DifficultyFactor => difficultyFactor;
        public float BaseThrowBlockDelay => baseThrowBlockDelay;
        public float BaseThrowPackDelay => baseThrowPackDelay;
    }
}