using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFlock : MonoBehaviour
{


    public Transform[] MovePaths;
    public int DestinationPoints;
    public float StoppingDistance;
    public float SPeed;
    public float Timer;
    public float MaxTimer;
    public float TimerSpeed;



    private void Start()
    {


        BirdMove();


    }


    private void Update()
    {
       
        BirdMove();


      


    }

    void BirdMove()
    {

     

        transform.position = Vector3.MoveTowards(transform.position, MovePaths[DestinationPoints].transform.position, SPeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, MovePaths[DestinationPoints].transform.position) < StoppingDistance)
        {
            Timer += TimerSpeed * Time.deltaTime;

            if (Timer > MaxTimer)
            {
                DestinationPoints = Random.Range(0, MovePaths.Length);
                Timer = 0f;

            }



        } else

        {

            RotateTowards();
        }

    }




    void RotateTowards()
    {


        Vector3 dir = MovePaths[DestinationPoints].transform.position - transform.position;

        Quaternion Rot = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(transform.rotation, Rot, 2f * Time.deltaTime);


    }
}
