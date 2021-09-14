using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{

    private Vector2 _startPos;

    void Start()
    {
        _startPos = transform.position;
    }

    void Update()
    {
        transform.position = _startPos + new Vector2(0f, Mathf.Sin(Time.time));
    }
}
