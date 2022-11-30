using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Presenter
{
    public class GamePresenter : MonoBehaviour
    {
        [SerializeField] private CupPresenter[] _cupPresenters;

        [SerializeField] private CoinPresenter _coin;

        [SerializeField] private float _shuffleDuration = 3;

        [SerializeField] private int _shufflesCountMin = 4;

        [SerializeField] private int _shufflesCountMax = 9;

        private float[] _cupPositionsX;

        private Task[] _shuffleTasks;

        private List<float> _shufflePositionsX;

        public event Action<int> OnPressed;

        public void UpdateDuration(float duration)
        {
            if (duration < 0)
                throw new ArgumentOutOfRangeException(nameof(duration));

            _shuffleDuration = duration;
        }

        public async Task ShuffleCupsRandomCountTimes()
        {
            PinCoinParent();

            int shufflesCount = Random.Range(_shufflesCountMin, _shufflesCountMax);

            for (int i = 0; i < shufflesCount; i++)
                await ShuffleCups();

            UnPinCoinParent();
        }

        private async Task ShuffleCups()
        {
            _shufflePositionsX = _cupPositionsX.ToList();

            ListExtensions.MixList(_shufflePositionsX);
            
            while (ValuesEqual(_cupPresenters.Select(x => x.transform.position.x).ToArray(), _shufflePositionsX)) 
                ListExtensions.MixList(_shufflePositionsX);

            _shuffleTasks = new Task[_shufflePositionsX.Count];

            for (int i = 0; i < _shufflePositionsX.Count; i++)
            {
                _shuffleTasks[i] = _cupPresenters[i].transform.DOMoveX(_shufflePositionsX[i], _shuffleDuration)
                    .AsyncWaitForCompletion();
            }

            await Task.WhenAll(_shuffleTasks);
        }

        private bool ValuesEqual(float[] massive, List<float> list)
        {
            for (int i = 0; i < Mathf.Min(massive.Length, list.Count); i++)
            {
                if (Math.Abs(massive[i] - list[i]) > .1f)
                    return false;
            }

            return true;
        }

        public async Task MoveCoinToIndex(int index)
        {
            await _coin.MoveToTable(index);
        }

        public async Task MoveCupUp(int index)
        {
            await _cupPresenters[index].MoveUp();
        }

        public async Task MoveAllCupsUp()
        {
            for (int i = 0; i < _cupPresenters.Length - 1; i++)
            {
                _cupPresenters[i].MoveUp();
            }

            await _cupPresenters[_cupPresenters.Length - 1].MoveUp();
        }

        public async Task MoveAllCupsNormal()
        {
            for (int i = 0; i < _cupPresenters.Length - 1; i++)
            {
                _cupPresenters[i].MoveNormal();
            }

            await _cupPresenters[_cupPresenters.Length - 1].MoveNormal();
        }

        public void ResetCupsPositions()
        {
            foreach (var cup in _cupPresenters) 
                cup.ToStartPositionX();
        }
        
        private void PinCoinParent()
        {
            _coin.transform.parent = _cupPresenters[_coin.LastIndexSet].transform;
        }

        private void UnPinCoinParent()
        {
            _coin.ResetParent();
        }

        private void Awake()
        {
            _cupPositionsX = _cupPresenters.Select(x => x.transform.position.x).ToArray();

            for (int i = 0; i < _cupPresenters.Length; i++)
            {
                int i1 = i;

                _cupPresenters[i].Pressed += delegate { OnPressed?.Invoke(i1); };
            }
        }
    }
}