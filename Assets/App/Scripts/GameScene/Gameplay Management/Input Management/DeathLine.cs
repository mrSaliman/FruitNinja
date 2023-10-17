using UnityEngine;

namespace App.GameScene.Gameplay_Management.Input_Management
{
    public class DeathLine
    {
        public Vector2 From;
        public Vector2 To;
        public readonly float Thickness;
        public float Speed;

        public DeathLine(Vector2 from, Vector2 to, float thickness, float speed)
        {
            From = from;
            To = to;
            Thickness = thickness;
            Speed = speed;
        }
    }
}