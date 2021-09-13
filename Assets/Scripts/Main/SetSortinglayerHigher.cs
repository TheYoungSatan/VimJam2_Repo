using UnityEngine;

public class SetSortinglayerHigher : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _compareRenderer;
    private SpriteRenderer _selfRenderer;

    private void Start()
    {
        _selfRenderer = GetComponent<SpriteRenderer>();
        _selfRenderer.sortingOrder = _compareRenderer.sortingOrder + 1;
    }
}
