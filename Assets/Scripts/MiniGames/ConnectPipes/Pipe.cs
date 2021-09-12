using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MiniGame.ConnectPipe
{
    public class Pipe : MonoBehaviour, IPointerClickHandler
    {
        [HideInInspector] public bool HasStartPoint = false;
        [HideInInspector] public bool HasEndPoint = false;
        public event Action OnPipeRotated;
        public event Action OnEndReached;

        public PipePoint[] Points => _points;
        private PipePoint[] _points;
        private bool _canRotate = true;        

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_canRotate) return;

            Vector3 currentRotation = transform.rotation.eulerAngles;
            currentRotation.z -= 90;
            transform.rotation = Quaternion.Euler(currentRotation);

            OnPipeRotated?.Invoke();
        }

        public void OnInitialize()
        {
            _points = GetComponentsInChildren<PipePoint>();

            foreach (var p in _points)
            {
                p.Pipe = this;
                p.Initialize();
                p.OnConncetion += CheckConnection;

                if (_canRotate)
                    _canRotate = p.IsStartPoint || p.IsEndPoint ? false : true;
            }

            if (!FindObjectOfType<EventSystem>())
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }

        public void CheckConnection(PipePoint point = null)
        {
            if (HasEndPoint)
            {
                OnEndReached?.Invoke();
                return;
            }

            foreach (var p in _points)
            {
                if (p != null)
                {
                    if (p != point)
                        p.CheckConnection();
                }
                else
                    p.CheckConnection();
            }
        }
    }
}
