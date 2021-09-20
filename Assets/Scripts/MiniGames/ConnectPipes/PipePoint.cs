using System;
using System.Linq;
using UnityEngine;

namespace MiniGame.ConnectPipe
{
    public class PipePoint : MonoBehaviour
    {
        public event Action<PipePoint> OnConncetion;
        public bool IsStartPoint => _isStartPoint;
        public bool IsEndPoint => _isEndPoint;

        [HideInInspector] public Vector3 Position => _transform.position;
        [HideInInspector] public Pipe Pipe;

        [SerializeField] private float _checkArea;
        [SerializeField] private bool _isStartPoint;
        [SerializeField] private bool _isEndPoint;

        private bool _alreadyChecked = false;
        private RectTransform _transform;

        public void Initialize()
        {
            _transform = GetComponent<RectTransform>();

            if (!Pipe.HasStartPoint && !Pipe.HasEndPoint)
            {
                Pipe.HasStartPoint = _isStartPoint;
                Pipe.HasEndPoint = _isEndPoint;
            }
        }

        public void CheckConnection()
        {
            if (_alreadyChecked || _isStartPoint || _isEndPoint) return;
            
            PipePoint point = null;

            foreach (var p in ConnectPipes.Points)
            {
                if(Vector3.Distance(p.Position, Position) < _checkArea && p != this)
                {
                    point = p;
                    break;
                }
            }

            _alreadyChecked = true;
            point?.SendCheckConnection();
        }

        public void SendCheckConnection()
        {
            OnConncetion?.Invoke(this);
        }

        public void ResetValues() => _alreadyChecked = false;

        private void OnDrawGizmos()
        {
            if (_isStartPoint)
                Gizmos.color = Color.green;
            else if (_isEndPoint)
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.yellow;

            if(_transform != null)
                Gizmos.DrawWireSphere(_transform.position, _checkArea);
        }
    }
}