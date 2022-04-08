using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class OpeningScene : MonoBehaviour
{
 public LoaderChecker lc;

    public AudioSource Source;
    public GameObject PressE;

    public AudioClip PopSound;
    public AudioClip GunSound;

    [Header("Dialogue")]
    public TextMeshProUGUI TextToDisplay;
    [TextArea(15, 20)]
    public string[] Scentences;
    public float TypingSpeed;
    public int Index;

    public bool PickUp;
    bool once;

    public GameObject BackGroundMusic;
    public PlayerMotor Pm;
    public PlayerAttack PA;
    public PlayerInfo PI;


 

    private void Start()
    {
        BackGroundMusic.SetActive(false);
        Pm.enabled = false;
        PA.enabled = false;
        PI.enabled = false;
        Source.Play();
        PickUp = false;
        once = true;
        
    }

    private void Update()
    {

       

        if (TextToDisplay.text == Scentences[Index])
        {
            if (Index == 5)
            {
                if (once)
                {
                    Source.clip = GunSound;
                    Source.Play();
                    once = false;
                }


                if(once==false)
                {
                    if(Input.GetKeyDown(KeyCode.E) && this.GetComponent<OpeningScene>().enabled == true)
                    {
                        lc.Check = true;
                        this.GetComponent<Animator>().enabled = true;
                        Pm.enabled = true;
                        PA.enabled = true;
                        PI.enabled = true;
                        BackGroundMusic.SetActive(true);
                        this.GetComponent<OpeningScene>().enabled = false;

                    }


                }
            }
        }

        if (!PickUp)
        {
            PressE.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                Source.loop = false;
                PressE.SetActive(false);

                PickUp = true;
                Source.Stop();
                StartCoroutine(type());
            }

        }


        if(PickUp)
        {

            if(TextToDisplay.text == Scentences[Index])
            {
                PressE.SetActive(true);
                if(Input.GetKeyDown(KeyCode.E))
                {
                    PressE.SetActive(false);

                    NextScentence();

                }

            }

        }
    }


    IEnumerator type()
    {


        foreach (char letters in Scentences[Index].ToCharArray())
        {
            TextToDisplay.text += letters;

            yield return new WaitForSeconds(TypingSpeed);

        }
       
    }



    public void NextScentence()
    {
        Source.clip = PopSound;
        Source.Play();

        if (Index < Scentences.Length -1)
        {
            Index++;
            TextToDisplay.text = "";
            StartCoroutine(type());

        } else
        {

            TextToDisplay.text = "";
        }


        


    }
}
