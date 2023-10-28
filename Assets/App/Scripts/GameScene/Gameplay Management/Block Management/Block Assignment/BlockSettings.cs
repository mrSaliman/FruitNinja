using System;
using App.GameScene.Blocks;
using App.GameScene.Blocks.SpecialBlocks;
using UnityEngine;

namespace App.GameScene.Gameplay_Management.Block_Management.Block_Assignment
{
    [Serializable]
    public class BlockSettings
    {
        public BlockType blockType;
        public ShadowController shadowController;
        public bool isInteractable;
        public bool isHalfable;
        public bool isDestructible;
        public bool isMissable;
        public DisappearingSprite disappearingSprite;
        public ParticleSystem splashParticle;
        public bool useDirectionForParticle;
    }
}