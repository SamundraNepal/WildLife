using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.AI;

public class CallForHelp : MonoBehaviour
{
    public EnemyMotor EM;

    public float VoiceRange;

    public GameObject[] NearByEnemies;
    public GameObject CalledEnemy;

    public GameObject player;

    public Vector3 playerSeenPos;

    public float NearByPeople;





    private void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        EM = GetComponent<EnemyMotor>();
        NearByEnemies = GameObject.FindGameObjectsWithTag("Enemies");

    }

    private void Update()
    {

        if (EM.CanSeePlayer)
        {
            foreach (var Enemy in NearByEnemies)
           

            {

                if (Vector3.Distance(transform.position, player.transform.position) < VoiceRange)
                {
                    playerSeenPos = player.transform.position;

                  
                        float dis = Vector3.Distance(Enemy.transform.position , transform.position);
                        if (dis <= NearByPeople)
                        {
                           CalledEnemy = Enemy;

                        }

                }
                else
                {
                    NearByPeople = 2f;
                    CalledEnemy = null;

                }

            }


        }

      
     

            if (CalledEnemy != null && CalledEnemy.GetComponent<EnemyMotor>().CanSeePlayer == false && CalledEnemy.GetComponent<EnemyMotor>().EnemyStates != EnemyMotor.EnemyBrain.GunShotHeard)
            {
            if (CalledEnemy.GetComponent<EnemHealth>().Isdead == false)
            {


                if (CalledEnemy.GetComponent<NavMeshAgent>().enabled == true)
                {


                    extraRotation();
                    CalledEnemy.GetComponent<EnemyMotor>().EnemyStates = EnemyMotor.EnemyBrain.HeardFreindVoice;
                    CalledEnemy.GetComponent<Animator>().SetBool(CalledEnemy.GetComponent<EnemyMotor>().Running, true);
                    CalledEnemy.GetComponent<NavMeshAgent>().speed = CalledEnemy.GetComponent<EnemyMotor>().ChaseSpeed;
                    CalledEnemy.GetComponent<NavMeshAgent>().destination = playerSeenPos;
                }
            }
            }

        
    }




    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.grey;

        Gizmos.DrawWireSphere(transform.position, VoiceRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, NearByPeople);


    }



    void extraRotation()
    {
        Vector3 lookrotation =  playerSeenPos- CalledEnemy.transform.position;
        CalledEnemy.transform.rotation = Quaternion.Slerp(CalledEnemy.transform.rotation, Quaternion.LookRotation(lookrotation), 10f * Time.deltaTime);
    }




}
