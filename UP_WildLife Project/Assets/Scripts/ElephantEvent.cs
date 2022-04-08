using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElephantEvent : MonoBehaviour
{

    public Collider ElephantHit;



    private void Start()
    {

        ElephantHit.enabled = false;

    }

    void Attack()
    {

        ElephantHit.enabled = true;


    }


    void DisbaledAttack()
    {
        ElephantHit.enabled = false;


    }
}
