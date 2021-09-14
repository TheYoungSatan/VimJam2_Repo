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
        [SerializeField, Range(0.2f, 2)]
        private float _waveLength = 2;

        public override void RunGame()
        {
            DrawSineWave(_startPos.position, Random.Range(0.2f, 2f), Random.Range(0.2f, 2f));
        }

        public override void UpdateGame()
        {
            DrawTravellingSineWave(_startPos.position, _amplitude, _waveLength, 2);
        }

        private void DrawTravellingSineWave(Vector3 startPoint, float amplitude, float wavelength, float waveSpeed)
        {
            float x = 0f;
            float y;
            float k = 2 * Mathf.PI / wavelength;
            float w = k * waveSpeed;
            _movingLine.positionCount = 140;
            for (int i = 0; i < _movingLine.positionCount; i++)
            {
                x += i * 0.001f;
                y = amplitude * Mathf.Sin(k * x + w * Time.time);
                _movingLine.SetPosition(i, new Vector3(x, y, 0) + startPoint);
            }
        }

        void DrawSineWave(Vector3 startPoint, float amplitude, float wavelength)
        {
            float x = 0f;
            float y;
            float k = 2 * Mathf.PI / wavelength;
            _referenceLine.positionCount = 140;
            for (int i = 0; i < _referenceLine.positionCount; i++)
            {
                x += i * 0.001f;
                y = amplitude * Mathf.Sin(k * x);
                _referenceLine.SetPosition(i, new Vector3(x, y, 0) + startPoint);
            }
        }

        public override void CheckInput()
        {
            
        }
    }

}
