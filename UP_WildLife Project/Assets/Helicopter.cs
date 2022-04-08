using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour
{


    public Transform HowerPoint;
    public Transform Player;
    public Transform HelicopterObj;
    public float MoveSPeed;
    public float HowerSpeed;
    public float StoppingDistance;
    public GameObject HitEffects;
    public GameObject ShootEffects;
    public Transform ShootPoint;

    public float ShootingTimer;
    public float ShootTimerSpeed;
    public float MaxShootTimer;
    public LayerMask ShootingMask;
    public float MaxRange;
    public float ReloadingTimer;
    public AudioSource Source;
    public AudioClip Clip;

    public float PlayerDamage;


    private void Update()
    {
        
        if(Vector3.Distance(HowerPoint.transform.position , transform.position) > StoppingDistance)
        {

           


            transform.position = Vector3.MoveTowards(transform.position, HowerPoint.transform.position, MoveSPeed * Time.deltaTime);

        } else
        {



         

            transform.RotateAround(HowerPoint.transform.position, Vector3.up, HowerSpeed * Time.deltaTime);
            if(Player.GetComponent<PlayerHealth>().IsDead == false)
            {
            Shooting();

            }


        }

    }





    public void Shooting()
    {


        Vector3 dis =( Player.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dis), 5f * Time.deltaTime);

        float DistnaceToTarget = Vector3.Distance(transform.position, Player.transform.position);

        if (ShootingTimer < MaxShootTimer)
        {

            RaycastHit hit;

            if (!Physics.Raycast(transform.position,dis, out hit , DistnaceToTarget, ShootingMask))
            {
          
                GameObject Effects = Instantiate(ShootEffects, ShootPoint.transform.position, Quaternion.identity);
                Destroy(Effects, 0.5f);
                Player.GetComponent<PlayerHealth>().Player_Health -= PlayerDamage;

               GameObject H = Instantiate(HitEffects,Player.transform.position + Player.transform.up * 0.25f, Quaternion.identity);
                Destroy(H, 1f);
         
                if(!Source.isPlaying)
                {
                    Source.clip = Clip;
                    Source.Play();
                }

                ShootingTimer += ShootTimerSpeed * Time.deltaTime;

                Debug.DrawRay(transform.position,dis * DistnaceToTarget, Color.red);

            }
            else
            {

                Debug.DrawRay(transform.position,dis * DistnaceToTarget, Color.grey);

            }



        }  else
        {


            ReloadingTimer += ShootTimerSpeed * Time.deltaTime;
            if(ReloadingTimer > 10f)
            {

                if (!Source.isPlaying)
                {
                    Source.clip = Clip;
                    Source.Stop();
                }

                ShootingTimer = 0f;
                ReloadingTimer = 0f;
            }
        }

       

    }
}
