using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallHelicopter : MonoBehaviour
{


    public GameObject Helicopter;


    private void Start()
    {

        Helicopter.SetActive(false);

    }

    public void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.tag == "Player")
        {
            Helicopter.SetActive(true);



        }
    }

}
