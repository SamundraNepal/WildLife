using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;
public class PlayerAttack : MonoBehaviour
{
    public int AttackType;
    public string Enemys;
    public string GunEnemy;
    public string Attack1;
    public string Attack2;
    public string Attack3;
    public string Attack4;
    public string Block;
    public float Range;



    public int RandomAttackValue;


    public Animator Anim;


    public GameObject[] Enemy;
    public GameObject closeEnemy;
    public GameObject[] GE;
    public GameObject ClosegunEnemy;
    public PlayerHealth Ph;
    public PlayerMotor Pm;
    public float MinimumDistance;
    public float GunEnemyMinDistance;
    public float dis;




    [Header("Shooting Mec")]
    public Transform Camera;
    public GameObject Gun;
    public GameObject CrossHair;
    public GameObject ShoootingEffects;
    public Transform ShootPoint;
    public Transform CurrentPoint;
    public Transform Holder;
    public Transform IntialPoint;
    public Transform NewLerpPoint;
    public Transform ADSMOD;
    public int NumberOfBullet;
    public float RealBullet;
    public float MaxShootRange;
    public LayerMask AvoidShootingPlaces;
    public LayerMask Avoid;
    public GameObject Bullets;
    public bool CanShoot;
    public AudioClip ShootingSound;
    public AudioSource Source;
    public float ShootingRangeSound;
    public TextMeshProUGUI BulltesCount;
    public GameObject GUNUI;
    public GameObject HealthDamage;
    public GameObject Sparks;
    public float GunDamageToBody;
    public float GunDamageToHead;
    public float HelicopterDamageRate;
    public bool ShootReady;
    public GameObject ArmsUI;



    [Header("Recoil")]
    public Vector3 UpRecoil;
     Vector3 Originalpos;
    public Vector3 HeardSoundPos;

    public bool HasKeyWithPlayer;
    public GameObject KeyUI;

    private void Start()
    {
        BulltesCount.text = NumberOfBullet.ToString();
        Anim = GetComponent<Animator>();
        Ph = GetComponent<PlayerHealth>();
        Pm = GetComponent<PlayerMotor>();
    }


