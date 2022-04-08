using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsEvents : MonoBehaviour
{
    public AudioSource PlayerPunchingSoundEffect;
    public AudioClip[] PunchingClips;


    public AudioClip[] PlayerHurt;
    public AudioClip[] Walking;
    public AudioClip[] FastBreathingWhileRunning;

    public AudioClip[] Fall_Damage;
    public AudioClip Jump;
    public AudioSource Source;
    public UnderWater UW;
    public float HitIntensity;




    public AudioClip Block;
    public AudioSource WalkingSource;
    Animator anime;
    public bool MoveLedgeClimb;
    public bool MoveLedegDrop;
    public string Kick;

    




    public Collider RightHandCol;
    public Collider LeftHandCol;
    public Collider BlockCollider;
    public PlayerMotor Pm;



    [Header("Grab Sneak Attack")]
    public Collider col;
    public GrabEnemy Enemy;
    public AudioClip GrabENemySound;
    public AudioClip SlamEnemy;


    [Header("Wall climb")]
    public AudioSource WallCimb;
    public AudioClip[] WallClimbAudio;

    private void Start()
    {
        Enemy = GetComponentInChildren<GrabEnemy>();
        PlayerPunchingSoundEffect = GetComponent<AudioSource>();
        anime = GetComponent<Animator>();
        Pm = GetComponent<PlayerMotor>();
        RightHandCol.enabled = false;
        LeftHandCol.enabled = false;
        col.enabled = false;


    }





    public void LedgeClimb()
    {
    MoveLedgeClimb = true;

    }


    public void LedgeClimbDown()
    {

        MoveLedegDrop = true;


    }


    public void KickAnimation()
    {

        anime.SetBool(Kick, false);


    }

    public void DisableRootMotion()
    {

        anime.applyRootMotion = false;

    }



   
    public void LeftHandAttack()
    {
        LeftHandCol.enabled = true;
        PlayerPunchingSoundEffect.clip = PunchingClips[Random.Range(0, PunchingClips.Length)];
        PlayerPunchingSoundEffect.Play();


    }


    public void RightHandAttack()
    {
        PlayerPunchingSoundEffect.clip = PunchingClips[Random.Range(0, PunchingClips.Length)];
        PlayerPunchingSoundEffect.Play();
        RightHandCol.enabled = true;

    }



    public void DisbledAttacks()
    {
        RightHandCol.enabled = false;
        LeftHandCol.enabled = false;

        BlockCollider.enabled = false;

        Pm.IsBlocking = false;


    }



    public void PlayerBlocking()
    {
        PlayerPunchingSoundEffect.clip = Block;
        PlayerPunchingSoundEffect.Play();
        RightHandCol.enabled = false;
        LeftHandCol.enabled = false;
        BlockCollider.enabled = true;


    }


    


    public void PlayerhitSound()
    {
        anime.SetBool("Hurt", false);

        Pm.IsPHurt = true;

        PlayerPunchingSoundEffect.clip = PlayerHurt[Random.Range(0, PlayerHurt.Length)];
        PlayerPunchingSoundEffect.Play();
        HitIntensity = Mathf.Clamp(HitIntensity, 0f, 0.6f);

        UW.HitIntensity += HitIntensity;


    }


 

   public void GrabFromBehind()
    {
        col.enabled = true;
        PlayerPunchingSoundEffect.clip = GrabENemySound;
        PlayerPunchingSoundEffect.Play();

    }




    public void dropGrapEnemy()
    {
        PlayerPunchingSoundEffect.clip = SlamEnemy;
        PlayerPunchingSoundEffect.Play();

        anime.SetBool("Stab", false);

        col.enabled = false;
        if(Enemy.GrabbedEnemy.transform!=null)
        {
        Enemy.GrabbedEnemy.transform.parent = null;

        }
    }


    public void Player_Hurt()
    {
        Pm.IsPHurt = false;

    }


   public void PlayerWalking()
    {

        WalkingSource.clip = Walking[Random.Range(0, Walking.Length)];
        WalkingSource.Play();

    }


    public void RunningSoundClip()
    {
        Source.clip = FastBreathingWhileRunning[Random.Range(0 , FastBreathingWhileRunning.Length)];
        Source.Play();

    }

    public void FallDamage()
    {


        UW.HitIntensity += 0.1f;
        anime.SetBool("HardItGround", false);

        PlayerPunchingSoundEffect.clip = Fall_Damage[Random.Range(0, Fall_Damage.Length)];
        PlayerPunchingSoundEffect.Play();


    }


    public void JumpUp()
    {
        PlayerPunchingSoundEffect.clip = Jump;
        PlayerPunchingSoundEffect.Play();



    }


    public void Wallmovement()
    {

        WallCimb.clip = WallClimbAudio[Random.Range(0, WallClimbAudio.Length)];
        WallCimb.Play();

    }




}

