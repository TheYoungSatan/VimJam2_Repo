using Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
        private float _amplitude;
        [SerializeField, Range(0.2f, 5)]
        private float _waveLength;

        private float _amplitudeModvalue;
        private float _waveLengthModvalue;

        private Texture2D _texture;
        public int TextureWidth = 128;
        public int TextureHeight = 128;
        public Renderer PixelPlaneRenderer;
        private Color[] _fillPixels;

        public MeshCollider PlaneBounds;

        private float _timer;
        private float _maxTime;

        [SerializeField]
        private Image _screenImage;
        [SerializeField]
        private Sprite _light0;
        [SerializeField]
        private Sprite _light1;
        [SerializeField]
        private Sprite _light2;
        [SerializeField]
        private Sprite _light3;
        [SerializeField]
        private Text _timeLeft;

        public override void RunGame()
        {
            InitializeTexture();

            _amplitudeModvalue = (float)System.Math.Round(Random.Range(0.2f, 0.8f), 1);
            _waveLengthModvalue = (float)System.Math.Round(Random.Range(1f, 2.5f), 1);

            _amplitude = (float)System.Math.Round(Random.Range(0.2f, 0.8f), 1);
            _waveLength = (float)System.Math.Round(Random.Range(1f, 2.5f), 1);

            if (Difficulty == MinigameHub.Difficulty.Easy) _maxTime = 30f;
            else if (Difficulty == MinigameHub.Difficulty.Medium) _maxTime = 20f;
            else if (Difficulty == MinigameHub.Difficulty.Hard) _maxTime = 12f;
        }

        public override void UpdateGame()
        {
            _timer += Time.deltaTime;

            _timeLeft.text = "00:" + Mathf.Round(_maxTime - _timer).ToString();

            DrawTravellingSineWave(_startPosReference.position, _startPosModifyable.position, _amplitude, _waveLength, 2, Color.green, Color.red);

            if ((float)System.Math.Round(_amplitudeModvalue,1) == _amplitude && (float)System.Math.Round(_waveLengthModvalue,1) == _waveLength)
            {
                Hub.OnGameSucces();
            }

            if (_timer >= _maxTime) Hub.OnGameOver(); 
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

            //here i clamp the values so you cant go outside the screen
            var clampedAmplitude = Mathf.Clamp(_amplitudeModvalue, 0.2f, 0.8f);

            var clampedWaveLength = Mathf.Clamp(_waveLengthModvalue, 1f, 2.5f);

            float xMod = 0f;
            float yMod;
            float kMod = (float)(2 * Mathf.PI / System.Math.Round(clampedWaveLength, 1));
            float wMod = kMod * waveSpeed;
            for (int i = 0; i < 80; i++)
            {
                xMod += i * 0.001f;
                yMod = (float)(System.Math.Round(clampedAmplitude, 1) * Mathf.Sin(kMod * xMod + wMod * Time.time));

                var worldPosMod = new Vector3(xMod, yMod, 0) + startPointMod;

                var xCoordMod = (((widthPlane / 2) + worldPosMod.x) / widthPlane) * TextureHeight;
                var yCoordMod = (((heightPlane / 2) - worldPosMod.y) / heightPlane) * TextureWidth;

                _texture.SetPixel((int)yCoordMod, (int)xCoordMod, colorMod);
            }

            _texture.Apply();
        }

        private void SwapSprites()
        {
            var currentAmpDifference = 0.8f - _amplitudeModvalue;
            var currentWaveDifference = 2.5f - _waveLengthModvalue;
            var avgDifference = (currentAmpDifference + currentWaveDifference) / 2.1f;

            if (avgDifference <= 2.1f && avgDifference > 1.575f) _screenImage.sprite = _light0;
            else if (avgDifference <= 1.575f && avgDifference > 1.05f) _screenImage.sprite = _light1;
            else if (avgDifference <= 1.05f && avgDifference > 0.525f) _screenImage.sprite = _light2;
            else if (avgDifference <= 0.525f && avgDifference >= 0f) _screenImage.sprite = _light3;
        }

        public void IncreaseAmplitude()
        {
            _amplitudeModvalue += 0.1f;

            SwapSprites();
            
            //AudioHub.PlaySound(AudioHub.SineWaveChange);
        }

        public void DecreaseAmplitude()
        {
            _amplitudeModvalue -= 0.1f;

            SwapSprites();
            //AudioHub.PlaySound(AudioHub.SineWaveChange);
        }

        public void IncreaseWaveLength()
        {
            _waveLengthModvalue += 0.1f;

            SwapSprites();
            //AudioHub.PlaySound(AudioHub.SineWaveChange);
        }

        public void DecreaseWaveLength()
        {
            _waveLengthModvalue -= 0.1f;

            SwapSprites();
            //AudioHub.PlaySound(AudioHub.SineWaveChange);
        }

        public override void CheckInput()
        {
            base.CheckInput();
        }
    }

}
