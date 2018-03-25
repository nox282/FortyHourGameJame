using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeController : MonoBehaviour {

    public Image Symptom;
    public Image Item;

    public void SetSymptom(Image icon) {
        Symptom.sprite = icon.sprite;
    }

    public void SetItem(Image icon) {
        this.Item.sprite = icon.sprite;
    }
}
