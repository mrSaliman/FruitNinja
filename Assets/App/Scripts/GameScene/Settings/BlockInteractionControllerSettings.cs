using UnityEngine;

namespace App.GameScene.Settings
{
    [CreateAssetMenu(menuName = "Settings/BlockInteractionControllerSettings", fileName = "New BlockInteractionControllerSettings")]
    public class BlockInteractionControllerSettings : ScriptableObject
    {
        [SerializeField] private float maxThrowawaySpeed;

        public float MaxThrowawaySpeed => maxThrowawaySpeed;
    }
}