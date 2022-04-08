using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFlowTest : MonoBehaviour
{


   public float scrollSpeed = 0.5f;
   public Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float offset = Time.time * scrollSpeed;
        rend.material.SetTextureOffset("_BaseMap", new Vector2(0, offset));
    }
}


