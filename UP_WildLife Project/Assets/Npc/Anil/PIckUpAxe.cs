using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIckUpAxe : MonoBehaviour
{
    public PlayerMotor Motor;

    public bool CanpickUP;

    public bool CanChop;
    public Animator ANime;
    public GameObject PressE;
    public float Range;
    public Transform Player;









    private void OnTriggerExit(Collider other)
    {

        PressE.SetActive(false);


    }



    private void Update()
    {


        if (Vector3.Distance(transform.position, Player.position) < Range)
        {


            CanpickUP = true;
            PressE.SetActive(true);

            if (CanpickUP)
            {
                

                if (Input.GetKeyDown(KeyCode.E))
                {
                    PressE.SetActive(false);
                    CanpickUP = false;
                    Motor.Canmove = false;
                    ANime.SetBool("PickUp", true);
                    CanChop = true;

                }

            }
            else
        {

            CanpickUP = false;

        }



        }

    }



    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);


    }
}
