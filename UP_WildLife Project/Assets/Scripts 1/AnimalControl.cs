using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalControl : MonoBehaviour
{

    public Transform SpawnPoint;
    public GameObject Animal;

    public bool IsAnimal;



    private void Start()
    {


        IsAnimal = false;
    }



    private void Update()
    {


        if (IsAnimal == false)
        {
            if (Input.GetKey(KeyCode.R))
            {

                GameObject AnimalFrn = Instantiate(Animal, SpawnPoint.transform.position, Quaternion.identity);
                IsAnimal = true;
            }




        }



    }
}
