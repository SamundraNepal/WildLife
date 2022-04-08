using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckIn : MonoBehaviour
{

    public bool TalkToAnil;
    public GameObject PressE;
    public float Range;
    public Transform Player;




    public void Update()
    {



            if (Vector3.Distance(transform.position, Player.position) < Range)
            {



                TalkToAnil = true;
                PressE.SetActive(true);


            }
            else
            {



                TalkToAnil = false;

            }

        
    }


    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
        PressE.SetActive(false);

        }



    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);


    }

}
