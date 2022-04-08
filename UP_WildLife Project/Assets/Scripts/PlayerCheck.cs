using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheck : MonoBehaviour
{

    PlayerMotor Motor;



    private void Start()
    {
        Motor = GameObject.Find("Player").GetComponent<PlayerMotor>();

    }

    private void OnTriggerEnter(Collider other)
    {



        if (other.gameObject.tag == "Player")
        {

            Debug.Log("He is here");
            Motor.Anim.SetBool("Walking", false);


        }

    }









}
