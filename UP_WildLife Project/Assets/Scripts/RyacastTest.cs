using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RyacastTest : MonoBehaviour
{
    public Animator anime;
    public bool IsLerping;

    public bool IsClimbing;

    bool InPosition;
    float posT;
    Vector3 StartPos;
    Vector3 TragetPos;
    Quaternion StartRot;
    Quaternion TargetRot;

    public float PositionOffset;
    public float WallOffset;
    public float SpeedMultiplier = 0.2f;

    Transform helper;

    float delta;

    private void Start()
    {

        Init();

    }

    public void Init()
    {


        helper = new GameObject().transform;
        helper.name = "climb Helper";
        CheckForClimb();

    }


    public void CheckForClimb()
    {




        Vector3 Origin = transform.position;
        Origin.y = 1.4f;
        Vector3 dir = transform.forward;
        RaycastHit hit;

        if(Physics.Raycast(Origin,dir,out hit , 5))
        {

            InitForClimb(hit);


        }

    }



    public void InitForClimb(RaycastHit hit)
    {

        IsClimbing = true;
        helper.transform.rotation = Quaternion.LookRotation(-hit.normal);
        StartPos = transform.position;
        TragetPos = hit.point + (hit.normal * WallOffset);
        posT = 0;
        InPosition = false;
        anime.SetBool("IsClimbing", true);


    }



    private void Update()
    {
        delta = Time.deltaTime;

        Tick(delta);

    }


    public void Tick(float Delta)
    {

        if(!InPosition)
        {
            GetinPosition();
            return;
        }
    }


    void GetinPosition( )
    {


        posT += delta;
        if(posT > 1)
        {
            posT = 1;
            InPosition = true;
        }

        Vector3 tp = Vector3.Lerp(StartPos, TragetPos, posT);
        transform.position = tp;


    }

    Vector3 PoswithOffset(Vector3 Origin , Vector3 target)
    {
        Vector3 direction = Origin - target;
        direction.Normalize();
        Vector3 offset = direction * WallOffset;
        return target + offset;


    }

}

