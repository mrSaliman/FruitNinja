using System;
using UnityEngine.Serialization;

namespace App.GameScene.Gameplay_Management.Block_Management
{
    [Serializable]
    public class ThrowZone
    {
        public float xIndentation;
        public float yIndentation;
        public float radius;
        public float platformAngle;
        public float startThrowAngle;
        public float endThrowAngle;
        public float startThrowVelocity;
        public float endThrowVelocity;
        public float probability;
        public bool showTrajectory;
    }
}