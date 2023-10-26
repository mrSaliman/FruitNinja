
using UnityEngine;

namespace App.GameScene.Gameplay_Management.Block_Management.Block_Interaction
{
    public class MagneticField
    {
        public Vector2 Position;
        public float TimeToDie;

        public MagneticField(Vector2 position, float timeToDie)
        {
            Position = position;
            TimeToDie = timeToDie;
        }
    }
}