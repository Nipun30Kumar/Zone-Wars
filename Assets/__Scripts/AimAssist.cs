using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimAssist : MonoBehaviour {

    public float minimumAngle;
    public float maximumAngle;
    public float minimumPower;
    public float maximumPower;
    public float maxArrowLength;
    public float minArrowLength;

    public GameObject arrow;
    public GameObject objectInMotion;
    public Button launchButton;

    private bool settingAngle = false;
    private bool aboutToFire = false;
    private float angle;
    private float power;
    private Vector3 velocity; 

    void OnEnable()
    {
        aboutToFire = false;
        ChangeLaunchButtonState(false);
    }

    void Start()
    {
        minimumPower =  objectInMotion.GetComponent<Coin>().minimumPower;
        maximumPower = objectInMotion.GetComponent<Coin>().maximumPower;
    }
	
	void Update () {
        Vector3 mouse_pos = Input.mousePosition;
        Vector3 launch_pos = Camera.main.WorldToScreenPoint(this.transform.position);

        mouse_pos.x -= launch_pos.x;
        mouse_pos.y -= launch_pos.y;

        angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        //Debug.Log("Angle : " + angle);

        if (Input.GetMouseButtonDown(0) && !aboutToFire)
        {
            settingAngle = true;
        }

        //ROTATION OF AIM LINE
        if (settingAngle)
        {
            arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        //VISIBILITY OF AIM LINE
        if (angle >= minimumAngle && angle <= maximumAngle && settingAngle)
        {
            arrow.SetActive(true);
        }

        //HIDING THE AIM LINE
        if ((angle <= minimumAngle || angle >= maximumAngle) && arrow.activeSelf == true && settingAngle)
        {
            settingAngle = false;
            arrow.SetActive(false);
        }        

        if (arrow.activeSelf == true && Input.GetMouseButtonUp(0) && settingAngle)
        {
            //Debug.Log("Touch Lifted , angle is set !!");
            aboutToFire = true;
            Vector2 lengthVector = arrow.GetComponent<SpriteRenderer>().size;
            settingAngle = false;
            float tempSize = lengthVector.x;
            arrow.GetComponent<Animation>().Stop();
            lengthVector = new Vector2(tempSize, 0.4f);
            

            // Direction Calculations
            Vector3 startPos = this.transform.position;
            Vector3 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 launchDirection = endPos - startPos;

            // Power Calculations
            power = (tempSize / maxArrowLength) * maximumPower;
            //Debug.Log("Power : " + power);

            velocity = (transform.rotation * launchDirection).normalized * power;
            ChangeLaunchButtonState(true);
        }             
	}

    public void Launch()
    {
        Debug.Log("LAUNCH PRESSED");
        //Function to be binded to an invisible button that only exists in the play area.
        arrow.SetActive(false);
        ChangeLaunchButtonState(false);

        if(objectInMotion != null)
        objectInMotion.GetComponent<Rigidbody2D>().velocity = velocity;

        // THE COIN HAS BEEN LAUNCHED. HENCE SETTING THE GAMEPLAY CONTROLLER BOOL TO TRUE.
        GameplayController.currentCoinLaunched = true;
        //objectInMotion = null;
    }

    private void ChangeLaunchButtonState(bool state)
    {
        launchButton.interactable = state;
        launchButton.gameObject.SetActive(state);
    }
}
