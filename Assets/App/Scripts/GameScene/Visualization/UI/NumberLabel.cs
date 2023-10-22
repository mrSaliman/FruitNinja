using TMPro;
using UnityEngine;

namespace App.GameScene.Visualization.UI
{
    public class NumberLabel : MonoBehaviour
    {
        [SerializeField] private float animationDuration;
        [SerializeField] private string startText;
        [SerializeField] private TextMeshProUGUI label;

        private float _targetLabelData;
        private float _currentLabelData;
        private float _dataInterval;

        private void Update()
        {
            if (!(_currentLabelData < _targetLabelData)) return;
            _currentLabelData += _dataInterval * Time.deltaTime;
            if (_currentLabelData > _targetLabelData) _currentLabelData = _targetLabelData;
            SetLabelText(((int)_currentLabelData).ToString());
        }

        public void ResetValue()
        {
            _targetLabelData = 0;
            _currentLabelData = 0;
            _dataInterval = 0;
            SetLabelText(0.ToString());
        }

        public void SetValue(float value)
        {
            _currentLabelData = _targetLabelData = value;
            SetLabelText(((int)value).ToString());
        }

        public void SetValueAnimated(float value)
        {
            _targetLabelData = value;
            _dataInterval = (_targetLabelData - _currentLabelData) / animationDuration;
        }

        public void AddValueAnimated(float value)
        {
            SetValueAnimated(_targetLabelData + value);
        }

        public float GetTargetData()
        {
            return _targetLabelData;
        }

        private void SetLabelText(string text)
        {
            label.text = startText + text;
        }
    }
}