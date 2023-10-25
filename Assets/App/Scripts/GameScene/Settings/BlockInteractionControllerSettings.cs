using UnityEngine;

namespace App.GameScene.Settings
{
    [CreateAssetMenu(menuName = "Settings/BlockInteractionControllerSettings", fileName = "New BlockInteractionControllerSettings")]
    public class BlockInteractionControllerSettings : ScriptableObject
    {
        [SerializeField] private float maxThrowawaySpeed;
        [SerializeField] private float bombPowerMultiplier;
        
        public float BombPowerMultiplier => bombPowerMultiplier;
        public float MaxThrowawaySpeed => maxThrowawaySpeed;
    }
}