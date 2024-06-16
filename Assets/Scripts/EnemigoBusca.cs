using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoBusca : MonoBehaviour
{
    public int rutinaE;
    public float cronometroE;
    public Animator aniE;
    public int direccionE;
    public float speedwalkE;
    public float speedrunE;
    public GameObject targetE;
    public bool atacandoE;
    public float rangovisionE;
    public float rangoataqueE;
    public GameObject rangoE;
    public GameObject HitE;

    void Start()
    {
        aniE = GetComponent<Animator>();
        targetE = GameObject.Find("Personaje");
        Debug.Log("EnemigoBusca está activo.");
    }

    void Update()
    {
        ComportamientoE();
    }

    public void ComportamientoE()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetE.transform.position);

        if (distanceToTarget > rangovisionE && !atacandoE)
        {
            aniE.SetBool("run", false);
            cronometroE += Time.deltaTime;
            if (cronometroE >= 3)
            {
                rutinaE = Random.Range(0, 2);
                cronometroE = 0;
            }

            switch (rutinaE)
            {
                case 0:
                    aniE.SetBool("walk", false);
                    break;

                case 1:
                    direccionE = Random.Range(0, 2);
                    rutinaE++;
                    break;

                case 2:
                    if (direccionE == 0)
                    {
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                        transform.Translate(Vector3.right * speedwalkE * Time.deltaTime);
                    }
                    else
                    {
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                        transform.Translate(Vector3.right * speedwalkE * Time.deltaTime);
                    }
                    aniE.SetBool("walk", true);
                    break;
            }
        }
        else
        {
            aniE.SetBool("walk", false);
            aniE.SetBool("run", true);
            if (distanceToTarget > rangoataqueE && !atacandoE)
            {
                if (transform.position.x < targetE.transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    transform.Translate(Vector3.right * speedrunE * Time.deltaTime);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    transform.Translate(Vector3.right * speedrunE * Time.deltaTime);
                }
            }
            else if (!atacandoE)
            {
                aniE.SetBool("run", false);
                if (transform.position.x < targetE.transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                aniE.SetBool("attack", true);
                atacandoE = true;
            }
        }
    }

    public void FinalAniE()
    {
        aniE.SetBool("attack", false);
        atacandoE = false;
        rangoE.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void ColliderWeaponTrue()
    {
        HitE.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void ColliderWeaponFalse()
    {
        HitE.GetComponent<BoxCollider2D>().enabled = false;
    }
}
