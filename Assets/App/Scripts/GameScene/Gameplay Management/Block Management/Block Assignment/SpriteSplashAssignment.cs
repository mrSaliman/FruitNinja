using System;
using JetBrains.Annotations;
using UnityEngine;

namespace App.GameScene.Gameplay_Management.Block_Management.Block_Assignment
{
    [Serializable]
    public class SpriteSplashAssignment
    {
        public Sprite sprite;
        [CanBeNull] public Sprite splash;
    }
}