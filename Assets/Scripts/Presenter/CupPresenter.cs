using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Presenter
{
    public class CupPresenter : MonoBehaviour
    {
        [SerializeField] private float _upY;

        [SerializeField] private float _normalY;

        [SerializeField] private float _duration = 2;

        private float _startPositionX;
        
        public event Action Pressed;

        public async Task MoveUp()
        {
            await transform.DOMoveY(_upY, _duration).AsyncWaitForCompletion();
        }

        public async Task MoveNormal()
        {
            await transform.DOMoveY(_normalY, _duration).AsyncWaitForCompletion();
        }

        public void ToStartPositionX()
        {
            transform.position = new Vector3(_startPositionX, transform.position.y, transform.position.z);
        }
        
        private void OnMouseDown()
        {
            Pressed?.Invoke();
        }

        private void Awake()
        {
            _startPositionX = transform.position.x;
        }
    }
}