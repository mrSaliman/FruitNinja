﻿using UnityEngine;

namespace App.GameScene.Settings
{
    [CreateAssetMenu(menuName = "Settings/BlockInteractionControllerSettings", fileName = "New BlockInteractionControllerSettings")]
    public class BlockInteractionControllerSettings : ScriptableObject
    {
        [SerializeField] private float maxThrowawaySpeed;
        [SerializeField] private float bombPowerMultiplier;
        [SerializeField] private float maxBlockToBombDistance;
        [SerializeField] private float magnetizeTime;
        [SerializeField] private Vector2 magnetRadius;
        [SerializeField] private float magnetPower;
        [SerializeField] private float mimicTime;
        
        public float MimicTime => mimicTime;
        public float MagnetPower => magnetPower;
        public Vector2 MagnetRadius => magnetRadius;
        public float MagnetizeTime => magnetizeTime;
        public float MaxBlockToBombDistance => maxBlockToBombDistance;
        public float BombPowerMultiplier => bombPowerMultiplier;
        public float MaxThrowawaySpeed => maxThrowawaySpeed;
    }
}