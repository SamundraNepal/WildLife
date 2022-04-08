using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRangeFlankers : MonoBehaviour
{
    public PlayerHealth Ph;
    public float Damage;
    public Animator Anime;
    public string Hurt;

    public void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag == "Player")
        {


            Ph.Player_Health -= Damage;
            Anime.SetBool(Hurt, true);
            StartCoroutine(DisabledHurt());
        }

    }




    IEnumerator DisabledHurt()
    {

        yield return new WaitForSeconds(0.2f);
        Anime.SetBool(Hurt, false);



    }


}
