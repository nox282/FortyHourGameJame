using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Text[] inventory_text;
    public bool display_xbox_controls = true;
    public Image[] inventory_icon;

    public string ItemIconTag = "ItemIcon";
    private List<Image> ItemIcons;

    // Use this for initialization
    void Start() {
        GameObject[] icons = GameObject.FindGameObjectsWithTag(ItemIconTag);
        if(icons.Length > 0) {
            ItemIcons = new List<Image>();
            foreach (var i in icons)
                ItemIcons.Add(i.GetComponent<Image>());
        }

        DataBank data = GameObject.FindObjectOfType<DataBank>();
        SetSymptoms(data.symptoms);
    }

    private void SetSymptoms(List<GameObject> symptoms) {
        RecipesUIController recipes = GetComponentInChildren<RecipesUIController>();
        foreach (GameObject s in symptoms)
            recipes.SetRecipe(s.GetComponent<Symptom>());
    }

    public void DisplayControls(bool xboxControls) {
        display_xbox_controls = xboxControls;
        DisplayControls();
    }

    public void DisplayControls() {
        if (display_xbox_controls) {
            inventory_text[0].text = "X";
            inventory_text[1].text = "Y";
            inventory_text[2].text = "B";
            inventory_text[3].text = "A";
        } else {
            inventory_text[0].text = "1";
            inventory_text[1].text = "2";
            inventory_text[2].text = "3";
            inventory_text[3].text = "4";

        }
    }

    public void OnItemReceived(int slot, Item item) {
        Debug.Log(item.gameObject.name + " received in : " + slot);

        foreach(var icon in ItemIcons) {
            if (item.itemName.Equals(icon.gameObject.name))
                SetIcon(slot, icon);
        }
    }

    public void SetIcon(int slot, Image icon) {
        inventory_icon[slot - 1].sprite = icon.sprite;
    }
}
