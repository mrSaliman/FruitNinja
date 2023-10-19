using TMPro;
using UnityEngine;

namespace App.GameScene.Visualization.UI
{
    public class NumberLabel : MonoBehaviour
    {
        [SerializeField] private float animationDuration;
        [SerializeField] private TextMeshProUGUI label;

        private float _targetLabelData;
        private float _currentLabelData;
        private float _dataInterval;

        private void Update()
        {
            if (!(_currentLabelData < _targetLabelData)) return;
            _currentLabelData += _dataInterval * Time.deltaTime;
            if (_currentLabelData > _targetLabelData) _currentLabelData = _targetLabelData;
            SetValue(_currentLabelData);
        }

        public void ResetValue()
        {
            _targetLabelData = 0;
            _currentLabelData = 0;
            _dataInterval = 0;
            label.text = 0.ToString();
        }

        private void SetValue(float value)
        {
            label.text = ((int)value).ToString();
        }

        private void SetValueAnimated(float value)
        {
            _targetLabelData = value;
            _dataInterval = (_targetLabelData - _currentLabelData) / animationDuration;
        }

        public void AddValueAnimated(float value)
        {
            SetValueAnimated(_targetLabelData + value);
        }
    }
}