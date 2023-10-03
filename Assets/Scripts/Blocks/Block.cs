using System;
using Scripts.Physics;
using UnityEngine;

namespace Scripts.Blocks
{
    public class Block : MonoBehaviour
    {
        private BlockData _blockData;
        private PhysicsObject2D _physicsObject;
        private Camera _camera;

        private void Awake()
        {
            _physicsObject = gameObject.GetComponent<PhysicsObject2D>();
        }

        private void Update()
        {
            throw new NotImplementedException();
        }
    }
}