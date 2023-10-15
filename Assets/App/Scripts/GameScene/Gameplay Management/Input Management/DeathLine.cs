using UnityEngine;

namespace App.GameScene.Gameplay_Management.Input_Management
{
    public class DeathLine
    {
        public Vector2 From;
        public Vector2 To;
        public readonly float Thickness;
        public bool Active = true;

        public DeathLine(Vector2 from, Vector2 to, float thickness)
        {
            From = from;
            To = to;
            Thickness = thickness;
        }
    }
}