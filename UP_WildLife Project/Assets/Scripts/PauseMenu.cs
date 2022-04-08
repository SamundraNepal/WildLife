using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseUIMenu;
    public GameObject NoNameInfo;
    public GameObject LoadingScreen;

    public WaterMovement Wm;
    public GameObject Source1 , Source2;

    public string MainMenu;

    public GameObject FirstMainMenu, WarningWenu;

    EventSystem Es;


    private void Start()
    {

        Es =  EventSystem.current;


        PauseUIMenu.SetActive(false);
        NoNameInfo.SetActive(false);
    }

    private void Update()
    {




        if(Wm.IsunderWater)
        {

            Source1.SetActive(false);
            Source2.SetActive(true);

        } else
        {

            Source1.SetActive(true);
            Source2.SetActive(false);

        }


        if (Input.GetKeyDown(KeyCode.Escape) && NoNameInfo.activeSelf == false)
        {

           Es.SetSelectedGameObject(FirstMainMenu);

            Debug.Log("Current Selected GameObject :" + Es.currentSelectedGameObject);
            PauseUIMenu.SetActive(true);
            Time.timeScale = 0f;


        }

    }


    public void Resume()
    {

        PauseUIMenu.SetActive(false);
        NoNameInfo.SetActive(false);
        Time.timeScale = 1f;


    }


    public void Menu()
    {

        NoNameInfo.SetActive(true);
        PauseUIMenu.SetActive(false);
        Es.SetSelectedGameObject(WarningWenu);



    }


    public void Warning()
    {

        LoadingScreen.SetActive(true);
        Time.timeScale = 1f;
        StartCoroutine(LeadToMainMenu());
        Es.SetSelectedGameObject(WarningWenu);
         Debug.Log("Current Selected GameObject :" + Es.currentSelectedGameObject);


    }



    IEnumerator LeadToMainMenu()
    {

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(MainMenu);
    }
}
