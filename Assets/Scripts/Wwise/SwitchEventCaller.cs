using UnityEngine;

public class SwitchEventCaller : AkTriggerBase
{
    [SerializeField] private GameObject _checkObject;
    public bool _checkTrigger = false;

    private void Awake()
    {
        if (!_checkObject)
            _checkObject = FindObjectOfType<PlayerController>().gameObject;
    }

    private void Update()
    {
        if (!_checkObject)
            _checkObject = FindObjectOfType<PlayerController>().gameObject;
    }

    public void CallSwitch()
    {
        if (triggerDelegate != null)
        {
            triggerDelegate(_checkObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_checkTrigger) return;

        if (collision.gameObject == _checkObject)
            CallSwitch();
    }
}