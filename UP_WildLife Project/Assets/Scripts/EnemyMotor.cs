using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyMotor : MonoBehaviour
{
    public Vector3 FoundPLayerPosition;
    public float NearDis;
    public float dis;
    public GameObject place;
    [Header("AudioClips")]
     AudioSource Source;
    public AudioClip PlayerNotice;
    public AudioClip PlayerSeen;
    public AudioClip SHooting;
    public GameObject SHootingEffects;
    public AudioSource AS;
    public Transform ShootPoint;
    public GameObject BloodEffects;
    public float GunDamageRate = 0.1f;




    public Collider BlockCollider;
    public PlayerMotor PM;
    public EnemHealth EH;

    public enum EnemyPetrol { RoamingEnemy, StandingEnemy }
    public EnemyPetrol EP;
    public enum EnemyRoles { HandAttackEnemy , ShutGun , }
    public EnemyRoles ER;
    public enum EnemyBrain { Patrolling, LookingAroud, Chasing, Searching, Notice, Nevrous, Circling, Attacking, Deathbody, GoandCheck , CheckThePlace , HeardFreindVoice , IsGrabbed , GunShotHeard , GranadeThrow , FindCOver,RapidFiring , FincingCover};
    public EnemyBrain EnemyStates;

    public enum FireArmType {ShotGun , AK47 , HandAttack};

    public FireArmType FAT;

    public bool Death;
    public Transform CarryPoint;
    public PlayerHealth Ph;
    public GameObject PA;

    [Header("UI")]
    public GameObject DetectionObject;
    public Image DetectionMeter;
    public float DetectionMeterInt;
    public float DetectionFillamount;


    [Header("Animations")]
    public string Running;
    public string Looking;
    public string Looking2;
    public string Faint;
    public string ThrowGranadeAnimations;
    public string AttackingPose;
    public string GunAnimations;
    public string NervousWalk;
    public string Idle;




    [Header("Enemy AI Movements")]
    public Transform[] MovePaths;
    public float RestTimer;
    public float RestTimeValue;
    public float MaxmimumWaitTime;
    public int DestinationPoints;
    public float PatrolSpeed;
   public NavMeshAgent Agent;
    public bool IsPlayerInsideRange;
    public bool MoveToNextPoint;
    public Animator anime;


    [Header("Enemy Field of View")]
    public float Range;
    public Vector3 PLayerposCheck;
    public Transform Player;
    public float Angle;
    public float CheckAngle;
    public bool CanSeePlayer;
    public LayerMask Obstacles;
    public float RoateSpeed;


    [Header("Chase Enemy")]
    public float ChaseSpeed;
    public float EnemyNervouWalkSpeed;
    public float AfterPlayerNotFoundWaitTime;
    public float AfterPlayerNotFoundWaitTimeTimer;
    public bool WaitTimeCheck;



    [Header("Bools")]
    public bool Once;
    public bool IsMoving;
    public bool CanCircle;
    public bool PlayerdeadMoveBackTOwayPoint;
    public bool Player_Notice;
    public bool isYelling;
    [Header("Integers")]
    public int CountForNervousMovements;
    public GameObject PlayerPoint;
    public float SearchingForPlayerCountDonwTime;
    public float SearchingForPlayerCountDonwTimeTimer;


    [Header("EnemySearchPoint")]
    public GameObject SearchPoint;
    private GameObject[] LookPoints;




    [Header("SneakAtack")]
    public string SneakAnimationDeath;
    public float SneakAngle;
    public float SneakRange;
    public GameObject SneakAttackIcon;

    [Header("Attack Ssytem")]
    public float CircleTime;
    public float AttackTime;
    public bool IsCircling;
    public bool IsAttacking;
    public float CoolDownTime;
    public int AttackValue;
    public string EnemyAttacks1;
    public string EnemyAttacks2;
    public string EnemyAttacks3;
    public string EnemyAttacks4UnbloackAble;
    public float StopToAttackPose;
    public float ShootTimer;

    public float CoolDownSpeed;
    public float CircleSpeed;
    public float AttackSpeed;
    public Vector3 DistanceBetweenPlayerAndEnemy;
    public Vector3 MoveBackEnemyPosition;
    public bool MoveTowardsThePlayer;
    public bool MoveBackFromPlayer;
    public bool MoveBackPosCheck;
    public float MaxCoolDownTime;
    public float Dis;
    public float Dis2;

    [Header("IdelEnemy")]
    public Vector3 PertolPosition;
    public float Timer;

    [Header("Shoot System")]
    public string WalkRight;
    public string Firirng;
    public float ShootingRnage;
    public bool CallBack;
    /* public GameObject[] Covers;
      public string CoverNamme;*/

   /* [Header("Find Covers")]
    public GameObject[] Covers;*/

    private void Start()
    {
        CallBack = false;

        /*  if(FAT == FireArmType.AK47)
           {
               Covers = GameObject.FindGameObjectsWithTag("Cover");

           }*/
        PA = GameObject.FindGameObjectWithTag("Player");
        Source = GetComponent<AudioSource>();
        anime = GetComponent<Animator>();
        if(EP == EnemyPetrol.StandingEnemy)
        {
           
                anime.SetBool(Idle, true);
            
        }
            PertolPosition = this.transform.position;
        EH = GetComponentInChildren<EnemHealth>();
        EnemyStates = EnemyBrain.Patrolling;
        Agent = GetComponent<NavMeshAgent>();
        Agent.autoBraking = false;
        GoToNextPoint();

        anime.SetBool(Faint, false);

    }







    private void Update()
    {



        if (!EH.Isdead)
        {
           
          
            if (EnemyStates != EnemyBrain.Deathbody)
            {
                if (EnemyStates == EnemyBrain.GranadeThrow)
                {
                    Agent.isStopped = true;
                    RoateEenemy();
                    if (Vector3.Distance(transform.position, Player.position) < Range)
                    {

                        ThrowGranade();
                    }
                }
              



                    float MoveBack = Vector3.Distance(Player.transform.position, transform.position);

                    if (MoveBack <= 0.1f)
                    {
                        Vector3 MOveBackdis = Player.transform.position - transform.position;
                        transform.position -= MOveBackdis * 10f * Time.deltaTime;
                    }

                    DetectionMeter.fillAmount = DetectionMeterInt;

                    if (Ph.IsDead)
                    {
                        EnemyStates = EnemyBrain.Patrolling;

                        if (PlayerdeadMoveBackTOwayPoint)
                        {

                            GoToNextPoint();
                            PlayerdeadMoveBackTOwayPoint = false;
                            anime.SetBool(AttackingPose, false);

                        }




                    }


                    if (!Death && !Ph.IsDead && !EH.Isdead)
                    {
                        SneakBackStab();


                        if (EnemyStates == EnemyBrain.Patrolling && EP == EnemyPetrol.RoamingEnemy)
                        {
                            anime.SetBool(AttackingPose, false);
                        }

                        LineOfSight();
                        EnemyStatesBehaviour();

                        if (!CanSeePlayer && EnemyStates != EnemyBrain.Deathbody)
                        {
                            Agent.speed = PatrolSpeed;
                            if (!Agent.pathPending && Agent.remainingDistance <= 0.1f)
                            {
                            MoveToNextPoint = true;
                                EnemyStates = EnemyBrain.LookingAroud;
                                Agent.isStopped = true;
                                anime.SetBool(Looking, true);
                                anime.SetBool(Running, false);

                                if (MoveToNextPoint &&  EP != EnemyPetrol.StandingEnemy)
                                {

                                    RestTimer += RestTimeValue * Time.deltaTime;

                                    if (RestTimer >= MaxmimumWaitTime)
                                    {
                                        Agent.isStopped = false;
                                        EnemyStates = EnemyBrain.Patrolling;
                                        GoToNextPoint();
                                        anime.SetBool(Looking, false);
                                        RestTimer = 0f;
                                        MoveToNextPoint = false;
                                    }
                                }


                            


                        }

                        }


                    }
                

            }




        }
        else
        {

            EnemyStates = EnemyBrain.Deathbody;
        }
    }

   

    public void GoToNextPoint()
    {

        if (EP == EnemyPetrol.RoamingEnemy)
        {
            if (MovePaths.Length == 0)
                return;
            Agent.speed = PatrolSpeed;
            Agent.isStopped = false;
            DestinationPoints = Random.Range(0, MovePaths.Length);
            Agent.destination = MovePaths[DestinationPoints].transform.position;
            

            if (SearchPoint != null)
            {
                LookPoints = GameObject.FindGameObjectsWithTag("EnemyPoint");
                for (int i = 0; i < LookPoints.Length; i++)
                {
                    Destroy(LookPoints[i]);

                }
            }
        }


        if (EP == EnemyPetrol.StandingEnemy)
        {
            if (Vector3.Distance(transform.position, PertolPosition)> 0.2f)
            {
             Agent.destination = PertolPosition;
                anime.SetBool(Idle, false);
                anime.SetBool(Looking, false);
            } else
            {
                anime.SetBool(Idle, true);

            }

        }
    }

    public void LineOfSight()
    {


        if (Vector3.Distance(transform.position, Player.position) < Range)
        {

            PLayerposCheck = Player.position;
            Vector3 dir =(PLayerposCheck - transform.position).normalized;
            Angle = Vector3.Angle(transform.forward,dir);



            if (Angle < CheckAngle)
            {



                    dis = (PLayerposCheck.y - transform.position.y);
                    if (dis > 0.4f && IsPlayerInsideRange && CanSeePlayer)
                    {
                   

                        EnemyStates = EnemyBrain.GranadeThrow;
                    
                   
                    } 
                if (dis < -0.3f)
                {

                  

                        EnemyStates = EnemyBrain.GranadeThrow;
                      


                }

                 if (dis < 0.4f && dis > -0.3f)
                   {
                    EnemyStates = EnemyBrain.Notice;
                   }




                float DistanceToTarget = Vector3.Distance(transform.position, Player.transform.position);
                if (!Physics.Raycast(transform.position + transform.up * 0.33f, dir, DistanceToTarget, Obstacles))
                {
                    CanSeePlayer = true;

                    FoundPLayerPosition = Player.transform.position;
                    if (DistanceToTarget <= 0.3f)
                    {

                        DetectionMeterInt += 0.5f;

                    }

                    if (EP == EnemyPetrol.StandingEnemy)
                    {
                        anime.SetBool(Idle, false);
                    }

                    DetectionObject.SetActive(true);
                    DetectionFunction();

                    if (DetectionMeterInt >= 1f)
                    {
                       

                            MoveToNextPoint = true;
                            if (EnemyStates == EnemyBrain.GranadeThrow)
                            {
                                WaitTimeCheck = true;

                            }

                            if (EnemyStates != EnemyBrain.GranadeThrow)
                            {

                               


                                EnemyStates = EnemyBrain.Notice;
                                IsPlayerInsideRange = true;
                                RoateEenemy();
                                WaitTimeCheck = true;
                                AfterPlayerNotFoundWaitTime = 0f;
                                RestTimer = 0f;
                                CountForNervousMovements = 0;
                                SearchingForPlayerCountDonwTime = 0f;
                                CanCircle = true;






                                if (DetectionMeterInt == 1f)
                                {
                                    Source.clip = PlayerSeen;
                                    Source.Play();
                                }


                                DetectionObject.SetActive(false);
                                SneakAttackIcon.SetActive(false);
                            }
                    


                        

                       

                    }



                  
                } else
                {
                    CanSeePlayer = false;

                    CanCircle = false;
                  /*  if (EP != EnemyPetrol.StandingEnemy)
                    {

                        if (EnemyStates == EnemyBrain.Circling || EnemyStates == EnemyBrain.Attacking)
                        {
                            SearchingForPlayerCountDonwTime += SearchingForPlayerCountDonwTimeTimer * Time.deltaTime;
                            if (SearchingForPlayerCountDonwTime > 50f)
                            {

                                SearchingForPlayerCountDonwTime = 0f;
                                EnemyStates = EnemyBrain.Patrolling;
                                GoToNextPoint();


                            }
                        }
                    }*/

                

                    
                }
            } else
            {

                DetectionObject.SetActive(false);


                if (DetectionMeterInt > 0f)
                {
                    DetectionMeterInt -= 0.1f * Time.deltaTime;

                }
            }

         

        }
        else

        {

            if (WaitTimeCheck)
            {
                EnemyStates = EnemyBrain.Searching;
               


                    CanSeePlayer = false;
                    Player_Notice = false;
                    Agent.isStopped = false;
                   // Agent.destination = FoundPLayerPosition;

                    if (ER == EnemyRoles.ShutGun)
                    {
                        anime.SetBool(GunAnimations, false);
                    }

                    if (EP == EnemyPetrol.StandingEnemy)
                    {
                        anime.SetBool(Idle, false);
                    }

                    if (FAT == FireArmType.AK47)
                    {
                        anime.SetBool(Firirng, false);
                    }


                    anime.SetBool(AttackingPose, false);
                    CheckAngle = 100f;

                    DetectionMeterInt = 0f;
                    IsPlayerInsideRange = false;

                    if (IsPlayerInsideRange == false)
                    {
                        if (!Agent.pathPending && Agent.remainingDistance < 1f)
                        {

                            Agent.isStopped = true;
                            AfterPlayerNotFoundWaitTime += AfterPlayerNotFoundWaitTimeTimer * Time.deltaTime;
                            anime.SetBool(AttackingPose, false);
                            if (EnemyStates != EnemyBrain.CheckThePlace)
                            {
                                anime.SetBool(Running, false);

                            }
                            anime.SetBool(Looking, true);

                            if (AfterPlayerNotFoundWaitTime > 30f)
                            {
                                RestTimer = 0f;
                                EnemyStates = EnemyBrain.Nevrous;
                                AfterPlayerNotFoundWaitTime = 0f;
                                CanSeePlayer = true;
                                Once = true;
                                WaitTimeCheck = false;


                            }
                        }
                    

                }
            }

            CircleTime = 0f;
            AttackTime = 0f;
            IsAttacking = false;
            if (ER == EnemyRoles.HandAttackEnemy)
            {
                anime.SetBool(EnemyAttacks1, false);
                anime.SetBool(EnemyAttacks2, false);
                anime.SetBool(EnemyAttacks3, false);
            }


        }

    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, Range);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SneakRange);


    }

    public void RoateEenemy()
    {
        Vector3 dir = (FoundPLayerPosition - transform.position).normalized;
        Quaternion Rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, Rot, 2f * Time.deltaTime);


    }

    public void EnemyStatesBehaviour()
    {




        if (EnemyStates == EnemyBrain.Nevrous)
        {
            CheckAngle = 100f;


            if (CountForNervousMovements <= 3)
            {
                if (Once)
                {
                    Agent.isStopped = false;
                    SearchPoint = Instantiate(PlayerPoint, Player.transform.position, Player.transform.rotation);
                    SearchPoint.transform.position = Player.transform.position;
                    Agent.destination = SearchPoint.transform.position;
                    Agent.speed = EnemyNervouWalkSpeed;

                    anime.SetBool(Looking, false);
                    anime.SetBool(Running, false);
                    anime.SetBool(NervousWalk, true);
                    IsMoving = true;
                    Once = false;

                }


                if (Vector3.Distance(transform.position, Agent.destination) < 0.5f && IsMoving)
                {
                    anime.SetBool(Looking, true);
                    anime.SetBool(NervousWalk, false);
                    CountForNervousMovements += 1;
                    Agent.isStopped = true;

                    IsMoving = false;
                }

                if (!IsMoving)
                {
                    SearchingForPlayerCountDonwTime += SearchingForPlayerCountDonwTimeTimer * Time.deltaTime;

                    if (SearchingForPlayerCountDonwTime > 50f)
                    {
                        anime.SetBool(Looking, false);

                        SearchingForPlayerCountDonwTime = 0f;
                        Once = true;
                    }
                }


            }
            else if (CountForNervousMovements >= 3)
            {
                Agent.isStopped = false;
                CanSeePlayer = false;
                SearchingForPlayerCountDonwTime = 0f;
                CallBack = true;
                EnemyStates = EnemyBrain.Patrolling;
                GoToNextPoint();
                CountForNervousMovements = 0;

                if (SearchPoint != null)
                {
                    LookPoints = GameObject.FindGameObjectsWithTag("EnemyPoint");
                    for (int i = 0; i < LookPoints.Length; i++)
                    {
                        Destroy(LookPoints[i]);

                    }

                }


            }
        }




        if (EnemyStates == EnemyBrain.Notice)
        {
            Agent.isStopped = false;
            anime.SetBool(Looking, false);
            EnemyStates = EnemyBrain.Chasing;

        }


        if (EnemyStates == EnemyBrain.Chasing)
        {
            RoateEenemy();
            if (FAT == FireArmType.AK47)
            {

                anime.SetBool(Firirng, false);
            }
            anime.SetBool(Running, true);
            anime.SetBool(NervousWalk, false);
            anime.SetBool(AttackingPose, false);
            anime.SetBool(Looking, false);

            Agent.isStopped = false; ;
            Agent.speed = PatrolSpeed;
            Agent.SetDestination(FoundPLayerPosition);

            if (!Agent.pathPending && Agent.remainingDistance < StopToAttackPose)
            {
                anime.SetBool(Running, false);
                Agent.isStopped = true;




                if (FAT != FireArmType.AK47 )
                {

                    EnemyStates = EnemyBrain.Circling;
                    IsCircling = true;
                }
                else if (FAT == FireArmType.AK47 )
                {
                    EnemyStates = EnemyBrain.RapidFiring;
                    //  FindCover();
                    RapidFire();


                }


            } 


        }

    


            if (ER == EnemyRoles.HandAttackEnemy)
        {


            if (EnemyStates == EnemyBrain.Circling && CanCircle)
            {


                if (IsCircling && !IsAttacking)
                {
                  // BlockCollider.enabled = true;

                    Agent.isStopped = true;
                    anime.SetBool(AttackingPose, true);
                    transform.RotateAround(Player.transform.position, Vector3.up, 30f * Time.deltaTime);
                    RoateEenemy();

                    CircleTime += CircleSpeed * Time.deltaTime;

                    if (Vector3.Distance(transform.position, Player.transform.position) < 0.3f)
                    {
                        Vector3 Dist = Player.transform.position - transform.position;

                        transform.position -= Dist * 2f * Time.deltaTime;
                    }

                }

                if (CircleTime > MaxCoolDownTime && IsCircling)
                {
                    CircleTime = 0f;
                    IsCircling = false;
                    IsAttacking = true;
                }



            }

            if (IsAttacking)
            {
                EnemyStates = EnemyBrain.Attacking;
                IsCircling = false;
                AttackTime += AttackSpeed * Time.deltaTime;



                if (EnemyStates == EnemyBrain.Attacking)
                {

                    CheckAngle = 360f;

     

                    if (CoolDownTime == 0)
                    {
                        AttackValue = Random.Range(1, 5);
                        anime.SetBool(AttackingPose, false);
                        MoveTowardsThePlayer = true;

                    }



                    Physics.IgnoreCollision(GetComponent<Collider>(), Player.GetComponent<Collider>());


                     float MoveBack = Vector3.Distance(Player.transform.position, transform.position);

                     if (MoveBack < 0.1f)
                     {
                         Vector3 MOveBackdis = Player.transform.position - transform.position;
                         transform.position -= MOveBackdis * 10f * Time.deltaTime;

                         MoveTowardsThePlayer = false;
                     }
                     else
                     {
                         MoveTowardsThePlayer = true;
                     }

                    if (Vector3.Distance(transform.position, Player.transform.position) < 1f)
                    {
                        if (MoveTowardsThePlayer)
                        {
                            Dis = Vector3.Distance(Player.transform.position, transform.position);

                            if (Dis >= 0.15f)
                            {
                                DistanceBetweenPlayerAndEnemy = Player.transform.position - transform.position;

                                transform.position += DistanceBetweenPlayerAndEnemy * 1f * Time.deltaTime;

                            }

                        }
                    }

                    if (Dis <= 0.15f)
                    {


                        if (MoveTowardsThePlayer)
                        {
                            if (AttackValue == 1)
                            {

                                anime.SetBool(EnemyAttacks1, true);

                            }
                            else if (AttackValue == 2)

                            {
                                anime.SetBool(EnemyAttacks2, true);

                        }
                        else if (AttackValue == 3)
                            {

                                anime.SetBool(EnemyAttacks3, true);


                        }
                        else if (AttackValue == 4)
                            {

                                anime.SetBool(EnemyAttacks4UnbloackAble, true);
                            MoveTowardsThePlayer = false;

                        }

                         }

                    }



                    CoolDownTime += CoolDownSpeed * Time.deltaTime;
                    if (CoolDownTime > 1f)
                    {
                        anime.SetBool(EnemyAttacks1, false);
                        anime.SetBool(EnemyAttacks2, false);
                        anime.SetBool(EnemyAttacks3, false);
                        anime.SetBool(EnemyAttacks4UnbloackAble, false);

                        anime.SetBool(AttackingPose, true);
                    }
                    if (CoolDownTime > 5f)
                    {
                        CoolDownTime = 0f;


                    }


                    if (AttackTime > 10f)
                    {
                        MoveTowardsThePlayer = false;

                        Dis2 = Vector3.Distance(Player.transform.position, transform.position);

                        if (Dis2 < 1f)
                        {
                            anime.SetBool(EnemyAttacks1, false);
                            anime.SetBool(EnemyAttacks2, false);
                            anime.SetBool(EnemyAttacks3, false);
                            MoveBackEnemyPosition = Player.transform.position - transform.position;
                            transform.position -= MoveBackEnemyPosition * Time.deltaTime;
                            StartCoroutine(BackToCircling());
                        }

                    }
                }




            }

            IEnumerator BackToCircling()
            {
               /// BlockCollider.enabled = false;

                yield return new WaitForSeconds(2f);
                AttackTime = 0f;
                AttackValue = 0;
                CoolDownTime = 0f;
                IsCircling = true;
                EnemyStates = EnemyBrain.Circling;
                IsAttacking = false;



            }

        }

        if(EnemyStates == EnemyBrain.Circling)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), Player.GetComponent<Collider>());

            if (ER == EnemyRoles.ShutGun)
            {

                CheckAngle = 360f;

               if(ShootTimer < 5f)
                {
                    transform.RotateAround(Player.transform.position, -Vector3.up, 5f * Time.deltaTime);
                    ShootTimer += 1f * Time.deltaTime;
                    anime.SetBool(GunAnimations, false);
                    anime.SetBool(WalkRight, true);

                }


                if (ShootTimer >= 5f)
                {

                    ShootTimer = 0f;
                    anime.SetBool(GunAnimations, true);
                    anime.SetBool(WalkRight, false);

                }

               

                if (Vector3.Distance(transform.position, Player.transform.position) < 0.3f)
                {

                    Vector3 dis = transform.position - Player.transform.position;
                    transform.position += dis * 1f * Time.deltaTime;

                }

            }

        }

    }


 
 /*   public void CarryEnemy()
    {
        transform.parent = CarryPoint.transform;
        transform.position = CarryPoint.transform.position;
        transform.rotation = CarryPoint.transform.rotation;
        Player.transform.GetComponent<Animator>().SetBool("Stab", false);

        if (PM.CarryEnemyHere != null)
            return;
         if(PM.CarryEnemyHere ==null)
        {
            PM.CarryEnemyHere = transform;
        }
        PM.Iscarrying = true;
        //anime.enabled = false;

    }*/
    public void SneakBackStab()
    {
        if (Vector3.Distance(transform.position, Player.position) < SneakRange && IsPlayerInsideRange == false &&  EnemyStates != EnemyBrain.Deathbody)
        {

            Vector3 Direction = Player.position - transform.position;
            Angle = Vector3.Angle(Direction, -transform.forward);
            if (Angle < SneakAngle)
            {

                float SneakDis = Vector3.Distance(transform.position, Player.position);
                if (SneakDis < 0.3f && PA.GetComponent<PlayerAttack>().AttackType == 0)
                {
                    SneakAttackIcon.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        EnemyStates = EnemyBrain.Deathbody;
                        Agent.enabled = false;
                        Death = true;
                        GetComponent<Rigidbody>().isKinematic = true;
                        SneakAttackIcon.SetActive(false);
                        Player.transform.GetComponent<Animator>().applyRootMotion = true;
                        Player.transform.GetComponent<Animator>().SetBool("Stab", true);
                        anime.SetBool(Looking, false);
                        anime.SetBool(AttackingPose, false);
                         anime.SetBool(Faint, true);
                        anime.applyRootMotion = false;
                        StartCoroutine(EnemyHealthSneakATtack());
                    }
                 

                } else
                {


                    SneakAttackIcon.SetActive(false);

                }

            }

        }
        else
        {


            SneakAttackIcon.SetActive(false);

        }



    }

    public void DetectionFunction()
    {

        DetectionMeterInt += DetectionFillamount;
        if(Player_Notice == false)
        {
           Source.clip = PlayerNotice;
             Source.Play();
            Player_Notice = true;
        }


    }


   IEnumerator EnemyHealthSneakATtack()
    {


        yield return new WaitForSeconds(0.1f);
        EH.Health = 0f;
    }


    public void ThrowGranade()
    {
        anime.SetBool(NervousWalk, false);
       
        anime.SetBool(Running, false);
        anime.SetBool(AttackingPose, true);
        anime.SetBool(Looking, false);
        Timer += 0.8f * Time.deltaTime;

        if(Timer > 5f)
        {
            anime.SetBool(ThrowGranadeAnimations, true);
            Timer = 0f;


        }




    }


    public void RapidFire()
    {
        CheckAngle = 360f;

        if (Agent.enabled == true)
        {

        Agent.isStopped = true;
        }

        Vector3 dir = (Player.position - transform.position).normalized;

        float DistanceToTarget = Vector3.Distance(transform.position, Player.transform.position);
        if (!Physics.Raycast(transform.position + transform.up * 0.33f,dir ,DistanceToTarget, Obstacles))
        {
            if (Player.GetComponent<PlayerHealth>().IsDead == false)
            {
                anime.SetBool(Firirng, true);
                if (!AS.isPlaying)
                {

                    AS.clip = SHooting;
                    AS.Play();
                }
                GameObject G = Instantiate(SHootingEffects, ShootPoint.transform.position, Quaternion.identity);
                Destroy(G, 1f);

                Player.GetComponent<PlayerHealth>().Player_Health -= GunDamageRate;
                Player.GetComponent<AnimationsEvents>().UW.HitIntensity += 0.1f;

                GameObject B = Instantiate(BloodEffects, Player.transform.position + Player.transform.up * 0.25f, Quaternion.identity);
                Destroy(B, 1f);

            }
            else
            {
                AS.Stop();

            }



        } else
        {
            AS.clip = SHooting;
            AS.Stop();

            anime.SetBool(Firirng, false);


        }
    }

    // optional

  /*  public void FindCover()
    {
        EnemyStates = EnemyBrain.FincingCover;

        foreach (var Cover in Covers)
        
        {
          Vector3 FoundDis = Cover.transform.position - FoundPLayerPosition;

            float dis = FoundDis.sqrMagnitude;

          if(dis < NearDis)
            {

                place = Cover;
                PatrolSpeed = 5f;
                Agent.isStopped = false;
                Agent.destination = place.transform.position;
                Debug.DrawRay(transform.position + transform.up * 0.33f, FoundDis * dis, Color.green);
                break;
            }




        }


    }*/

}





