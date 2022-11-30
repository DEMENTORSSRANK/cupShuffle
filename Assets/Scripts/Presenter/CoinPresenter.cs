using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Presenter
{
    public class CoinPresenter : MonoBehaviour
    {
        [SerializeField] private float _tableY;

        [SerializeField] private float _durationMove = 1.5f;

        [SerializeField] private float[] _tablePlacesX;

        private Transform _normalParent;
        
        public int LastIndexSet { get; private set; }
        
        public async Task MoveToTable(int index)
        {
            LastIndexSet = index;
            
            await transform.DOMove(new Vector2(_tablePlacesX[index], _tableY), _durationMove).AsyncWaitForCompletion();
        }

        public void ResetParent()
        {
            transform.parent = _normalParent;
        }

        private void Awake()
        {
            _normalParent = transform.parent;
        }
    }
}