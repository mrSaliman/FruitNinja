using UnityEngine;

namespace App.GameScene.Settings
{
    [CreateAssetMenu(menuName = "Settings/ScoreControllerSettings", fileName = "New ScoreControllerSettings")]
    public class ScoreControllerSettings : ScriptableObject
    {
        [SerializeField] private float comboTimerDelay;
        [SerializeField] private int maxCombo;
        [SerializeField] private float timeToSave;
        
        public float TimeToSave => timeToSave;
        public float ComboTimerDelay => comboTimerDelay;
        public int MaxCombo => maxCombo;
    }
}