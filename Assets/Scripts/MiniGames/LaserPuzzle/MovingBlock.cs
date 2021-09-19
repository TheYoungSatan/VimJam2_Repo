using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{

    private Vector2 _startPos;
    public bool CanMove;

    void Start()
    {
        _startPos = transform.position;
    }

    void Update()
    {
        if (CanMove) transform.position = _startPos + new Vector2(0f, Mathf.Sin(Time.time)/4);
    }
}
