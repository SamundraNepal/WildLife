using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationsEvent : MonoBehaviour
{

    public AudioClip EnemyBlockAttackSoundEffect;
    public AudioClip EnemySlamAttackSoundEffect;


    public AudioClip[] Walking;
    public AudioSource WalkingSoundSource;

    public AudioClip[] PunchingClips;
    public AudioSource EnemyPunchSoundEffect;
    public AudioClip[] HitSounds;
    public AudioClip[] BlockSounds;
    public Collider LeftForArm;
    public Collider RightForArm;
    public Collider BlockCollider;
    public GameObject UnbloackAbleAttack;
    public Collider UnblockCOllider;
    public Transform player;
    public PlayerHealth PH;
    public UnbloackAbleAttack UA;
    public Collider EnemyHitCollider;
    EnemyMotor EM;

    public GameObject Granade;
    private void Start()
    {
        EnemyPunchSoundEffect = GetComponent<AudioSource>();

        LeftForArm.enabled = false;
        RightForArm.enabled = false;
        UnblockCOllider.enabled = false;

        EM = GetComponent<EnemyMotor>();

    }

    public void LeftAttack()
    {
        EnemyPunchSoundEffect.clip = PunchingClips[Random.Range(0, PunchingClips.Length)];
        EnemyPunchSoundEffect.Play();
        LeftForArm.enabled = true;
        BlockCollider.enabled = true;
       EnemyHitCollider.enabled = false;

        
    }

    public void RightAttack()
    {
        EnemyPunchSoundEffect.clip = PunchingClips[Random.Range(0, PunchingClips.Length)];
        EnemyPunchSoundEffect.Play();
        RightForArm.enabled = true;
        BlockCollider.enabled = true;
        EnemyHitCollider.enabled = false;

    }


    public void UnBlockableAttack()
    {
        EnemyPunchSoundEffect.clip = EnemyBlockAttackSoundEffect;
        EnemyPunchSoundEffect.Play();
        UnbloackAbleAttack.SetActive(true);
        EnemyHitCollider.enabled = false;

    }

    public void EnableUnbloackABleCOllider()
    {
        EnemyPunchSoundEffect.clip = EnemySlamAttackSoundEffect;
        EnemyPunchSoundEffect.Play();
        BlockCollider.enabled = true;
        UnblockCOllider.enabled = true;

    }

    public void DisabledUnblockAble()
    {


        UnbloackAbleAttack.SetActive(false);

    }


    public void DisbaledCollider()
    {

        UnblockCOllider.enabled = false;
       
       

    }


    public void UnblockAlldisabled()
    {

        player.parent = null;
        if(UA.CaughtPlayer)
        {
        PH.Player_Health = 0f;

        }
        EM.PlayerdeadMoveBackTOwayPoint = true;
        UnbloackAbleAttack.SetActive(false);

    }


    public void DisabledCollider()
    {
        player.parent = null;
        LeftForArm.enabled = false;
        RightForArm.enabled = false;
      //       BlockCollider.enabled = false;
        UnblockCOllider.enabled = false;
        UnbloackAbleAttack.SetActive(false);
        EnemyHitCollider.enabled = true;


    }

    public void IsHurtWhilePlayerIsBAck()
    {

        EM.CheckAngle = 360f;

    }



    public void EnemyHurtSoundEffect()
    {

        EnemyPunchSoundEffect.clip = HitSounds[Random.Range(0, HitSounds.Length)];
        EnemyPunchSoundEffect.Play();


    }


    public void EnemyBlockedPlayerAttacks()
    {

        EnemyPunchSoundEffect.clip = BlockSounds[Random.Range(0, BlockSounds.Length)];
        EnemyPunchSoundEffect.Play();


    }

    public void EnemySlam()
    {

        EnemyPunchSoundEffect.clip = EnemySlamAttackSoundEffect;
        EnemyPunchSoundEffect.Play();

    }


    public void IsWalking()
    {
        WalkingSoundSource.volume = 1f;

        WalkingSoundSource.clip = Walking[Random.Range(0 , Walking.Length)];

        WalkingSoundSource.Play();
    }


    public void SideWalking()
    {
        WalkingSoundSource.volume = 0.5f;

        WalkingSoundSource.clip = Walking[Random.Range(0, Walking.Length)];
        WalkingSoundSource.Play();


    }



    public void ThrowGranade()
    {


        GameObject G = Instantiate(Granade, transform.position, Quaternion.identity);
        GetComponent<Animator>().SetBool("Toss Granade", false);


    }



}
