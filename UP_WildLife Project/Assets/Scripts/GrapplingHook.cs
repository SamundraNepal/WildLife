using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField]
    private Material HighLightMaterial;
    [SerializeField]
    private Material DefaultMaterial;
    private Transform _Selection;

    [Header("Bool check")]
    public static bool Fired;
    public bool hooked;
    public Transform HolderRoty;

    [Header("Speed Values")]
    public float HookTravelSpeed;
    public float PlayerTravelSpeed;
    public float MaxDistance;
    private float CurrentDistance;
    public float HookableDistance;
   // public float RaycastShootPos;
  //  public float RaycastXValue;
    public float FlyValue;


    [Header("Gameobject")]
    public GameObject Hook;
     public   Transform Cam;
    public GameObject HookHolder;
    PlayerMotor Motor;
    public Animator Anime;
    public Transform Player;
    public Transform RopeFirePoint;
    public CameraController CameraRotate;

    [Header("Arrays")]
    public GameObject[] HookedGameObj;


   

    private void Start()
    {
        Motor = GetComponent<PlayerMotor>();
        Cam = Camera.main.transform;
    }

    private void Update()
    {
// changing hookable color.
        if(_Selection!= null)
        {
            var selectionRenderer = _Selection.GetComponent<Renderer>();
            selectionRenderer.material = DefaultMaterial;
            _Selection = null;

        }

           HookedGameObj = GameObject.FindGameObjectsWithTag("HookAble");

            RaycastHit hit;
            if (Physics.Raycast(Cam.position, Cam.forward, out hit, HookableDistance))
            {
               Debug.DrawLine(Cam.position, Cam.forward * HookableDistance, Color.yellow);
                if (hit.transform.tag == "HookAble")
                {
                       var Selection = hit.transform;
                    var selectionRenderer = Selection.GetComponent<Renderer>();
                if(selectionRenderer != null)
                {
                    selectionRenderer.material = HighLightMaterial;
                }

                 _Selection = Selection;
                      if (Input.GetMouseButtonDown(0) && Fired == false)
                        Fired = true;
                } 

                

            }
        

        if (Fired)
        {

            Anime.SetBool("Grapple", true);
            Motor.Canmove = false;
            LineRenderer Rope = Hook.GetComponent<LineRenderer>();
        /*    Rope.SetVertexCount(2);
            Rope.SetPosition(0, RopeFirePoint.transform.position);
            Rope.SetPosition(1, Hook.transform.position);*/
            

        } 
   



        if(Fired == true && hooked == false)
        {
            //this.transform.rotation = Cam.transform.rotation;

            // transform.localRotation = Quaternion.Euler(0, HolderRoty.rotation.y, 0);
              transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, HolderRoty.localEulerAngles.y, transform.localEulerAngles.z);

            CameraRotate.CanRotate = false;
            Motor.Controller.enabled = false;
            Motor.Canmove = false;
            Hook.transform.position = Vector3.MoveTowards(Hook.transform.position,hit.transform.position,HookTravelSpeed * Time.deltaTime);
            CurrentDistance = Vector3.Distance(this.transform.position, Hook.transform.position);
            
           
            if (CurrentDistance >= MaxDistance)
                ReturnHook();
          

        }
        

        if(hooked == true && Fired == true)
        {

            foreach (var Obj in HookedGameObj)
            {

            Hook.transform.parent = Obj.transform;
                break;

            }

            //adding Values like fly and speed while using grappling hook
             CameraRotate.CanRotate = true;
             transform.Translate(Vector3.up * FlyValue * Time.deltaTime);
             transform.position = Vector3.MoveTowards(transform.position, Hook.transform.position, PlayerTravelSpeed * Time.deltaTime);
             float DistnaceToHook = Vector3.Distance(transform.position, Hook.transform.position);
            Anime.SetBool("Grapple", false);

            Anime.SetBool("InAir", true);
           

            

     
            if(DistnaceToHook < 0.5)
            {
                if(Motor.Controller.isGrounded == false)
               {


                    this.transform.Translate(Vector3.forward * Time.deltaTime * 17f);
                    this.transform.Translate(Vector3.up * Time.deltaTime * 18f);

                }
                StartCoroutine("Climb");
            } 

        } else

        {
              
                Hook.transform.parent = HookHolder.transform;
               Motor.Controller.enabled = true;
                Motor.Canmove = true;
      
        }


    }



    IEnumerator Climb()
    {


        yield return new WaitForSeconds(0.1f);
        ReturnHook();
        Anime.SetBool("InAir", false);



    }

    void ReturnHook()
    {

        Hook.transform.rotation = HookHolder.transform.rotation;
        Hook.transform.position = HookHolder.transform.position;
        Fired = false;
        hooked = false;

        /*LineRenderer Rope = Hook.GetComponent<LineRenderer>();
        Rope.SetVertexCount(0);*/


    }


 
}
