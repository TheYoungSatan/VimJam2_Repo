using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MiniGame
{
    public class CircleGame : Minigame
    {
        private Vector2 _mousePos;
        private float _circleRadius;

        [SerializeField]
        private float _maxDist;
        private CircleCollider2D[] _circles;

        private Texture2D _texture;
        public int TextureWidth = 128;
        public int TextureHeight = 128;
        public Renderer PixelPlaneRenderer;
        private Color[] _fillPixels;
        public MeshCollider PlaneBounds;

        private bool _canStart;

        public override void RunGame()
        {
            if (Difficulty == MinigameHub.Difficulty.Easy) _maxDist = 2f;
            else if (Difficulty == MinigameHub.Difficulty.Medium) _maxDist = 1.75f;
            else if (Difficulty == MinigameHub.Difficulty.Hard) _maxDist = 1.5f;

            _circles = FindObjectsOfType<CircleCollider2D>();
            InitializeTexture();
        }

        public override void CheckInput()
        {
            _mousePos = Mouse.current.position.ReadValue();
        }

        public override void UpdateGame()
        {
            //gets the mousepos in a Vector2 for the circle and a float2 for the calculations on distance
            var scaledMousePos = Camera.main.ScreenToWorldPoint(_mousePos);
            var correctMousePos = new Vector3(scaledMousePos.x, scaledMousePos.y, 0);
            var float2MousePos = new float2(correctMousePos.x, correctMousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(new Vector2(correctMousePos.x, correctMousePos.y), Vector2.zero, 0);

            if (!_canStart)
            {
                if (hit)
                {
                    if (hit.collider.CompareTag("StartBox"))
                    {
                        _canStart = true;
                    }
                }
            }
            else
            {
                if (hit)
                {
                    if (hit.collider.CompareTag("EndBox"))
                    {
                        Hub.OnGameSucces();
                    }
                }

                //assigns the radius of the circle to the closests distance to a circle
                _circleRadius = ClosestDistToCircle(float2MousePos) + 0.45f;

                //normalize the radius so it can be used to display the color from green to red
                var normalizedRadius = _circleRadius / _maxDist;

                _texture.SetPixels(_fillPixels);
                _texture.Apply();

                var widthPlane = PlaneBounds.bounds.size.x;
                var heightPlane = PlaneBounds.bounds.size.y;

                var xCoord = (((widthPlane / 2) + correctMousePos.x) / widthPlane) * TextureHeight;
                var yCoord = (((heightPlane / 2) - correctMousePos.y) / heightPlane) * TextureWidth;

                var pixelRadius = (TextureWidth / widthPlane) * _circleRadius;

                DrawPixelCircle((int)yCoord, (int)xCoord, (int)pixelRadius, normalizedRadius);

                //if the circle is too big, its game over
                if (_circleRadius >= _maxDist) Hub.OnGameOver();
            }
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

        private void DrawPixelCircle(int xm, int ym, int r, float normalizedradius)
        {
            int x = -r;
            int y = 0;
            int err = 2 - 2 * r;

            do
            {
                Color color = Color.Lerp(Color.yellow, Color.magenta, normalizedradius);

                if (xm - x >= 11 && xm - x <= 46) _texture.SetPixel(xm - x, ym + y, color);
                if (xm - y >= 11 && xm - y <= 46) _texture.SetPixel(xm - y, ym - x, color);
                if (xm + x >= 11 && xm + x <= 46) _texture.SetPixel(xm + x, ym - y, color);
                if (xm + y >= 11 && xm + y <= 46) _texture.SetPixel(xm + y, ym + x, color);

                r = err;
                if (r <= y) err += ++y * 2 + 1;
                if (r > x || err > y) err += ++x * 2 + 1;
            } while (x < 0);

            _texture.Apply();
        }

        private float Length(float2 v)
        {
            return math.sqrt(v.x * v.x + v.y * v.y);
        }

        private float DistToCircle(float2 p, float2 centre, float radius)
        {
            return Length(centre - p) - radius;
        }

        private float ClosestDistToCircle(float2 mousePos)
        {
            float distToScene = _maxDist;

            for (int i = 0; i < _circles.Length; i++)
            {
                float distToCircle = DistToCircle(mousePos, new float2(_circles[i].transform.position.x, _circles[i].transform.position.y), _circles[i].radius);
                distToScene = math.min(distToCircle, distToScene);
            }

            return distToScene;
        }
    }
}
