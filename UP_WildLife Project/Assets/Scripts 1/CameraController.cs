using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public bool CanRotate;

    public float MouseSensitivity = 10;
    public Transform target;
    public float DistnaceFromTar = 2f;
    public Vector2 pitchMinMax = new Vector2(-40, 85);
    public float RotationSmoothTime = .12f;
    Vector3 RotationSmoothVelocity;
    Vector3 CurrentRotation;
    public Transform Cam;
    float Yaw;
    float pitch;
    [Header("CameraWaterClamp")]
    public float SurfaceCheck;
    public Vector3 CamhitPos;
    public Vector3 DeepwaterCamClamp;
    public float _InsideWaterCheck;
    public bool UnderWaterEffects;
    public WaterMovement Movements;
    public InWaterCheck _InwaterCheck;
    public UnderWater wEffects;
    public bool InsidewaterCheck;
    public LayerMask Water, Ground;



    private void Start()
    {

     Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CanRotate = true;

    }

    private void Update()
    {

        if (CanRotate)
        {




            Yaw += Input.GetAxisRaw("Mouse X") * MouseSensitivity;
            pitch -= Input.GetAxisRaw("Mouse Y") * MouseSensitivity;

         
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

            

            Vector3 targetRotation = new Vector3(pitch, Yaw);
            transform.eulerAngles = targetRotation;

            CurrentRotation = Vector3.SmoothDamp(CurrentRotation, new Vector3(pitch, Yaw), ref RotationSmoothVelocity, RotationSmoothTime);
            transform.eulerAngles = CurrentRotation;


            transform.position = target.position - transform.forward * DistnaceFromTar;


        }



        if(Movements.IsunderWater == false && _InwaterCheck.OnWaterSurface == true )
        {
          pitch = Mathf.Clamp(pitch, CamhitPos.y + 5f, pitchMinMax.y);
           Inwater();

        }

        if(Movements.IsunderWater == true && UnderWaterEffects == true)
        {
            wEffects.EnableFog = true;
            wEffects.NewFogEnabled = false;
            wEffects.FocalLength = 15f;
            wEffects.Intensity = 0.27f;

        } else

        {
            wEffects.NewFogEnabled = true;
            //wEffects.EnableFog = false;
            wEffects.FocalLength = 0f;
            wEffects.Intensity = 0f;



        }


        InsideWaterCheck();
        UnderWater();
        UnderWaterclamp();


    }




    public void Inwater()
    {
        RaycastHit Surfacehit;

        if(Physics.Raycast(transform.position, -transform.up , out Surfacehit , SurfaceCheck))
        {

            Debug.DrawRay(transform.position, -transform.up * SurfaceCheck, Color.cyan);

            if(Surfacehit.transform.tag == "Water")
            {
               
                CamhitPos = Surfacehit.point;
           

            }
        }



    }


    public void InsideWaterCheck()
    {

        RaycastHit UnderWater;

        if (Physics.Raycast(transform.position, transform.up, out UnderWater, _InsideWaterCheck,Water))
        {

            Debug.DrawRay(transform.position, transform.up * _InsideWaterCheck, Color.cyan);

         
                UnderWaterEffects = true;
                DeepwaterCamClamp = UnderWater.point;

            
        }

    }

    public void UnderWater()
    {

        RaycastHit Underwaterhit;

        if (Physics.Raycast(transform.position, -transform.up, out Underwaterhit, _InsideWaterCheck,Ground))
        {

            Debug.DrawRay(transform.position, -transform.up * _InsideWaterCheck, Color.cyan);


                InsidewaterCheck = true;
            
        } else

        {
            InsidewaterCheck = false;
        }
    }


    public void UnderWaterclamp()
    {
        if(Movements.IsunderWater == true && InsidewaterCheck == true)
        {

            pitch = Mathf.Clamp(pitch, pitchMinMax.x, DeepwaterCamClamp.y);

        }

    }

}


  



