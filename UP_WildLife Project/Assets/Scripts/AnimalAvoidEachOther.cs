using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalAvoidEachOther : MonoBehaviour
{
    public string AnimalTags;
    public GameObject[] Foxes;
    public Transform Player;
 


    private void Start()
    {


        Foxes = GameObject.FindGameObjectsWithTag(AnimalTags);

    }


    private void Update()
    {
    
        AvoidEachOther();

        

        


    }



    void AvoidEachOther()
    {


        foreach (var Fox in Foxes)
        {



            float CurrentDiatnace = Vector3.Distance(Fox.transform.position, Fox.transform.position);

            if(CurrentDiatnace < 0.4f)
            {

                Vector3 dis = (Fox.transform.position - Fox.transform.position).normalized;
                Fox.transform.GetComponent<NavMeshAgent>().destination += dis * 5f * Time.deltaTime;


            }



            Physics.IgnoreCollision(Player.GetComponent<Collider>(), Fox.GetComponent<Collider>());


        }




    }






}
