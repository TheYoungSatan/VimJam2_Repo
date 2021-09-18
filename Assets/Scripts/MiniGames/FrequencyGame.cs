using Sound;
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
        private Transform _startPosReference;
        [SerializeField]
        private Transform _startPosModifyable;

        [Header("ReferenceWave")]
        [SerializeField, Range(0.2f, 2)]
        private float _amplitude = 0.5f;
        [SerializeField, Range(0.2f, 5)]
        private float _waveLength = 2;

        private float _amplitudeMod;
        private float _waveLengthMod;

        private Texture2D _texture;
        public int TextureWidth = 128;
        public int TextureHeight = 128;
        public Renderer PixelPlaneRenderer;
        private Color[] _fillPixels;

        public MeshCollider PlaneBounds;

        public override void RunGame()
        {
            InitializeTexture();
            _amplitudeMod = Random.Range(0.2f, 2f);
            _waveLengthMod = Random.Range(0.5f, 3f);
        }

        public override void UpdateGame()
        {
            DrawTravellingSineWave(_startPosReference.position, _startPosModifyable.position, _amplitude, _waveLength, 2, Color.green, Color.red);

            //DrawTravellingSineWave(_startPosModifyable.position, Random.Range(0.2f, 2f), Random.Range(0.2f, 5f), 2, Color.red);
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

        private void DrawTravellingSineWave(Vector3 startPointRef, Vector3 startPointMod, float amplitude, float wavelength, float waveSpeed, Color colorRef, Color colorMod)
        {
            _texture.SetPixels(_fillPixels);
            _texture.Apply();

            var widthPlane = PlaneBounds.bounds.size.x;
            var heightPlane = PlaneBounds.bounds.size.y;

            float xRef = 0f;
            float yRef;
            float kRef = 2 * Mathf.PI / wavelength;
            float wRef = kRef * waveSpeed;
            for (int i = 0; i < 80; i++)
            {
                xRef += i * 0.001f;
                yRef = amplitude * Mathf.Sin(kRef * xRef + wRef * Time.time);

                var worldPosRef = new Vector3(xRef, yRef, 0) + startPointRef;

                var xCoordRef = (((widthPlane / 2) + worldPosRef.x) / widthPlane) * TextureHeight;
                var yCoordRef = (((heightPlane / 2) - worldPosRef.y) / heightPlane) * TextureWidth;

                _texture.SetPixel((int)yCoordRef, (int)xCoordRef, colorRef);
            }

            float xMod = 0f;
            float yMod;
            float kMod = (float)(2 * Mathf.PI / System.Math.Round(_waveLengthMod, 1));
            float wMod = kMod * waveSpeed;
            for (int i = 0; i < 80; i++)
            {
                xMod += i * 0.001f;
                yMod = (float)(System.Math.Round(_amplitudeMod, 1) * Mathf.Sin(kMod * xMod + wMod * Time.time));

                var worldPosMod = new Vector3(xMod, yMod, 0) + startPointMod;

                var xCoordMod = (((widthPlane / 2) + worldPosMod.x) / widthPlane) * TextureHeight;
                var yCoordMod = (((heightPlane / 2) - worldPosMod.y) / heightPlane) * TextureWidth;

                _texture.SetPixel((int)yCoordMod, (int)xCoordMod, colorMod);
            }

            _texture.Apply();
        }

        public void IncreaseAmplitude()
        {
            _amplitudeMod += 0.1f;
            AudioHub.PlaySound(AudioHub.SineWaveChange);
        }

        public void DecreaseAmplitude()
        {
            _amplitudeMod -= 0.1f;
            AudioHub.PlaySound(AudioHub.SineWaveChange);
        }

        public void IncreaseWaveLength()
        {
            _waveLengthMod += 0.1f;
            AudioHub.PlaySound(AudioHub.SineWaveChange);
        }

        public void DecreaseWaveLength()
        {
            _waveLengthMod -= 0.1f;
            AudioHub.PlaySound(AudioHub.SineWaveChange);
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
