using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovement : MonoBehaviour
{

    public Animator Anime;


    public WallClimber WC;
    public PlayerMotor Motor;
    public PlayerHealth Ph;

    public LayerMask Water, Ground;
    public float Distnace;


    public bool IsSInking;

    public bool CanMove;

    public float YValue;

    public float WallOffset;

    public float GroundCheck;

    public bool ShallowWater;
    public bool IsunderWater;
    public bool ShoreCheck;
    public bool DoingGroundCheck;









    public float WalkSPeed = 1f;
    public float RunSpeed = 6f;
    public float TrunSmoothTime = 0.2f;
    public float SpeedSmoothTime;
    float SPeedSmoothVelocity;
    float TrunSmoothVelocity;
    float CurrentSpeed;
    Transform CameraT;
    public CharacterController Controller;


    [Header("Player Physic Values")]
    public float Gravity = -12;
    public float Jumpheight = 1f;
    [Range(0, 1)]
    public float airControlPercent;
    public float VelocityY;



    public Vector3 WaterLevelPos;
    public Vector3 UnderWaterGroundLevel;



    private void Start()
    {
        WC = GetComponent<WallClimber>();
        Ph = GetComponent<PlayerHealth>();
        CameraT = Camera.main.transform;
        Controller = GetComponent<CharacterController>();
    }



    private void FixedUpdate()
    {
        if (!Ph.IsDead && !WC.IsPlayerClimbing)
        {


            if (CanMove)
            {
                Movement();
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    Anime.SetBool("Swimming", true);
                    Vector3 Lerping = Vector3.Lerp(transform.position, WaterLevelPos, 2f * Time.deltaTime);
                    transform.position = Lerping;
                    Gravity = 0f;
                    DoingGroundCheck = false;
                }
                else

                {
                    DoingGroundCheck = true;
                }



                // Submerge

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    Anime.SetBool("Swimming", true);
                    Gravity -= 0.1f * Time.deltaTime;

                }

            }


            SurfaceCheck();


            NearToGroundCheck();
            OnShoreCheck();

            if (IsunderWater == false && ShallowWater == true && ShoreCheck == true)
            {
                Anime.SetBool("Swimming", false);
                Motor.enabled = true;
                CanMove = false;


            }



            // DeSubmarge
        }
    }


    public void SurfaceCheck()
    {

        RaycastHit hit;

        if (Physics.Raycast(transform.position + transform.up * 0.33f, transform.up, out hit, Distnace,Water))
        {

            Debug.DrawRay(transform.position + transform.up * 0.33f, transform.up * Distnace, Color.cyan);

         
              
                  Anime.SetBool("Swimming", true);
                  WaterLevelPos = hit.point - (-hit.normal * WallOffset);
                    Motor.enabled = false;
                    CanMove = true;
                   IsunderWater = true;

                
            

        



        }
        else
        {

            IsunderWater = false;
        }





       

    }


    public void NearToGroundCheck()
    {

        RaycastHit hit2;

        if(Physics.Raycast(transform.position , -transform.up , out hit2 , GroundCheck,Ground))
        {

            Debug.DrawRay(transform.position , -transform.up * GroundCheck, Color.cyan);


                if(DoingGroundCheck == true)
                {
                Gravity = -5f;
                ShoreCheck = true;

                }


              
            

        }
        else
        {

            ShoreCheck = false;
        }

    }



    public void OnShoreCheck()
    {

        RaycastHit LowerWaterCheck;

        if(Physics.Raycast(transform.position  , transform.forward , out LowerWaterCheck , 1f,Ground))
        {

            Debug.DrawRay(transform.position, transform.forward * 1f, Color.red);

            

                ShallowWater = true;

            
        }
        else

        {

            ShallowWater = false;
        }





    }


    public void Movement()
    {

        IsSInking = false;


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
          //  Anime.SetBool("Jump", false);

        }
        // playing animation based on speed.
        float AnimationSpeedPercent = ((Running) ? CurrentSpeed / RunSpeed : CurrentSpeed / WalkSPeed * .5f);
        Anime.SetFloat("WaterSwimming", AnimationSpeedPercent, SpeedSmoothTime, Time.deltaTime); //Playing animation along with smoothing the value.

    }

    float GetModifiedSmoothTime(float smoothTime)
    {


        if (Controller.isGrounded)
        {
            return smoothTime;
        }

        if (airControlPercent == 0)
        {
            return float.MaxValue;
        }

        return smoothTime / airControlPercent;
    }



}




