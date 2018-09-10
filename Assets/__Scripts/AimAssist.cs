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
    public bool reset = false;

    public GameObject arrow;
    public GameObject objectInMotion;
    public Button resetButton;
    public Button launchButton;

    private bool settingAngle = false;
    private float angle;
    private float power;
    private Vector3 velocity; 

    void Start()
    {
        minimumPower =  objectInMotion.GetComponent<Coin>().minimumPower;
        maximumPower = objectInMotion.GetComponent<Coin>().maximumPower;
        ChangeResetButtonState(false);
    }
	
	void Update () {
        Vector3 mouse_pos = Input.mousePosition;
        Vector3 launch_pos = Camera.main.WorldToScreenPoint(this.transform.position);

        mouse_pos.x -= launch_pos.x;
        mouse_pos.y -= launch_pos.y;

        angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        //Debug.Log("Angle : " + angle);

        if ((Input.GetMouseButtonDown(0) || (Input.GetMouseButtonDown(0) && reset)) && resetButton.gameObject.activeSelf == false)
        {
            settingAngle = true;
            reset = false;
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
            Vector2 lengthVector = arrow.GetComponent<SpriteRenderer>().size;
            settingAngle = false;
            float tempSize = lengthVector.x;
            arrow.GetComponent<Animation>().Stop();
            lengthVector = new Vector2(tempSize, 0.4f);
            ChangeLaunchButtonState(true);
            //ChangeResetButtonState(true);

            // Direction Calculations
            Vector3 startPos = this.transform.position;
            Vector3 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 launchDirection = endPos - startPos;

            // Power Calculations
            power = (tempSize / maxArrowLength) * maximumPower;
            //Debug.Log("Power : " + power);

            velocity = (transform.rotation * launchDirection).normalized * power;
        }             
	}

    public void ResetAngleButton()
    {
        // Allow user to reset the angle if they press on the coin.
        reset = true;

        // If the user presses the reset button, turn the interactable off after the press.
        arrow.SetActive(false);
        arrow.GetComponent<Animation>().Play();
        //ChangeResetButtonState(false);
    }

    public void Launch()
    {
        //Function to be binded to an invisible button that only exists in the play area.
        arrow.SetActive(false);
        ChangeLaunchButtonState(false);
        //ChangeResetButtonState(false);
        if(objectInMotion != null)
        objectInMotion.GetComponent<Rigidbody2D>().velocity = velocity;

        objectInMotion = null;
    }

    private void ChangeResetButtonState(bool state)
    {
        resetButton.interactable = state;
        resetButton.gameObject.SetActive(state);
    }

    private void ChangeLaunchButtonState(bool state)
    {
        launchButton.interactable = state;
        launchButton.gameObject.SetActive(state);
    }
}
