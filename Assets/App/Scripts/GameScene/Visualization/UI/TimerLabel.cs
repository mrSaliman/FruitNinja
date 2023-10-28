using TMPro;
using UnityEngine;

namespace App.GameScene.Visualization.UI
{
    public class TimerLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;

        public void ClearValue()
        {
            label.text = "";
        }

        public void SetValue(int seconds)
        {
            label.text = seconds.ToString();
        }
    }
}