using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [Header("Timer")]
    public float RopeSlowDownTime;
    public float SlowDownSpeed;
    public float ResetTime;
    public float Length;
    public bool Qpressed;
    public bool Islerping;
    public Animator anime;
    public Transform Joint;
    public Transform Hook;
    public Rigidbody rb;
    public bool IsLerping;
    public Vector3 MovePosition;
    public bool IsHooked;
    public PlayerMotor Pm;
    public CharacterController cc;
    public bool CanDrawRope;
    public float Min, max;
    public float SecondMin, SecondMax;
    public LineRenderer _Rope;
    public Vector3 CUrrentpos;
    public float Movepos;
    public bool IsRopping;
    public bool AddJoint;
    public Quaternion q;
    public LayerMask CanHookObject;
    public LayerMask Ground;
    public float AnimeAcc;
    public float AnimeSpeed = 1f;
    public float SwingSpeed;
    public bool IsGettingGroundUp;
    public bool IGettingGroundDown;
    public float MinLimits , MaxLimits;

   

    private void Start()
    {


        rb.isKinematic = true;
        rb.useGravity = false;
        q = transform.rotation;
        IsHooked = false;
    }

    private void FixedUpdate()
    {


        if (IsHooked == true && cc.isGrounded == false)
        {
            if (Qpressed == false)
            {


                Joint.GetComponent<HingeJoint>().useLimits = true;
                JointLimits Limits = Joint.GetComponent<HingeJoint>().limits;
                Limits.min = MinLimits;
                Limits.max = MaxLimits;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Joint.transform.rotation, 10f * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, Joint.transform.position, 10f * Time.deltaTime);
            RopeMovement();

        }






        if (IsRopping)
        {


            if (Input.GetKey(KeyCode.LeftShift))
            {
                AnimeAcc = Mathf.Clamp(AnimeAcc, 0, 1f);

                AnimeAcc += Time.deltaTime * AnimeSpeed;
                anime.SetFloat("RopeClimb", AnimeAcc);

                if (IsGettingGroundUp)
                {
                    Joint.GetComponent<HingeJoint>().anchor = new Vector3(0f, Joint.GetComponent<HingeJoint>().anchor.y - 0.01f, 0f);
                    transform.position = Vector3.Lerp(transform.position, Joint.transform.position, 10f * Time.deltaTime);

                }


            }

            else
            {
                anime.SetFloat("RopeClimb", AnimeAcc);
                AnimeAcc -= Time.deltaTime * AnimeSpeed;
                if (AnimeAcc <= 0)
                {
                    AnimeAcc = 0;
                }

            }



            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (IGettingGroundDown)
                {

                    Joint.GetComponent<HingeJoint>().anchor = new Vector3(0f, Joint.GetComponent<HingeJoint>().anchor.y + 0.01f, 0f);
                    transform.position = Vector3.Lerp(transform.position, Joint.transform.position, 10f * Time.deltaTime);

                }





            }
        }
    }

    private void Update()
    {

      


        MovementCheck();

      
 



        if (AddJoint)
        {
            Joint.transform.gameObject.AddComponent<HingeJoint>();
            Joint.gameObject.GetComponent<HingeJoint>().autoConfigureConnectedAnchor = false;
            Joint.gameObject.GetComponent<HingeJoint>().connectedAnchor = new Vector3(0f, 0f, 0f);
            AddJoint = false;


        }
        LeaveRope();


        if (CanDrawRope == true)
        {
            _Rope.SetPosition(0, this.transform.position);
            _Rope.SetPosition(1, MovePosition);

        }

       

        if (IsRopping == false && Qpressed == true)
        {
            rb.drag = 1f;

            checkforGround();

        }





        if (IsLerping)
        {
            _Rope.SetPosition(0, this.transform.position);
            _Rope.SetPosition(1, Hook.transform.position);

            Hook.transform.position = Vector3.Lerp(Hook.position, MovePosition, 10f * Time.deltaTime);
            anime.SetBool("RopeThrow", true);

            if (Vector3.Distance(Hook.position, MovePosition) < 0.01f)
            {
                IsLerping = false;
                anime.SetBool("RopeThrow", false);

                float  V = Vector3.Distance(transform.position , MovePosition);

                Joint.GetComponent<HingeJoint>().connectedBody = Hook.GetComponent<Rigidbody>();
                Joint.GetComponent<HingeJoint>().anchor = new Vector3(0f, V, 0f);

                IsHooked = true;
                CanDrawRope = true;

            }

        }

        CheckForHit();
        if (IsHooked == true && cc.isGrounded == false)
        {
            JumpForHook();

            Joint.parent = null;
            Joint.parent = Hook.transform;
            anime.SetBool("Jump", false);
            anime.SetBool("Ropping", true);
        }


    }


  


    public void CheckForHit()
    {
        RaycastHit hit;



        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 20f, CanHookObject))
        {


                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    _Rope.enabled = true;
                    IsLerping = true;
                    MovePosition = hit.transform.position;
                    AddJoint = true;
                    IsRopping = true;

                }


        }

    }


    public void JumpForHook()
    {

        rb.isKinematic = false;
        rb.useGravity = true;
        Pm.enabled = false;
        // IsHooked = false;


    }

    public void RopeMovement()
    {
        if (rb.drag == 0f)
        {
            IsGettingGroundUp = false;
            IGettingGroundDown = false;
        }


        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");

        SwingSpeed += Time.deltaTime * 1f;
        SwingSpeed = Mathf.Clamp(SwingSpeed, 2f, 3f);

        anime.SetFloat("RopeSwing", SwingSpeed);
        anime.SetFloat("RopeSwing", SwingSpeed);
        if (Horizontal != 0 || Vertical!= 0 )
        {
            rb.drag = 0f;
            RopeSlowDownTime = ResetTime;
           
        } else
        {

            RopeSlowDownTime -= Time.deltaTime * SlowDownSpeed;

            if(RopeSlowDownTime <= 0f)
            {
                rb.drag = 5f;
                RopeSlowDownTime = ResetTime;

            }


        }
      

           Joint.GetComponent<Rigidbody>().AddForce(transform.forward * Vertical * 1f, ForceMode.Acceleration);
            Joint.GetComponent<Rigidbody>().AddForce(transform.right * Horizontal * 1f, ForceMode.Acceleration);

        }

    public void LeaveRope()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            anime.SetBool("InAir", true);
            anime.SetBool("Ropping", false);
            Qpressed = true;
            CanDrawRope = false;
            IsRopping = false;
            rb.AddRelativeForce(Vector3.forward * 100f);
            Destroy(Joint.GetComponent<HingeJoint>());
            Joint.parent = null;
            _Rope.enabled = false;
        }



    }



    public void MovementCheck()
    {

        RaycastHit hitup;

        if(Physics.Raycast(transform.position + transform.up * 0.35f , transform.up , out hitup , Length,CanHookObject))
        {

            Debug.DrawRay(transform.position + transform.up * 0.35f, transform.up * Length, Color.blue);

         

                IsGettingGroundUp = false;
            

        } else
        {
            IsGettingGroundUp = true;
            Debug.DrawRay(transform.position + transform.up * 0.35f, transform.up * Length, Color.blue);

        }


        RaycastHit HitDown;

        if (Physics.Raycast(transform.position, -transform.up, out HitDown, Length,Ground))
        {

         

                IGettingGroundDown = false;
            
            Debug.DrawRay(transform.position , -transform.up * Length, Color.blue);

        }
        else
        {
            IGettingGroundDown = true;

            Debug.DrawRay(transform.position, -transform.up * Length, Color.blue);

        }

    }

    public void checkforGround()
    {

        RaycastHit hit;

    
        if (Physics.Raycast(transform.position , -transform.up, out hit, 1f) || (Physics.Raycast(transform.position,transform.up , out hit , 1f,Ground)) )

        {
        
                IsHooked = false;
                Pm.enabled = true;
                rb.isKinematic = true;
                Joint.parent = this.transform;
                Joint.transform.position = this.transform.position;
                Joint.transform.rotation = transform.rotation;
                transform.rotation = q;
                Qpressed = false;
                anime.SetBool("Ropping", false);
                anime.SetBool("InAir", false);

            



        }

    }

}

