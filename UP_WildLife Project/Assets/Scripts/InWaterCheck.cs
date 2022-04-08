using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InWaterCheck : MonoBehaviour
{

    public bool OnWaterSurface;
    public WaterMovement WM;

    public void OnTriggerEnter(Collider other)
    {



        if (other.gameObject.tag == "Water")
        {

            OnWaterSurface = true;
            WM.enabled = true;

        }
    }


    private void OnTriggerExit(Collider other)
    {

        OnWaterSurface = false;


    }








}