    private void FixedUpdate()
    {

      
        if(HasKeyWithPlayer)
        {

            KeyUI.SetActive(true);
        }
        else
        {

            KeyUI.SetActive(false);

        }


        if (Physics.Raycast(Camera.transform.position, transform.forward, 1f, Avoid))
        {
            ShootReady = false;
            

        }
        else
        {
            
            ShootReady = true;
        }



        if (AttackType == 1)
        {





            BulltesCount.text = NumberOfBullet.ToString();


            if (Input.GetKey(KeyCode.Mouse1))
            {
                CurrentPoint.transform.position = Vector3.Lerp(CurrentPoint.transform.position, ADSMOD.transform.position, 2f * Time.deltaTime);


            }
            else
            {
                CurrentPoint.transform.position = Vector3.Lerp(CurrentPoint.transform.position, NewLerpPoint.transform.position, 1f * Time.deltaTime);


            }

            if (GetComponent<WallClimber>().IsPlayerClimbing == false && Ph.IsDead == false && Pm.IsAttacking == false)
            {
                ArmsUI.SetActive(false);

                GUNUI.SetActive(true);
                CrossHair.SetActive(true);
                CurrentPoint.transform.position = Vector3.Lerp(CurrentPoint.transform.position, NewLerpPoint.transform.position, 1f * Time.deltaTime);
                Gun.SetActive(true);
                Anim.SetLayerWeight(Anim.GetLayerIndex("Gun Layer"), 1f);
                //  Anim.SetBool("TakeRfileOut", true);

                if (ShootReady)
                {


                    CheckShoot();

                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        HeardSoundPos = transform.position;

                        if (NumberOfBullet > 0f)
                        {

                            Vector3 dir = Holder.transform.position - transform.position;

                            dir.y = 0f;
                            Quaternion rot = Quaternion.LookRotation(dir);
                            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 100f * Time.deltaTime);


                            Enemy = GameObject.FindGameObjectsWithTag(Enemys);
                            GE = GameObject.FindGameObjectsWithTag(GunEnemy);

                            CallEnemyies();
                            AddRecoil();
                            GameObject Effects = Instantiate(ShoootingEffects, ShootPoint.transform.position, Quaternion.identity);
                            Destroy(Effects, 1.5f);

                            if (!Source.isPlaying)
                            {

                                Source.clip = ShootingSound;
                                Source.Play();
                            }
                            CanShoot = true;


                            RealBullet -= 1f;


                            if (RealBullet < 1f)
                            {
                                if (NumberOfBullet > 1)
                                {
                                    RealBullet = 5f;
                                }

                                NumberOfBullet -= 1;
                            }
                        }

                    }
                    else
                    {
                        if (Source.isPlaying)
                        {


                            Source.Stop();
                        }

                        CanShoot = false;
                        StopRecoil();
                    }


                }

            }
            else
            {

                AttackType = 0;
            }
        }
    }


    private void Update()
    {


        NumberOfBullet = Mathf.Clamp(NumberOfBullet, 0, 50);




        AttackType = Mathf.Clamp(AttackType, 0, 1);

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {

            AttackType += 1;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // back
        {
            AttackType -= 1;
        }

        if (AttackType == 0)
        {
            ArmsUI.SetActive(true);
            CrossHair.SetActive(false);
            GUNUI.SetActive(false);

            CurrentPoint.transform.position = Vector3.Lerp(CurrentPoint.transform.position, IntialPoint.transform.position, 1f * Time.deltaTime);

            Gun.SetActive(false);
            Anim.SetLayerWeight(Anim.GetLayerIndex("Gun Layer"), 0f);
            //Anim.SetBool("TakeRfileOut", false);


            if (Input.GetKeyDown(KeyCode.Mouse0) && GetComponent<WallClimber>().IsPlayerClimbing == false && Ph.IsDead == false && Pm.IsAttacking == false && GetComponent<CharacterController>().isGrounded && Pm.Ps!=PlayerMotor.PlayerStates.grabbed)

            {

                Pm.IsAttacking = true;
                RandomAttackValue = Random.Range(1, 5);

                if (RandomAttackValue == 1)
                {

                    Anim.applyRootMotion = true;
                    Anim.SetBool(Attack1, true);


                }
                else if (RandomAttackValue == 2)
                {

                    Anim.applyRootMotion = true;

                    Anim.SetBool(Attack2, true);



                }
                else if (RandomAttackValue == 3)
                {

                    Anim.applyRootMotion = true;

                    Anim.SetBool(Attack3, true);


                }
                else if (RandomAttackValue == 4)
                {
                    Anim.applyRootMotion = true;


                    Anim.SetBool(Attack4, true);

                }
                SnapAttackToEnemyPos();
            }
            else

            {
                Anim.SetBool(Attack4, false);
                Anim.SetBool(Attack1, false);
                Anim.SetBool(Attack2, false);
                Anim.SetBool(Attack3, false);
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartCoroutine(Attack());
            }

            if (Input.GetKeyDown(KeyCode.Mouse1) && GetComponent<WallClimber>().Ishitting == false && Ph.IsDead == false && GetComponent<CharacterController>().isGrounded)
            {
                Pm.IsBlocking = true;

                Anim.applyRootMotion = true;
                Anim.SetBool(Block, true);
                StartCoroutine(BlockDisabled());
                SnapAttackToEnemyPos();

            }

        }


    }

    IEnumerator BlockDisabled()
    {
        yield return new WaitForSeconds(0.5f);
        Anim.SetBool(Block, false);



    }

    IEnumerator Attack()
    {

        yield return new WaitForSeconds(1.5f);
        Pm.IsAttacking = false;

    }

    public void SnapAttackToEnemyPos()
    {
        if (closeEnemy != null && closeEnemy.GetComponent<EnemHealth>().Isdead == false)
        {
            Vector3 dir_ = closeEnemy.transform.position - transform.position;
            
            dir_.y = 0f;
            Quaternion Lookrot_ = Quaternion.LookRotation(dir_);
            transform.rotation = Quaternion.Slerp(transform.rotation, Lookrot_, 100f * Time.deltaTime);
        }




        foreach (var E in Enemy)

        {

            if (Vector3.Distance(transform.position, E.transform.position) < Range)
            {



                dis = Vector3.Distance(E.transform.position, transform.position);

                if (dis <= MinimumDistance)
                {

                    //   MinimumDistance = dis;
                    closeEnemy = E;
                    break;

                }


            }
            else
            {
                closeEnemy = null;

            }

        }







        foreach (var Gun in GE)
        {

            if (Vector3.Distance(transform.position, Gun.transform.position) < Range)
            {


                float dis = Vector3.Distance(Gun.transform.position, transform.position);
                if (dis <= GunEnemyMinDistance)
                {
                    GunEnemyMinDistance = dis;
                    // GunEnemyMinDistance = dis;
                    ClosegunEnemy = Gun;


                }

            }
            else
            {


                ClosegunEnemy = null;
                GunEnemyMinDistance = 0.9f;
            }

        }


        if (ClosegunEnemy != null)
        {
            Vector3 dir = ClosegunEnemy.transform.position - transform.position;
            dir.y = 0f;
            Quaternion Lookrot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, Lookrot, 100f * Time.deltaTime);
        }





    }


    private void OnDrawGizmosSelected()
    {


        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + transform.up * 0.28f, Range);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ShootingRangeSound);

    }




    public void CheckShoot()
    {

       



            RaycastHit hit;


            if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hit, MaxShootRange, AvoidShootingPlaces))
            {

                if (hit.collider.tag == GunEnemy)
                {

                    CrossHair.GetComponent<RawImage>().color = Color.yellow;
                    EnemHealth E = hit.transform.GetComponent<EnemHealth>();

                    if (CanShoot)
                    {
                        if (E != null)
                        {


                            GameObject B = Instantiate(HealthDamage, hit.point, Quaternion.identity);
                            Destroy(B, 1f);
                            E.DieTime = 0.1f;

                            E.Health -= GunDamageToBody;


                        }

                    }





                }


            if (hit.collider.tag == "Helicopter")
            {

                CrossHair.GetComponent<RawImage>().color = Color.yellow;
                HelicopterHealth E = hit.transform.GetComponent<HelicopterHealth>();

                if (CanShoot)
                {
                    if (E != null)
                    {


                        GameObject B = Instantiate(Sparks, hit.point, Quaternion.identity);
                        Destroy(B, 1f);
                        E.Health -= HelicopterDamageRate;
                       

                    }

                }





            }




            if (hit.collider.tag == "BlockCollider")
                {

                    CrossHair.GetComponent<RawImage>().color = Color.yellow;
                    EnemHealth E = hit.transform.GetComponentInParent<EnemHealth>();

                    if (CanShoot)
                    {
                        if (E != null)
                        {

                        GameObject B = Instantiate(HealthDamage, hit.point, Quaternion.identity);
                            Destroy(B, 1f);
                            E.DieTime = 0.1f;

                            E.Health -= GunDamageToBody;


                        }

                    }


                }


                if (hit.collider.tag == Enemys)
                {
                    CrossHair.GetComponent<RawImage>().color = Color.yellow;
                    EnemHealth E = hit.transform.GetComponent<EnemHealth>();

                    if (CanShoot)
                    {
                        if (E != null)
                        {

                        GameObject B = Instantiate(HealthDamage, hit.point, Quaternion.identity);
                            Destroy(B, 1f);
                            E.DieTime = 0.1f;

                            E.Health -= GunDamageToBody;


                        }

                    }

                }
                else if (hit.collider.tag == "EnemyHead")
                {
                    CrossHair.GetComponent<RawImage>().color = Color.red;


                    if (CanShoot)
                    {
                        EnemHealth E = hit.transform.GetComponentInParent<EnemHealth>();

                        if (E != null)
                        {

                        GameObject B = Instantiate(HealthDamage, hit.point, Quaternion.identity);
                            Destroy(B, 1f);
                            E.DieTime = 0.1f;
                            E.Health -= GunDamageToHead;



                        }


                    }



                }
                else
                {


                    CrossHair.GetComponent<RawImage>().color = Color.white;


                }






            }
            else

            {


                CrossHair.GetComponent<RawImage>().color = Color.white;

            }



        


    }




    public void AddRecoil()
    {




        CurrentPoint.localEulerAngles += UpRecoil;

    }

    public void StopRecoil()
    {

        CurrentPoint.localEulerAngles = Originalpos;




    }

    public void CallEnemyies()
    {
        foreach (var E in Enemy)

        {

            if (Vector3.Distance(transform.position, E.transform.position) < ShootingRangeSound)
            {
                if (E.GetComponent<EnemyMotor>().gameObject.activeSelf == true)
                {
                    if (E.GetComponent<EnemyMotor>().CanSeePlayer == false)
                    {

                        E.GetComponent<EnemyMotor>().EnemyStates = EnemyMotor.EnemyBrain.GunShotHeard;

                        if (E.GetComponent<EnemyMotor>().Agent.pathPending && E.GetComponent<EnemyMotor>().EnemyStates != EnemyMotor.EnemyBrain.GunShotHeard)
                        {

                            E.GetComponent<EnemyMotor>().Agent.ResetPath();
                        }


                        if (E.GetComponent<EnemyMotor>().EnemyStates == EnemyMotor.EnemyBrain.GunShotHeard)
                        {
                            if (E.GetComponent<EnemyMotor>().EP == EnemyMotor.EnemyPetrol.StandingEnemy)
                            {

                                E.GetComponent<Animator>().SetBool(E.GetComponent<EnemyMotor>().Idle, false);

                            }

                            E.GetComponent<Animator>().SetBool(E.GetComponent<EnemyMotor>().Looking, false);
                            E.GetComponent<Animator>().SetBool(E.GetComponent<EnemyMotor>().NervousWalk, false);


                            Vector3 dir = E.transform.position - transform.position;
                            dir.y = 0f;

                            E.transform.rotation = Quaternion.Slerp(E.transform.rotation, Quaternion.LookRotation(-dir), 50f * Time.deltaTime);
                            E.GetComponent<NavMeshAgent>().speed = E.GetComponent<EnemyMotor>().ChaseSpeed;

                            if (E.GetComponent<EnemHealth>().Isdead == true && E.GetComponent<NavMeshAgent>().enabled == true)
                            {

                                E.GetComponent<EnemyMotor>().Agent.isStopped = true;
                            }
                            else if ((E.GetComponent<EnemHealth>().Isdead == false && E.GetComponent<NavMeshAgent>().enabled == true))
                            {
                                E.GetComponent<EnemyMotor>().Agent.isStopped = false;

                            }
                            if (E.GetComponent<EnemyMotor>().CanSeePlayer == false && E.GetComponent<NavMeshAgent>().enabled == true)
                            {


                                // E.GetComponent<EnemyMotor>().Agent.ResetPath();
                                E.GetComponent<EnemyMotor>().Agent.destination = HeardSoundPos;
                                E.GetComponent<EnemyMotor>().anime.SetBool(E.GetComponent<EnemyMotor>().Running, true);
                            }
                        }

                    }
                }
            }
      
        }





        foreach (var GUnE in GE)
        {
            if (Vector3.Distance(transform.position, GUnE.transform.position) < ShootingRangeSound)
            {

                if (GUnE.GetComponent<GunEnemyMotor>().gameObject.activeSelf == true)
                {


                    GUnE.GetComponent<GunEnemyMotor>().EC = GunEnemyMotor.EnemeyCon.LookingAtPlayer;
                    Vector3 dir = transform.position - GUnE.transform.position;
                    dir.y = 0f;
                    GUnE.transform.rotation = Quaternion.Slerp(GUnE.transform.rotation, Quaternion.LookRotation(dir), 2f * Time.deltaTime);

                    GUnE.GetComponent<GunEnemyMotor>().ViewRange = 20f;

                    GUnE.GetComponent<GunEnemyMotor>().ViewAngle = 360f;

                }
            }



        }
    }




}