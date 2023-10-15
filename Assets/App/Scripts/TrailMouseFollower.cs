using UnityEngine;

namespace App
{
    public class TrailMouseFollower : MonoBehaviour
    {
        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private Camera mainCamera;

        private void Start()
        {
            trailRenderer.autodestruct = false;
        }

        private void Update()
        {
            transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 10;
        }
    }
}