using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalCage : MonoBehaviour
{
    public FinalSceneStart FSS;
    public enum AnimalType { Rhino , Others}
    public AnimalType AT;

    public PlayerAttack PT;
    public AnimalInstinct AI;
    public Rigidbody[] Rigi;

    AudioSource Source;

    public Transform Player;

    public float Dis;

    public GameObject OpenCageUI;


    public bool InsideCage;
    public Transform[] CageAnimal;
   


    public Transform[] WayPoints;
    public int MoveSpot;
    private void Start()
    {

        Source = GetComponent<AudioSource>();
        InsideCage = true;
        OpenCageUI.SetActive(false);

        Rigi = GetComponentsInChildren<Rigidbody>();
        foreach (var R in Rigi)
        {

            R.GetComponent<Rigidbody>().isKinematic = true;
            R.GetComponent<Rigidbody>().useGravity = false;


        }


    }



    private void Update()
    {
        if (AT == AnimalType.Others)
        {


            if (InsideCage)
            {
                //  Source.Play();
                MoveBetweenPos();
            }
            else
            {
                Source.Stop();

                foreach (var Animal in CageAnimal)
                {

                    Animal.GetComponent<Animator>().SetBool("Run", false);


                }
            }
        }


      if(Vector3.Distance(Player.transform.position , transform.position) < Dis && InsideCage)
        {

            OpenCageUI.SetActive(true);

            if (AT == AnimalType.Others)
            {



                if (Input.GetKeyDown(KeyCode.E))
                {



                    foreach (var R in Rigi)
                    {

                        R.GetComponent<Rigidbody>().isKinematic = false;
                        R.GetComponent<Rigidbody>().useGravity = true;


                    }
                    OpenCageUI.SetActive(false);
                    InsideCage = false;



                }
            }
            if (AT == AnimalType.Rhino)
            {
                if (Input.GetKeyDown(KeyCode.E) && PT.HasKeyWithPlayer)
                {
                    FSS.Rhinos -= 1;

                    PT.HasKeyWithPlayer = false;

                        AI.CageOpen = true;
                        AI.Timer = 20f;
                    
                    foreach (var R in Rigi)
                    {

                        R.GetComponent<Rigidbody>().isKinematic = false;
                        R.GetComponent<Rigidbody>().useGravity = true;


                    }
                    OpenCageUI.SetActive(false);
                    InsideCage = false;

                }
            }




        } else
        {

            OpenCageUI.SetActive(false);

        }




    }



    public void MoveBetweenPos()
    {
        foreach (var Animal in CageAnimal)
        {

            Animal.transform.position = Vector3.MoveTowards(Animal.position, WayPoints[MoveSpot].transform.position, 0.1f * Time.deltaTime);
            Animal.GetComponent<Animator>().SetBool("Run", true);

            if (Vector3.Distance(Animal.position, WayPoints[MoveSpot].transform.position) < 0.1f)
            {


                MoveSpot = Random.Range(0, WayPoints.Length);
               

            }
            else
            {


                Vector3 NewRot = (WayPoints[MoveSpot].transform.position - Animal.position);
                NewRot.y = 0f;
                Quaternion ROt = Quaternion.LookRotation(NewRot);
                Animal.transform.rotation = Quaternion.Slerp(Animal.transform.rotation, ROt, 50f * Time.deltaTime);
            }



        }






    }
}
