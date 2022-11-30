using System;
using UnityEngine;

namespace Model
{
    public class Score
    {
        private const string BestKey = "BestScore";
        
        public int Value { get; private set; }

        public int Best
        {
            get => PlayerPrefs.GetInt(BestKey);
            private set => PlayerPrefs.SetInt(BestKey, value);
        }
        
        public event Action<int> OnValueChanged; 
        
        /// <summary>
        /// Add 1 score
        /// </summary>
        public void Add()
        {
            Value++;

            if (Value > Best)
                Best = Value;
            
            OnValueChanged?.Invoke(Value);
        }
    }
}