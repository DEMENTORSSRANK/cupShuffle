using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.Screens
{
    public class EndScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currentScore;

        [SerializeField] private TMP_Text _best;

        [SerializeField] private Button _menu;

        public event Action OnMenuClicked;
        
        public void UpdateCurrentScore(int score)
        {
            _currentScore.text = $"Your score: {score}";
        }

        public void UpdateBestScore(int bestScore)
        {
            _best.text = $"Best: {bestScore}";
        }

        private void Start()
        {
            _menu.onClick.AddListener(delegate
            {
                OnMenuClicked?.Invoke();
            });
        }
    }
}