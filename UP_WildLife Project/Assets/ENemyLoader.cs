using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENemyLoader : MonoBehaviour
{



    public GameObject WalkingWave;
    public GameObject GunenemyWave;
    public GameObject FlankerEnemyWave;


    [Header("Disable Prevois One")]
    public GameObject[] WalkingEnemy;
    public GameObject[] GunEnemy;
    public string WalkingEnemyName;
    public string GunEnemyEnemName;

    GameObject player;
    PlayerPositionChecker PPC;



    private void Awake()
    {

        PPC = GameObject.FindGameObjectWithTag("GM").GetComponent<PlayerPositionChecker>();

    }
    public void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");

        WalkingWave.SetActive(false);
        GunenemyWave.SetActive(false);
        FlankerEnemyWave.SetActive(false);
        WalkingEnemy = GameObject.FindGameObjectsWithTag(WalkingEnemyName);
        GunEnemy = GameObject.FindGameObjectsWithTag(GunEnemyEnemName);

    }
    public void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag == "Player")
        {

            WalkingWave.SetActive (true);
            GunenemyWave.SetActive(true);
            FlankerEnemyWave.SetActive(true);


            Prevoius();

            PPC.PlayerCheckPointPos = player.transform.position;
            PPC.IsplayerHere = true;
        }


    }




    void Prevoius()
    {


        foreach (var E in WalkingEnemy)
        {


          if(  E.GetComponent<EnemHealth>().Isdead == true){

                E.SetActive(false);

            }


        }


        foreach (var E in GunEnemy)
        {


            if (E.GetComponent<EnemHealth>().Isdead == true)
            {

                E.SetActive(false);

            }


        }



    }
}
