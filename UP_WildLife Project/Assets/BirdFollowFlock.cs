using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFollowFlock : MonoBehaviour
{
    public Transform LeaderBird;
    public float FollowSpeed;
    public float StoppingDIstnace;

    public string TagName;
    GameObject[] Birds;


    private void Start()
    {
        Birds = GameObject.FindGameObjectsWithTag(TagName);



    }


    private void Update()
    {
        
            AvoidEachOther();
        if(Vector3.Distance(transform.position , LeaderBird.transform.position) > StoppingDIstnace)
            {


            Vector3 dir = LeaderBird.transform.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 2f * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, LeaderBird.position, FollowSpeed * Time.deltaTime);


        }

    }




    public void AvoidEachOther()
    {


        foreach (var Birds in Birds)
        {


            float CurrentDiatnace = Vector3.Distance(transform.position, Birds.transform.position);

            if (CurrentDiatnace < 0.5f)
            {
                Vector3 dis = (transform.position - Birds.transform.position);
                transform.transform.position += dis * 2f * Time.deltaTime;


            }





        }



    }
}
