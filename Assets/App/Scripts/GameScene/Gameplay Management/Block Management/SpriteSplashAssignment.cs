using System;
using JetBrains.Annotations;
using UnityEngine;

namespace App.GameScene.Gameplay_Management.Block_Management
{
    [Serializable]
    public class SpriteSplashAssignment
    {
        public Sprite sprite;
        [CanBeNull] public Sprite splash;
    }
}