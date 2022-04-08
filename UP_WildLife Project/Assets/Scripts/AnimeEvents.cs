using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimeEvents : MonoBehaviour
{
    public Transform Playerpos;
    public Vector3 pos;
    public WallClimber WC;




    private void Update()
    {

        pos = Playerpos.transform.position;


    }





}
