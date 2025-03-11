using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoEsquive : StateMachineBehaviour
{
    // Start is called before the first frame update
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Esquivando", true);

    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerSkills playerSkills = animator.GetComponent<PlayerSkills>();

        animator.SetBool("Esquivando", false);
        playerSkills.estaOcupado = false;
        Debug.Log("Animación terminada, estado de ocupado restablecido.");
    }
}
