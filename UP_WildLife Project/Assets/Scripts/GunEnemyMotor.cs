using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEnemyMotor : MonoBehaviour
{
    public GameObject ShootParticle;
    public Transform ShootPoint;
    public AudioClip ShootingSound;
    AudioSource source;
    public GameObject PlayerShootEffetcs;

    public Transform Raycast;
    Animator anim;
    public bool Isdead;
    public PlayerHealth Phealth;
    public EnemHealth EH;
    public float DamageRate;

    public enum EnemeyCon {LookingAtPlayer , NotLookingAtPlayer };
    public EnemeyCon EC;
    public enum EnemyBrain { Petrol, Nervous, FoundPlayer, Shoot, Reload, HandAttack , IsFalling }
    public EnemyBrain EnemyStates;
    public LayerMask WalkingMask;
    public LayerMask ShootMask;

    public float ViewAngle;
    public float ViewRange;
    public float CanSeeAngle;
    public GameObject Player;
    public LayerMask BlockObjects;
    public Vector3 PlayerpPos;
    public Vector3 SawPosition;



    [Header("Shoot Object")]
    public float ShootTimer;
    public float MaxSHootImer;
    public float NumberOfBullet;
    public float Speed;
    public Transform LaserPoint;
    public LineRenderer LR;
    public float walkSpeed;


    public Transform[] WayPoints;
    int MOveSPot;
    public float Timer;


    [Header("Reset Timer")]
    public float ReturnToPertol;



    [Header("Animators")]
    public string Walk;
    public string LookAround;
    public string Shoot;
    public bool CanDrawLineToShoot;


    [Header("Bools")]
    public bool Canwalk;
    public bool SawPlayer;
    public bool ShootDone;
    public bool CanShoot;
    public bool HandAttack;



    [Header("Attack Animations")]
    public string Attack1;
    public string Attack2;
    public string Attack3;
    public string Attack4;
    public string TossGranade;
    public float CoolDownTime;
    public int RandNumber;
    public float HandAtackRange;




    [Header("Sneak Attack")]
    public float SneakRnage;
    public float SneakAngle;
    public float CanSeeSneakAngle;
    public string DeathAnimation;
    public string BackAttack;




    [Header("Granade Throw")]
    public bool CanSeeButCantShoot;
    public bool InRnage;
    public bool CantShoot;
    public float GranadeThrowTimer;


    private void Start()
    {


        EC = EnemeyCon.NotLookingAtPlayer;
        source = GetComponent<AudioSource>();
        EnemyStates = EnemyBrain.Petrol;
        Player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        LR = GetComponent<LineRenderer>();
        EH = GetComponent<EnemHealth>();
    }




    private void Update()
    {


        if(EH.Isdead)
        {
            LR.enabled = false;



        }
        if (!EH.Isdead)
        {

            if (!Phealth.IsDead)
            {


                if (!HandAttack)
                {
                    if (!Isdead)
                    {
                        Sneak();
                        LineOfSight();
                        if (EnemyStates == EnemyBrain.Petrol && EC == EnemeyCon.NotLookingAtPlayer)
                        {

                            MoveBetweenWaypoints();

                        }
                        if (EnemyStates == EnemyBrain.FoundPlayer)
                        {
                            anim.SetBool(Shoot, false);
                            MoveTowardsThePlayer();

                        }
                        if (EnemyStates == EnemyBrain.Shoot && SawPlayer)
                        {
                            anim.SetBool(Shoot, true);
                            anim.SetBool(Walk, false);

                            RotateTowardsPlayer();
                            EnemyShoot();



                            if (Vector3.Distance(transform.position, Player.transform.position) > ViewRange)
                            {

                                EnemyStates = EnemyBrain.Nervous;



                            }
                        }
                        if (EnemyStates == EnemyBrain.Nervous)
                        {
                            CanDrawLineToShoot = false;
                            ReturnToPertol += 2f * Time.deltaTime;

                            if (ReturnToPertol > 50f && EnemyStates == EnemyBrain.Nervous)
                            {
                                EnemyStates = EnemyBrain.Petrol;
                            }


                        }
                        if (CanDrawLineToShoot)
                        {
                            LR.enabled = true;
                            PlayerpPos = Player.transform.position;
                            PlayerpPos.y += 0.33f;
                            LR.SetPosition(0, LaserPoint.transform.position);
                            LR.SetPosition(1, PlayerpPos);
                        }
                        else
                        {

                            LR.enabled = false;
                        }
                       
                    }
                }



                if (SawPlayer)
                {

                    if (Vector3.Distance(transform.position, Player.transform.position) < HandAtackRange)
                    {
                        if (Vector3.Distance(Player.transform.position, transform.position) < 0.4f)
                        {
                            HandAttack = true;
                            EnemyStates = EnemyBrain.HandAttack;
                            LR.enabled = false;
                            IfPlayerTooCloseThenAttack();
                            Physics.IgnoreCollision(transform.GetComponent<Collider>(), Player.GetComponent<Collider>());
                            RotateTowardsPlayer();

                            if (Canwalk)
                            {
                               
                                if(Vector3.Distance(transform.position , Player.transform.position) > 0.1f)
                                {
                                    Vector3 dis = transform.position - Player.transform.position;
                                    transform.position -= dis * Time.deltaTime;

                                    IfPlayerTooCloseThenAttack();
                                }
                            }

                        }
                        else
                        {

                            HandAttack = false;
                            anim.SetBool("Attack", false);

                        }

                    }
                    else
                    {

                        HandAttack = false;
                        anim.SetBool("Attack", false);

                    }

                }
            }
            else
            {

                CanDrawLineToShoot = false;


            }

           

        }

    }



    public void LineOfSight()
        {


        ThrowGranade();

            if (Vector3.Distance(transform.position, Player.transform.position) < ViewRange)
            {

                PlayerpPos = Player.transform.position;
                Vector3 PlayerDir = (Player.transform.position - transform.position).normalized;
                ViewAngle = Vector3.Angle(transform.forward, PlayerDir);
                Physics.IgnoreCollision(transform.GetComponent<Collider>(), Player.GetComponent<Collider>());



                if (ViewAngle < CanSeeAngle)
                {

                InRnage = true;
                if (HandAttack)
                {
                    RotateTowardsPlayer();
                }


                   float IsplayerSeen = Vector3.Distance(transform.position, Player.transform.position);

                    if (!Physics.Raycast(transform.position + transform.up * 0.33f, PlayerDir, IsplayerSeen, BlockObjects))
                    {
                        RotateTowardsPlayer();
                        Debug.DrawRay(transform.position + transform.up * 0.33f, PlayerDir * IsplayerSeen, Color.red);
                        SawPlayer = true;
                        EnemyStates = EnemyBrain.FoundPlayer;

                        SawPosition = Player.transform.position;


                    }
                    else
                    {
                        SawPlayer = false;
                        Debug.DrawRay(transform.position + transform.up * 0.33f, PlayerDir * IsplayerSeen, Color.grey);
                        EnemyStates = EnemyBrain.Petrol;


                    }
                    if (!Physics.Raycast(transform.position + transform.up * 0.33f, PlayerDir, IsplayerSeen, ShootMask) && SawPlayer)
                    {
                        RotateTowardsPlayer();
                        Debug.DrawRay(transform.position + transform.up * 0.33f, PlayerDir * IsplayerSeen, Color.green);

                        CanShoot = true;
                        CanDrawLineToShoot = true;
                    CantShoot = false;

                    }
                    else
                    {
                        Debug.DrawRay(transform.position + transform.up * 0.33f, PlayerDir * IsplayerSeen, Color.gray);

                        CanDrawLineToShoot = false;
                        CanShoot = false;
                    CantShoot = true;


                }


            }
                else

                {
                   InRnage = false;
                    CanDrawLineToShoot = false;
                }
            } else
        {

            InRnage = false;
        }
        
    }




    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ViewRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SneakRnage);


        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, HandAtackRange);


    }

    public void RotateTowardsPlayer()
    {

        Vector3 dir = transform.position - Player.transform.position;
        dir.y = 0f;
        Quaternion q = Quaternion.LookRotation(-dir);

        transform.rotation = Quaternion.Slerp(transform.rotation, q, 5f * Time.deltaTime);


    }



    public void MoveBetweenWaypoints()
    {

        


            transform.position = Vector3.MoveTowards(transform.position, WayPoints[MOveSPot].position, walkSpeed * Time.deltaTime);




            anim.SetBool(Shoot, false);
            anim.SetBool(Walk, true);

            if (Vector3.Distance(transform.position, WayPoints[MOveSPot].position) < 0.1f)
            {

                anim.SetBool(Walk, false);



                if (Timer <= 0f)
                {
                    MOveSPot = Random.Range(0, WayPoints.Length);
                    Timer = 10f;

                }
                else
                {

                    Timer -= Time.deltaTime;
                }

            }
            else
            {


                Vector3 NewRot = (WayPoints[MOveSPot].transform.position - transform.position);
                NewRot.y = 0f;
                Quaternion ROt = Quaternion.LookRotation(NewRot);
                transform.rotation = Quaternion.Slerp(transform.rotation, ROt, 50f * Time.deltaTime);
            }

        
    }



    public void MoveTowardsThePlayer()
    {

    


        if (Physics.Raycast(Raycast.position, -Raycast.transform.up + Raycast.transform.forward * 0.5f, 0.2f, WalkingMask))
        {
            Canwalk = true;

            Timer = 10f;

            if (Canwalk && SawPlayer)
            {

                if (Vector3.Distance(transform.position, PlayerpPos) > 0.3f)
                {

                    
                    transform.position = Vector3.MoveTowards(transform.position, PlayerpPos, 1f * Time.deltaTime);




                } else
                {

                    if (SawPlayer && CanShoot)
                    {
                        EnemyStates = EnemyBrain.Shoot;

                    }

                }
            }
            Debug.DrawRay(Raycast.position, -Raycast.transform.up + Raycast.transform.forward * 0.5f * 0.2f, Color.red);


        }
        else
        {

            if (SawPlayer && CanShoot)
            {
                EnemyStates = EnemyBrain.Shoot;

            }

            if (Canwalk && !SawPlayer)
            {

                ReturnToPertol += Time.deltaTime;
                if (ReturnToPertol > 50f)
                {

                    EnemyStates = EnemyBrain.Petrol;
                    ReturnToPertol = 0f;
                    Vector3 NewRot = (WayPoints[MOveSPot].transform.position - transform.position);
                    NewRot.y = 0f;
                    Quaternion ROt = Quaternion.LookRotation(NewRot);
                    transform.rotation = Quaternion.Slerp(transform.rotation, ROt, 50f * Time.deltaTime);
                    Canwalk = false;

                }

            }



            Debug.DrawRay(Raycast.position, -Raycast.transform.up + Raycast.transform.forward * 0.5f * 0.2f, Color.grey);

        }


    }


    public void EnemyShoot()
    {

        if (SawPlayer)
        {
            if (ShootTimer <= MaxSHootImer)
            {
                Player.GetComponent<Animator>().SetBool("Hurt", false);

                ShootDone = true;
                ShootTimer -= Time.deltaTime;

            }

            if (ShootTimer <= 0f && ShootDone)
            {
            

                Vector3 PlayerDir = (Player.transform.position - transform.position).normalized;
                float IsplayerSeen = Vector3.Distance(transform.position, Player.transform.position);


                if (!Physics.Raycast(transform.position + transform.up * 0.33f , PlayerDir , IsplayerSeen , ShootMask))
                {
                    source.clip = ShootingSound;
                    source.Play();
                    GameObject S = Instantiate(ShootParticle, ShootPoint.transform.position, Quaternion.identity);
                    Destroy(S, 1f);
                    GameObject E = Instantiate(PlayerShootEffetcs, Player.transform.position + Player.transform.up * 0.25f, Quaternion.identity);
                    Destroy(E, 1f);
                    if (Player.GetComponent<WallClimber>().IsPlayerClimbing == false)
                    {
                    Player.GetComponent<Animator>().SetBool("Hurt", true);

                    }
                    Phealth.Player_Health -= DamageRate;

                    ShootTimer = MaxSHootImer;

                    ShootDone = false;
 
                }



            }


        }



    }

    public void IfPlayerTooCloseThenAttack()
    {

        anim.SetBool("Shoot", false);
        anim.SetBool("LookAroud", false);
        anim.SetBool("IsWalking", false);
        anim.SetBool("Attack", true);



        if (CoolDownTime == 0)
        {
            RotateTowardsPlayer();
            anim.applyRootMotion = true;
            RandNumber = Random.Range(1, 5);

        }

        CoolDownTime += 1f * Time.deltaTime;
        anim.applyRootMotion = false;


        if (RandNumber == 1)
        {
            anim.SetBool(Attack1, true);


        }
        else if(RandNumber == 2)
        {
            anim.SetBool(Attack2, true);


        }
        else if(RandNumber == 3)
        {
            anim.SetBool(Attack3, true);



        } else if(RandNumber == 4)
        {

            anim.SetBool(Attack4, true);

        }

        if (CoolDownTime > 0.1f)
        {
            anim.SetBool(Attack1, false);
            anim.SetBool(Attack2, false);
            anim.SetBool(Attack3, false);
            anim.SetBool(Attack4, false);

        }


        if (CoolDownTime >= 2f)
        {
            CoolDownTime = 0f;

        }


    }





    public void Sneak()
    {



        if(Vector3.Distance(transform.position , Player.transform.position) < SneakRnage)
        {

            Vector3 Distance = transform.position - Player.transform.position;
            SneakAngle = Vector3.Angle(-transform.forward, Distance);



            if(SneakAngle < CanSeeSneakAngle)
            {

                if(Input.GetKeyDown(KeyCode.E))
                {

                   Player.GetComponent<Animator>().SetBool(BackAttack, true);

                    StartCoroutine(Kick());
                 
                }



            }




        }


    }


    IEnumerator Kick()
    {

        yield return new WaitForSeconds(0.5f);
        GetComponent<Rigidbody>().AddForce(Vector3.forward * 100f);
        EnemyStates = EnemyBrain.IsFalling;
        anim.SetBool(DeathAnimation, true);
        GetComponent<CapsuleCollider>().radius = 0f;
        Isdead = true;
    }



    public void ThrowGranade()
    {

        if(InRnage == true && CantShoot == true)
        {
            GranadeThrowTimer += 0.5f * Time.deltaTime;

            if(GranadeThrowTimer > 3f)
            {
                anim.SetBool(TossGranade, true);
                GranadeThrowTimer = 0f;

            }



        } else

        {


            GranadeThrowTimer = 0f;
        }


    }

}
