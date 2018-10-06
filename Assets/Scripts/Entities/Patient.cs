using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Patient : MonoBehaviour
{
    // Initial number of seconds the patient has before they die
    public int lifespan = 10;

    // Number of seconds the patient has left before they die
    public int timeLeft;
    
    // Relative positions of the symptoms text UI
    public Vector3[] popUpsOffset =
    {
        new Vector3(-1f, 0f, 0),
        new Vector3(0f, 1f, 0),
        new Vector3(1f, 0f, 0),
    };

    // Patient current states
    private bool alive = true;
    private bool treating = false;
    private bool pronouncedDead = false;

    // Patient symptoms
    private List<Symptom> symptoms = new List<Symptom>();

    // Points that the player has earned for this patient
    private int score = 0;

    private GameController gameController;
    private LevelManager levelManager;

    private WwiseInterface wwInterface;


    void Start()
    {
        timeLeft = lifespan;

        wwInterface = GetComponent<WwiseInterface>();
        gameController = GameObject.FindObjectOfType<GameController>();
        levelManager = GameObject.FindObjectOfType<LevelManager>();

        if (levelManager != null && !levelManager.inTutorial)
            StartCoroutine(CountDown());
    }

    void Update()
    {
        if (alive && timeLeft <= 0 && !treating && symptoms.Count > 0)
            Die();
    }

    void OnGUI()
    {
        if (GetSymptoms().Count == 0)
            ShowSymptom(popUpsOffset[0], "No symptom");
        else
        {
            int i = 0;
            foreach (Symptom s in GetSymptoms())
            {
                if (i < popUpsOffset.Length)
                    ShowSymptom(popUpsOffset[i++], s.name);
            }
        }
    }


    public void SetSymptoms(List<Symptom> _symptoms)
    {
        if (symptoms != null)
            symptoms = _symptoms;
    }

    public void AddSymptom(Symptom symptom)
    {
        if (alive && symptom != null)
            symptoms.Add(symptom);
    }

    public List<Symptom> GetSymptoms()
    {
        return symptoms;
    }

    public string GetStringOfSymptoms()
    {
        string str = "";
        foreach (Symptom s in symptoms)
            str += s.name + ", ";

        return str;
    }

    public int GetPointsEarned()
    {
        return score;
    }

    public void AddTime(int time)
    {
        if (alive)
            timeLeft += time;
    }

    public void ResetTimer()
    {
        timeLeft = lifespan;
    }

    public void TreatSymptom(Item item)
    {
        if (item != null)
        {
            foreach (Symptom symptom in symptoms)
            {
                // Find symptom that can be cured by the item
                if (string.Equals(symptom.cure.GetComponent<Item>().itemName, item.itemName))
                {
                    StartCoroutine(ApplyTreatment(symptom));
                    break;
                }
            }
        }
    }

    public bool IsAlive()
    {
        return alive || symptoms.Count <= 0;
    }

    public bool IsPronouncedDead()
    {
        return pronouncedDead;
    }

    public void Dismiss()
    {
        if (alive && symptoms.Count > 0)
            Die();
        else if (alive)
            wwInterface.callEvent("Player_Success");

        if (gameController != null)
            gameController.PatientWasDismissed(gameObject);
    }

    private IEnumerator CountDown()
    {
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
        }
    }

    private IEnumerator ApplyTreatment(Symptom symptom)
    {
        Debug.Log("treating: " + symptom.name + " - " + symptom.cure.GetComponent<Item>().duration);
        treating = true;

        yield return new WaitForSeconds(symptom.cure.GetComponent<Item>().duration);

        score += symptom.points;
        symptoms.Remove(symptom);

        Debug.Log("Treatment for " + symptom.name + " over");
        treating = false;

        switch (symptom.cure.GetComponent<Item>().itemName)
        {
            case "ice":
                symptoms.Add(GameObject.FindObjectOfType<DataBank>().GetSymptom("wet"));
                AddTime(5);
                break;

            case "needle and thread":
                symptoms.Add(GameObject.FindObjectOfType<DataBank>().GetSymptom("injured paw"));
                AddTime(3 + 5);
                break;
        }
    }

    private void Die()
    {
        Debug.Log("Dead");

        GetComponentInChildren<Animator>().SetTrigger("Dead");

        alive = false;
        pronouncedDead = true;

        wwInterface.callEvent("Player_Failure");

        if (gameController != null)
            gameController.PatientDied();
    }

    private void ShowSymptom(Vector3 offset, string symptom)
    {
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position + offset);
        GUI.Box(new Rect(pos.x, Screen.height - pos.y, 100f, 25f), symptom);
    }
}
