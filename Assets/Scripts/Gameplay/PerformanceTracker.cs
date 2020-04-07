using UnityEngine;
using UnityEngine.UI;
using System;
using Boo.Lang;

namespace Eurovision.Gameplay
{
    public class PerformanceTracker : MonoBehaviour
    {
        public Action onPerformanceFull;

        [SerializeField] private int _pointsNeeded = 5;
        [SerializeField] private Image[] _performanceBars; 

        private int _performancePoints;

        private void Start()
        {
            _performancePoints = 0;
        }

        public void AddPoints(int score)
        {
            _performancePoints += score;

            if (_performancePoints >= _pointsNeeded)
            {
                _performancePoints = _pointsNeeded;

                UpdateUI();

                onPerformanceFull?.Invoke();

                ResetPerformancePoints();                 
            }

            UpdateUI();
        }

        public void ResetPerformancePoints()
        {
            _performancePoints = 0;
        }

        private void UpdateUI()
        {
            float normalizedScore = (float)_performancePoints / (float)_pointsNeeded;
            for (int i = 0; i < _performanceBars.Length; i++)
            {
                _performanceBars[i].fillAmount = normalizedScore;
            }
        }
    }
}

