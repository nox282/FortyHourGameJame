using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipesUIController : MonoBehaviour {

    public string SymptomIconTag = "SymptomIcon";
    public string ItemIconTag = "ItemIcon";

    public GameObject RecipePrefab;
    public Vector3 RecipePosition;
    public float RecipeYOffset;
    public List<RecipeController> Recipes;

    private List<Image> SymptomIcons;
    private List<Image> ItemIcons;

    private float CurrentRecipeYOffsetAmount;

	// Use this for initialization
	void Awake () {
        GameObject[] icons = GameObject.FindGameObjectsWithTag(SymptomIconTag);	
        if(icons.Length > 0) {
            SymptomIcons = new List<Image>();
            foreach (var i in icons)
                SymptomIcons.Add(i.GetComponent<Image>());
        }

        icons = GameObject.FindGameObjectsWithTag(ItemIconTag);
        if (icons.Length > 0) {
            ItemIcons = new List<Image>();
            foreach (var i in icons)
                ItemIcons.Add(i.GetComponent<Image>());
        }
    }

    public void SetRecipe(Symptom symptom) {
        RecipeController recipe = CreateNewRecipe();
        Recipes.Add(recipe);

        Image symptomIcon = GetSymptomIcon(symptom);
        if(symptomIcon != null)
            recipe.SetSymptom(symptomIcon);

        Image itemIcon = GetItemIcon(symptom.cure.GetComponent<Item>());
        if (itemIcon != null)
            recipe.SetItem(itemIcon);
    }

    private Image GetSymptomIcon(Symptom symptom) {
        foreach(Image icon in SymptomIcons) {
            if (icon.name.Equals(symptom.symptomName))
                return icon;
        }
        return null;
    }

    private Image GetItemIcon(Item item) {
        foreach (Image icon in ItemIcons) {
            if (icon.name.Equals(item.itemName))
                return icon;
        }
        return null;
    }

    private RecipeController CreateNewRecipe() {
        Vector3 pos = RecipePosition;
        pos.y += CurrentRecipeYOffsetAmount;
        CurrentRecipeYOffsetAmount += RecipeYOffset;

        Debug.Log("supposed pos : " + pos);

        GameObject instance = Instantiate(RecipePrefab, Vector3.zero, Quaternion.identity, this.transform);

        instance.transform.localPosition = pos;
        Debug.Log("acutal pos : " + instance.transform.position);

        return instance.GetComponent<RecipeController>();
    }
}
