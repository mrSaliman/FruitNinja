﻿using System;
using System.Collections.Generic;
using App.GameScene.Blocks;

namespace App.GameScene.Gameplay_Management.Block_Management
{
    [Serializable]
    public class BlockAssignment
    {
        public Block blockPrefab;
        public List<SpriteSplashAssignment> spriteSplashAssignments;
    }
}