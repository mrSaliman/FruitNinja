using UnityEngine;

namespace App.GameScene.Gameplay_Management.Input_Management
{
    [CreateAssetMenu(menuName = "Settings/TouchManagerSettings", fileName = "New TouchManagerSettings")]
    public class InputControllerSettings : ScriptableObject
    {
        [SerializeField] private float deathLineThickness;
        [SerializeField] private float minSliceSpeed;
        [SerializeField] private float minTravelDistance;
        
        public float MinTravelDistance => minTravelDistance;
        public float DeathLineThickness => deathLineThickness;
        public float MinSliceSpeed => minSliceSpeed;
    }
}