using App.GameScene.Gameplay_Management.Block_Management;
using UnityEditor;
using UnityEngine;

namespace App.GameScene.Editors
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(BlockThrowManager))]
    internal class BlockThrowManagerEditor : Editor
    {
        private bool _showThrowZonesFoldout = true;
        
        public override void OnInspectorGUI()
        {
            var blockThrowManager = (BlockThrowManager)target;

            DrawDefaultInspector();
            
            if (GUILayout.Button("Add New ThrowZone"))
            {
                blockThrowManager.ThrowZones.Add(new ThrowZone());
            }
            
            if (blockThrowManager.ThrowZones.Count > 0 && GUILayout.Button("Remove Last ThrowZone"))
            {
                var lastIndex = blockThrowManager.ThrowZones.Count - 1;
                blockThrowManager.ThrowZones.RemoveAt(lastIndex);
            }
            
            _showThrowZonesFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_showThrowZonesFoldout, "ThrowZones");
            if (_showThrowZonesFoldout)
            {
                EditorGUI.indentLevel++;
                foreach (var throwZone in blockThrowManager.ThrowZones)
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
            throwZone.XIndentation = EditorGUILayout.Slider("XIndentation", throwZone.XIndentation, 0, 1);
            throwZone.YIndentation = EditorGUILayout.Slider("YIndentation", throwZone.YIndentation, 0, 1);

            EditorGUI.BeginChangeCheck();
            throwZone.StartThrowAngle = EditorGUILayout.Slider("StartThrowAngle", throwZone.StartThrowAngle, 0, 180);
            if (EditorGUI.EndChangeCheck())
            {
                if (throwZone.EndThrowAngle < throwZone.StartThrowAngle)
                    throwZone.EndThrowAngle = throwZone.StartThrowAngle;
            }

            EditorGUI.BeginChangeCheck();
            throwZone.EndThrowAngle = EditorGUILayout.Slider("EndThrowAngle", throwZone.EndThrowAngle, 0, 180);
            if (EditorGUI.EndChangeCheck())
            {
                if (throwZone.EndThrowAngle < throwZone.StartThrowAngle)
                    throwZone.StartThrowAngle = throwZone.EndThrowAngle;
            }

            throwZone.PlatformAngle = EditorGUILayout.Slider("PlatformAngle", throwZone.PlatformAngle, -180, 180);
            
            EditorGUI.BeginChangeCheck();
            throwZone.StartThrowVelocity = EditorGUILayout.Slider("StartThrowVelocity", throwZone.StartThrowVelocity, 0, 30);
            if (EditorGUI.EndChangeCheck())
            {
                if (throwZone.EndThrowVelocity < throwZone.StartThrowVelocity)
                    throwZone.EndThrowVelocity = throwZone.StartThrowVelocity;
            }

            EditorGUI.BeginChangeCheck();
            throwZone.EndThrowVelocity = EditorGUILayout.Slider("EndThrowVelocity", throwZone.EndThrowVelocity, 0, 30);
            if (EditorGUI.EndChangeCheck())
            {
                if (throwZone.EndThrowVelocity < throwZone.StartThrowVelocity)
                    throwZone.StartThrowVelocity = throwZone.EndThrowVelocity;
            }

            EditorGUI.BeginChangeCheck();
            throwZone.Radius = EditorGUILayout.FloatField("Radius", throwZone.Radius);
            if (EditorGUI.EndChangeCheck())
            {
                if (throwZone.Radius < 0) throwZone.Radius = 0;
            }

            throwZone.Probability = EditorGUILayout.FloatField("Probability", throwZone.Probability);

            throwZone.ShowTrajectory = EditorGUILayout.Toggle("Show Trajectories", throwZone.ShowTrajectory);

            EditorApplication.QueuePlayerLoopUpdate();
        }
    }
    #endif
}