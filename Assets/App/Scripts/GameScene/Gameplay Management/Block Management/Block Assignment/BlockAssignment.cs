using System;
using System.Collections.Generic;

namespace App.GameScene.Gameplay_Management.Block_Management.Block_Assignment
{
    [Serializable]
    public class BlockAssignment
    {
        public BlockSettings blockSettings;
        public float probability;
        public List<SSPAssignment> sspAssignments;
    }
}