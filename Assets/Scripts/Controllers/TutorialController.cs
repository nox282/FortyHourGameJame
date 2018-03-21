using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialController : MonoBehaviour
{
    public Vector3 spawnLocation;
    public GameObject patientPrefab;

    // UI panels
    public GameObject mainSpeech;
    public GameObject timerLegend;
    public GameObject curesLegend;
    public GameObject inventoryLegend;
    public GameObject deathsLegend;
    public GameObject endTuto;

    public List<Bed> beds;
    public List<GameObject> items;
    
    public int step = 0;
    public bool stepIsComplete = true;

    private GameObject symptom;
    private GameObject patient;

    private string item1;
    private string item2;
    private string item3;
    private string item4;

	
	void Start ()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        item1 = player.slot1.ToString().Replace("Alpha", "");
        item2 = player.slot2.ToString().Replace("Alpha", "");
        item3 = player.slot3.ToString().Replace("Alpha", "");
        item4 = player.slot4.ToString().Replace("Alpha", "");
    }

    private Item SearchItem(string name)
    {
        foreach (GameObject gb in items)
        {
            Item item = gb.GetComponent<Item>();
            if (string.Equals(item.itemName, name))
                return item;
        }

        return null;
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space) && stepIsComplete)
        {
            step++;
            stepIsComplete = UpdateSpeech();
        }
        else if (Input.GetKeyDown(KeyCode.F1))
        {
            FindObjectOfType<LevelManager>().LoadNextStage();
        }
    }


    private bool UpdateSpeech()
    {
        switch (step)
        {
            case 1:
                return Step1();
            case 2:
                return Step2();
            case 3:
                return Step3();
            case 4:
                return Step4();
            case 5:
                return Step5();
            case 7:
                return Step7();
            case 8:
                return Step8();
            case 10:
                return Step10();
            case 11:
                return Step11();
            case 13:
                return Step13();
            case 15:
                return Step15();
            case 16:
                return Step16();
            case 17:
                return Step17();
            case 18:
                return Step18();
        }

        return false;
    }

    private bool Step1()
    {
        Text speechText = GameObject.FindGameObjectWithTag("Speech").GetComponent<Text>();

        speechText.text = "In ‘Cat-tastrophe!’, you are a doctor\ncharged with treating your patients as fast as you can.\n" +
            "Your patients are very sick and will only survive for 10 more seconds without your help!";

        return true;
    }

    private bool Step2()
    {
        SpawnPatient();

        Text speechText = GameObject.FindGameObjectWithTag("Speech").GetComponent<Text>();
        speechText.text = "Look! A patient has just arrived. Let’s go have a look and see what’s making them so pawly.";

        return true;
    }

    private bool Step3()
    {
        mainSpeech.SetActive(false);
        timerLegend.SetActive(true);
        return true;
    }

    private bool Step4()
    {
        Text timerText = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
        timerText.text = "Miaowzers! They look very ill!\nThe icons by the patient tell you which symptoms the patient is displaying.\n" + 
            "Now go get your doctoring tools at the desk to treat the patient.";
        return true;
    }

    private bool Step5()
    {
        timerLegend.SetActive(false);
        return false;
    }

    public void Step6()
    {
        curesLegend.SetActive(true);

        step++;
        stepIsComplete = true;
    }

    private bool Step7()
    {
        curesLegend.SetActive(false);
        mainSpeech.SetActive(true);

        Text speechText = GameObject.FindGameObjectWithTag("Speech").GetComponent<Text>();

        
        speechText.text = "To pick up your tools, press " + item1 + ", " + item2 + ", " + 
            item3 + ", or " + item4 + " to assign them to the correlating slot in your inventory.";
        
        return true;
    }

    private bool Step8()
    {
        mainSpeech.SetActive(false);
        return false;
    }

    public void Step9()
    {
        inventoryLegend.SetActive(true);

        step++;
        stepIsComplete = true;
    }

    private bool Step10()
    {
        inventoryLegend.SetActive(false);

        mainSpeech.SetActive(true);

        Text speechText = GameObject.FindGameObjectWithTag("Speech").GetComponent<Text>();
        speechText.text = "Time to save a life! Move next to the patient's bed, and press " + 
            item1 + ", " + item2 + ", " + item3 + ", or " + item4 + " to give them the pills.";

        return true;
    }

    private bool Step11()
    {
        mainSpeech.SetActive(false);
        return false;
    }

    public void Step12()
    {
        mainSpeech.SetActive(true);

        Text speechText = GameObject.FindGameObjectWithTag("Speech").GetComponent<Text>();
        speechText.text = "Yay! You have cured the patient's cough! You can now dismiss them by pressing the TAB key, " +
            "so they can go back home and free the bed for another patient.";

        step++;
        stepIsComplete = true;
    }

    private bool Step13()
    {
        mainSpeech.SetActive(false);
        return false;
    }

    public void Step14()
    {
        mainSpeech.SetActive(true);

        Text speechText = GameObject.FindGameObjectWithTag("Speech").GetComponent<Text>();
        speechText.text = "Some patients might not have any symptoms, so they can be dismissed without needing any treatment.\n" +
            "Always make sure that the patient doesn't have ANY symptom when you dismiss them, otherwise they will die as soon as they leave..";

        step++;
        stepIsComplete = true;
    }

    private bool Step15()
    {
        Text speechText = GameObject.FindGameObjectWithTag("Speech").GetComponent<Text>();
        speechText.text = "Some cures are instant (like the blue pills), but others take a few seconds to be administered: " + 
            "during this time, you will not be able to move your character until you are done treating the symptom.";

        return true;
    }

    private bool Step16()
    {
        Text speechText = GameObject.FindGameObjectWithTag("Speech").GetComponent<Text>();
        speechText.text = "Finally, some cures have side effects and add symptoms to the patient after you use them.";

        return true;
    }

    private bool Step17()
    {
        mainSpeech.SetActive(false);
        deathsLegend.SetActive(true);

        return true;
    }

    private bool Step18()
    {
        deathsLegend.SetActive(false);
        endTuto.SetActive(true);

        return true;
    }

    private void SpawnPatient()
    {
        GameObject patientObject = Instantiate(patientPrefab, spawnLocation, Quaternion.identity) as GameObject;
        patientObject.GetComponent<Patient>().AddSymptom(symptom.GetComponent<Symptom>());

        Patient patient = patientObject.GetComponent<Patient>();
        patient.inTutorial = true;

        Bed bed = beds[0];
        patientObject.transform.position = bed.transform.position;
        patientObject.transform.rotation = Quaternion.LookRotation(bed.transform.forward);
        patientObject.transform.Rotate(new Vector3(-90, 0, 0));

        bed.SetPatient(patient);
        bed.isOccupied = true;
        foreach (Component component in bed.GetComponentsInChildren<UseZoneController>())
            component.GetComponent<UseZoneController>().patient = patient;
    }

    public void PatientWasDismissed(GameObject patient)
    {
        /*if (patient.GetComponent<Patient>().IsAlive())
            score += patient.GetComponent<Patient>().GetPointsEarned();

        int index = -1;
        foreach (KeyValuePair<int, GameObject> pair in patientList)
        {
            if (GameObject.Equals(pair.Value, patient))
            {
                index = pair.Key;
                break;
            }
        }

        if (index >= 0)
        {
            bedList[index].isOccupied = false;
            availableBeds.Add(index);
            patientList.Remove(index);
        }

        DestroyObject(patient);*/
    }
}
