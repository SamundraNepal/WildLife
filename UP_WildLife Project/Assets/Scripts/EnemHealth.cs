using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemHealth : MonoBehaviour
{
    public GameObject Keybox;
    public float Health;
    public float CurrentHealth;
    public Rigidbody[] Rigis;
    public Collider[] col;
    public Rigidbody Rb;
    public bool Isdead;
    public string DeadAnimations;
    public Animator anime;
    public float DieTime = 1f;

    public enum Key { HasKey , HasNoKey}
    public Key KeyEnemy;
    public bool PickUpOnce;
    public bool Ammo;
    public GameObject AmmoDrop;

    public bool Ignore;
    private void Awake()
    {

        Ammo = false;
        if(KeyEnemy==Key.HasKey)
        {
            PickUpOnce = true;

        }
        CurrentHealth = Health;

        anime = GetComponent<Animator>();

        Rigis = GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < Rigis.Length; i++)
        {


            Rigis[i].GetComponent<Rigidbody>().isKinematic = true;
            Rigis[i].GetComponent<Rigidbody>().useGravity = false;


        }


        for (int j = 0; j < col.Length; j++)
        {

            col[j].enabled = false;

        }


       
    }
    public void Start()
    {

        Rb.useGravity = true;
        Rb.isKinematic = false;
    }


    public void Update()
    {
       


        if (Health <= 0f)
        {
            Isdead = true;

            if(KeyEnemy == Key.HasKey && PickUpOnce)
            {

                GameObject K = Instantiate(Keybox, transform.position, transform.rotation);
                PickUpOnce = false;

            }

            if (!Ignore)
            {


                if (GetComponent<EnemyMotor>().FAT == EnemyMotor.FireArmType.AK47 || GetComponent<EnemyMotor>().FAT  == EnemyMotor.FireArmType.ShotGun && Ammo == false)
                {

                    GameObject g = Instantiate(AmmoDrop, transform.position + transform.right * 0.25f, Quaternion.identity);
                    Ammo = true;



                }

            }
            if(GetComponent<NavMeshAgent>()!= null)
            {

                GetComponent<NavMeshAgent>().enabled = false;
            }
        

            StartCoroutine(EnemyDieAnime());


        } else
        {


            Isdead = false;
        }



    }



    void EnableRagDoll()
    {

        for (int i = 0; i < Rigis.Length; i++)
        {


            Rigis[i].GetComponent<Rigidbody>().isKinematic = false;
            Rigis[i].GetComponent<Rigidbody>().useGravity = true;


        }


        for (int j = 0; j < col.Length; j++)
        {

            col[j].enabled = true;

        }


    }


    IEnumerator EnemyDieAnime()
    {
        anime.SetBool(DeadAnimations, true);
        yield return new WaitForSeconds(DieTime);
        GetComponent<CapsuleCollider>().radius = 0f;
        GetComponent<CapsuleCollider>().height = 0f;

        anime.enabled = false;
          Rb.useGravity = false;
          Rb.isKinematic = false;

        EnableRagDoll();
    }

}
