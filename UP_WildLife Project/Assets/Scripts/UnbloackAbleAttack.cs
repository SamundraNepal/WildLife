using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnbloackAbleAttack : MonoBehaviour
{

    public Transform Player;
    public PlayerMotor motor;
    public Transform CatchTrans;
   // public CameraController CC;
    public CharacterController cc;
    public Animator Anime;
    public bool CaughtPlayer;
    bool Lerp;

    private void Start()
    {

        CaughtPlayer = false;

    }
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag == "Player" && motor.Ps == PlayerMotor.PlayerStates.NotGrabbed)
        {
            motor.Ps = PlayerMotor.PlayerStates.grabbed;
            motor.enabled = false;
            Debug.Log("Got him");
            CaughtPlayer = true;
            Player.parent = CatchTrans;
            motor.Canmove = false;
          //  CC.CanRotate = false;
            cc.enabled = false;
            Lerp = true;
            Player.rotation = CatchTrans.rotation;
            Player.position = CatchTrans.position;

        }

    }


    private void Update()
    {
        

        if(Lerp)
        {

            Anime.SetBool("Thrown", true);

        }

    }
}
