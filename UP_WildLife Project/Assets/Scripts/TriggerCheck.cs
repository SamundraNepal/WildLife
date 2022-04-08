using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCheck : MonoBehaviour
{

    public bool IsPlayerAttached;


    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag == "Player")
        {
            //other.transform.gameObject = transform.parent;

            other.transform.parent = this.transform ;
            IsPlayerAttached = true;

        }


    }


    private void OnTriggerExit(Collider other)
    {


        if (other.gameObject.tag == "Player")
        {

            other.transform.parent = null;
            IsPlayerAttached = false;


        }


    }





}
