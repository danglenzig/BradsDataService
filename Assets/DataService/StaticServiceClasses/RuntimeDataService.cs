using UnityEngine;
namespace BradsDataService
{

    // a static class that sits between the RuntimeDataOwner singleton
    // and the customer game objects.
    // handles defensive null checking so the game objects don't have to.

    public static class RuntimeDataService
    {
        public static bool TryGetRuntimeData(out RuntimeData data)
        {
            if (RuntimeDataOwner.Instance != null)
            {
                data = RuntimeDataOwner.Instance.GetFullRuntimeData();
                return true;
            }
            data = null;
            return data != null;
        }


        public static bool TryGetRuntimeValue<T>(string fieldID, out T value)
        {
            value = default;
            if(RuntimeDataOwner.Instance != null)
            {
                if(RuntimeDataOwner.Instance.TryGetRuntimeValue(fieldID, out T _value))
                {
                    value = _value;
                    return true;
                }
            }
            return false;
        }
        public static bool TryUpdateRuntimeDatum<T>(T value, string fieldID)
        {
            if (RuntimeDataOwner.Instance != null)
            {
                if (RuntimeDataOwner.Instance.TryUpdateRuntimeDatum<T>(value, fieldID))
                {
                    return true;
                }
            }
            return false;
        }



    }
}


