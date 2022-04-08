using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTreeAnimationsEvents : MonoBehaviour
{
    public Transform NPC;

    public DistanceChecker checker;
    public Transform Tree;
    public GameObject Axe;
    public MeshRenderer OldAxe;
    public PlayerMotor motor;
    public CuttingTree _AXe;
    public float AttackCount;
    public Animator CutANime;
    public GameObject Effect;
    public Transform Point;
    Animator anime;
    public bool WoodDown;




    private void Start()
    {
        anime = GetComponent<Animator>();

    }





    private void Update()
    {

        if (AttackCount >= 5f)
        {

            CutANime.enabled = true;
            WoodDown = true;
            _AXe.enabled = false;
           

        }

        if(WoodDown)
        {
            checker.CheckPoint = NPC;
            Axe.SetActive(false);

        }

    }



    public void AxePickUp()
    {
        checker.CheckPoint = Tree;
        Axe.SetActive(true);
        OldAxe.enabled = false;
        anime.SetBool("PickUp", false);


    }


    public void CanMOve()
    {

        motor.Canmove = true;
    }

    public void Axefalse()
    {

        AttackCount += 1f;
        anime.SetBool("CutTRee", false);
        motor.Canmove = true;
        _AXe.CanChop = true;
    }


    public void ParticleEffect()
    {
        GameObject E = Instantiate(Effect, Point.transform.position, Point.transform.rotation);
        Destroy(E, 2f);


    }
}
