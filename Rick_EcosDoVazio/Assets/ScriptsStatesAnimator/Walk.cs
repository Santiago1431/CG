using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;


public class walk : StateMachineBehaviour
{
    public float timer;
    public float minTime;
    public float maxTime;

    public float speed;
    public float attackRange = 0.5f;



    private Transform playerPosition;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        timer = Random.Range(minTime, maxTime);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        //recupera vida enquanto anda
        animator.GetComponent<BossController>().RecoverHealth();

        float distance = Vector2.Distance(animator.transform.position, playerPosition.position);

        // ► SE PERTO O SUFICIENTE → ATAQUE
        if (distance <= attackRange)
        {
            animator.SetTrigger("Attack");
            return;
        }
        if(timer <= 0)
        {
            animator.SetTrigger("idle");
        }
        else
        {
            timer -= Time.deltaTime;
        }

        Vector2 target = new Vector2(playerPosition.position.x, animator.transform.position.y);
        animator.transform.position = Vector2.MoveTowards(animator.transform.position, target, speed * Time.deltaTime);
        //flip do personagem
        if(animator.transform.position.x < playerPosition.position.x)
        {
            animator.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }
        else
        {
            animator.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }



        

        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

    void Flip(Animator animator)
    {
        Vector3 theScale = animator.transform.localScale;
        theScale.x *= -1;
        animator.transform.localScale = theScale;
    }

    

}
