using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurt : MonoBehaviour
{
 

    public string Hurt;
    Animator Anime;
    public EnemHealth Eh;
    public float DamageRate;
    public bool IsBlocked;


    private void Start()
    {

        Anime = GetComponentInParent<Animator>();
        Eh = GetComponentInParent<EnemHealth>();

     


    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerAttack")
        {
            if(!Eh.Isdead)
            {
                Anime.SetBool(Hurt, true);
                if (!IsBlocked)
                {
                    Eh.Health -= DamageRate;

                }
                StartCoroutine(Ehurt());
            }

           

        }

    }



    IEnumerator Ehurt()
    {

        yield return new WaitForSeconds(0.1f);
        Anime.SetBool(Hurt, false);

    }

}
