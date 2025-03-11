using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemigoComportamiento;

public class EstadoHit : StateMachineBehaviour
{
    
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Golpeado", true);

    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerSkills playerSkills = animator.GetComponent<PlayerSkills>();
        SkillManager skillmanager = animator.GetComponent<SkillManager>();
        if (skillmanager != null)
        {
            animator.SetTrigger("HitPlayer");
        }
        animator.SetBool("Golpeado", false);
        playerSkills.estaOcupado = false;
        Debug.Log("Animación terminada, estado de ocupado restablecido.");
    }
}
