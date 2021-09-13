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
        private float _maxDist = 3f;
        [SerializeField, Range(4, 96)]
        private float _circlePoints = 48;
        [SerializeField]
        private LineRenderer _lineRenderer;

        private CircleCollider2D[] _circles;

        public override void RunGame()
        {
            _circles = FindObjectsOfType<CircleCollider2D>();
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

            //assigns the radius of the circle to the closests distance to a circle
            _circleRadius = ClosestDistToCircle(float2MousePos);

            //normalize the radius so it can be used to display the color from green to red
            var normalizedRadius = _circleRadius / _maxDist;

            _lineRenderer.startColor = Color.Lerp(Color.green, Color.red, normalizedRadius);
            _lineRenderer.endColor = Color.Lerp(Color.green, Color.red, normalizedRadius);

            //draws a shape with the given amount of points, more points = rounder
            DrawPolygon(_circlePoints, _circleRadius, correctMousePos, 0.1f, 0.1f);

            //if the circle is too big, its game over
            if (_circleRadius >= _maxDist) Hub.OnGameOver();
        }

        private void DrawPolygon(float vertexNumber, float radius, Vector3 centerPos, float startWidth, float endWidth)
        {
            _lineRenderer.startWidth = startWidth;
            _lineRenderer.endWidth = endWidth;
            _lineRenderer.loop = true;
            float angle = 2 * Mathf.PI / vertexNumber;
            _lineRenderer.positionCount = (int)vertexNumber;

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
