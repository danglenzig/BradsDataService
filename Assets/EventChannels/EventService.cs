using UnityEngine;
namespace BradsEvents
{
    public static class EventService
    {
        public static bool TryTriggerDataUpdatedEvent()
        {
            if (EventRelay.Instance == null) { return false; }
            EventRelay.Instance.TriggerDataUpdatedEvent();
            return true;
        }
    }
}


