using UnityEngine;

public class SwitchEventCaller : AkTriggerBase
{
    [SerializeField] private GameObject _checkObject;
    
    private void Awake()
    {
        if (_checkObject == null)
            _checkObject = FindObjectOfType<PlayerController>().gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != _checkObject)
            return;

        if (triggerDelegate != null)
            triggerDelegate(_checkObject);
    }
}