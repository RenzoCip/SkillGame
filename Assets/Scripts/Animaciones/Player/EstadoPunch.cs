using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoPunch : StateMachineBehaviour
{
   
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        animator.SetBool("Atacando", true);    
    
    }
      
    // Start is called before the first frame update
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerSkills playerSkills = animator.GetComponent<PlayerSkills>();
        SkillManager skillmanager = animator.GetComponent<SkillManager>();
        if (skillmanager != null)
        {
            //animator.SetTrigger("Punch");
        }
        animator.SetBool("Atacando", false);
        playerSkills.estaOcupado = false;
        Debug.Log("Animación terminada, estado de ocupado restablecido.");
    }
}
