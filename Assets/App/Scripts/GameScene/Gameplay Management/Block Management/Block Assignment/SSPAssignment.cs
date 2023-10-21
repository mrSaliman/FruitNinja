using System;
using JetBrains.Annotations;
using UnityEngine;

namespace App.GameScene.Gameplay_Management.Block_Management.Block_Assignment
{
    /// <summary>
    ///   <para>Represents a Sprite/Splash/ParticleColor assignment.</para>
    /// </summary>
    [Serializable]
    public class SSPAssignment
    {
        public Sprite sprite;
        [CanBeNull] public Sprite splash;
        public Color particleColor;
    }
}