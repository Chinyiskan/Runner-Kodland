using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    // Velocidad a la que girará el escudo (la puedes cambiar en el Inspector)
    [SerializeField] float rotationSpeed = 100f;

    void Update()
    {
        // TAREA #2: LA ANIMACIÓN DE ROTACIÓN (Hecha por código, cero estrés)
        // Esto rota el objeto en el eje Y constantemente
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    // TAREA #1: EVITAR QUE SE QUEDE ATASCADO EN OBSTÁCULOS
    private void OnTriggerEnter(Collider other)
    {
        // Si el escudo aparece tocando algo que tiene la etiqueta "Obstaculo"...
        if (other.gameObject.CompareTag("Obstaculo"))
        {
            // ...destruimos el obstáculo para limpiar el área
            Destroy(other.gameObject);
        }
    }
}