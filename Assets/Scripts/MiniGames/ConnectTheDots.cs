using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace MiniGame
{
    public class ConnectTheDots : Minigame
    {
        [SerializeField] private Dot _dotObject;
        [SerializeField] private RectTransform _spawnSpace;
        [Space]
        [SerializeField] private int _dotAmount = 6;
        [SerializeField] private float _displayTime = 3f;
        [SerializeField] private float _screenSpaceReduction = 5f;

        private bool _runGame;
        private int _current = 0;
        //private LineRenderer _line;
        //private List<Vector3> _lineSegments = new List<Vector3>();
        private List<Dot> _dotList = new List<Dot>();

        private Texture2D _texture;
        public int TextureWidth = 128;
        public int TextureHeight = 128;
        public Renderer PixelPlaneRenderer;
        private Color[] _fillPixels;
        public MeshCollider PlaneBounds;

        //private Vector3 _mp;

        public override void RunGame()
        {
            if (Difficulty == MinigameHub.Difficulty.Easy) { _dotAmount = 4; _displayTime = 2f; }
            else if (Difficulty == MinigameHub.Difficulty.Medium) { _dotAmount = 6; _displayTime = 2f; }
            else if (Difficulty == MinigameHub.Difficulty.Hard) { _dotAmount = 8; _displayTime = 2.5f; }

            for (int i = 1; i <= _dotAmount; i++)
            {
                Dot d = Instantiate(_dotObject.gameObject, FindSpawnPoint(), Quaternion.identity).GetComponent<Dot>();
                d.transform.SetParent(_spawnSpace.transform);
                d.transform.localScale = Vector3.one;
                d.Number = i;
                d.SetText();

                _dotList.Add(d);

                d.OnClicked += CheckClicked;
            }

            //if (TryGetComponent(out LineRenderer line))
                //_line = line;
            //else
                //_line = gameObject.AddComponent<LineRenderer>();

            //_line.enabled = false;

            if (!FindObjectOfType<EventSystem>())
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));

            InitializeTexture();
            StartCoroutine(DelayTime());
        }

        private void InitializeTexture()
        {
            _texture = new Texture2D(TextureWidth, TextureHeight, TextureFormat.ARGB32, false);
            _texture.filterMode = FilterMode.Point;
            Color fillColor = Color.clear;
            _fillPixels = new Color[TextureWidth * TextureHeight];

            for (int i = 0; i < _fillPixels.Length; i++)
            {
                _fillPixels[i] = fillColor;
            }

            _texture.SetPixels(_fillPixels);
            _texture.Apply();

            PixelPlaneRenderer.material.mainTexture = _texture;
        }

        public override void UpdateGame()
        {
            //_mp = Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), 0));
            //_mp.z = 0;
            //_line.SetPosition(_current, _mp);
        }

        private void CheckClicked(Dot d)
        {
            if (!_runGame) return;

            if (d.Number == _current + 1)
            {
                _current = d.Number;
                Vector3 pos = Camera.main.ScreenToWorldPoint(d.Position);
                pos.z = 0;

                if (d.Number != 1)
                {
                    Vector3 postest = new Vector3(_dotList[_current - 2].Position.x, _dotList[_current - 2].Position.y);
                    var endpos = _dotList[_current-1].Position;

                    var widthPlane = PlaneBounds.bounds.size.x;
                    var heightPlane = PlaneBounds.bounds.size.y;

                    var startXCoord = (((widthPlane / 2) + postest.x) / widthPlane) * TextureHeight;
                    var startYCoord = (((heightPlane / 2) - postest.y) / heightPlane) * TextureWidth;

                    var endXCoord = (((widthPlane / 2) + endpos.x) / widthPlane) * TextureHeight;
                    var endYCoord = (((heightPlane / 2) - endpos.y) / heightPlane) * TextureWidth;

                    DrawLineAlgorithm((int)startYCoord, (int)startXCoord, (int)endYCoord, (int)endXCoord, Color.white);
                }
                    
                //_lineSegments.Add(pos);
                //_line.enabled = true;
                //_line.positionCount = _lineSegments.Count + 1;
            }
            else
            {
                Hub.OnGameOver(Difficulty);
                //_line.positionCount = _lineSegments.Count;
            }

            if (_current == _dotAmount)
            {
                Hub.OnGameSucces(Difficulty);
                //_line.positionCount = _lineSegments.Count;
            }

            //_line.SetPositions(_lineSegments.ToArray());
        }

        private Vector3 FindSpawnPoint()
        {
            _screenSpaceReduction = Mathf.Abs(_screenSpaceReduction);
            Vector3 screenPosition = CheckPossiblePosition();

            return screenPosition;
        }

        private Vector3 CheckPossiblePosition()
        {
            bool val = false;
            Vector3[] worldCorners = new Vector3[4];
            _spawnSpace.GetWorldCorners(worldCorners);

            Vector3 returnVal = new Vector3(UnityEngine.Random.Range(worldCorners[0].x, worldCorners[2].x), UnityEngine.Random.Range(worldCorners[0].y, worldCorners[2].y), 0);

            for(int i = 0; i < _dotList.Count;)
            {
                Rect scanArea = new Rect(_dotList[i].Position.x, _dotList[i].Position.y, 1, 1);
                if (scanArea.Contains(returnVal))
                    returnVal = new Vector3(UnityEngine.Random.Range(worldCorners[0].x, worldCorners[2].x), UnityEngine.Random.Range(worldCorners[0].y, worldCorners[2].y), 0);
                else
                    i++;
            }

            return returnVal;
        }

        IEnumerator DelayTime()
        {
            yield return new WaitForSeconds(_displayTime);
            _runGame = true;

            foreach (var item in _dotList)
                item.SetText(false);
        }

        private void DrawLineAlgorithm(int x0, int y0, int x1, int y1, Color color)
        {
            var dx = Mathf.Abs(x1 - x0);
            var sx = x0 < x1 ? 1 : -1;
            var dy = Mathf.Abs(y1 - y0);
            var sy = y0 < y1 ? 1 : -1;

            var err = dx - dy;

            var loop = true;
            while (loop)
            {
                _texture.SetPixel(x0, y0, color);
                if ((x0 == x1) && (y0 == y1)) loop = false;
                var e2 = 2 * err;
                if (e2 > -dy)
                {
                    err = err - dy;
                    x0 = x0 + sx;
                }
                if (e2 < dx)
                {
                    err = err + dx;
                    y0 = y0 + sy;
                }
            }
            _texture.Apply();
        }
    }
}