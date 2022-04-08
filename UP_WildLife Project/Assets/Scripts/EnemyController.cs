using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

   public GunEnemyMotor GEM;
    public GameObject[] WalkingEnemies;



    public Transform P;
    public float ViewRange;

    public GameObject CurrentSelectedEnemyToHelp;
    private void Start()
    {

        GEM = GetComponent<GunEnemyMotor>();

        WalkingEnemies = GameObject.FindGameObjectsWithTag("Enemies");

    }


    private void Update()
    {


        if (GEM.SawPlayer)

        {

           

            for (int E = 0; E < WalkingEnemies.Length; E++)
            {
              


                
               float Dis  = Vector3.Distance(transform.position, WalkingEnemies[E].transform.position);
                     


                if(Dis < ViewRange)
                {
                   
                    

                       
                    CurrentSelectedEnemyToHelp = WalkingEnemies[E];
                    ViewRange = Dis;
                    CurrentSelectedEnemyToHelp.GetComponent<EnemyMotor>().EnemyStates = EnemyMotor.EnemyBrain.CheckThePlace;



                }
            }

            if (CurrentSelectedEnemyToHelp != null)
            {
                if (CurrentSelectedEnemyToHelp.GetComponent<EnemyMotor>().CanSeePlayer == false)
                {
                    if (CurrentSelectedEnemyToHelp.GetComponent<EnemyMotor>().EP == EnemyMotor.EnemyPetrol.StandingEnemy)
                    {
                        CurrentSelectedEnemyToHelp.GetComponent<Animator>().SetBool(CurrentSelectedEnemyToHelp.GetComponent<EnemyMotor>().Idle, false);

                    }


                    if (CurrentSelectedEnemyToHelp.GetComponent<EnemyMotor>().EnemyStates == EnemyMotor.EnemyBrain.CheckThePlace && CurrentSelectedEnemyToHelp.GetComponent<EnemyMotor>().Agent.enabled == true)
                    {
                        CurrentSelectedEnemyToHelp.GetComponent<EnemyMotor>().Agent.destination =GEM.GetComponent<GunEnemyMotor>().SawPosition;
                        CurrentSelectedEnemyToHelp.GetComponent<EnemyMotor>().Agent.isStopped = false;
                        CurrentSelectedEnemyToHelp.GetComponent<NavMeshAgent>().speed = CurrentSelectedEnemyToHelp.GetComponent<EnemyMotor>().PatrolSpeed = 0.8f;
                        CurrentSelectedEnemyToHelp.GetComponent<Animator>().SetBool(CurrentSelectedEnemyToHelp.GetComponent<EnemyMotor>().Looking, false);
                        CurrentSelectedEnemyToHelp.GetComponent<Animator>().SetBool(CurrentSelectedEnemyToHelp.GetComponent<EnemyMotor>().Running, true);
                    }


                  
                }


                if (Vector3.Distance(CurrentSelectedEnemyToHelp.transform.position, GEM.GetComponent<GunEnemyMotor>().SawPosition) < 0.1f)
                {
                    CurrentSelectedEnemyToHelp.GetComponent<Animator>().SetBool(CurrentSelectedEnemyToHelp.GetComponent<EnemyMotor>().Running, false);
                    ViewRange = 10f;
                    CurrentSelectedEnemyToHelp = null;


                }
            }

           


        }


      

    }


}

