using UnityEngine;

public class idle : StateMachineBehaviour
{
    public float timer;
    public float minTime;
    public float maxTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        timer = Random.Range(minTime, maxTime);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //se o boss estiver no estado de idle ele recupera vida 
        animator.GetComponent<BossController>().RecoverHealth();

        // após o tempo de idle, vai para o estado de walk
        if(timer <= 0)
        {
            animator.SetTrigger("walk");
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

}
