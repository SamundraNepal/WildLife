using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elephentcheck : MonoBehaviour
{

    public bool IsElephentHooked;
    public BoxCollider col;


    private void Start()
    {

        IsElephentHooked = false;
        col = GetComponent<BoxCollider>();

    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag == "Player")
        {

            IsElephentHooked = true;
            col.enabled = false;

           // this.enabled = false;


        }    

    }

}
