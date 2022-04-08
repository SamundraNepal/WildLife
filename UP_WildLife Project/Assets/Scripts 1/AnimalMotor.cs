using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalMotor : MonoBehaviour
{


    [Header("Animal Move")]
    public float WalkSPeed = 1f;
    public float RunSpeed = 6f;
    public float TrunSmoothTime = 0.2f;
    public float SpeedSmoothTime;
    float SPeedSmoothVelocity;
    float TrunSmoothVelocity;
    float CurrentSpeed;
    Transform CameraT;
    public float Speed;


    [Header("Scripts")]

    PlayerMotor M_Motor;
    CameraController H_older;
    CharacterController contorl;



    [Header("Animal Range")]
    public float Range1;
    public float Range2;
    public float MountingRange;
    public Transform MountingPoint;
    GameObject Player;
    bool ECanMove;
    bool MoveToplayer;
    NavMeshAgent Agent;
    Animator Anim;



    [Header("Elephent Physic Values")]
    public float Gravity = -12;
    public float Jumpheight = 1f;
    [Range(0, 1)]
    public float airControlPercent;
    float VelocityY;

    private void Start()
    {

        Anim = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
        ECanMove = false;
        CameraT = Camera.main.transform;
        MoveToplayer = true;
        M_Motor = GameObject.Find("Player").GetComponent<PlayerMotor>();
        H_older = GameObject.Find("Holder").GetComponent<CameraController>();
        contorl = GetComponent<CharacterController>();
        contorl.enabled = false;

    }




    private void Update()
    {

        VelocityY += Time.deltaTime * Gravity;


        if (MoveToplayer)
        {



            if (Vector3.Distance(this.transform.position, Player.transform.position) >= Range1)
            {
                Agent.SetDestination(Player.transform.position);
                Anim.SetBool("IsWalking", true);

            }


            if (Vector3.Distance(this.transform.position, Player.transform.position) <= Range2)
            {
                Anim.SetBool("IsWalking", false);
            }

        }

       // PlayerSitting();
        Unmount();


        if (ECanMove == true)
        {
            AnimalMove();

        }

    }


/*    public void PlayerSitting()
    {



        if (IsHookHit.IsElephentHooked)
        {
            MoveToplayer = false;
            Anim.SetBool("IsWalking", false);
            Agent.enabled = false;
            ECanMove = true;
            Player.transform.position = MountingPoint.transform.position;
            Player.transform.rotation = MountingPoint.transform.rotation;
            Player.transform.parent = this.transform;
            P_hook.enabled = false;
            M_Motor.Anim.SetBool("IsMounting", true);
            M_Motor.enabled = false;
            contorl.enabled = true;
            H_older.DistnaceFromTar = 1f;
            
        }


    }*/

    void AnimalMove()


    {


        // moving input key

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Vector2 inputdir = input.normalized;




        if (inputdir != Vector2.zero)
        {
            // calculating the rotation of the player.
            float TargetRotation = Mathf.Atan2(inputdir.x, inputdir.y) * Mathf.Rad2Deg + CameraT.eulerAngles.y;

            // Making the movement smooth by smoothdampangle function and feeding it with ref : self Modified Value.
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, TargetRotation, ref TrunSmoothVelocity, GetModifiedSmoothTime(TrunSmoothTime));

        }


        bool Running = Input.GetKey(KeyCode.LeftShift);// run key
        float TargetSpeed = ((Running) ? RunSpeed : WalkSPeed) * inputdir.magnitude; // checking if the the player pressed run key or not if not then run speed is equal to walk speed.\

        //Smoothing the speed value.
        CurrentSpeed = Mathf.SmoothDamp(CurrentSpeed, TargetSpeed, ref SPeedSmoothVelocity, GetModifiedSmoothTime(SpeedSmoothTime));


        VelocityY += Time.deltaTime * Gravity;
        Vector3 velocity = transform.forward * CurrentSpeed + Vector3.up * VelocityY;// moving player
        contorl.Move(velocity * Time.deltaTime);
        CurrentSpeed = new Vector2(contorl.velocity.x, contorl.velocity.z).magnitude;

        if (contorl.isGrounded)
        {

            VelocityY = 0f;
        }
        // playing animation based on speed.
        float AnimationSpeedPercent = ((Running) ? CurrentSpeed / RunSpeed : CurrentSpeed / WalkSPeed * .5f);
        Anim.SetFloat("Speed", AnimationSpeedPercent, SpeedSmoothTime, Time.deltaTime); //Playing animation along with smoothing the value.


    }



    void Unmount()
    {


        if (Input.GetKey(KeyCode.E))
        {
            M_Motor.Anim.SetBool("IsMounting", false);
            Anim.SetBool("IsWalking", false);

          //  IsHookHit.IsElephentHooked = false;
            StartCoroutine("Player_Unmount");

        }


    }




    float GetModifiedSmoothTime(float smoothTime)
    {


        if (contorl.isGrounded)
        {
            return smoothTime;
        }

        if (airControlPercent == 0)
        {
            return float.MaxValue;
        }

        return smoothTime / airControlPercent;
    }

    IEnumerator Player_Unmount()

    {
        yield return new WaitForSeconds(1f);

        MoveToplayer = true;
        Anim.SetBool("IsWalking", false);
        Agent.enabled = true;
        ECanMove = false;
        Player.transform.parent = null;
     //   P_hook.enabled = true;
        M_Motor.enabled = true;
        contorl.enabled = false;
        H_older.DistnaceFromTar = 0.5f;


    }



}

