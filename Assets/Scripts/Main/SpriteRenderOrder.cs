using UnityEngine;

public class SpriteRenderOrder : MonoBehaviour
{
    private SpriteRenderer _playerRenderer;
    private SpriteRenderer[] _renderers;

    private void Awake()
    {
        _renderers = FindObjectsOfType<SpriteRenderer>();
        _playerRenderer = FindObjectOfType<PlayerController>().gameObject.GetComponentInChildren<SpriteRenderer>();

        foreach (var renderer in _renderers)
            renderer.sortingOrder = (int)(renderer.transform.position.y * -100);
    }

    private void Update()
    {
        _playerRenderer.sortingOrder = (int)(_playerRenderer.transform.position.y * -100);
    }
}