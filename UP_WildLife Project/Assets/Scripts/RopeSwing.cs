using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSwing : MonoBehaviour
{

    public float Length;
    public LayerMask WhatIsGrapple;
    public Transform cam, Player;
    public Vector3 Grapplepoint;
    private SpringJoint Joint;
    public LineRenderer Lr;
    public bool Isgrappling;

    [Header("Joint Attributes")]
    public bool ConnectedAnchor;
    public float SpringValue;
    public float DampleValue;
    public float MasScaleValue;


    [Header("Scripts Refrences")]
    public PlayerMotor PM;



    private void Update()
    {
        

        if(Input.GetMouseButtonDown(0))
            {
            StartGrapple();
        }
        StopGrapple();
        if(Isgrappling)
        {
            Quaternion Lookrotation = Quaternion.LookRotation(Grapplepoint);
            transform.rotation = Quaternion.Slerp(transform.rotation, Lookrotation, 50f * Time.deltaTime);


        }
    }

    public void LateUpdate()
    {

        DrawRope();


    }



    void StartGrapple()
    {

        RaycastHit Rope;

        if(Physics.Raycast( cam.transform.position,cam.transform.forward , out Rope , Length , WhatIsGrapple))
        {
            Isgrappling = true;
            Grapplepoint = Rope.point;
            Joint = Player.gameObject.AddComponent<SpringJoint>();
            Joint.autoConfigureConnectedAnchor = ConnectedAnchor;
            Joint.connectedAnchor = Grapplepoint;




            // Clamp Values
            Joint.maxDistance = transform.position.y + 0.40f;
            Joint.minDistance = 2f;
            PM.Canmove = false;




            Joint.spring = SpringValue;
            Joint.damper = DampleValue;
            Joint.massScale = MasScaleValue;

            Lr.positionCount = 2;
        }
     

    }



    void DrawRope()
    {
        if (!Joint)
            return;
        Lr.SetPosition(0, Player.position);
        Lr.SetPosition(1, Grapplepoint);

    }


    void StopGrapple()
    {

        if(Input.GetKeyDown(KeyCode.Q))
        {

            Destroy(Joint);
            PM.Canmove = true;
            Isgrappling = false;

        }
    }



}
