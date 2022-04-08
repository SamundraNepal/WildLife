using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class FollowerAI : MonoBehaviour
{

    public Transform Leader;
    public float Speed;
    public NavMeshAgent Agent;
    public float StoppingDistnace;



    public string Run;
    public Animator Anime;




    private void Start()
    {
        Anime = GetComponent<Animator>();

        Agent = GetComponent<NavMeshAgent>();
        Agent.speed = Speed;



    }




    private void Update()
    {
        Physics.IgnoreCollision(Leader.GetComponent<Collider>(), transform.GetComponent<Collider>());

        if(Vector3.Distance(transform.position , Leader.transform.position) <= StoppingDistnace)
        {

            Anime.SetBool(Run, false);
            Agent.isStopped = true;

        } else
        {

            MOvement();

        }

        float Dis = Vector3.Distance(Leader.transform.position, transform.position);
        
        if(Dis < 0.4f)
        {

            Vector3 AvoidDis = transform.position - Leader.transform.position;

            transform.position += AvoidDis * 2f * Time.deltaTime;

        }



    }


    void MOvement()
    {
        Agent.isStopped = false;
        Anime.SetBool(Run, true);

        Agent.destination = Leader.position;

    }




}
