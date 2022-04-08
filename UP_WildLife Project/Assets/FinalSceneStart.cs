using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalSceneStart : MonoBehaviour
{

    public GameObject cages;
    public GameObject Rhino;
    public bool Once;
    public GameObject UI;
    public TextMeshProUGUI NumberOfRhino;
    public int Rhinos;
    public GameObject[] finalScene;


    public GameObject[] FirstWaves;


    PlayerPositionChecker GameManager;


    public GameObject instructions;

    public GameObject CLimbTheTower;
    public GameObject Tower;
    bool once;
    public DistanceChecker DC;
    public GameObject DCText;


    private void Start()
    {
        once = true;
        Tower.SetActive(false);
        GameManager = GameObject.FindGameObjectWithTag("GM").GetComponent<PlayerPositionChecker>();

        Once = true;
        cages.SetActive(false);
        Rhino.SetActive(false);

    }


    private void Update()
    {
        if(Rhinos ==0)
        {
            Tower.SetActive(true);
            UI.SetActive(false);
            if(once)
            {
            CLimbTheTower.SetActive(true);
                once = false;
                DC.enabled = true;
                DCText.SetActive(true);
            }
            StartCoroutine(ClimbTowerInfo());


        }


        NumberOfRhino.text = Rhinos.ToString();

    }

    public void OnTriggerEnter(Collider other)
    {
        if (Once)
        {


            if (other.gameObject.tag == "Player")
            {
                disbaleEnemies();
                cages.SetActive(true);
                Rhino.SetActive(true);

                  UI.SetActive(true);
                  Once = false;
                GameManager.IsplayerHere = true;
                  GameManager.PlayerCheckPointPos  = other.gameObject.transform.position;

                instructions.SetActive(true);
                StartCoroutine(FinalInstructions());

            }

        }

    }




    IEnumerator FinalInstructions()
    {

        yield return new WaitForSeconds(10f);
        instructions.SetActive(false);



    }
    void disbaleEnemies()
    {

        foreach (var E in FirstWaves)
        {

            E.SetActive(false);



        }


        foreach (var F in finalScene)
        {


            F.SetActive(true);

        }

    }


    IEnumerator ClimbTowerInfo()
    {

        yield return new WaitForSeconds(10f);
        CLimbTheTower.SetActive(false);



    }
}
