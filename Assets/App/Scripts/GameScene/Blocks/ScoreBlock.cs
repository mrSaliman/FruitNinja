using System;
using App.GameScene.Gameplay_Management.Input_Management;
using UnityEngine;

namespace App.GameScene.Blocks
{
    public class ScoreBlock : Block
    {
        private void Awake()
        {
            IsHalfable = true;
        }
    }
}