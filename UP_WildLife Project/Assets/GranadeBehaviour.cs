using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeBehaviour : MonoBehaviour
{

    GameObject Player;
    public float GranadeSpeed;
    public float ExplodeTimer;
    public float MinimumExplodeDis;
    public float ExplodeRnage;
    public GameObject ExplosionEffect;
    public float GrandeExplodeSpeed;
    public bool Once;
    public bool Explode;
    public bool DestroyTime;
    public AudioSource Source;
    public AudioSource Explos;
    public AudioClip Timer, Explosion;
    private void Start()
    {


        Player = GameObject.FindGameObjectWithTag("Player");

        Once = true;
        DestroyTime = true;

    }



    private void Update()
    {

        if(Once)
        {
        transform.position = Vector3.Lerp(transform.position, Player.transform.position, GranadeSpeed * Time.deltaTime);

        }    

        if(Explode)
            ExplodeTimer += GrandeExplodeSpeed;
        {
            if (ExplodeTimer < 5f)
            {


                if (!Source.isPlaying)
                {

                    Source.clip = Timer;
                    Source.Play();
                }

            }
            if (ExplodeTimer > 5f)
            {

                Source.Stop();

                if (Vector3.Distance(Player.transform.position, transform.position) < ExplodeRnage )

                {

                  


                    if (DestroyTime)
                    {
                        if (!Explos.isPlaying)
                        {
                            Explos.clip = Explosion;
                            Explos.Play();

                        }

                        Player.GetComponent<Animator>().SetBool("Hurt", true);
                        Player.GetComponent<PlayerHealth>().Player_Health -= 30f;
                        GameObject G = Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
                        Destroy(G, 1f);
                        DestroyTime = false;
                    }
                 


                }
                else
                {

                 

                    if (DestroyTime)
                    {
                        if (!Explos.isPlaying)
                        {
                            Explos.clip = Explosion;
                            Explos.Play();

                        }
                        GameObject G = Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
                        Destroy(G, 1f);
                        DestroyTime = false;

                    }
                }

                ExplodeTimer = 0f;
                Destroy(this.gameObject, 1f);



            }

        }



        if (Vector3.Distance(Player.transform.position , transform.position) < MinimumExplodeDis)
        {
          //  GetComponent<Rigidbody>().useGravity = true;
         //   GetComponent<Rigidbody>().isKinematic = false;
            Once = false;
            Explode = true;
           

        }

    }


}
