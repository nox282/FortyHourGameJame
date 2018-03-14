using System;
using System.Collections.Generic;
using UnityEngine;

public class DataBank : MonoBehaviour
{
    public List<GameObject> items;

    public SymptomsList symptomsList;
    public List<Symptom> symptoms = new List<Symptom>();

    void Awake() {
        BuildSymptoms(symptomsList.GetSymptoms());
    }

    private Item SearchItem(string name) {
        foreach (GameObject gb in items)
        {
            Item item = gb.GetComponent<Item>();
            if (String.Equals(item.itemName, name))
                return item;
        }

        return null;
    }

    private void BuildSymptoms(Dictionary<string, string> source) {
        foreach (KeyValuePair<string, string> pair in source) {
            Item item = SearchItem(pair.Value);
            if (item || string.Equals(pair.Key, "fatigue")) {
                Symptom symptom = new Symptom(pair.Key, item, 0);
                symptoms.Add(symptom);
            }
        }
    }

    public Symptom GetSymptom(string name)
    {
        foreach (Symptom s in symptoms)
            if (string.Equals(s.GetName(), name))
                return s;

        return null;
    }
}
