using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{

    public GameObject ScrollWHeel;
    public GameObject SpaceBar;
    //    public GameObject Climb;



    private void Start()
    {

        StartCoroutine(ScrollWheelInfo());


    }
    IEnumerator ScrollWheelInfo()
    {


        yield return new WaitForSeconds(2f);
        ScrollWHeel.SetActive(true);

        yield return new WaitForSeconds(5f);
        ScrollWHeel.SetActive(false);
        SpaceBar.SetActive(true);
        yield return new WaitForSeconds(10f);
        SpaceBar.SetActive(false);






    }

}
