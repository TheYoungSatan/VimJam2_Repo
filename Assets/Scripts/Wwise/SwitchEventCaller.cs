using UnityEngine;

public class SwitchEventCaller : AkTriggerBase
{
    public void CallSwitch()
    {
        if (triggerDelegate != null)
        {
            triggerDelegate(this.gameObject);
        }
    }
}