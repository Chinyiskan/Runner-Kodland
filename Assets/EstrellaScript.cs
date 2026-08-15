using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstrellaScript : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 100f;

    void Update()
    {
        // Al agregar Space.World, obligamos a la estrella a girar sobre el eje Y global (vertical absoluto)
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
    }
}