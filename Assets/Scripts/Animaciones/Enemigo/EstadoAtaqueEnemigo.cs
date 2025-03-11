using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoAtaqueEnemigo : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ActivarDañoEnemigo activarDañoEnemigo = animator.GetComponent<ActivarDañoEnemigo>();
        if (activarDañoEnemigo != null)
        {
            activarDañoEnemigo.DesactivarDerEnemigo(); // Llama a la función que desactiva el collider.
        }
    }
}

