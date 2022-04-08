using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{

    public string HurtAnimations;
    public Animator anime;
    public PlayerHealth Health;
    public float DamageRate;



    public void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag == "Player")
        {
            Health.Player_Health -= DamageRate;

            anime.SetBool(HurtAnimations, true);
            StartCoroutine(Hurt());
            
        }



    }



    IEnumerator Hurt()
    {


        yield return new WaitForSeconds(0.1f);
        anime.SetBool(HurtAnimations, false);
        

    }
}
