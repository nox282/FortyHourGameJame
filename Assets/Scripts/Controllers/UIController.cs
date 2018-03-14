using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Text[] inventory_text;

    public bool display_xbox_controls = true;

    public Image[] inventory_icon;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

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

    public void SetIcon(int slot, Image icon) {
        inventory_icon[slot - 1] = icon;
    }
}
