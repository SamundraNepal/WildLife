using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveVegetattion : MonoBehaviour
{
    public Material[] Materials;

    public Transform Player;
    Vector3 pos;





    private void Start()
    {
        StartCoroutine("WriteMaterial");

    }

    IEnumerator WriteMaterial()
    {


        while(true)
        {
            pos = Player.transform.position;


            for (int i = 0; i < Materials.Length; i++)
            {

                Materials[i].SetVector("_position", pos);
            }

            yield return null;
        }
    }


}
