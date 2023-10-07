using UnityEngine;
using UnityEngine.Serialization;

namespace App.GameScene.Gameplay_Management.Block_Management
{
    [CreateAssetMenu(menuName = "Settings/BlockThrowManagerSettings", fileName = "New BlockThrowManagerSettings")]
    public class BlockThrowManagerSettings : ScriptableObject
    {
        [SerializeField] private float baseThrowBlockDelay;
        [SerializeField] private float baseThrowPackDelay;
        [SerializeField] private float difficultyFactor;
        [SerializeField] private Vector2Int packSizeRange;
        
        public Vector2Int PackSizeRange => packSizeRange;
        public float DifficultyFactor => difficultyFactor;
        public float BaseThrowBlockDelay => baseThrowBlockDelay;
        public float BaseThrowPackDelay => baseThrowPackDelay;
    }
}