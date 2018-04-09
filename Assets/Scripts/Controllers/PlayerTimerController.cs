using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTimerController : MonoBehaviour {
    public GameObject TimerStand;
    public Vector3 offset;

    private Image clockTexture;
    private Image clockBG;
    private Text text;

    private PlayerController player;
    private Item currentItem;
    private Camera camera;

    private float timer;

    // Use this for initialization
    void Start() {
        camera = GameObject.FindObjectOfType<Camera>();
        

        player = GameObject.FindObjectOfType<PlayerController>();
        player.RegisterTimer(this);

        Image[] images = GetComponentsInChildren<Image>();

        foreach (Image img in images) {
            if (img.name.Equals("Image")) {
                clockTexture = img;
            } else
                clockBG = img;
        }
        text = GetComponentInChildren<Text>();

        Reset();
    }

    // Update is called once per frame
    void Update() {
        if (clockTexture.enabled) {
            transform.position = camera.WorldToScreenPoint(TimerStand.transform.position);
            clockTexture.fillAmount = timer / currentItem.duration;
            text.text = timer.ToString();
            if (timer <= 0) {
                clockBG.enabled = false;
                text.enabled = false;
            }
        }
    }

    public void OnPlayerUsingItem(Item item) {
        clockTexture.enabled = true;
        clockBG.enabled = true;
        text.enabled = true;
        currentItem = item;

        StartCoroutine(Timer(item));
    }

    private IEnumerator Timer(Item item) {
        timer = item.duration;
        while (timer > 0) {
            yield return new WaitForSeconds(1);
            timer--;
        }
        Reset();
    }

    public void Reset() {
        clockTexture.fillAmount = 1;
        clockTexture.enabled = false;
        clockBG.enabled = false;
        text.enabled = false;
    }
}