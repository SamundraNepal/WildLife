using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keybox : MonoBehaviour
{

    public bool HasKey;


    GameObject Player;

    public float Dis;

    public GameObject PickUPUI;
    public AudioSource Source;
    public AudioClip Clip;
    private void Start()
    {
        Source = GetComponent<AudioSource>();
        HasKey = true;
        Player = GameObject.FindGameObjectWithTag("Player");
    }


    private void Update()
    {

        if (Vector3.Distance(transform.position, Player.transform.position) < Dis)
        {
            PickUPUI.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E) && HasKey)
            {
                Source.clip = Clip;
                Source.Play();

                if(Player.GetComponent<PlayerAttack>().HasKeyWithPlayer == false)
                {

                    Player.GetComponent<PlayerAttack>().HasKeyWithPlayer =true;
                    HasKey = false;
                    Destroy(this.gameObject, 1f);

                }

            }



        } else
        {
            PickUPUI.SetActive(false);

        }

    }
}
