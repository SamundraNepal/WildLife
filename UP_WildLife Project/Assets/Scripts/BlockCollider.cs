using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollider : MonoBehaviour
{

    public string Block;
    public Animator anime;
    public EnemyHurt Hurt;
    public EnemHealth Eh;



    public void Start()
    {
       
        
        anime = GetComponentInParent<Animator>();
       


    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerAttack")
        {
            if(!Eh.Isdead)
            {
                Hurt.IsBlocked = true;
                anime.SetBool(Block, true);
                StartCoroutine(BBlock());
            }

        }


    }



    IEnumerator BBlock()
    {

        yield return new WaitForSeconds(0.1f);

        anime.SetBool(Block, false);
        Hurt.IsBlocked = false;

    }
}
