using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimber : MonoBehaviour
{
    public GameObject ClimbInstructions;
    public GameObject DropInfo;
    public GameObject CImbOverInfo;

    PlayerHealth health;
    public WaterMovement WM;
    public LayerMask AvoidMoveMask;
  

    public bool IsPlayerClimbing;

    [Header("CLimb Value Check")]
    public float Upwardvalue;
    public float Length;
    public LayerMask ClimbAbleObjectCheck;
    public LayerMask GroundLayer;

    [Header("Right Value")]
    public float MoveRightValue;
    public float MaxDistance;
    public float MoveRightUpwardValue;

    [Header("Move Left Value")]
    public float MoveLeftValue;
    public float MoveLeftUpwardValue;




    [Header("LedgeCheck")]
    public bool IsGroundCheck;
    public Vector3 LedgeClimb;

    [Header("Animations")]
    public Animator Anime;
    public float AnimaationsVelocity = 0.0f;
    public float SecondAnimaationsVelocity = 0.0f;
    public float AnimationsAccleration = 0.1f;
    public float AnimationsDeceleration = 0.5f;
    public float SecondDeceleration = 0.5f;
    public bool HangAnimationType1;
    public bool HangAnimationType2;


    public float UpwardMovementanimVelocity = 0.0f;
    public float UpwardMovementAccleration = 0.5f;
    public float DownwardMovementanimVelocity = 0.0f;
    public float DownwardMovementAccleration = 0.5f;



    [Header("Scripts")]
    public PlayerMotor PMotor;
    public CharacterController CC;
    public AnimationsEvents Events;


    [Header("Mask")]
    RaycastHit FirstRaycast;
    RaycastHit LeftRotation;
    RaycastHit RightRotHIt;




    [Header("Bools")]
    public bool Ishitting;
    public bool SeccondHitting;
    public bool IsJumping;
    public bool MoveRight;
    public bool MoveLeft;
    public bool CanHold;
    public bool IsFalling;
    public bool CanDropForHang;
    public bool Move_RightCheck;
    public bool Move_LeftCheck;



    public bool DownMOving;


    [Header("Floats")]
    public float MoveSpeed;
    public float RightRayCastLocation;
    public float Gravity;
    public float JumpHeight;
    public float WaitTimeUp = 5f;
    public float WallOffset;
    public float ClimbingUpwardSpeed;
    public float DownwardClimbingForce;
    public float DownWardWallOffset;

    public bool CanMove;
    [Header("Vectors")]
    public Vector3 playerpos;
    public Vector3 VelocityY;
    public Vector3 DropForHangPosition;
    public Quaternion DropHangRotation;
    public LayerMask OverHeadCheckMask;
    public bool CanMoveUp;
    public float OvreheadCheckLength;

    public enum ClimbState
    {

        Walking,
        Climbing,
        LedgeClimb,
        jumping,
        DropToClimb,
    }
    public ClimbState State;
    private void Start()
    {
        WM = GetComponent<WaterMovement>();

        IsPlayerClimbing = false;

        health = GetComponent<PlayerHealth>();

        IsGroundCheck = true;
        State = ClimbState.Walking;

    }
    private void FixedUpdate()
    {
        if ((!health.IsDead && !WM.IsunderWater))
            {



            AvoidMovingLeftandRight();


            if (IsJumping == true)
            {
                Anime.SetBool("Jump", true);
                Anime.SetBool("IsClimbing", false);
                Anime.SetBool("SecondClimb", false);

                VelocityY.y += Gravity * Time.deltaTime;
                CC.Move(VelocityY * Time.deltaTime);
                if (CC.isGrounded && CC.velocity.y < 0)
                {
                    VelocityY.y = -2f;
                }
            }


            if (State == ClimbState.Climbing)
            {
                MovementCheck();

            }
        }



    }
    private void Update()
    {
        if (!health.IsDead && !WM.IsunderWater )
        {
            if (Ishitting == false && SeccondHitting == false)
            {
                CImbOverInfo.SetActive(false);

            }
            else
            {
                CImbOverInfo.SetActive(false);

            }

            if (Input.GetKey(KeyCode.Q) && IsPlayerClimbing)
            {

                Anime.SetBool("HopDown", true);
                PMotor.Canmove = true;
                IsGroundCheck = true;
                IsPlayerClimbing = false;
                State = ClimbState.Walking;

            }


            if (IsGroundCheck)
            {
               GroundCheck();

            }



            if (CanMove)
            {

                MovePosition();
            }

            if (State == ClimbState.Climbing)
            {

                OverheadCheck();

                HangAnimationsCheck();
                CheckForPlatue();

                if(CanMoveUp)
                {
                UpwardMovement();
                Jump();

                }
                DownwardMovement();


                if(Ishitting == true && SeccondHitting == true)
                {
                    DropInfo.SetActive(true);

                } else
                {
                    DropInfo.SetActive(false);

                }

                if (Ishitting == true && SeccondHitting == false)

                {
                    CImbOverInfo.SetActive ( true);


                    if (Input.GetKeyDown(KeyCode.E) && State == ClimbState.Climbing)
                    {

                        State = ClimbState.LedgeClimb;
                        Anime.SetBool("ClimbUp", true);
                        Anime.SetBool("IsClimbing", false);
                        Anime.SetBool("SecondClimb", false);
                        Anime.SetBool("Climbing Down And Up", false);

                    }

                }

              

            }

            if (State == ClimbState.LedgeClimb)
            {

                if (Events.MoveLedgeClimb)

                {
                    ClimbLedge();

                }


            }



            if (Events.MoveLedegDrop)
            {

                transform.position = DropForHangPosition;
                PMotor.Canmove = false;
                Events.MoveLedegDrop = false;
                transform.rotation = Quaternion.Slerp(transform.rotation, DropHangRotation, 500f * Time.deltaTime);
                transform.position = new Vector3(DropForHangPosition.x, DropForHangPosition.y - 0.40f, DropForHangPosition.z);
                Anime.SetBool("ClimbDownToGrab", false);
                IsGroundCheck = true;
                State = ClimbState.Climbing;


            }



            CheckForClimb();
            CLimbDown();
            if (State == ClimbState.Walking)
            {
                IsJumping = false;
                IsPlayerClimbing = false;
                PMotor.Canmove = true;
                Anime.SetBool("IsClimbing", false);
                Anime.SetBool("SecondClimb", false);
                Anime.SetBool("Climbing Down And Up", false);




            }


            if (State == ClimbState.jumping)
            {
                IsGroundCheck = true;

            }

        }

        if(IsPlayerClimbing)
        {

            ClimbInstructions.SetActive(false);
        }


    }
    public void CheckForClimb()
    {




        if (Physics.Raycast(transform.position + transform.up * Upwardvalue, transform.forward, out FirstRaycast, Length, ClimbAbleObjectCheck))
        {
            Ishitting = true;
            if(!IsPlayerClimbing)
            {
            ClimbInstructions.SetActive(true);


            }

            Debug.DrawRay(transform.position + transform.up * Upwardvalue, transform.forward * Length, Color.blue);



            CanHold = true;
            if (CanHold)
            {
                if (State == ClimbState.Walking || State == ClimbState.jumping)
                {

                    if (Input.GetKey(KeyCode.E))
                    {
                        IsPlayerClimbing = true;
                        ClimbInstructions.SetActive(false);
                        ClimbInstructions.SetActive(false);
                        playerpos = FirstRaycast.point + (FirstRaycast.normal * WallOffset);
                        Anime.SetBool("Jump", false);
                        Anime.SetBool("HopUp", false);
                        State = ClimbState.Climbing;
                        IsJumping = false;
                        CanMove = true;
                        PMotor.Canmove = false;

                    }
                }

            }
        }
        else
        {
            Ishitting = false;
            DropInfo.SetActive(false);

            CanHold = false;
            if(!CanDropForHang)
            {
                ClimbInstructions.SetActive(false);

            }
        }
    }
    public void MovementCheck()
    {


        float Move_Left = Input.GetAxisRaw("Horizontal");
        float Move_Right = Input.GetAxisRaw("Horizontal");


        RaycastHit Hit2;
        if (Physics.Raycast(transform.position + transform.up * MoveRightUpwardValue + transform.right * MoveRightValue, transform.forward, out Hit2, MaxDistance, ClimbAbleObjectCheck))
        {
            Debug.DrawRay(transform.position + transform.up * MoveRightUpwardValue + transform.right * MoveRightValue, transform.forward * MaxDistance, Color.blue);



            MoveRight = true;

            if (Move_Right > 0 && Move_RightCheck)
            {




                Vector3 MoveRightdir = Hit2.point + (Hit2.normal * WallOffset);
                transform.Translate(Move_Right * MoveSpeed * Time.deltaTime, 0f, 0f);
                Vector3 Lerpingpos = Vector3.Lerp(transform.position, new Vector3(MoveRightdir.x, transform.position.y, MoveRightdir.z), MoveSpeed * Time.deltaTime);
                transform.position = Lerpingpos;






                Quaternion look = Quaternion.LookRotation(-Hit2.normal);



                transform.rotation = Quaternion.Slerp(transform.rotation, look, 5f * Time.deltaTime);


                if (HangAnimationType1 == true)
                {


                    AnimaationsVelocity += Time.deltaTime * AnimationsAccleration;
                    Anime.SetFloat("HangingMovement", AnimaationsVelocity);
                    AnimaationsVelocity = Mathf.Clamp(AnimaationsVelocity, -1f, 1f);

                }

                if (HangAnimationType2 == true)
                {



                    AnimaationsVelocity += Time.deltaTime * AnimationsAccleration;
                    Anime.SetFloat("LongHangMoveRight", AnimaationsVelocity);
                    AnimaationsVelocity = Mathf.Clamp(AnimaationsVelocity, -1f, 1f);



                }






            }
            else
            {

                if (HangAnimationType1 == true)
                {


                    AnimaationsVelocity = Mathf.Clamp(AnimaationsVelocity, -1f, 1f);
                    AnimaationsVelocity -= Time.deltaTime * AnimationsDeceleration;
                    Anime.SetFloat("HangingMovement", AnimaationsVelocity);
                }


                if (HangAnimationType2 == true)
                {


                    AnimaationsVelocity = Mathf.Clamp(AnimaationsVelocity, -1f, 1f);
                    AnimaationsVelocity -= Time.deltaTime * AnimationsDeceleration;
                    Anime.SetFloat("LongHangMoveRight", AnimaationsVelocity);

                }


            }



        }
        else

        {

            MoveRight = false;
            if (Physics.Raycast(transform.position + transform.up * RightRayCastLocation + transform.right * 0.31f + transform.forward * 0.08f, -transform.right, out RightRotHIt, 0.3f, ClimbAbleObjectCheck))
            {

                Debug.DrawRay(transform.position + transform.up * RightRayCastLocation + transform.right * 0.31f + transform.forward * 0.08f, -transform.right * 0.3f, Color.cyan);

                float Angle = Vector3.Angle(RightRotHIt.normal, transform.up);






                if (Move_Right > 0)
                {
                    transform.Translate(Move_Right * MoveSpeed * Time.deltaTime, 0f, 0f);
                    Vector3 MoveRightdir = RightRotHIt.point + (RightRotHIt.normal * WallOffset);
                    Vector3 Lerpingpos = Vector3.Lerp(transform.position, new Vector3(MoveRightdir.x, transform.position.y, MoveRightdir.z), MoveSpeed * Time.deltaTime);
                    transform.position = Lerpingpos;

                    Quaternion LookRotation = Quaternion.LookRotation(-RightRotHIt.normal);
                    transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, 5f * Time.deltaTime);



                }



            }
        }






        Debug.DrawRay(transform.position + transform.up * MoveLeftUpwardValue - transform.right * MoveLeftValue, transform.forward * MaxDistance, Color.blue);

        if (Physics.Raycast(transform.position + transform.up * MoveLeftUpwardValue - transform.right * MoveLeftValue, transform.forward, out Hit2, MaxDistance, ClimbAbleObjectCheck))
        {

            MoveLeft = true;

            if (Move_Left < 0 && Move_LeftCheck)
            {


                transform.Translate(Move_Left * MoveSpeed * Time.deltaTime, 0f, 0f);
                Vector3 MoveRightdir = Hit2.point + (Hit2.normal * WallOffset);
                Vector3 Lerpingpos = Vector3.Lerp(transform.position, new Vector3(MoveRightdir.x, transform.position.y, MoveRightdir.z), MoveSpeed * Time.deltaTime);
                transform.position = Lerpingpos;







                Quaternion look = Quaternion.LookRotation(-Hit2.normal);


                transform.rotation = Quaternion.Slerp(transform.rotation, look, 5f * Time.deltaTime);


                if (HangAnimationType1 == true)
                {
                    SecondAnimaationsVelocity -= Time.deltaTime * AnimationsAccleration;
                    Anime.SetFloat("HangingMovementLeft", SecondAnimaationsVelocity);
                    SecondAnimaationsVelocity = Mathf.Clamp(SecondAnimaationsVelocity, -1f, 1f);
                }


                if (HangAnimationType2 == true)
                {


                    SecondAnimaationsVelocity -= Time.deltaTime * AnimationsAccleration;
                    Anime.SetFloat("LongHangMoveLeft", SecondAnimaationsVelocity);
                    SecondAnimaationsVelocity = Mathf.Clamp(SecondAnimaationsVelocity, -1f, 1f);


                }

            }
            else
            {


                if (HangAnimationType1 == true)
                {

                    SecondAnimaationsVelocity = Mathf.Clamp(SecondAnimaationsVelocity, -1f, 1f);
                    SecondAnimaationsVelocity += Time.deltaTime * SecondDeceleration;
                    Anime.SetFloat("HangingMovementLeft", SecondAnimaationsVelocity);

                }

                if (HangAnimationType2 == true)
                {


                    SecondAnimaationsVelocity = Mathf.Clamp(SecondAnimaationsVelocity, -1f, 1f);
                    SecondAnimaationsVelocity += Time.deltaTime * SecondDeceleration;
                    Anime.SetFloat("LongHangMoveLeft", SecondAnimaationsVelocity);



                }

            }

        }
        else
        {
            MoveLeft = false;

            if (Physics.Raycast(transform.position + transform.up * 0.33f + transform.right * -0.31f + transform.forward * 0.08f, transform.right, out LeftRotation, 0.3f, ClimbAbleObjectCheck))
            {

                Debug.DrawRay(transform.position + transform.up * 0.33f + transform.right * -0.31f + transform.forward * 0.08f, transform.right * 0.3f, Color.cyan);



                float Angle = Vector3.Angle(LeftRotation.normal, transform.up);






                if (Move_Left < 0)
                {


                    transform.Translate(Move_Left * MoveSpeed * Time.deltaTime, 0f, 0f);
                    Vector3 MoveRightdir = LeftRotation.point + (LeftRotation.normal * WallOffset);
                    Vector3 Lerpingpos = Vector3.Lerp(transform.position, new Vector3(MoveRightdir.x, transform.position.y, MoveRightdir.z), MoveSpeed * Time.deltaTime);
                    transform.position = Lerpingpos;



                    Quaternion LookRotation = Quaternion.LookRotation(-LeftRotation.normal);
                    transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, 5f * Time.deltaTime);

                }





            }

        }







    }
    public void Jump()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            VelocityY.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
            IsJumping = true;
            State = ClimbState.jumping;


        }

    }
    public void GroundCheck()
    {

        RaycastHit Hit4;
        if (Physics.Raycast(transform.position, -transform.up, out Hit4, 0.2f, GroundLayer))
        {

            Ishitting = false;
            CanHold = false;
            State = ClimbState.Walking;
            Anime.SetBool("HopDown", false);





        }

    }
    public void MovePosition()
    {


        Vector3 tp = Vector3.Lerp(transform.position, new Vector3(playerpos.x, playerpos.y - 0.40f, playerpos.z), 20f * Time.deltaTime);
        transform.position = tp;

        Quaternion lookrotation = Quaternion.LookRotation(-FirstRaycast.normal);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookrotation, 2f * Time.deltaTime);






        if (Vector3.Distance(transform.position, new Vector3(playerpos.x, playerpos.y - 0.40f, playerpos.z)) < 0.01f)
        {

            CanMove = false;



        }

    }
    public void UpwardMovement()
    {
        float MoveVertical = Input.GetAxisRaw("Vertical");


        RaycastHit Upwardhit;

        if (Physics.Raycast(transform.position + transform.up * 0.33f, transform.forward, out Upwardhit, 0.3f, ClimbAbleObjectCheck))
        {

            Debug.DrawRay(transform.position + transform.up * 0.33f, transform.forward * 0.3f, Color.green);



            if (Ishitting && SeccondHitting)
            {


                if (MoveVertical > 0)
                {
                    transform.Translate(0f, MoveVertical * ClimbingUpwardSpeed * Time.deltaTime, 0f);


                    Vector3 LerpPos = Upwardhit.point + (Upwardhit.normal * WallOffset);
                    Vector3 Playerpos = Vector3.Lerp(transform.position, LerpPos, ClimbingUpwardSpeed * Time.deltaTime);
                    transform.position = Playerpos;








                    Quaternion LookRotation = Quaternion.LookRotation(-Upwardhit.normal);

                    transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, 5f * Time.deltaTime);


                    // For Animations


                    Anime.SetBool("Climbing Down And Up", true);
                    Anime.SetBool("IsClimbing", false);
                    Anime.SetBool("SecondClimb", false);



                    UpwardMovementanimVelocity = Mathf.Clamp(UpwardMovementanimVelocity, -1f, 1f);
                    UpwardMovementanimVelocity += Time.deltaTime * UpwardMovementAccleration;
                    Anime.SetFloat("Climbing Up", UpwardMovementanimVelocity);


                } else
                {
                    UpwardMovementanimVelocity = Mathf.Clamp(UpwardMovementanimVelocity, -1f, 1f);
                    UpwardMovementanimVelocity -= Time.deltaTime * UpwardMovementAccleration;
                    Anime.SetFloat("Climbing Up", UpwardMovementanimVelocity);

                }



            }

        }



    }
    public void DownwardMovement()
    {
        float MoveDown = Input.GetAxisRaw("Vertical");

        RaycastHit Downward;

        if (Physics.Raycast(transform.position + transform.up * DownWardWallOffset, transform.forward, out Downward, 0.3f, ClimbAbleObjectCheck))
        {
            Debug.DrawRay(transform.position + transform.up * DownWardWallOffset, transform.forward * 0.3f, Color.green);




            if (Ishitting == true)
            {
                if (MoveDown < 0)
                {

                    DownMOving = true;


                    transform.Translate(0f, MoveDown * DownwardClimbingForce * Time.deltaTime, 0f);

                    Vector3 LerpPos = Downward.point + (Downward.normal * WallOffset);
                    Vector3 newpos = Vector3.Lerp(transform.position, LerpPos, DownwardClimbingForce * Time.deltaTime);
                    transform.position = newpos;



                    Quaternion QRot = Quaternion.LookRotation(-Downward.normal);
                    Quaternion Q = Quaternion.Slerp(transform.rotation, QRot, 5f * Time.deltaTime);
                    transform.rotation = Q;


                    Anime.SetBool("Climbing Down And Up", true);
                    Anime.SetBool("IsClimbing", false);
                    Anime.SetBool("SecondClimb", false);

                    DownwardMovementanimVelocity = Mathf.Clamp(DownwardMovementanimVelocity, -1f, 1f);
                    DownwardMovementanimVelocity -= Time.deltaTime * DownwardMovementAccleration;
                    Anime.SetFloat("Climbing Down", DownwardMovementanimVelocity);




                } else
                {

                    DownwardMovementanimVelocity = Mathf.Clamp(DownwardMovementanimVelocity, -1f, 1f);
                    DownwardMovementanimVelocity += Time.deltaTime * DownwardMovementAccleration;
                    Anime.SetFloat("Climbing Down", DownwardMovementanimVelocity);


                }
            }




        }

    }
    public void HangAnimationsCheck()
    {

        RaycastHit AnimationType;

        if (Physics.Raycast(transform.position + transform.up * 0.10f, transform.forward, out AnimationType, 0.2f, ClimbAbleObjectCheck))
        {

            Debug.DrawRay(transform.position + transform.up * 0.10f, transform.forward * 0.2f, Color.blue);




            Anime.SetBool("IsClimbing", true);
            Anime.SetBool("SecondClimb", false);
            Anime.SetBool("Climbing Down And Up", false);

            HangAnimationType1 = true;
            HangAnimationType2 = false;


        }
        else

        {
            Anime.SetBool("Climbing Down And Up", false);

            Anime.SetBool("IsClimbing", false);
            Anime.SetBool("SecondClimb", true);

            HangAnimationType2 = true;
            HangAnimationType1 = false;



        }

    }
    public void ClimbLedge()
    {
        IsGroundCheck = false;
        CC.height = 0f;
        CC.center = new Vector3(0f, 0.47f, 0f);
        transform.position = Vector3.Lerp(transform.position, new Vector3(LedgeClimb.x, LedgeClimb.y - 0.50f, LedgeClimb.z), 50f * Time.deltaTime);
        if (Vector3.Distance(transform.position, new Vector3(LedgeClimb.x, LedgeClimb.y - 0.50f, LedgeClimb.z)) <= 0.01f)
        {
            Events.MoveLedgeClimb = false;
            Anime.SetBool("ClimbUp", false);
            transform.position = LedgeClimb;
            State = ClimbState.Walking;
            CC.height = 0.43f;
            CC.center = new Vector3(0f, 0.29f, 0f);

        }

    }
    public void CLimbDown()
    {
        RaycastHit ClimbDown;
        if (Physics.Raycast(transform.position + transform.forward * 0.05f - transform.up * 0.06f, -transform.forward, out ClimbDown, 0.1f, ClimbAbleObjectCheck))
        {
            Debug.DrawRay(transform.position + transform.forward * 0.05f - transform.up * 0.06f, -transform.forward * 0.1f, Color.red);
            if(!IsPlayerClimbing)
            {
            ClimbInstructions.SetActive(true);

            }
            CanDropForHang = true;
            if (Input.GetKeyDown(KeyCode.E) && CanDropForHang)
            {

                IsGroundCheck = false;
                IsPlayerClimbing = true;
                State = ClimbState.DropToClimb;
                DropForHangPosition = ClimbDown.point + ClimbDown.normal * WallOffset;
                DropHangRotation = Quaternion.LookRotation(-ClimbDown.normal);
                Anime.SetBool("ClimbDownToGrab", true);
                DropInfo.SetActive(true);


            }


        }
        else
        {
            if(!IsPlayerClimbing && !Ishitting)
            {

                ClimbInstructions.SetActive(false);

            }

            CanDropForHang = false;
        }
    }
    public void CheckForPlatue()
    {

        RaycastHit hit3;
        RaycastHit HIt4;
        if (Physics.Raycast(transform.position + transform.up * 0.40f, transform.forward, out hit3, 0.1f, ClimbAbleObjectCheck))
        {
            Debug.DrawRay(transform.position + transform.up * 0.40f, transform.forward * 0.2f, Color.cyan);
            SeccondHitting = true;
        }
        else

        {
            if (Physics.Raycast(transform.position + transform.up * 0.40f + transform.forward * 0.12f, -transform.up, out HIt4, 0.1f, ClimbAbleObjectCheck))
            {
                Debug.DrawRay(transform.position + transform.up * 0.40f + transform.forward * 0.12f, -transform.up * 0.1f, Color.cyan);

                LedgeClimb = HIt4.point + (HIt4.normal * 0.04f);
            }

            SeccondHitting = false;

        }
    }
    public void AvoidMovingLeftandRight()
    {

        if (Physics.Raycast(transform.position + transform.up * 0.33f, transform.right,  0.1f, AvoidMoveMask))
        {

            Debug.DrawRay(transform.position + transform.up * 0.33f, transform.right * 0.1f, Color.red);
            Move_RightCheck = false;

        } else
        {
            Move_RightCheck = true;


        }



        if (Physics.Raycast(transform.position + transform.up * 0.33f, -transform.right, 0.1f, AvoidMoveMask))
        {

            Debug.DrawRay(transform.position + transform.up * 0.33f, -transform.right * 0.1f, Color.red);

            Move_LeftCheck = false;

        } else
        {

            Move_LeftCheck = true ;


        }
    }
    public void OverheadCheck()
    {

        if(Physics.Raycast(transform.position + transform.up * 0.33f , transform.up , OvreheadCheckLength , OverHeadCheckMask))
            {

            CanMoveUp = false;

            Debug.DrawRay(transform.position + transform.up * 0.33f, transform.up * OvreheadCheckLength, Color.red);

        } else
        {
            Debug.DrawRay(transform.position + transform.up * 0.33f, transform.up * OvreheadCheckLength, Color.green);

            CanMoveUp = true;


        }



    }

    }



