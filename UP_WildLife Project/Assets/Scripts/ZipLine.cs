using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZipLine : MonoBehaviour
{



    public bool IsZipping;


    public Transform Player;
    public Transform PointB;
    public PlayerMotor motor;

    private void Start()
    {

        IsZipping = false;

    }

    public void OnTriggerStay(Collider other)
    {



        if (other.gameObject.tag == "Player")
        {


            Debug.Log("Can press E");


            if (Input.GetKey(KeyCode.E))
            {

                IsZipping = true;
                motor.enabled = false;
            }


        }



    }

    private void Update()
    {
        

        if(IsZipping == true)
        {

            
            Player.transform.position = Vector3.Lerp(Player.transform.position, PointB.transform.position, 0.5f * Time.deltaTime);
            Player.LookAt(PointB);


        }

    }

}
