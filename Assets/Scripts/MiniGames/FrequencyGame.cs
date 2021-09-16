using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MiniGame
{
    public class FrequencyGame : Minigame
    {
        [SerializeField]
        private LineRenderer _movingLine;
        [SerializeField]
        private LineRenderer _referenceLine;
        [SerializeField]
        private Transform _startPos;

        [SerializeField, Range(0.2f, 2)]
        private float _amplitude = 1;
        [SerializeField, Range(0.2f, 5)]
        private float _waveLength = 2;


        private Texture2D _texture;
        public int TextureWidth = 128;
        public int TextureHeight = 128;
        public Renderer PixelPlaneRenderer;
        private Color[] _fillPixels;

        public MeshCollider PlaneBounds;

        public override void RunGame()
        {
            InitializeTexture();
            //DrawSineWave(_startPos.position, Random.Range(0.2f, 2f), Random.Range(0.2f, 2f));
        }

        public override void UpdateGame()
        {
            DrawTravellingSineWave(_startPos.position, _amplitude, _waveLength, 2);
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

        private void DrawTravellingSineWave(Vector3 startPoint, float amplitude, float wavelength, float waveSpeed)
        {
            _texture.SetPixels(_fillPixels);
            _texture.Apply();

            float x = 0f;
            float y;
            float k = 2 * Mathf.PI / wavelength;
            float w = k * waveSpeed;
            for (int i = 0; i < 80; i++)
            {
                x += i * 0.001f;
                y = amplitude * Mathf.Sin(k * x + w * Time.time);

                var worldPos = new Vector3(x, y, 0) + startPoint;

                var widthPlane = PlaneBounds.bounds.size.x;
                var heightPlane = PlaneBounds.bounds.size.y;

                var xCoord = (((widthPlane / 2) + worldPos.x) / widthPlane) * TextureHeight;
                var yCoord = (((heightPlane / 2) - worldPos.y) / heightPlane) * TextureWidth;

                _texture.SetPixel((int)yCoord, (int)xCoord, Color.red);
                _texture.Apply();
            }
        }

        void DrawSineWave(Vector3 startPoint, float amplitude, float wavelength)
        {
            float x = 0f;
            float y;
            float k = 2 * Mathf.PI / wavelength;
            for (int i = 0; i < 140; i++)
            {
                x += i * 0.001f;
                y = amplitude * Mathf.Sin(k * x);

                var worldPos = new Vector3(x, y, 0) + startPoint;

                var widthPlane = PlaneBounds.bounds.size.x;
                var heightPlane = PlaneBounds.bounds.size.y;

                var xCoord = (((widthPlane / 2) + worldPos.x) / widthPlane) * TextureHeight;
                var yCoord = (((heightPlane / 2) - worldPos.y) / heightPlane) * TextureWidth;

                _texture.SetPixel((int)yCoord, (int)xCoord, Color.red);
                _texture.Apply();
            }
        }

        public override void CheckInput()
        {
            
        }
    }

}
