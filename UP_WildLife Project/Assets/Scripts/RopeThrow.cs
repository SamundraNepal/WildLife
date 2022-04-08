using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeThrow : MonoBehaviour
{


    public Vector3 Destiny;
    public float Distance;
    public GameObject PlayerPoint;
  //  public GameObject[] RopesLines; 

    private void Start()
    {

        PlayerPoint = GameObject.FindGameObjectWithTag("Player");
       // RopesLines = GameObject.FindGameObjectsWithTag("Rope");

    }
    public void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Destiny, 5f * Time.deltaTime);
      //  ArrayCheck();

    }


  /*  public void ArrayCheck()
    {
        foreach (var RopesMan in RopesLines)
        {

            RaycastHit hit;
            if(Physics.Raycast(RopesMan.transform.position , RopesMan.transform.forward *0.01f, out hit , 0.3f ))
            {
                Debug.DrawRay(RopesMan.transform.position, RopesMan.transform.forward * 0.01f * 0.3f, Color.yellow);

                if(hit.collider != null)
                {
                    Debug.Log("Hit");
                RopesMan.gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
                break;

                }


            }


        }*/


        


    



   



}
