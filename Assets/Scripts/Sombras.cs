using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sombras : MonoBehaviour
{
    public float rangovision;
    public float rangoataque;
    public float speedwalk;
    public float speedrun;
    public GameObject rango;
    public GameObject Hit;

    private GameObject target; 
    private Animator ani;
    public bool atacando;
    private int rutina;
    private float cronometro;
    private int direccion;

    void Start()
    {
        ani = GetComponent<Animator>();
        StartCoroutine(BuscarEnemigoPeriodicamente());
    }

    void Update()
    {
        if (target != null)
        {
            Comportamientos();
        }
    }

    IEnumerator BuscarEnemigoPeriodicamente()
    {
        while (true)
        {
            BuscarEnemigo();
            yield return new WaitForSeconds(2f); // Espera 2 segundos antes de reintentar
        }
    }

    void BuscarEnemigo()
    {
        target = null;
        Transform enemigosParent = GameObject.Find("Enemigos").transform;
        foreach (Transform child in enemigosParent)
        {
            if (child.gameObject.activeInHierarchy)
            {
                target = child.gameObject;
                Debug.Log("Objetivo 'Enemigo' encontrado: " + target.name);
                return;
            }
        }
        Debug.LogWarning("No se encontró ningún objetivo 'Enemigo' activo bajo 'Enemigos'.");
    }


    public void Comportamientos()
    {
        if (target == null)
        {
            return; // Salir si target es null
        }

        if (Mathf.Abs(transform.position.x - target.transform.position.x) > rangovision && !atacando)
        {
            ani.SetBool("run", false);
            cronometro += 1 * Time.deltaTime;
            if (cronometro >= 3)
            {
                rutina = Random.Range(0, 2);
                cronometro = 0;
            }

            switch (rutina)
            {
                case 0:
                    ani.SetBool("walk", false);
                    break;

                case 1:
                    direccion = Random.Range(0, 2);
                    rutina++;
                    break;

                case 2:
                    switch (direccion)
                    {
                        case 0:
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                            transform.Translate(Vector3.right * speedwalk * Time.deltaTime);
                            break;

                        case 1:
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                            transform.Translate(Vector3.right * speedwalk * Time.deltaTime);
                            break;
                    }
                    ani.SetBool("walk", true);
                    break;
            }
        }
        else
        {
            if (Mathf.Abs(transform.position.x - target.transform.position.x) > rangoataque && !atacando)
            {
                if (transform.position.x < target.transform.position.x)
                {
                    ani.SetBool("walk", false);
                    ani.SetBool("run", true);
                    transform.Translate(Vector3.right * speedrun * Time.deltaTime);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    ani.SetBool("attack", false);
                }
                else
                {
                    ani.SetBool("walk", false);
                    ani.SetBool("run", true);
                    transform.Translate(Vector3.right * speedrun * Time.deltaTime);
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    ani.SetBool("attack", false);
                }
            }
            else
            {
                if (!atacando)
                {
                    if (transform.position.x < target.transform.position.x)
                    {
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else
                    {
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    ani.SetBool("walk", false);
                    ani.SetBool("run", false);
                }
            }
        }
    }

    public void Finalani()
    {
        ani.SetBool("attack", false);
        atacando = false;
        if (rango != null)
        {
            rango.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            Debug.LogWarning("El objeto 'rango' es nulo.");
        }
    }

    public void ColliderWeaponTrue()
    {
        if (Hit != null)
        {
            Hit.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            Debug.LogWarning("El objeto 'Hit' es nulo.");
        }
    }

    public void ColliderWeaponFalse()
    {
        if (Hit != null)
        {
            Hit.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            Debug.LogWarning("El objeto 'Hit' es nulo.");
        }
    }
}
