using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Remoting.Messaging;
using UnityEditor;
using UnityEngine;
public class PlayerMotor : MonoBehaviour
{
    public WallClimber WC;
    public WaterMovement Wm;
    public enum PlayerStates { NotGrabbed , grabbed }
    public PlayerStates Ps;

    public bool IsCrouching;
    public bool IsUnMounted;
    public bool Iscarrying;
    public float CanUnChrouch;
    public bool IsPHurt;

    public Transform CarryEnemyHere;
    

    [Header("Player Movement Values")]

    public float WalkSPeed = 1f;
    public float RunSpeed = 6f;

    public float CrouchWalkSpeed;
    public float CrouchRunSpeed; 

    public float TrunSmoothTime = 0.2f;
    public float SpeedSmoothTime;
    float SPeedSmoothVelocity;
    float TrunSmoothVelocity;
    float CurrentSpeed;
    Transform CameraT;
    public  CharacterController Controller;

    public Animator Anim;
    public bool Canmove;
    public bool IsAttacking;
    public bool IsBlocking;

    [Header("Player Physic Values")]
    public float Gravity = -12;
    public float Jumpheight = 1f;
    [Range(0, 1)]
    public float airControlPercent;
   public float VelocityY;


  





    private void Start()
    {
        Wm.GetComponent<WaterMovement>();
        WC.GetComponent<WallClimber>();
        Ps = PlayerStates.NotGrabbed;
        CameraT = Camera.main.transform;
        Controller = GetComponent<CharacterController>();
        IsCrouching = false;
    }




    private void Update()
    {
        if(WC.IsPlayerClimbing)
        {

            GetComponent<Animator>().applyRootMotion = false;
        }

        if (WC.IsPlayerClimbing == false)
        {



            if (Input.GetKeyDown(KeyCode.Space) && Canmove && GetComponent<PlayerHealth>().IsDead == false)
            {
                if (Controller.slopeLimit <= 45)
                {

                    Jump();
                }
            }


        }



    }
    private void FixedUpdate()
    {






            if (IsUnMounted)
            {
                UnmountJump();
                //  IsUnMounted = false;

            }

            if (Canmove == true && !IsCrouching && GetComponent<PlayerHealth>().IsDead == false && !IsAttacking && !IsPHurt && Ps == PlayerStates.NotGrabbed && !IsBlocking && WC.IsPlayerClimbing == false && Wm.IsunderWater == false)
            {
                Anim.SetBool("CarryBoolCheck", false);
                Anim.SetBool("Crouch", false);


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
                Controller.Move(velocity * Time.deltaTime);
                CurrentSpeed = new Vector2(Controller.velocity.x, Controller.velocity.z).magnitude;

                if (Controller.isGrounded)
                {

                    VelocityY = 0f;
                    Anim.SetBool("Jump", false);

                }
                // playing animation based on speed.
                float AnimationSpeedPercent = ((Running) ? CurrentSpeed / RunSpeed : CurrentSpeed / WalkSPeed * .5f);
                Anim.SetFloat("Speed", AnimationSpeedPercent, SpeedSmoothTime, Time.deltaTime); //Playing animation along with smoothing the value.

            }

            if (!IsCrouching && Controller.isGrounded)
            {

                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    IsCrouching = true;



                }

            }


            if (IsCrouching)
            {
                CrouchWalking();

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {

                    Anim.SetBool("Crouch", false);
                    IsCrouching = false;
                }

            }

        
    }



    public void Jump()
    {


        if (Controller.isGrounded)
        {
            Anim.SetBool("Jump", true);
          
            float JumpVelocity = Mathf.Sqrt(-2 * Gravity * Jumpheight);
            VelocityY = JumpVelocity;

        }

    


    }



    float GetModifiedSmoothTime(float smoothTime)
    {


        if(Controller.isGrounded)
        {
            return smoothTime;
        }

        if(airControlPercent ==0)
        {
            return float.MaxValue;
        }    

        return smoothTime / airControlPercent;
    }



    void UnmountJump()
    {
        
        this.transform.Translate(Vector3.forward * Time.deltaTime * 13f);
        this.transform.Translate(Vector3.up * Time.deltaTime * 5f);
    }



   


    public void CrouchWalking()
    {

        Anim.SetBool("Crouch", true);

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
        float TargetSpeed = ((Running) ? CrouchRunSpeed : CrouchWalkSpeed) * inputdir.magnitude; // checking if the the player pressed run key or not if not then run speed is equal to walk speed.\

        //Smoothing the speed value.
        CurrentSpeed = Mathf.SmoothDamp(CurrentSpeed, TargetSpeed, ref SPeedSmoothVelocity, GetModifiedSmoothTime(SpeedSmoothTime));


        VelocityY += Time.deltaTime * Gravity;

        Vector3 velocity = transform.forward * CurrentSpeed + Vector3.up * VelocityY;// moving player
        Controller.Move(velocity * Time.deltaTime);
        CurrentSpeed = new Vector2(Controller.velocity.x, Controller.velocity.z).magnitude;

        // playing animation based on speed.
        float AnimationSpeedPercent = ((Running) ? CurrentSpeed / CrouchRunSpeed : CurrentSpeed / CrouchWalkSpeed * .5f);
        Anim.SetFloat("CrouchMovement", AnimationSpeedPercent, SpeedSmoothTime, Time.deltaTime); //Playing animation along with smoothing the value.





    }


  




}
