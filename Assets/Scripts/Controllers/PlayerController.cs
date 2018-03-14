using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Tag
    public string T_PICKUP;
    public string T_USE;

    // Config
    public float speed;
    public float turningSpeed;
    public float dashSpeed;
    public int dashCooldown;

    // Keyboard Controls
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    public KeyCode dash;
    public KeyCode slot1;
    public KeyCode slot2;
    public KeyCode slot3;
    public KeyCode slot4;
    public KeyCode dismiss;

    // Components
    private Rigidbody body;
    public Item[] inventory;
    private Animator anim;

    private Vector3 movement;
    public bool isDashing;
    private bool isDashActive = true;
    private bool showItemInformation = false;
    private bool isTreating = false;

    // Environemental
    public Item currentItem;
    public Patient currentPatientTarget;
    public Bed currentBed;

    public bool inTutorial = false;


	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        movement = new Vector3(0, 0, 0);
        isDashing = false;
        inventory = new Item[4];
	}
	
	// Update is called once per frame
	void Update () {
        CheckForInputs();
        
        if (!IsPickingUp() && !isTreating)
        {
            anim.SetBool("isMoving", IsMoving());
            ApplyMovement();
        }
        
    }
    bool IsPickingUp() {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PickUp");
    }

    bool IsMoving() {
        return movement != Vector3.zero;
    }

    void CheckForInputs()
    {
        if (Input.GetKey(up))
            movement += new Vector3(0, 0, 1);
        
        if (Input.GetKey(down))
            movement += new Vector3(0, 0, -1);
        
        if (Input.GetKey(right))
            movement += new Vector3(1, 0, 0);
        
        if (Input.GetKey(left))
            movement += new Vector3(-1, 0, 0);
        
        if (Input.GetKeyDown(dash))
        {
            if (!isDashing && isDashActive)
            {
                isDashing = true;
                movement += movement * dashSpeed;
            }
        }

        if (Input.GetKeyDown(slot1)) 
            InventoryAction(1);
        
        if (Input.GetKeyDown(slot2)) 
            InventoryAction(2);
        
        if (Input.GetKeyDown(slot3)) 
            InventoryAction(3);
        
        if (Input.GetKeyDown(slot4)) 
            InventoryAction(4);
        
        if (Input.GetKeyDown(dismiss)) 
            Dismiss();
    }

    void ApplyMovement()
    {
        Vector3 target = transform.position + movement;

        float currentSpeed = isDashing ? dashSpeed : speed;

        if (isDashing)
        {
            currentSpeed = dashSpeed;
            StartCoroutine(CoolDashDown());
            isDashing = false;
        }
        else
            currentSpeed = speed;

        transform.position = Vector3.MoveTowards(body.position, target, Time.deltaTime * currentSpeed);

        if (movement != Vector3.zero) {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(movement),
                Time.deltaTime * turningSpeed
            );
        }
        movement = new Vector3(0, 0, 0);
    }

    private IEnumerator CoolDashDown()
    {
        isDashActive = false;
        yield return new WaitForSeconds(dashCooldown);
        isDashActive = true;
    }

    // Actions
    void InventoryAction(int slot)
    {
        if (currentItem) PickUp(slot);
        if (currentPatientTarget) Use(slot);
    }

    void PickUp(int slot) {
        anim.SetTrigger("Pickup");
        if (currentItem)
        {
            inventory[slot - 1] = currentItem;

            if (inTutorial)
            {
                TutorialController controller = GameObject.FindObjectOfType<TutorialController>();
                if (controller && controller.step == 8 && !controller.stepIsComplete && 
                        string.Equals(currentItem.itemName, "Blue pill"))
                {
                    controller.Step9();
                }
            }
        }
    }
    
    void Use(int slot)
    {
        anim.SetTrigger("Pickup");

        Item item = inventory[slot - 1];

        if (item && currentBed != null && currentBed.isOccupied)
        {
            currentPatientTarget.TreatSymptom(item); //use item on currentPatientTarget  
            StartCoroutine(LockMovementWhileTreating(item));

            if (inTutorial)
            {
                TutorialController controller = GameObject.FindObjectOfType<TutorialController>();

                if (controller && controller.step == 11 && !controller.stepIsComplete &&
                        string.Equals(item.itemName, "Blue pill"))
                {
                    controller.Step12();
                }
            }
        }
    }

    private IEnumerator LockMovementWhileTreating(Item item)
    {
        isTreating = true;
        yield return new WaitForSeconds(item.duration);
        isTreating = false;
    }

    void Dismiss()
    {
        if (currentPatientTarget)
        {
            currentPatientTarget.Dismiss();

            if (inTutorial)
            {
                TutorialController controller = GameObject.FindObjectOfType<TutorialController>();
                Debug.Log(controller.step);
                if (controller && controller.step == 13 && !controller.stepIsComplete)
                    controller.Step14();
            }
        }
    }
 
    // Triggers
    void OnTriggerEnter(Collider other)
    {
        TutorialController controller = GameObject.FindObjectOfType<TutorialController>();

        if (other.CompareTag(T_PICKUP))
        {
            currentItem = other.gameObject.GetComponent<PickUpZoneController>().item;
            showItemInformation = true;

            if (inTutorial && controller)
            {
                if (controller.step == 5 && !controller.stepIsComplete)
                    controller.Step6();
            }
        }
        if (other.CompareTag(T_USE))
        {
            currentPatientTarget = other.gameObject.GetComponent<UseZoneController>().patient;
            currentBed = other.gameObject.GetComponentInParent<Bed>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(T_PICKUP))
        {
            currentItem = null;
            showItemInformation = false;
        }

        if (other.CompareTag(T_USE))
        {
            currentPatientTarget = null;
            currentBed = null;
        } 
    }

    void OnGUI() {
        if (showItemInformation && currentItem) {
            Vector2 pos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(-1f, 1f, 0));
            GUI.Box(new Rect(pos.x, Screen.height - pos.y, 100f, 20f), currentItem.itemName);
        }
    }
}
