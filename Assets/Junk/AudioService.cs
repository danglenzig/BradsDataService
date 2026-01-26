using UnityEngine;

public static class AudioService
{

    // a static class that sits between the AudioRuntime singleton
    // and the customer game objects.
    // handles defensive null checking so the game objects don't have to.

    public static bool TryPlaySoundEffect(string effectID)
    {
        if (AudioRuntime.Instance != null)
        {
            AudioRuntime.Instance.PlaySoundEffect(effectID);
            return true;
        }
        return false;
    }
    // and so on
}
