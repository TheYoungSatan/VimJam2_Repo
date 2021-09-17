using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignCamera : MonoBehaviour
{
    public Canvas Canvas;

    void Start()
    {
        Canvas.worldCamera = Camera.main;
    }

}
