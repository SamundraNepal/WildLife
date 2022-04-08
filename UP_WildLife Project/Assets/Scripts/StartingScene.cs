using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingScene : MonoBehaviour
{



    public AudioClip clip;
    public AudioSource source;
    public DistanceChecker DC;


    public void Start()
    {

        StartCoroutine(Starting());

    }

    IEnumerator Starting()
    {
        source.clip = clip;
        source.Play();

        yield return new WaitForSeconds(3f);
        DC.enabled = true;

    }







}
