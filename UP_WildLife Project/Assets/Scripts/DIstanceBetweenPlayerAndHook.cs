using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DIstanceBetweenPlayerAndHook : MonoBehaviour
{


    public Transform Player;
    public float Range;


    private void Update()
    {
    

        if(Vector3.Distance(Player.position , transform.position ) < Range)
        {

            Vector3 V = transform.position - Player.position;
            print(V);

        }

    }





    private void OnDrawGizmos()
    {

        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(transform.position, Range);

    }

}
