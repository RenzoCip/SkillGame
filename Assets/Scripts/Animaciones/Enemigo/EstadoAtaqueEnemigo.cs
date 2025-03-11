using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoAtaqueEnemigo : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ActivarDa�oEnemigo activarDa�oEnemigo = animator.GetComponent<ActivarDa�oEnemigo>();
        if (activarDa�oEnemigo != null)
        {
            activarDa�oEnemigo.DesactivarDerEnemigo(); // Llama a la funci�n que desactiva el collider.
        }
    }
}

