using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatioEvent : MonoBehaviour
{
    public CapsuleCollider GunRange;

    public AudioClip ShootingSound;
    public AudioClip ReloadSound;
    AudioSource AS;
    public AudioSource ShootingSource;




    public Transform gun;
    public Transform GunPoint1;
    public Transform GunPoint2;
    public Transform Shoot;
    public Transform Spine;
    public Transform RadpidFireSHooting;


    [Header("PartcileSystem")]
    public GameObject ShootingParticleEffects;
    public Transform ShootingPEPoint;
    public GameObject Granade;




    private void Start()
    {

        GunRange.enabled = false;
        AS = GetComponent<AudioSource>();

    }

    void SpinePos()
    {


        gun.parent = Spine.transform;
        gun.transform.position = Spine.transform.position;
        gun.transform.rotation = Spine.transform.rotation;


    }

    void ReLoad()
    {


        gun.parent = GunPoint2.transform;
        gun.transform.position = GunPoint2.transform.position;
        gun.transform.rotation = GunPoint2.transform.rotation;

        AS.volume = 0.5f;
        AS.clip = ReloadSound;
        AS.Play();


    }


    void WalkingRight()
    {

        gun.parent = GunPoint1.transform;
        gun.transform.position = GunPoint1.transform.position;
        gun.transform.rotation = GunPoint1.transform.rotation;

    }


    void Shooting()
    {
        gun.parent = Shoot.transform;
        gun.transform.position = Shoot.transform.position;
        gun.transform.rotation = Shoot.transform.rotation;


 
    }


 public void ShootingSOund()
    {
        GameObject go = Instantiate(ShootingParticleEffects, ShootingPEPoint.transform.position, Quaternion.identity);
        Destroy(go, 1f);

        ShootingSource.volume = 1f;

        ShootingSource.clip = ShootingSound;
        ShootingSource.Play();
    }


    public void ShootingFunctions()
    {
        GunRange.enabled = true;
      
    }

    public void DisableShootFunctions()
    {
        GunRange.enabled = false;



    }

    public void RapidFire()
    {

        gun.parent = RadpidFireSHooting.transform;
        gun.transform.position = RadpidFireSHooting.transform.position;
        gun.transform.rotation = RadpidFireSHooting.transform.rotation;

    }


    public void ThrowGranade()
    {


        GameObject G = Instantiate(Granade, transform.position, Quaternion.identity);
        GetComponent<Animator>().SetBool("Toss Granade", false);


    }
}
