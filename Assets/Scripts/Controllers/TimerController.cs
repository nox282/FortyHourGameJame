using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public Bed bed;
    public Vector3 offset;
    public bool freezeTimer = false;
    public GameObject TimerStand;

    private Patient currentPatient;
    private Image clockTexture;
    private Image clockBG;
    private Text text;

    void Start()
    {
        Camera camera = GameObject.FindObjectOfType<Camera>();
        transform.position = camera.WorldToScreenPoint(TimerStand.transform.position);

        bed.RegisterTimer(this);

        Image[] images = GetComponentsInChildren<Image>();
        foreach (Image img in images)
        {
            if (img.name.Equals("Image"))
                clockTexture = img;
            else
                clockBG = img;
        }

        clockTexture.enabled = !clockTexture.enabled;
        clockBG.enabled = !clockBG.enabled;

        text = GetComponentInChildren<Text>();
        text.enabled = !text.enabled;
    }

    void Update()
    {
        if (clockTexture.enabled && !freezeTimer)
        {
            clockTexture.fillAmount = (float) currentPatient.timeLeft / currentPatient.lifespan;
            text.text = currentPatient.timeLeft.ToString();
            if (currentPatient.timeLeft <= 0)
            {
                clockBG.enabled = false;
                text.enabled = false;
            }
        }
    }

    public void OnBedReceivingPatient(Patient patient)
    {
        clockTexture.enabled = true;
        clockBG.enabled = true;
        text.enabled = true;
        currentPatient = patient;
    }

    public void Reset()
    {
        clockTexture.fillAmount = 1;
        clockTexture.enabled = false;
        clockBG.enabled = false;
        text.enabled = false;
    }
}
