using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabEnemy : MonoBehaviour
{
    public GameObject GrabPos;
    public GameObject GrabbedEnemy;
    
    public void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag == "Enemies")
        {
             GrabbedEnemy = other.gameObject;
             GrabbedEnemy.transform.parent = GrabPos.transform;


             GrabbedEnemy.gameObject.transform.position = GrabPos.transform.position;
            GrabbedEnemy.gameObject.transform.rotation = GrabPos.transform.rotation;

            GrabbedEnemy.transform.tag = "Untagged";

        }


    }



  
}
