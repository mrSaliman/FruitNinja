using UnityEngine;
using UnityEngine.Serialization;

namespace App.GameScene.Settings
{
    [CreateAssetMenu(menuName = "Settings/BlockInteractionControllerSettings", fileName = "New BlockInteractionControllerSettings")]
    public class BlockInteractionControllerSettings : ScriptableObject
    {
        [SerializeField] private float maxThrowawaySpeed;
        [SerializeField] private float bombPowerMultiplier;
        [SerializeField] private float maxBlockToBombDistance;
        
        public float MaxBlockToBombDistance => maxBlockToBombDistance;
        public float BombPowerMultiplier => bombPowerMultiplier;
        public float MaxThrowawaySpeed => maxThrowawaySpeed;
    }
}