using UnityEngine;

namespace App.GameScene.Gameplay_Management.Block_Management
{
    [CreateAssetMenu(menuName = "Settings/BlockInteractionControllerSettings", fileName = "New BlockInteractionControllerSettings")]
    public class BlockInteractionControllerSettings : ScriptableObject
    {
        [SerializeField] private float maxThrowawaySpeed;

        public float MaxThrowawaySpeed => maxThrowawaySpeed;
    }
}