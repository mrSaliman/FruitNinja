using UnityEngine;

namespace Blocks
{
    [CreateAssetMenu(fileName = "New Block", menuName = "Block")]
    public class BlockData : ScriptableObject
    {
        public float radius;
        public Sprite sprite;
    }
}