using System.Collections.Generic;
using UnityEngine;

namespace App.GameScene.Gameplay_Management.Block_Management.Block_Assignment
{
    [CreateAssetMenu(menuName = "Settings/BlockAssignmentsContainer", fileName = "New BlockAssignmentsContainer")]
    public class BlockAssignmentsContainer : ScriptableObject
    {
        [SerializeField] private int scoreBlockAssignmentId;
        [SerializeField] private List<BlockAssignment> blockAssignments;
        
        public int ScoreBlockAssignmentId => scoreBlockAssignmentId;
        public List<BlockAssignment> BlockAssignments => blockAssignments;
    }
}