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
        private LineRenderer _line;
        private List<Vector3> _lineSegments = new List<Vector3>();
        private List<Dot> _dotList = new List<Dot>();

        private Texture2D _texture;
        public int TextureWidth = 128;
        public int TextureHeight = 128;
        public Renderer PixelPlaneRenderer;
        private Color[] _fillPixels;

        public override void RunGame()
        {
            for (int i = 1; i <= _dotAmount; i++)
            {
                Dot d = Instantiate(_dotObject.gameObject, FindSpawnPoint(), Quaternion.identity).GetComponent<Dot>();
                d.transform.SetParent(_spawnSpace.transform);
                d.Number = i;
                d.SetText();

                _dotList.Add(d);

                d.OnClicked += CheckClicked;
            }

            if (TryGetComponent(out LineRenderer line))
                _line = line;
            else
                _line = gameObject.AddComponent<LineRenderer>();

            _line.enabled = false;

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
            Vector3 mp = Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), 0));
            mp.z = 0;
            _line.SetPosition(_current, mp);
        }

        private void CheckClicked(Dot d)
        {
            if (!_runGame) return;

            if (d.Number == _current + 1)
            {
                _current = d.Number;
                Vector3 pos = Camera.main.ScreenToWorldPoint(d.Position);
                pos.z = 0;
                _lineSegments.Add(pos);
                _line.enabled = true;
                _line.positionCount = _lineSegments.Count + 1;
            }
            else
            {
                Hub.OnGameOver();
                _line.positionCount = _lineSegments.Count;
            }

            if (_current == _dotAmount)
            {
                Hub.OnGameSucces();
                _line.positionCount = _lineSegments.Count;
            }

            _line.SetPositions(_lineSegments.ToArray());
        }

        private Vector3 FindSpawnPoint()
        {
            _screenSpaceReduction = Mathf.Abs(_screenSpaceReduction);
            Vector3 screenPosition = new Vector3(
                UnityEngine.Random.Range(_screenSpaceReduction, Screen.width - _screenSpaceReduction), 
                UnityEngine.Random.Range(_screenSpaceReduction, Screen.height - _screenSpaceReduction), 
                0);
            return screenPosition;
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