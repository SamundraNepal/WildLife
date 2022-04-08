using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAvoidance : MonoBehaviour
{

    public GameObject[] Enemies;
    public float AvoidDis= 0.5f;





    private void Start()
    {
        

        Enemies = GameObject.FindGameObjectsWithTag("Enemies");


    }
    private void Update()
    {



        Avoidance();

    }



    public void Avoidance()
    {
        foreach (var Enemy in Enemies)
        {

            if (Enemy.GetComponent<NavMeshAgent>().enabled == true)
            {



                if (Enemy != null)
                {
                    float CurrentDistnace = Vector3.Distance(Enemy.transform.position, transform.position);


                    if (CurrentDistnace < AvoidDis)
                    {
                        //  AvoidDis = CurrentDistnace;
                        Physics.IgnoreCollision(Enemy.GetComponent<Collider>(), transform.GetComponent<Collider>());
                          Vector3 dis = (transform.position - Enemy.transform.transform.position).normalized;
                      
                    if(Vector3.Distance(Enemy.transform.position , transform.position) < 0.5f)
                        {
                            if (transform.GetComponent<NavMeshAgent>().enabled == true)
                            {

                                transform.GetComponent<EnemyMotor>().Agent.destination += dis * 2f * Time.deltaTime;
                            }
                        }
                        
                    }



                   

                
                             /* if (transform.GetComponent<EnemyMotor>().CanSeePlayer == false && transform.GetComponent<EnemyMotor>().EnemyStates==EnemyMotor.EnemyBrain.GunShotHeard || transform.GetComponent<EnemyMotor>().EnemyStates == EnemyMotor.EnemyBrain.HeardFreindVoice)
                              {

                            if (transform.GetComponent<EnemyMotor>().Agent.remainingDistance < 0.3f && transform.GetComponent<EnemyMotor>().Agent.pathPending == false)
                               {
                                         
                               transform.GetComponent<EnemyMotor>().EnemyStates = EnemyMotor.EnemyBrain.LookingAroud;
                               // transform.GetComponent<EnemyMotor>().MoveToNextPoint = true;


                               }
                            }*/
                         }

                
            }
           
        }

    }


        
        

       

  

}
