using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AnilDialogueSystem : MonoBehaviour
{
    public bool CanLookAtPlayer;
    public Transform Player;
    public CutTreeAnimationsEvents Events;
    AnilDialogueSystem ADS;
    public CutTreeAnimationsEvents CT;
    public DistanceChecker Checker;
    public ActionChanger AC;
    public Transform Axe;
    PlayerCheckIn Checkin;
    public PlayerMotor Motor;
    public Animator Anime;
    public float Duration;
    public GameObject PressE;

    [Header("Addio and Source")]
    public AudioSource Source;
    public AudioClip TalkToAnil;
    public AudioClip AnilReplyBack;


    [Header("Choices")]
    public GameObject FirstChoices;
    public GameObject LastRepltCHoice;
    public AudioClip Choice1AUdio;
    public AudioClip Choice2AUdio;
    public AudioClip LastReply;


    [Header("Reply")]
    public AudioClip Choice1AUdioReply;
    public AudioClip Choice2AUdioReply;

    public bool FirstConvDone;



    [Header("Wood Cut Complete")]
    public AudioClip AnilCLip1;
    public AudioClip PlayerClip1;
    public GameObject FinalOption;
    public AudioClip GivingGloves;


    public GameObject Gloves;
    public GameObject Tree;
    public GameObject SlecectionUI;



    private void Start()
    {
        Checkin = GetComponent<PlayerCheckIn>();
        Source = GetComponent<AudioSource>();
        Duration = Source.clip.length;
        Anime = GetComponent<Animator>();
        ADS = GetComponent<AnilDialogueSystem>();
       

    }




    private void Update()
    {

        if(CanLookAtPlayer)
        {
            Vector3 dir = (Player.position - transform.position).normalized;
            Quaternion q = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, 1f * Time.deltaTime);
        }
      

        if (Checkin.TalkToAnil && FirstConvDone== false)
        {

            if(Input.GetKeyDown(KeyCode.E))
            {

                Motor.Canmove = false;
                Checkin.TalkToAnil = false;
                PressE.SetActive(false);
                FirstConvDone = true;
                CanLookAtPlayer = true;
                StartCoroutine(FirstTlakToANil());
                

            }
        }



        if(Events.WoodDown && Checkin.TalkToAnil&& FirstConvDone)
        {


          
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Motor.Canmove = false;
                    Checkin.TalkToAnil = false;
                    StartCoroutine(WoodGiving());
                    PressE.SetActive(false);
                    CanLookAtPlayer = true;

            }






        }




    }




    IEnumerator FirstTlakToANil()
    {
       
        Source.clip = TalkToAnil;
        Source.Play();
        yield return new WaitForSeconds(Duration);
        StartCoroutine(ReplyBack());
    }



    IEnumerator ReplyBack()
    {
        Source.clip = AnilReplyBack;
        Source.Play();
        Anime.SetBool("IsTalking", true);
        yield return new WaitForSeconds(10f);
        FirstChoices.SetActive(true);
        Anime.SetBool("IsTalking", false);


    }



    public void Choice1()
    {

        Source.clip = Choice1AUdio;
        Source.Play();
        FirstChoices.SetActive(false);
        StartCoroutine(Choice1Reply());

    }


    public void Choice2()
    {
        Source.clip = Choice2AUdio;
        Source.Play();
        FirstChoices.SetActive(false);
        StartCoroutine(Choice2Reply());

    }


    public void LeaveForWoods()
    {
        LastRepltCHoice.SetActive(false);
        Checker.enabled = true;
        Source.clip = LastReply;
        Checker.CheckPoint = Axe;
        Source.Play();
        Motor.Canmove = true;
        FirstConvDone = true;
        CanLookAtPlayer = false;
    }

    IEnumerator Choice1Reply()
    {

        yield return new WaitForSeconds(4f);
        Anime.SetBool("IsTalking", true);

        Source.clip = Choice1AUdioReply;
        Source.Play();
        yield return new WaitForSeconds(Duration);
        Motor.Canmove = true;
        Anime.SetBool("IsTalking", false);

    }

    IEnumerator Choice2Reply()
    {
        yield return new WaitForSeconds(4f);
        Anime.SetBool("IsTalking", true);

        Source.clip = Choice2AUdioReply;
        Source.Play();
        yield return new WaitForSeconds(14f);
        LastRepltCHoice.SetActive(true);
        Anime.SetBool("IsTalking", false);


    }





    IEnumerator WoodGiving()
    {
        Source.clip = AnilCLip1;
        Anime.SetBool("IsTalking", true);
        Source.Play();
        yield return new WaitForSeconds(6f);
        FinalOption.SetActive(true);
        Anime.SetBool("IsTalking", false);



    }



    public void GivingWoodsToAnil()
    {

        FinalOption.SetActive(false);
        Source.clip = PlayerClip1;
        Source.Play();
        StartCoroutine(FInalReplyByAnil());

    }



    IEnumerator FInalReplyByAnil()
    {

        yield return new WaitForSeconds(8f);
        Anime.SetBool("IsTalking", true);
        Source.clip = GivingGloves;
        Source.Play();
        yield return new WaitForSeconds(6f);
        Motor.Canmove = true;
        Anime.SetBool("IsTalking", false);
        Gloves.SetActive(true);
        AC.ClampValueMax = 1;
        yield return new WaitForSeconds(6f);
        Gloves.SetActive(false);
        Destroy(Tree);
        Destroy(SlecectionUI);
        Destroy(CT);
        Destroy(ADS);



    }








}
