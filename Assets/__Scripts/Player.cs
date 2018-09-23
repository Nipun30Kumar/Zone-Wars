using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public bool isAI;
    public int moveCount;
    public bool isMyTurn;
    
    [SerializeField] Transform arrow;
    [SerializeField] Transform powerBar;
    [SerializeField] bool twoStep = false;
    [SerializeField] bool inverse = false;
    [SerializeField, Range(1, 10)] float maxArrowLength = 2;

    private float lastAngle;
    private float angle;
    private float power;

    private bool angleIsSet;
    
    
    public void Initialize() {
        isMyTurn = false;
        gameObject.SetActive(true);
        lastAngle = arrow.localEulerAngles.z;
    }

    void UpdateAngle() {
        Vector3 mouse_pos = Input.mousePosition;
        mouse_pos.z = Camera.main.transform.position.z * -1;
        mouse_pos = Camera.main.ScreenToWorldPoint(mouse_pos);
        
        if (!angleIsSet) {
            Vector3 launch_pos = arrow.position;
            if(inverse) {
                mouse_pos = launch_pos - mouse_pos;
            } else {
                mouse_pos -= launch_pos;
            }
            
            angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            var clampedAngle = Mathf.Clamp(angle, GameplayController.MINIMUM_AIM_ANGLE, GameplayController.MAXIMUM_AIM_ANGLE);
            //this will help with sudden angle change
            float angleDiff = Mathf.Abs(clampedAngle - lastAngle);
            if (angleDiff >= 90) {
                Debug.Log("Difference is high");
                clampedAngle = lastAngle;
            }
            arrow.localRotation = Quaternion.Euler(0, 0, clampedAngle);
            lastAngle = clampedAngle;
        }

        if (!twoStep || (twoStep && angleIsSet)) {
            power = (Mathf.Clamp(Vector3.Distance(mouse_pos, arrow.position), 0, maxArrowLength)) / maxArrowLength;
            powerBar.transform.localScale = new Vector3(power, 1f, 1f);
        }
    }

    void Update() {
        if(!isMyTurn) { return; }
        if (Input.GetMouseButtonDown(0)) {
            UpdateAngle();
            arrow.gameObject.SetActive(true);
        } else if (Input.GetMouseButton(0)) {
            UpdateAngle();
        } else if (Input.GetMouseButtonUp(0)) {
            UpdateAngle();
            if (twoStep) {
                if (!angleIsSet) {
                    angleIsSet = true;
                } else {
                    Launch();
                }
            } else {
                Launch();
            }
        }
    }

    public void Launch() {
        Debug.Log("****** LAUNCH ******");
        isMyTurn = false;
        angleIsSet = false;
        arrow.gameObject.SetActive(false);
        power = 1;
        powerBar.transform.localScale = new Vector3(power, 1f, 1f);
        GameplayControllerV2.Instance.Fire((arrow.right * (inverse ? -1 : 1)), power);
    }

    public void MyTurn() {
        isMyTurn = true;
    }
}
