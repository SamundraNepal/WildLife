using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class SceneLoader : MonoBehaviour
{


    public string LeveleName;
    public GameObject TranssitionsEffect;

    EventSystem Es;

    public GameObject Start_Game;


    private void Start()
    {
        Es = EventSystem.current;

        Es.SetSelectedGameObject(Start_Game);
    }

    public  void StartGame()
    {

        TranssitionsEffect.SetActive(true);
        StartCoroutine(StartScene());

    }



    public  void Quit()
    {
        Application.Quit();


    }

    IEnumerator StartScene()
    {


        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(LeveleName);

    }
}
