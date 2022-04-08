using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{

    Animator anime;
    public string Parry;


    private void Start()
    {

        anime = GetComponentInParent<Animator>();
    }


    private void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.tag == "BlockCollider")
        {

            anime.SetBool(Parry, true);
            StartCoroutine(DParry());
        }

    }


        IEnumerator DParry()
        {

            yield return new WaitForSeconds(0.1f);
            anime.SetBool(Parry, false);

        }
    }

