using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarDa�oEnemigo : MonoBehaviour
{
    public GameObject golpeEnemigoDer;
 public void ActivarDereEnemigo()
    {
        golpeEnemigoDer.SetActive(true);
    }
    public void DesactivarDerEnemigo()
    {
        golpeEnemigoDer.SetActive(false);
    }
}
