using System.Collections.Generic;
using App.GameScene.Blocks;
using UnityEngine;

namespace App.GameScene.Gameplay_Management.Block_Management.Block_Assignment
{
    [CreateAssetMenu(menuName = "Settings/BlockAssignmentsContainer", fileName = "New BlockAssignmentsContainer")]
    public class BlockAssignmentsContainer : ScriptableObject
    {
        [SerializeField] private Block blockPrefab;
        [SerializeField] private ShadowController shadowPrefab;
        [SerializeField] private int scoreBlockAssignmentId;
        [SerializeField] private List<BlockAssignment> blockAssignments;


        public Block BlockPrefab => blockPrefab;
        public ShadowController ShadowPrefab => shadowPrefab;
        public int ScoreBlockAssignmentId => scoreBlockAssignmentId;
        public List<BlockAssignment> BlockAssignments => blockAssignments;
    }
}