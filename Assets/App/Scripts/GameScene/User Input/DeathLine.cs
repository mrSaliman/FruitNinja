using UnityEngine;

namespace App.GameScene.User_Input
{
    public class DeathLine
    {
        public Vector2 From;
        public Vector2 To;
        public bool Active = true;

        public DeathLine(Vector2 from, Vector2 to)
        {
            From = from;
            To = to;
        }
    }
}