using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // UI
    public Text deadPatients_ui;
    public Text deadAllowed_ui;

    // Config
    public float spawnRate;
    public Vector3 spawnLocation;
    public Vector3 bedOffset;
    public Vector3 inversedBedOffset;
    public float stageDuration = 120;
    public int deadAllowed = 4;
    public int maxPatientInClinic = 6;
    public KeyCode pauseKey;

    // Containers
    public PatientFactory patientFactory;
    public List<GameObject> waitingRoom = new List<GameObject>();
    public Dictionary<int, GameObject> patientList;
    public List<Bed> bedList;
    public List<int> availableBeds = new List<int>();
    public GameObject PauseUILayer;

    private float spawnTimer;
    private int deadPatients = 0;
    private int score = 0;


    void Start()
    {
        deadAllowed_ui.text = deadAllowed.ToString();
        deadPatients_ui.text = deadPatients.ToString();

        patientList = new Dictionary<int, GameObject>(maxPatientInClinic);

        // At the start of the game, all beds are available
        for (int i = 0; i < maxPatientInClinic; i++)
            availableBeds.Add(i);

        // Set input keycode for pause
        pauseKey = GameObject.FindObjectOfType<PlayerController>().pause;

        StartCoroutine(StartLevel());
    }

    public void Update()
    {
        if (Input.GetKeyDown(pauseKey))
            UpdatePauseState();    
    }

    private IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(3);
        StartCoroutine(TickStageTimer());
        StartCoroutine(SpawnPatient());
    }

    private IEnumerator TickStageTimer()
    {
        yield return new WaitForSeconds(stageDuration);
        EndStage();
    }

    private IEnumerator SpawnPatient()
    {
        while (true)
        {
            if (spawnTimer >= spawnRate)
            {
                spawnTimer = 0.0f;
                // Spawn new patient in waiting room
                waitingRoom.Add(patientFactory.CreatePatient(spawnLocation));
                Debug.Log(waitingRoom.Count);

                // Fill up clinic
                if (patientList.Count < maxPatientInClinic)
                {
                    // Select bed number randomly
                    int index = 0;

                    do
                    {
                        index = availableBeds[Random.Range(0, availableBeds.Count - 1)];
                    }
                    while (bedList[index].isOccupied);

                    GameObject patientObject = GetAliveWaitingPatient();

                    if (patientObject != null)
                    {
                        Debug.Log(index);
                        // Admit patient into clinic and take them to the first unoccupied bed
                        AdmitPatient(index, bedList[index], patientObject);
                    }
                }
            }
            spawnTimer++;
            yield return new WaitForSeconds(1);
        }
    }

    private GameObject GetAliveWaitingPatient()
    {
        int waitingIndex = 0;
        GameObject patientObject = null;

        // Remove all the patients that died in the waiting room
        do
        {
            if (patientObject != null)
            {
                waitingRoom.RemoveAt(waitingIndex);
                deadPatients++;

                // Check that user hasn't reached dead quota
                if (deadPatients >= deadAllowed)
                {
                    EndStage();
                    break;
                }
            }
            patientObject = waitingRoom[waitingIndex++];
        }
        while (waitingIndex < waitingRoom.Count && (patientObject == null || !patientObject.GetComponent<Patient>().IsAlive()));

        return patientObject;
    }

    private void AdmitPatient(int bedIndex, Bed bed, GameObject patientObject)
    {
        Patient patient = patientObject.GetComponent<Patient>();

        patientList.Add(bedIndex, patientObject);
        SnapToBed(bed, patientObject);
        bed.SetPatient(patient);
        availableBeds.Remove(bedIndex);

        patient.ResetTimer();

        bed.isOccupied = true;

        // Send reference to patient to surrounding use zones
        foreach (Component component in bed.GetComponentsInChildren<UseZoneController>())
            component.GetComponent<UseZoneController>().patient = patient;

        // Remove patient from waiting room
        waitingRoom.RemoveAt(0);
    }

    private void SnapToBed(Bed bed, GameObject patient)
    {
        if (!bed.reversed)
        {
            patient.transform.position = bed.transform.position + bedOffset;
            patient.transform.rotation = Quaternion.LookRotation(bed.transform.forward);
        }
        else
        {
            patient.transform.position = bed.transform.position + inversedBedOffset;
            patient.transform.rotation = Quaternion.Euler(275, 180, 0);            
        }
            
    }

    private void EndStage()
    {
        StopCoroutine(TickStageTimer());
        StopCoroutine(SpawnPatient());

        if (deadPatients >= deadAllowed)
            Lose();
        else
            Win();
    }

    private void Lose()
    {
        GameObject.FindObjectOfType<LevelManager>().LoadLevel("Lose");
    }

    private void Win()
    {
        GameObject.FindObjectOfType<LevelManager>().LoadLevel("Win");
    }

    public void PatientDied()
    {
        deadPatients++;
        deadPatients_ui.text = deadPatients.ToString();

        if (deadPatients >= deadAllowed)
            EndStage();
    }

    public void PatientWasDismissed(GameObject patient)
    {
        if (patient.GetComponent<Patient>().IsAlive())
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

        DestroyObject(patient);
    }
    
    public void UpdatePauseState()
    {
        Time.timeScale = Mathf.Abs(Time.timeScale - 1);
        Time.fixedDeltaTime = Time.timeScale;

        GameController instance = GameObject.FindObjectOfType<GameController>();
        instance.PauseUILayer.SetActive(Time.timeScale < 1.0f);
    }
}