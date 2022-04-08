using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpawnner : MonoBehaviour
{

    Mesh mesh;
   public Vector3[] v;

    [Header("TweakAble Values")]
    public int MaxFireCount;
    public LayerMask FireMask;
    public GameObject fire;

    public int j;

     int MinValue;
    public int maxvalue = 1800;
    float timer;
   public int prevoisSpawnPoint;
    bool StopFire;
    int Value;


    private void Start()
    {


        mesh = GetComponent<MeshFilter>().mesh;
        v = mesh.vertices;
        StopFire = false;
        


    }


    private void Update()
    {


        for (j = v.Length; j > 0; j--)
        {


        }


      /*  for (int j = 0; j < v.Length; j++)
            {

                Vector3 vert = transform.TransformPoint(v[j]);

                if (Physics.Raycast(new Vector3(vert.x, vert.y - 10f, vert.z), Vector3.up, 50f, FireMask))
                {


                    Debug.DrawRay(vert, Vector3.up * 50f, Color.green);

                }
                else

                {
                    Debug.DrawRay(vert, Vector3.up * 50f, Color.red);
                }


        }*/


        if (!StopFire)
        {
            timer += 1f * Time.deltaTime;

        }
        else
        {

            timer = 0;
        }




        if (timer >= 5f)
        {

            if (MinValue < maxvalue)
            {
                StartCoroutine(FireCountDownTime());
                MinValue += 100;
            }
            else
            {
                StopFire = true;
            }
            timer = 0f;
        }



    }




    IEnumerator FireCountDownTime()
    {
        prevoisSpawnPoint = Value;
        yield return new WaitForSeconds(1f);
        Value += MaxFireCount;
        for (int i = prevoisSpawnPoint; i < Value; i++)
        {

            Instantiate(fire, transform.TransformPoint(v[i]), transform.rotation);


        

        }
    }

}
