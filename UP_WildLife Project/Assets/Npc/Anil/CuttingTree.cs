using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingTree : MonoBehaviour
{

    public bool PlayerIsHere;
    public Animator anime;
    public PlayerMotor Motor;
    public bool CanChop;
    public PIckUpAxe Axe;
    public Transform Player;
    public float Range;

    public void Update()
    {





        if (Vector3.Distance(transform.position, Player.position) < Range)
        {

            PlayerIsHere = true;


            if (PlayerIsHere)
            {

                if (Input.GetKeyDown(KeyCode.Mouse0) && CanChop && Axe.CanChop)
                {
                    anime.SetBool("CutTRee", true);

                    Motor.Canmove = false;
                    CanChop = false;
                }

            }



        } else
        {

            PlayerIsHere = false;

        }




        if(!PlayerIsHere)
        {
            anime.SetBool("CutTRee", false);

        }

    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);


    }



}
