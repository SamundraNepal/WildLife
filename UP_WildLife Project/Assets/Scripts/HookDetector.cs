using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookDetector : MonoBehaviour
{

    public GameObject player;
    public GrapplingHook Hook;



  

    
    private void OnTriggerEnter(Collider other)
    {
        


        if(other.gameObject.tag == "HookAble")
        {


           Hook.hooked = true;
      
     //   Hook.HookedGameObj = other.gameObject;



        }


    }




}
