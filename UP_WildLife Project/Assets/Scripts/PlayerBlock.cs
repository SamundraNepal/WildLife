using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{

    public string Block;
    public Animator anime;
    public EnemHealth Eh;



    public void Start()
    {


        anime = GetComponentInParent<Animator>();



    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "EnemyAttack" && (other.gameObject.tag == "PlayerBlock"))
        {


                Debug.Log("Player Blocked");
                if (!Eh.Isdead)
                {
                    anime.SetBool(Block, true);
                    StartCoroutine(BBlock());
                }
        }

    }



    IEnumerator BBlock()
    {
        yield return new WaitForSeconds(0.1f);
        anime.SetBool(Block, false);

    }
}
