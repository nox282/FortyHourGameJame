using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    public bool isOccupied = false;
    public bool reversed;
    private Patient patient;
    public TimerController timerController;
    
    void Update()
    {
        if (!patient)
            timerController.Reset();
    }

    public void RegisterTimer(TimerController tc)
    {
        timerController = tc;
    }

    public void SetPatient(Patient _patient)
    {
        timerController.Reset();
        patient = _patient;
        timerController.OnBedReceivingPatient(patient);
    }
}
