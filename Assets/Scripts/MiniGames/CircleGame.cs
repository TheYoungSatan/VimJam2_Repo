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

        private float _circleRadius = 3;
        private float _maxDist = 30f;

        //private BoxCollider2D[] _boxes;
        private CircleCollider2D[] _circles;

        [SerializeField]
        private LineRenderer _lineRenderer;

        public override void RunGame()
        {
            //_boxes = FindObjectsOfType<BoxCollider2D>();
            _circles = FindObjectsOfType<CircleCollider2D>();
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

        private float Length(float2 v)
        {
            return math.sqrt(v.x * v.x + v.y * v.y);
        }

        private float DistToCircle(float2 p, float2 centre, float radius)
        {
            return Length(centre - p) - radius;
        }

        //private float DistToBox(float2 p, float2 centre, float2 size)
        //{
        //    float2 offset = math.abs(p - centre) - size;

        //    float unsignedDist = Length(math.max(offset, 0));

        //    float distInsideBox = math.max(math.min(offset, 0));

        //    return unsignedDist + distInsideBox;
        //}

        private float DistanceFromClosestBox(float2 mousePos)
        {
            float distToScene = _maxDist;

            //for (int i = 0; i < _boxes.Length; i++)
            //{
            //    float currentDistToBox = DistToBox(mousePos, new float2(_boxes[i].transform.position.x, _boxes[i].transform.position.y), _boxes[i].size);
            //    distToBox = math.min(currentDistToBox, distToBox);
            //}

            for (int i = 0; i < _circles.Length; i++)
            {
                float distToCircle = DistToCircle(mousePos, new float2(_circles[i].transform.position.x, _circles[i].transform.position.y), _circles[i].radius);
                distToScene = math.min(distToCircle, distToScene);
            }

            return distToScene;
        }

        public override void UpdateGame()
        {
            var scaledMousePos = Camera.main.ScreenToWorldPoint(_mousePos);
            var correctMousePos = new Vector3(scaledMousePos.x, scaledMousePos.y, 0);

            var float2MousePos = new float2(correctMousePos.x, correctMousePos.y);
            _circleRadius = DistanceFromClosestBox(float2MousePos);

            DrawPolygon(48, _circleRadius, correctMousePos, 0.1f, 0.1f);
        }
    }
}
