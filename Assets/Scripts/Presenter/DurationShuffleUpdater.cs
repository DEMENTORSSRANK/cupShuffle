using System;
using UnityEngine;

namespace Presenter
{
    [Serializable]
    public class DurationShuffleUpdater
    {
        [SerializeField] private float _startDuration = 1;

        [SerializeField] private float _minDuration = .3f;

        [SerializeField] private float _stepRemove = .05f;

        private float _duration;

        public float Duration
        {
            get => _duration;
            set => _duration = Mathf.Clamp(value, _minDuration, _startDuration);
        }

        public Action<float> DurationUpdated;
        
        public void DeCreaseDuration()
        {
            Duration -= _stepRemove;
            
            DurationUpdated?.Invoke(Duration);
        }

        public void ResetDuration()
        {
            Duration = _startDuration;
            
            DurationUpdated?.Invoke(Duration);
        }
    }
}