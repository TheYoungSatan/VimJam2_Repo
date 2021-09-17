using UnityEngine;

namespace Wwise
{
    public class SwitchEventCaller : AkTriggerBase
    {
        public void CallSwitch()
        {
            if (triggerDelegate != null)
            {
                triggerDelegate(null);
            }
        }
    }
}