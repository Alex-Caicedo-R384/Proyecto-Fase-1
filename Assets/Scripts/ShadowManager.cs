using UnityEngine;

public class ShadowManager : MonoBehaviour
{
    public GameObject shadowPrefab;
    private int shadowCount = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            CreateShadowAtPlayerPosition();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            DestroyShadow();
        }
    }

    public void CreateShadowAtPlayerPosition()
    {
        if (shadowCount > 0)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Principal");

            if (player != null)
            {
                Vector3 playerPosition = player.transform.position;
                Vector3 shadowPosition = playerPosition;
                Instantiate(shadowPrefab, shadowPosition, Quaternion.identity);
                shadowCount--;
            }
            else
            {
                Debug.LogWarning("No se encontró un objeto con tag 'Principal' para crear la sombra.");
            }
        }
        else
        {
            Debug.LogWarning("No hay subditos disponibles. No se puede crear una nueva sombra.");
        }
    }

    public void DestroyShadow()
    {
        GameObject[] shadows = GameObject.FindGameObjectsWithTag("Sombra");

        if (shadows.Length > 0)
        {
            GameObject shadowToDestroy = null;
            foreach (var shadow in shadows)
            {
                if (shadow.name == "Sombra(Clone)")
                {
                    shadowToDestroy = shadow;
                    break;
                }
            }

            if (shadowToDestroy != null)
            {
                Destroy(shadowToDestroy);
                shadowCount++;
            }
            else
            {
                Debug.LogWarning("No se encontró la sombra 'sombra' para destruir.");
            }
        }
        else
        {
            Debug.LogWarning("No hay sombras disponibles para destruir.");
        }
    }


    public void IncrementarSubditos()
    {
        shadowCount++;
    }
}
