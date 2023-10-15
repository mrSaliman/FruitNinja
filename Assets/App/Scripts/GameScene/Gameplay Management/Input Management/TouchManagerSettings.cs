using UnityEngine;

namespace App.GameScene.Gameplay_Management.Input_Management
{
    [CreateAssetMenu(menuName = "Settings/TouchManagerSettings", fileName = "New TouchManagerSettings")]
    public class TouchManagerSettings : ScriptableObject
    {
        [SerializeField] private float deathLineThickness;
        [SerializeField] private float minSliceSpeed;
        
        public float DeathLineThickness => deathLineThickness;
        public float MinSliceSpeed => minSliceSpeed;
    }
}