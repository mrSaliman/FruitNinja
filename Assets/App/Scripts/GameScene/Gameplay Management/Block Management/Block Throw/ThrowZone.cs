using System;

namespace App.GameScene.Gameplay_Management.Block_Management.Block_Throw
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