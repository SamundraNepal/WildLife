using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class HelicopterHealth : MonoBehaviour
{

    public float Health;

    public Helicopter H;

    public Rigidbody[] rb;

    public GameObject Effects;

    public GameObject Explosioneffects;

    public GameObject GAMEFINISHEDUI;

    public GameObject TransitionScreen;
    public GameObject ExplosionSound;
    public bool Once;

    private void Start()
    {
        Once = true;

        H = GetComponentInParent<Helicopter>();
        Effects.SetActive(false);
        Explosioneffects.SetActive(false);

        rb = GetComponentsInChildren<Rigidbody>();
        rb = GetComponentsInParent<Rigidbody>();

    }


    public void Update()
    {
        
        if(Health <= 0f)
        {
           
            DestroyThis();
            StartCoroutine(Finished());

        }

        if(Health < 25f)
        {

            Effects.SetActive(true);

        }

    }



    public void DestroyThis()
    {
        Explosioneffects.SetActive(true);
        if(Once)
        {

            GameObject g = Instantiate(ExplosionSound, transform.position, Quaternion.identity);
            Destroy(g, 5f);
            Once = false;

        }

        foreach (var R in rb)
        {
            R.GetComponent<Rigidbody>().isKinematic = false;
            H.enabled = false;
        }


    }



   IEnumerator Finished()
    {
        yield return new WaitForSeconds(2f);
        GAMEFINISHEDUI.SetActive(true);

        yield return new WaitForSeconds(5f);

        TransitionScreen.SetActive(true);


        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Menu");

    }



}
