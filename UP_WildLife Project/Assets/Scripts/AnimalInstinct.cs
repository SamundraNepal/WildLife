using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalInstinct : MonoBehaviour
{

    public enum AnimalType { Fox, Elephant , Rhino};
    public AnimalType AT;

    public LayerMask Obstacles;
    public enum AnimalStates { MovingAroud , Graze , Growling, AttackPlayer }
    public AnimalStates AS;


    public bool FinaLScene;
    public Transform[] MovePaths;
    public int DestinationPoints;
    public NavMeshAgent Agent;
    public float PatrolSpeed;
    // public float ChaseSpeed;


    [Header("Animal Timers")]
    public float Timer;
    public float RetreatTimer;
    public float AttackTimer;
    public float MaxAttackTimer;
    public float StopAttackDis = 0.4f;


    [Header("Animal Range")]
    public Transform Player;
    public Vector3 PlayerPos;
    public float Range;
    public float ViewAngle;
    public float MaxAngle;
    public bool SawPlayer;

    [Header("Animations")]
    public Animator Anime;
    public string Walk;
    public string Run;
    public string Attack;
    public GameObject HitCollider;
    public AudioSource AudioS;
    public AudioClip AC;
    public bool PlayerIshere;
    public bool CageOpen;
   
   
   



    private void Start()
    {
        if(AT != AnimalType.Elephant)
        {
        HitCollider.SetActive(false);

        }
     //   Anime = GetComponent<Animator>();
        Anime.SetBool(Run, true);

        AS = AnimalStates.MovingAroud;
        Agent.speed = PatrolSpeed;
        if(AT!=AnimalType.Rhino)
        {

        MoveBetweenPoints();
        }

        if(AT ==AnimalType.Rhino)
        {

            Agent.enabled = false;
        }

    }
    private void Update()
    {
        if (CageOpen == true)
        {
            Agent.enabled = true;


            if (AS != AnimalStates.Growling)
            {



                if (!Agent.pathPending && Agent.remainingDistance <= 0.2f)
                {
                    Anime.SetBool(Run, false);

                    Agent.isStopped = true;
                    Timer += 0.1f * Time.deltaTime;
                    AS = AnimalStates.Graze;

                    if (Timer >= 5f)
                    {
                        MoveBetweenPoints();
                        Timer = 0f;

                    }
                }
            }
        }
        

        if(!FinaLScene)
        {
        LineOfSight();

        }
    

  
    }


   
    public void MoveBetweenPoints()
    {
        Anime.SetBool(Run, true);

        AS = AnimalStates.MovingAroud;
        if (MovePaths.Length == 0)
            return;
        Agent.isStopped = false;
        DestinationPoints = Random.Range(0, MovePaths.Length);
        Agent.destination = MovePaths[DestinationPoints].transform.position;
    }




    public void LineOfSight()
    {


        if (Vector3.Distance(transform.position, Player.transform.position) < Range)
        {
            Vector3 Dis = Player.transform.position - transform.position;
             MaxAngle = Vector3.Angle(transform.forward , Dis);

            PlayerPos = Player.transform.position;

            if(MaxAngle < ViewAngle)
            {
                float DistanceToTarget = Vector3.Distance(transform.position, Player.transform.position);
                if (!Physics.Raycast(transform.position + transform.up * 0.33f, Dis, DistanceToTarget, Obstacles))
                {
                    PlayerIshere = true;
                    Agent.isStopped = true;
                    AS = AnimalStates.Growling;
                    ViewAngle = 360f;
                    RotateTowardsThePlayer();
                    SawPlayer = true;

                    if(AS != AnimalStates.AttackPlayer)
                    {
                        Anime.SetBool(Run, false);
                        Anime.SetBool(Walk, false);
                    }
                    

                    if (!AudioS.isPlaying)
                    {
                    AudioS.clip = AC;
                    AudioS.Play();

                    }

                    

                }




            }

        } else

        {
            PlayerIshere = false;

            if (AudioS.isPlaying)
            {
                AudioS.Stop();

            }
          
            if(SawPlayer)
            {
                Anime.SetBool(Run, false);

                Anime.SetBool(Walk, false);

                RetreatTimer += 0.1f * Time.deltaTime;
                if(RetreatTimer >5f)
                {
                    ViewAngle = 100f;

                    AS = AnimalStates.MovingAroud;
                    MoveBetweenPoints();
                    RetreatTimer = 0f;
                    SawPlayer = false;

                }


            }



        }
        if (AT != AnimalType.Elephant)
        {


            if (SawPlayer)
            {


                AttackPlayer();
            }

        }

        if(SawPlayer)
        {
            ElephantAttack();
        }
        




    }



    public void RotateTowardsThePlayer()
    {
        Vector3 Dir = Player.transform.position - transform.position;
        Dir.y = 0f;
        Quaternion Rot = Quaternion.LookRotation(Dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, Rot, 5f * Time.deltaTime);



    }


    public void AttackPlayer()
    {
        AS = AnimalStates.AttackPlayer;

        float dis = Vector3.Distance(Player.transform.position, transform.position);

        if(dis > 1f)
        {
            Anime.SetBool(Run, false);

            Anime.SetBool(Walk, false);

        }

        if(dis < 1f)
        {

            Vector3 newDis = Player.transform.position - transform.position;
            AttackTimer += MaxAttackTimer * Time.deltaTime;
            if(AttackTimer < 1f)
            {

                Anime.SetBool(Run, false);

                Anime.SetBool(Walk, true);

                transform.position -= newDis * 2f * Time.deltaTime;

            } 

            if(AttackTimer > 1f)
            {
                float ATtackDis = Vector3.Distance(Player.transform.position, transform.position);

                if (ATtackDis > StopAttackDis)
                {
                    Anime.SetBool(Walk, false);

                    Anime.SetBool(Run, true);

                    Vector3 ATtack = Player.transform.position - transform.position;

                    transform.position += ATtack * 5f * Time.deltaTime;
                    HitCollider.SetActive(true);

                    StartCoroutine(BackTimer());

                }


            }


        }




    }



    public void ElephantAttack()
    {


     


        if(AT == AnimalType.Elephant)
        {

            AS = AnimalStates.AttackPlayer;

            float Dis = Vector3.Distance(PlayerPos, transform.position);

            if(Dis > 1.1f)
            {
                Anime.SetBool(Walk, true);

                Agent.isStopped = false;
                Vector3 WalkCloser = transform.position - PlayerPos;
                Agent.destination -= WalkCloser;

            }

            if(Dis <= 1.1f)
            {
                if(PlayerIshere)
                {

                    Anime.SetBool(Attack, true);
                }
                else
                {
                    Anime.SetBool(Attack, false);

                }


            } else
            {
                Anime.SetBool(Attack, false);
            }

            if(Dis < 0.5f)
            {
                Anime.SetBool(Walk, true);

                Vector3 WalkCloser = transform.position - PlayerPos;

                Agent.destination += WalkCloser;



            }

                if (!Agent.pathPending && Agent.remainingDistance <= 1.1f)
                {


                    Agent.isStopped = true;

                }
            

          




        }



    }


    IEnumerator BackTimer()
    {

        yield return new WaitForSeconds(1f);
        HitCollider.SetActive(false);

        AttackTimer = 0f;

    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, Range);



    }


}
        


