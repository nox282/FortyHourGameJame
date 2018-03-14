using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseInterface : MonoBehaviour {
    public void callEvent(string id) {
        AkSoundEngine.PostEvent(id, gameObject);
        Debug.Log("[Wwise] Event: " + id + " called at " + Time.time);
    }

    public void callTrigger(string id) {
        AkSoundEngine.PostTrigger(id, gameObject);
        Debug.Log("[Wwise] Trigger: " + id + " called at " + Time.time);
    }

    public void callSwitch(string id, string state) {
        AkSoundEngine.SetSwitch(id, state, gameObject);
        Debug.Log("[Wwise] State: " + id + " switched to " + state + " called at " + Time.time);
    }

    public void setState(string groupState, string state) {
        AkSoundEngine.SetState(groupState, state);
        Debug.Log("[Wwise] State: " + groupState + " switched to " + state + " called at " + Time.time);
    }

    public void setGameParameter(string id, int value) {
        AkSoundEngine.SetRTPCValue(id, value);
        Debug.Log("[Wwise] RTPC: " + id + " value set at " + value + " called at " + Time.time);
    }
}
