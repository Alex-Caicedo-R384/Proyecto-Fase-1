using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSombras : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemigo"))
        {
            print("Da�o");
        }
    }
}
