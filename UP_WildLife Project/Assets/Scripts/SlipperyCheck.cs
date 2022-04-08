using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperyCheck : MonoBehaviour
{
    public LayerMask Mask;
    public LayerMask GroundMask;
    public float Distnace;
    public float GroudDistance;
    public bool IsSliding;
    public CapsuleCollider col;
    public bool GroundCheckOnce;

   public CharacterController cc;
  public PlayerMotor PM;
   public    Rigidbody rb;
    public ActionChanger Ac;


    private void Start()
    {
      


    }




    private void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position , -transform.up, out hit , Distnace , Mask))
        {
            Debug.DrawRay(transform.position, -transform.up * Distnace, Color.blue);
            if(hit.transform.tag == "Slider")
            {
                IsSliding = true;


            } else
            {

                IsSliding = false;


            }

        }



        if(IsSliding)
        {

            rb.useGravity = true;
            rb.isKinematic = false;
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            PM.enabled = false;
            cc.enabled = false;
            col.enabled = true;
            Ac.ActionNumber = 0;



        } else
        {

            rb.isKinematic = true;
            rb.useGravity = false;
           col.enabled = false;
        }



        if (Physics.Raycast(transform.position, -transform.up,  GroudDistance, GroundMask))
        {

            Debug.DrawRay(transform.position, -transform.up * GroudDistance, Color.red);

            if (IsSliding == false )
            {
                GroundCheckOnce = false;
                PM.enabled = true;
                cc.enabled = true;

            }

        }


    }



  


}
