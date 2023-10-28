using UnityEngine;

namespace App.GameScene.Settings
{
    [CreateAssetMenu(menuName = "Settings/BlockThrowManagerSettings", fileName = "New BlockThrowManagerSettings")]
    public class BlockThrowControllerSettings : ScriptableObject
    {
        [SerializeField] private float baseThrowBlockDelay;
        [SerializeField] private float baseThrowPackDelay;
        [SerializeField] private float difficultyFactor;
        [SerializeField] private float maxDifficulty;
        [SerializeField] private Vector2Int packSizeRange;
        [SerializeField] [Range(0, 1)] private float minScoreBlockPercent;
        [SerializeField] private Vector2Int stringBagSizeRange;
        [SerializeField] private float stringBagImmortalityTime;
        [SerializeField] private int samuraiPackSizeMultiplier;
        [SerializeField] private float samuraiThrowPackDelayMultiplier;
        [SerializeField] private float samuraiModeTime;
        
        public int SamuraiPackSizeMultiplier => samuraiPackSizeMultiplier;
        public float SamuraiThrowPackDelayMultiplier => samuraiThrowPackDelayMultiplier;
        public float SamuraiModeTime => samuraiModeTime;
        public float StringBagImmortalityTime => stringBagImmortalityTime;
        public Vector2Int StringBagSizeRange => stringBagSizeRange;
        public float MinScoreBlockPercent => minScoreBlockPercent;
        public float MaxDifficulty => maxDifficulty;
        public Vector2Int PackSizeRange => packSizeRange;
        public float DifficultyFactor => difficultyFactor;
        public float BaseThrowBlockDelay => baseThrowBlockDelay;
        public float BaseThrowPackDelay => baseThrowPackDelay;
    }
}