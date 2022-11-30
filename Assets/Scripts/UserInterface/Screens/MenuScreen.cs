using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.Screens
{
    public class MenuScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Text _bestScore;

        [SerializeField] private Button _start;

        public event Action OnStartClicked;

        public void UpdateBestScore(int value)
        {
            _bestScore.text = $"Best score: {value}";
        }

        private void Start()
        {
            _start.onClick.AddListener(delegate
            {
                OnStartClicked?.Invoke();
            });
        }
    }
}