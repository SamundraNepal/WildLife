using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{

    public GameObject PickUpUI;
   public bool oncePickup;
    GameObject Player;
    public Rigidbody[] rigis;
    public AudioClip Clip;
    AudioSource SOurce;
    private void Start()
    {
        SOurce = GetComponent<AudioSource>();
        oncePickup = true;
        Player = GameObject.FindGameObjectWithTag("Player");
        rigis = GetComponentsInChildren<Rigidbody>();

        foreach (var R in rigis)
        {

            R.GetComponent<Rigidbody>().isKinematic = true;

        }
    }


    private void Update()
    {
        if(Vector3.Distance(Player.transform.position , transform.position) < 0.5f)
        {

            PickUpUI.SetActive(true);
            if (oncePickup == true)
            {

                if (Input.GetKeyUp(KeyCode.E) && Player.GetComponent<PlayerAttack>().NumberOfBullet < 50)
                {
                    SOurce.clip = Clip;
                    SOurce.Play();
                    foreach (var R in rigis)
                    {

                        R.GetComponent<Rigidbody>().isKinematic = false;
                        R.GetComponent<Rigidbody>().useGravity = true;

                    }

                    PickUpUI.SetActive(false);

                    Player.GetComponent<PlayerAttack>().NumberOfBullet += 50;

                    oncePickup = false;
                }
            }

        } else

        {
            PickUpUI.SetActive(false);


        }


    }
   
}
