using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderEffectController : MonoBehaviour
{

    public Transform player; // Referencia al jugador
    public EnemyManager enemyManager;
    public Transform enemigoMasCercano; // Referencia al enemigo

    // Materiales afectados
    public Material pipes;
    public Material marketWallMetal;
    public Material securityDoor;
    public Material marketSigns;
    public Material marketDoorway;
    public Material floorMetalAlbedo;
    public Material floorWhiteTileFloor;

    public Material[] materialsToAffect; // Materiales de suelo, paredes, techo

    public float normalHeightScale = 1f; // Escala normal de Height Map
    public float combatHeightScale = 3f; // Escala de combate de Height Map

    public float normalOcclusionStrength = 1f; // Fuerza normal de oclusión
    public float combatOcclusionStrength = 2f; // Fuerza de combate de oclusión

    public float minDist = 3f; // Distancia mínima para efecto máximo
    public float maxDist = 20f; // Distancia máxima para efecto mínimo

    //private bool inCombat = false; // Indica si el jugador está en combate

    // Start is called before the first frame update

    [System.Serializable]
    public class MaterialBackup
    {
        public float originalHeightScale;
        public float originalOcclusionStrength;
    }
    private Dictionary<Material, MaterialBackup> materialBackups = new Dictionary<Material, MaterialBackup>();
    void Start()
    {


        // Asigna los materiales al array
        materialsToAffect = new Material[]
        {
            pipes,
            marketWallMetal,
            securityDoor,
            marketSigns,
            marketDoorway,
            floorMetalAlbedo,
            floorWhiteTileFloor
        };
        SaveOriginalProperties();
    }

    // Update is called once per frame
    void Update()
    {
        List<GameObject> enemigosActivos = enemyManager.enemigosActivos;

        // Encuentra el enemigo más cercano
        enemigoMasCercano = ObtenerEnemigoMasCercano(enemigosActivos);
        if (enemigoMasCercano == null)
        {
            RestoreOriginalProperties();
            return;
        }
        // Calcula la distancia al enemigo más cercano
        float distancia = Vector3.Distance(player.position, enemigoMasCercano.position);
        Debug.Log(distancia);

        if (enemigoMasCercano == null || distancia > maxDist)
           // Debug.Log("no hay enemigo cerca");
            RestoreOriginalProperties();
        else
        {
            if (distancia < maxDist)
            {
                Debug.Log("distancia de jugadr es menor a 20");
                // Calcula el factor de interpolación
                float t = Mathf.InverseLerp(maxDist, minDist, distancia);
                Debug.Log(t);
                AplicarCambios(t);
            }
        }

    }
    public void AplicarCambios(float factorInterpolacion)
    {
        Debug.Log(factorInterpolacion);
        foreach (Material mat in materialsToAffect)
        {
            mat.SetFloat("_Parallax", Mathf.Lerp(normalHeightScale, combatHeightScale, factorInterpolacion));
            mat.SetFloat("_OcclusionStrength", Mathf.Lerp(normalOcclusionStrength, combatOcclusionStrength, factorInterpolacion));
        }
    }

    public void SaveOriginalProperties()
    {
        foreach (Material mat in materialsToAffect)
        {
            if (!materialBackups.ContainsKey(mat))
            {
                MaterialBackup backup = new MaterialBackup
                {
                    originalHeightScale = mat.GetFloat("_Parallax"),    // Asumiendo `_Parallax` para Height Scale
                    originalOcclusionStrength = mat.GetFloat("_OcclusionStrength") // Occlusion Strength
                };

                materialBackups.Add(mat, backup);
            }
        }
    }

    // Encuentra el enemigo más cercano al jugador
    private Transform ObtenerEnemigoMasCercano(List<GameObject> enemigosActivos)
    {
        Transform enemigoCercano = null;
        float distanciaMinima = Mathf.Infinity;

        foreach (GameObject enemigo in enemigosActivos)
        {
            if (enemigo == null) continue;

            float distancia = Vector3.Distance(player.position, enemigo.transform.position);
            if (distancia < distanciaMinima)
            {
                distanciaMinima = distancia;
                enemigoCercano = enemigo.transform;
            }
        }

        return enemigoCercano;
    }
    public void RestoreOriginalProperties()
    {
        foreach (var entry in materialBackups)
        {
            Material mat = entry.Key;
            MaterialBackup backup = entry.Value;

            mat.SetFloat("_Parallax", backup.originalHeightScale);
            mat.SetFloat("_OcclusionStrength", backup.originalOcclusionStrength);
        }
    }
}
