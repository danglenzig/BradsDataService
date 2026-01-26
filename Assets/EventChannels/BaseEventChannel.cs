using UnityEngine;
namespace BradsEvents
{
    public abstract class BaseEventChannel<T> : ScriptableObject
    {
        public event System.Action<T> OnEventTriggered;
        public void TriggerEvent(T payload)
        {
            OnEventTriggered?.Invoke(payload);
        }
    }
    public abstract class BaseEventChannel: ScriptableObject
    {
        public event System.Action OnEventTriggered;
        public void TriggerEvent()
        {
            OnEventTriggered?.Invoke();
        }
    }

    [CreateAssetMenu(fileName = "EmptyEvent", menuName = "Event Channels/Empty Payload")]
    public class EmptyEventChannel: BaseEventChannel { }

    [CreateAssetMenu(fileName = "StringEvent", menuName = "Event Channels/String Payload")]
    public class StringEventChannel : BaseEventChannel<string> { }

    [CreateAssetMenu(fileName = "BoolEvent", menuName = "Event Channels/Bool Payload")]
    public class BoolEventChannel : BaseEventChannel<bool> { }

    [CreateAssetMenu(fileName = "IntEvent", menuName = "Event Channels/Int Payload")]
    public class IntEventChannel : BaseEventChannel<int> { }

    [CreateAssetMenu(fileName = "FloatEvent", menuName = "Event Channels/Float Payload")]
    public class FloatEventChannel : BaseEventChannel<float> { }
}



