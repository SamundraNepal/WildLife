using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceChecker : MonoBehaviour
{


    public Transform CheckPoint;
    public Text DistanceText;
    public Transform Player;
    public Vector3 UiOffset;
    public GameObject Distance;


    public float DistanceBewtween;

    public GameObject Helicopter;

    PlayerPositionChecker PPC;

    private void Awake()
    {
        PPC = GameObject.FindGameObjectWithTag("GM").GetComponent<PlayerPositionChecker>();




    }


    private void Update()
    {

        float Minx = DistanceText.GetPixelAdjustedRect().width / 2;
        float MaxX = Screen.width - Minx;

        float MinY = DistanceText.GetPixelAdjustedRect().height / 2;
        float MaxY = Screen.height - MinY;




        Vector2 pos = Camera.main.WorldToScreenPoint(CheckPoint.transform.position + UiOffset);

        



        if(Vector3.Dot((CheckPoint.transform.position - Player.transform.position),Camera.main.transform.forward) < 0)
        {
            if(pos.x < Screen.width /2)
            {
                
                pos.x = MaxX;
            } else

            {

                pos.x = Minx;
            }

        }

        pos.x = Mathf.Clamp(pos.x, Minx, MaxX);
        pos.y = Mathf.Clamp(pos.y, MinY, MaxY);

        DistanceText.transform.position = pos;
        CheckDistnaceBetweenTwoObjects();
        IsReachedDestination();

    }

    void CheckDistnaceBetweenTwoObjects()
    {


        DistanceBewtween = Vector3.Distance(Player.position, CheckPoint.position);


        DistanceText.text = " " + DistanceBewtween.ToString("F1") + " M";


    }


    void IsReachedDestination()
    {

        if(DistanceBewtween < 0.2)
        {
            PPC.PlayerCheckPointPos = Player.transform.position;
            PPC.IsplayerHere = true;
            Distance.SetActive(false);
            Helicopter.SetActive(true);
        } 





    }


  



}
