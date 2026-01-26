using UnityEngine;
using System.Collections.Generic;
using BradsDataService;

public class AudioRuntime : Singleton<AudioRuntime>
{
    //////////////////////////////////////////////////////
    // attach this to an empty game object in the scene //
    //////////////////////////////////////////////////////

    private Dictionary<string, AudioClip> audioDict = new();

    // ...

    public void PlaySoundEffect(string effectID)
    {
        if (audioDict.TryGetValue(effectID, out AudioClip clip))
        {
            // play clip
        }
    }

    // and so on
}
