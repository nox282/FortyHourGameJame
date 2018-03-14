using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Symptom {
    private string symptomName;
    private Item cure;
    private int points;

    public Symptom(string _symptomName, Item _cure, int _points) {
        symptomName = _symptomName;
        points = _points;
        cure = _cure;
    }

    public string GetName() {
        return symptomName;
    }

    public Item GetCure() {
        return cure;
    }

    public int GetPoints() {
        return points;
    }
}
