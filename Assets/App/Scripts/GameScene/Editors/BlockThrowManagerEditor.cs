using System.Collections.Generic;
using App.GameScene.Gameplay_Management.Block_Management;
using App.GameScene.Gameplay_Management.Block_Management.Block_Throw;
using UnityEditor;
using UnityEngine;

namespace App.GameScene.Editors
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(BlockThrowController))]
    internal class BlockThrowManagerEditor : Editor
    {
        private bool _showThrowZonesFoldout;
        
        public override void OnInspectorGUI()
        {
            var blockThrowManager = (BlockThrowController)target;

            DrawDefaultInspector();
            
            if (GUILayout.Button("Add New ThrowZone"))
            {
                blockThrowManager.throwZones ??= new List<ThrowZone>();
                blockThrowManager.throwZones.Add(new ThrowZone());
            }
            
            if (blockThrowManager.throwZones is not null && blockThrowManager.throwZones.Count > 0 && GUILayout.Button("Remove Last ThrowZone"))
            {
                var lastIndex = blockThrowManager.throwZones.Count - 1;
                blockThrowManager.throwZones.RemoveAt(lastIndex);
            }
            
            _showThrowZonesFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_showThrowZonesFoldout, "ThrowZones");
            if (_showThrowZonesFoldout)
            {
                EditorGUI.indentLevel++;
                if (blockThrowManager.throwZones != null)
                    foreach (var throwZone in blockThrowManager.throwZones)
                    {
                        EditorGUILayout.LabelField("ThrowZone Settings");
                        EditorGUI.indentLevel++;

                        DrawThrowZoneEditor(throwZone);

                        EditorGUI.indentLevel--;
                    }

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void DrawThrowZoneEditor(ThrowZone throwZone)
        {
            throwZone.xIndentation = EditorGUILayout.Slider("XIndentation", throwZone.xIndentation, 0, 1);
            throwZone.yIndentation = EditorGUILayout.Slider("YIndentation", throwZone.yIndentation, 0, 1);

            EditorGUI.BeginChangeCheck();
            throwZone.startThrowAngle = EditorGUILayout.Slider("StartThrowAngle", throwZone.startThrowAngle, 0, 180);
            if (EditorGUI.EndChangeCheck())
            {
                if (throwZone.endThrowAngle < throwZone.startThrowAngle)
                    throwZone.endThrowAngle = throwZone.startThrowAngle;
            }

            EditorGUI.BeginChangeCheck();
            throwZone.endThrowAngle = EditorGUILayout.Slider("EndThrowAngle", throwZone.endThrowAngle, 0, 180);
            if (EditorGUI.EndChangeCheck())
            {
                if (throwZone.endThrowAngle < throwZone.startThrowAngle)
                    throwZone.startThrowAngle = throwZone.endThrowAngle;
            }

            throwZone.platformAngle = EditorGUILayout.Slider("PlatformAngle", throwZone.platformAngle, -180, 180);
            
            EditorGUI.BeginChangeCheck();
            throwZone.startThrowVelocity = EditorGUILayout.Slider("StartThrowVelocity", throwZone.startThrowVelocity, 0, 30);
            if (EditorGUI.EndChangeCheck())
            {
                if (throwZone.endThrowVelocity < throwZone.startThrowVelocity)
                    throwZone.endThrowVelocity = throwZone.startThrowVelocity;
            }

            EditorGUI.BeginChangeCheck();
            throwZone.endThrowVelocity = EditorGUILayout.Slider("EndThrowVelocity", throwZone.endThrowVelocity, 0, 30);
            if (EditorGUI.EndChangeCheck())
            {
                if (throwZone.endThrowVelocity < throwZone.startThrowVelocity)
                    throwZone.startThrowVelocity = throwZone.endThrowVelocity;
            }

            EditorGUI.BeginChangeCheck();
            throwZone.radius = EditorGUILayout.FloatField("Radius", throwZone.radius);
            if (EditorGUI.EndChangeCheck())
            {
                if (throwZone.radius < 0) throwZone.radius = 0;
            }

            throwZone.probability = EditorGUILayout.FloatField("Probability", throwZone.probability);

            throwZone.showTrajectory = EditorGUILayout.Toggle("Show Trajectories", throwZone.showTrajectory);

            EditorApplication.QueuePlayerLoopUpdate();
        }
    }
    #endif
}