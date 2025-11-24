using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;


public class Attack : StateMachineBehaviour
{

    private float attackDelay = 2.2f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        // Após o ataque, retorna ao estado de idle
        attackDelay -= Time.deltaTime;
        if (attackDelay <= 0)
        {
            animator.SetTrigger("idle");
        }

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    

}
