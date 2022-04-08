using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionChecker : MonoBehaviour
{


    public Vector3 PlayerCheckPointPos;
    private static PlayerPositionChecker PPC;
    public bool IsplayerHere;

    private void Awake()
    {
        if(PPC == null)
            {

            PPC = this;
            DontDestroyOnLoad(PPC);
        }
        else
        {


            Destroy(gameObject);

        }



    }





}
