using System;
using System.Collections.Generic;
using UnityEngine;

public class DataBank : MonoBehaviour
{
    public int stage;
    public List<GameObject> items;
    public List<GameObject> symptoms;

    public Symptom GetSymptom(string name)
    {
        foreach (GameObject s in symptoms)
            if (Equals(s.GetComponent<Symptom>().name, name))
                return s.GetComponent<Symptom>();

        return null;
    }
}
