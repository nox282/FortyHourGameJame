using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymptomsList : MonoBehaviour {

    public int stage = 1;

    public Dictionary<string, string> GetSymptoms()
    {
        switch (stage) {
            case 1:
                return GetStage1();
            case 2:
                return GetStage2();
            case 3:
                return GetStage3();
            case 4:
                return GetStage4();
            case 5:
                return GetStage5();
            default:
                return GetStage5();
        }
    }

    private Dictionary<string, string> GetStage1() {
        Dictionary<string, string> ret = new Dictionary<string, string>();
        ret.Add("Head cold", "Blue pill");
        ret.Add("Cough", "Blue pill");
        ret.Add("Fever", "Ice");
        ret.Add("Wet", "Hairdryer");
        
        return ret;
    }

    private Dictionary<string, string> GetStage2() {
        Dictionary<string, string> ret = new Dictionary<string, string>();
        ret.Add("Vomiting", "Yellow pill");
        ret.Add("Diarrhea", "Yellow pill");
        ret.Add("Fur ball", "Yellow pill");
        ret.Add("Flatulence", "Cork");

        return ret;
    }

    private Dictionary<string, string> GetStage3() {
        Dictionary<string, string> ret = new Dictionary<string, string>();
        ret.Add("Injured paw", "Bandages");
        ret.Add("Broken tail", "Bandages");
        ret.Add("Bleeding", "Needle and thread");
        ret.Add("Damaged teeth", "Cat dentures");
        
        return ret;
    }

    private Dictionary<string, string> GetStage4() {
        Dictionary<string, string> ret = new Dictionary<string, string>();
        ret.Add("Blindness", "torch");
        ret.Add("Deafness", "Paper bag");
        ret.Add("Damaged teeth", "cat dentures");
        ret.Add("Itch", "Cat scratcher");

        return ret;
    }

    private Dictionary<string, string> GetStage5() {
        Dictionary<string, string> ret = new Dictionary<string, string>();

        ret.Add("Uncontrollable miaowing", "Paper bag");
        ret.Add("Itch", "Cat scratcher");
        ret.Add("Hyperactivity", "Ice");
        ret.Add("Deafness", "Paper bag");

        return ret;
    }
}
