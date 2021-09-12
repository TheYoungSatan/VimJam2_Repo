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
        private Vector2 _distanceVector;
        private float _circleRadius = 3;
        private Vector2 _mousePos;

        [SerializeField]
        private LineRenderer _lineRenderer;

        public override void RunGame()
        {
            
        }

        public override void CheckInput()
        {
            _mousePos = Mouse.current.position.ReadValue();
        }

        private void DrawPolygon(int vertexNumber, float radius, Vector3 centerPos, float startWidth, float endWidth)
        {
            _lineRenderer.startWidth = startWidth;
            _lineRenderer.endWidth = endWidth;
            _lineRenderer.loop = true;
            float angle = 2 * Mathf.PI / vertexNumber;
            _lineRenderer.positionCount = vertexNumber;

            for (int i = 0; i < vertexNumber; i++)
            {
                Matrix4x4 rotationMatrix = new Matrix4x4(new Vector4(Mathf.Cos(angle * i), Mathf.Sin(angle * i), 0, 0),
                                                         new Vector4(-1 * Mathf.Sin(angle * i), Mathf.Cos(angle * i), 0, 0),
                                           new Vector4(0, 0, 1, 0),
                                           new Vector4(0, 0, 0, 1));
                Vector3 initialRelativePosition = new Vector3(0, radius, 0);
                _lineRenderer.SetPosition(i, centerPos + rotationMatrix.MultiplyPoint(initialRelativePosition));

            }
        }
         

        public override void UpdateGame()
        {
            var scaledMousePos = Camera.main.ScreenToWorldPoint(_mousePos);
            var correctMousePos = new Vector3(scaledMousePos.x, scaledMousePos.y, 0);

            DrawPolygon(48, _circleRadius, correctMousePos, 0.2f, 0.2f);
        }
    }
}
