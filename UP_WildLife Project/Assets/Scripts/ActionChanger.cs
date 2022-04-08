using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChanger : MonoBehaviour
{

    public int ActionNumber;
    public int ClampValueMax = 5, ClampMinVa = 0;


    public WallClimber WC;
    public Rope RS;
    public WaterMovement WM;
    public void Update()
    {




        ActionNumber = Mathf.Clamp(ActionNumber, ClampMinVa, ClampValueMax);

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            ActionNumber += 1;

        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {

            ActionNumber -= 1;
        }




        if (ActionNumber == 0)
        {


            WC.enabled = false;
            RS.enabled = false;
            WM.enabled = true;
        } else if (ActionNumber == 1)
        {
            WC.enabled = true;
            RS.enabled = false;
            WM.enabled = false;


        } else if(ActionNumber ==2)
        {

            RS.enabled = true;
            WC.enabled = false;
            WM.enabled = false;

        }


    }

}
