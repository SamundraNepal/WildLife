using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AnimalMoveAround : MonoBehaviour
{
    NavMeshAgent Agent;


    public Transform[] WolfWayPoint;
    int CurrentIndex;
    public enum State { Idle, Walking, Hunting, Running };
    public State WolfState;
    public Animator Anime;

    public int RandomNUmber;


    public string Walking;
    public string Running;


    public float TimerToRun;
    public float TimerReset;

    public bool RandomChoose;
    public bool newCheck;

    public int NewIndex;
        

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        CurrentIndex = Random.Range(0, WolfWayPoint.Length-1);
        Agent.SetDestination(WolfWayPoint[CurrentIndex].transform.position);
        RandomChoose = true;
        newCheck = true;

        WolfState = State.Idle;
    }



     void Update()
    {
        Debug.Log("Hey");

        if (RandomChoose)
        {

            if (Vector3.Distance(WolfWayPoint[CurrentIndex].transform.position,transform.position) < 1f)
            {


                RandomNUmber = Random.Range(0, 3);

                print("Random Check");

                if (RandomNUmber == 0)
                {
                    WolfState = State.Idle;
                    RandomChoose = false;

                }
                else if (RandomNUmber == 1)
                {

                    WolfState = State.Walking;
                    RandomChoose = false;



                }
                else if (RandomNUmber == 2)
                {

                    WolfState = State.Running;
                    RandomChoose = false;

                }

            }



        }

        if(RandomChoose == false)
        {
            Checks();


        }
    }



    void Checks()
    {


        if (WolfState == State.Running)
        {
            if (newCheck)
            {
                CurrentIndex = Random.Range(0, WolfWayPoint.Length - 1);
                newCheck = false;

            }

            Agent.SetDestination(WolfWayPoint[CurrentIndex].transform.position);

            if (Vector3.Distance(this.transform.position, WolfWayPoint[CurrentIndex].transform.position) < 1f)
            {

                RandomChoose = true;
                newCheck = true;

            }

        }


        if (WolfState == State.Walking)
        {


            if (newCheck)
            {
                CurrentIndex = Random.Range(0, WolfWayPoint.Length - 1);
                newCheck = false;

            }

            Agent.SetDestination(WolfWayPoint[CurrentIndex].transform.position);

            if (Vector3.Distance(this.transform.position, WolfWayPoint[CurrentIndex].transform.position) < 1f)
            {

                RandomChoose = true;
                newCheck = true;

            }

        }
    }


}





