using System;
using System.Collections.Generic;
using App.GameScene.Blocks;
using UnityEngine.Serialization;

namespace App.GameScene.Gameplay_Management.Block_Management.Block_Assignment
{
    [Serializable]
    public class BlockAssignment
    {
        public Block blockPrefab;
        public List<SSPAssignment> sspAssignments;
    }
}