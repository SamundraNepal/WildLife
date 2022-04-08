using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{ public CharacterController C_C;
    public float MaxfallForce;
    public float MaxFallDamage;
    public float Player_Health;
    public float CurrentHealth;
    public bool IsDead;

    public float Velocity;

    public CameraController CC;
    public PlayerMotor Pm;
    public AnimationsEvents AV;
    public WaterMovement Wm;


    public Rigidbody[] RagDollRigi;
    public Collider[] Col;

    public float healTimer;
    public float RegenerationSpeed;
    public bool StopHealing;
    public bool CanBreathSound;
    public AudioSource Source;
    public AudioClip BreathClip;
    public AudioClip DeathSound;
    public bool PlayDeathOnce;
    public GameObject DeathScreen;
     PlayerPositionChecker Gm;
    public AudioSource HeartBeat;



    private void Awake()
    {

        Gm = GameObject.FindGameObjectWithTag("GM").GetComponent<PlayerPositionChecker>();
     
    }
    private void Start()
    {
       

        if(Gm.IsplayerHere)
        {

            StartCoroutine(CheckpointCall());
        }

        PlayDeathOnce = true;
        C_C.GetComponent<CharacterController>();
        Pm.GetComponent<PlayerMotor>();
        AV.GetComponent<AnimationsEvents>();

        CurrentHealth = Player_Health;


        RagDollRigi = GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < RagDollRigi.Length; i++)
        {

            RagDollRigi[i].GetComponent<Rigidbody>().isKinematic = true;


        }

        for (int j = 0; j < Col.Length; j++)
        {

            Col[j].enabled = false;

        }
    }




    private void Update()
    {


      

        if(Player_Health < 80f)
        {

            if(!HeartBeat.isPlaying)
            {


                HeartBeat.Play();
            } 

        }
        else
        {


            if (HeartBeat.isPlaying)
            {


                HeartBeat.Stop();
            }

        }
        FallDamage();

        Player_Health = Mathf.Clamp(Player_Health, 0f, 100f);


        if (Player_Health < 1f)
        {
           
            AV.UW.HitIntensity = 1f;

            StartCoroutine(ReloadThegame());

            IsDead = true;
            if(PlayDeathOnce)
            {
            Source.clip = DeathSound;
            Source.Play();
                PlayDeathOnce = false;
            }
            GetComponent<CharacterController>().enabled = false;
            GetComponent<Animator>().enabled = false;
            CC.CanRotate = false;
            EnabledRagDoll();
        }
     

        PlayerHeal();
     
    }

    public void EnabledRagDoll()
    {
        for (int i = 0; i < RagDollRigi.Length; i++)
        {

            RagDollRigi[i].GetComponent<Rigidbody>().isKinematic = false;


        }

        for (int j = 0; j < Col.Length; j++)
        {

            Col[j].enabled = true;

        }


    }


    public void PlayerHeal()
    {
        if(Player_Health >= 100f)
        {
            healTimer = 0f;
        }   

        if(Player_Health < 100f && !IsDead)
        {
            if(Pm.IsPHurt == false)
            {
                if(healTimer <=20f)
                {
                healTimer += 0.01f;

                }

                if(healTimer >= 20f)
                {
                    CanBreathSound = true;
                    StopHealing = false;
                    if(StopHealing == false)
                    {
                    StartCoroutine(StartToHeal());

                    }


                }

            } else
            {

                healTimer = 0f;
            }


        }


    }



    IEnumerator StartToHeal()
    {

        yield return new WaitForSeconds(5f);
        if(Player_Health < 100f)
        {
        Player_Health += RegenerationSpeed;

        }

        if(Player_Health >= 100f)
        {
            if(CanBreathSound)
            {

                Source.clip = BreathClip;
                Source.Play();
                CanBreathSound = false;

            }
            AV.UW.HitIntensity = 0f;
            StopHealing = true;

        }
    }



    public void FallDamage()
    {

        if(C_C.isGrounded == false && Wm.IsunderWater==false && GetComponent<WallClimber>().IsPlayerClimbing == false)
        {
         Velocity = Mathf.Abs( C_C.velocity.y);

        }

        if(C_C.isGrounded)
        {

            if (Velocity > MaxfallForce)
            {
                GetComponent<Animator>().applyRootMotion = true;
                float damage = Velocity * MaxFallDamage;
                Velocity = 0f;
                Player_Health -= damage;
                GetComponent<Animator>().SetBool("HardItGround", true);
                Pm.IsPHurt = true;


            }
        }

    }   
    


   IEnumerator ReloadThegame()
    {

        yield return new WaitForSeconds(2f);

        DeathScreen.SetActive(true);


        StartCoroutine(CallDeath());
    }

    IEnumerator CallDeath()
    {


        yield return new WaitForSeconds(2f);
       
        SceneManager.LoadScene("Level");

       
          
        
    }




    IEnumerator CheckpointCall()
    {
        yield return new WaitForSeconds(1f);

        DeathScreen.SetActive(true);

        CC.enabled = false;
        Pm.enabled = false;

        transform.position = Gm.PlayerCheckPointPos;
        

        yield return new WaitForSeconds(8f);

        DeathScreen.SetActive(false);

        CC.enabled = true;
        Pm.enabled = true;
      //  Gm.IsplayerHere = false;
        

    }
}
