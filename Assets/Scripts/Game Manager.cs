using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    EnemyManager enemyManager;
    // Start is called before the first frame update
    void Start()
    {
        FindAnyObjectByType<EnemyManager>();
        //enemyManager.AparecerEnemigo(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
