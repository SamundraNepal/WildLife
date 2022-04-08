using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingTheGame : MonoBehaviour
{

    AudioSource source;
    public float Time;
    public AudioClip clip;
    public GameObject ConScreen;
    public GameObject ThisScreen;
    Animator anime;


    private void Start()
    {
        anime = GetComponentInChildren<Animator>();
        source = GetComponent<AudioSource>();
        StartCoroutine(ThreeAmTime());
        StartCoroutine(StartTime());
    }




    IEnumerator ThreeAmTime()
    {

        yield return new WaitForSeconds(5f);
        anime.enabled = true;

    }



    IEnumerator StartTime()

    {

        yield return new WaitForSeconds(Time);
        source.loop = false;
        source.clip = clip;
        source.Play();

        yield return new WaitForSeconds(3f);
        ThisScreen.SetActive(false);
        ConScreen.SetActive(true);

    }


}
