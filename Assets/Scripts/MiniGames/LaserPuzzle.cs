using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class LaserPuzzle : Minigame
    {
        public int Reflections;
        public float MaxLength;

        [SerializeField]
        private LineRenderer _lineRenderer;
        [SerializeField]
        private GameObject _laserPointer;
        private Ray2D _ray;
        private RaycastHit2D _hit;
        private Vector3 _direction;


        public override void RunGame()
        {
            base.RunGame();
        }

        public override void UpdateGame()
        {
            var origin = new Vector2(_laserPointer.transform.position.x, _laserPointer.transform.position.y);
            _ray = new Ray2D(origin, _laserPointer.transform.right);

            _lineRenderer.positionCount = 1;
            _lineRenderer.SetPosition(0, origin);
            float remainingLength = MaxLength;

            for (int i = 0; i < Reflections; i++)
            {
                _hit = Physics2D.Raycast(_ray.origin, _ray.direction, remainingLength);
                if (_hit)
                {
                    _lineRenderer.positionCount += 1;
                    var hitpoint = new Vector2(_hit.point.x, _hit.point.y);
                    _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, hitpoint);
                    remainingLength -= Vector2.Distance(_ray.origin, hitpoint);
                    _ray = new Ray2D(hitpoint, Vector2.Reflect(_ray.direction, _hit.normal));

                    if (_hit.collider.tag != "Mirror")
                    {
                        break;
                    }  
                }
                else
                {
                    _lineRenderer.positionCount += 1;
                    _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _ray.origin + _ray.direction * remainingLength);
                }
            }
        }

        public override void CheckInput()
        {
            base.CheckInput();
        }
    }

}
