using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private Text _text;
    private RectTransform _transform;

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
    }

    public void SetActive(bool value) => gameObject.SetActive(value);
    public void SetPosition(Vector3 pos) => _transform.position = pos;
    public void SetText(string text) => _text.text = text;
}