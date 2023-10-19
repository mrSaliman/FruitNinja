﻿using UnityEngine;

namespace App.GameScene.Gameplay_Management.UI_Management
{
    [CreateAssetMenu(menuName = "Settings/ScoreControllerSettings", fileName = "New ScoreControllerSettings")]
    public class ScoreControllerSettings : ScriptableObject
    {
        [SerializeField] private float comboTimerDelay;
        [SerializeField] private int maxCombo;
        
        public float ComboTimerDelay => comboTimerDelay;
        public int MaxCombo => maxCombo;
    }
}