using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemigoComportamiento;

public class EstadoAtaque : StateMachineBehaviour
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Atacando", true);

    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerSkills playerSkills = animator.GetComponent<PlayerSkills>();
        ActivarDa�o activarDa�o = animator.GetComponent<ActivarDa�o>();
        if (activarDa�o != null )
        {
            activarDa�o.DesactivarManos(); // Llama a la funci�n que desactiva el collider.
        }
        playerSkills.estaOcupado = false;
        animator.SetBool("Atacando", false);
        Debug.Log("el estado de animacion tiene que ser false"+ playerSkills.estaOcupado);
    }
}

