using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderChecker : MonoBehaviour
{
    public bool Check;
    public PlayerMotor Pm;
    public PlayerAttack PA;
    static LoaderChecker LC;

    private void Awake()
    {

        if(LC ==null)
        {


            LC = this;
            DontDestroyOnLoad(this.gameObject);


        }
        else if(LC!=null)
        {
            Pm.enabled = true;
            PA.enabled = true;
            LC.gameObject.SetActive(false);
            Destroy(this.gameObject);


        }


    }
}
