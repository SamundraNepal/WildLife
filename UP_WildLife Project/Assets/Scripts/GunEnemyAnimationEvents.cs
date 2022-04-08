using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEnemyAnimationEvents : MonoBehaviour
{

    public Collider RightCollider;
    public Collider LeftHandCollider;
    public Collider KickCollider;
    public Collider BlockCollider;
    public GunEnemyMotor GEM;


    public AudioClip[] Punching;
    public AudioClip[] HitSounds;

    public AudioSource Source;



    [Header("Toss Granade")]
    public GameObject Granade;

    private void Start()
    {
        Source = GetComponent<AudioSource>();
        GEM = GetComponent<GunEnemyMotor>();
        RightCollider.enabled = false;
        LeftHandCollider.enabled = false;
        KickCollider.enabled = false;


    }
    public void RightHandCol()
    {
        Source.clip = Punching[Random.Range(0, Punching.Length)];
        Source.Play();
        RightCollider.enabled = true;
        BlockCollider.enabled = true;

    }



    public void LeftHandCol()
    {
        Source.clip = Punching[Random.Range(0, Punching.Length)];
        Source.Play();
        LeftHandCollider.enabled = true;
        BlockCollider.enabled = true;



    }





    public void KickCol()
    {
        Source.clip = Punching[Random.Range(0, Punching.Length)];
        Source.Play();
        KickCollider.enabled = true;
        BlockCollider.enabled = true;



    }


    public void DisabledCollider()
    {
        BlockCollider.enabled = false;
        RightCollider.enabled = false;
        LeftHandCollider.enabled = false;
        KickCollider.enabled = false;



    }


    public void Hurt()
    {
        Source.clip = HitSounds[Random.Range(0, HitSounds.Length)];
        Source.Play();

        GEM.ViewAngle = 360f;

    }




    public void ThrowGranade()
    {


        GameObject G = Instantiate(Granade, transform.position, Quaternion.identity);
        GetComponent<Animator>().SetBool("Toss Granade", false);


    }
}
