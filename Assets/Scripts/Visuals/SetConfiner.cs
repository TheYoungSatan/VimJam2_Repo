using UnityEngine;
using Cinemachine;

public class SetConfiner : MonoBehaviour
{
    [SerializeField] private Collider2D _boundingShape;
    private CinemachineConfiner _cConfiner;

    private void Awake()
    {
        _cConfiner = GetComponent<CinemachineConfiner>();
        _cConfiner.m_BoundingShape2D = _boundingShape;
    }
}
