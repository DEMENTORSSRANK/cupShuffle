using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UserInterface.Screens
{
    public class GameScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Text _score;

        [SerializeField] private GameObject _chooseHelper;

        private Color _startColor;

        public GameObject ChooseHelper => _chooseHelper;

        public async void UpdateScoreAsync(int score)
        {
            _score.text = score.ToString();

            _score.transform.DOPunchScale(Vector3.one, .3f, 5, .5f);

            await _score.DOColor(Color.yellow, .15f).AsyncWaitForCompletion();

            _score.DOColor(_startColor, .15f);
        }

        private void Awake()
        {
            _startColor = _score.color;
        }
    }
}