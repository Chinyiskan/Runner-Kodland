using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<GameObject> locations = new List<GameObject>();
    [SerializeField] Transform player;
    [SerializeField] GameObject shield;

    float tileLength = 106f;
    int count = 4;
    float spawnZ;

    void Start()
    {
        spawnZ = transform.position.z;
        for (int i = 0; i < count; i++)
        {
            CreateLocation();
        }

        // Llamamos al primer escudo
        GenerateObject();
    }

    void CreateLocation()
    {
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, spawnZ);
        int randomIndex = Random.Range(0, locations.Count);
        Instantiate(locations[randomIndex], spawnPos, transform.rotation);
        spawnZ += tileLength;
    }

    void Update()
    {
        if (player.position.z > spawnZ - (tileLength * count))
        {
            CreateLocation();
        }
    }

    void GenerateObject()
    {
        float distance = Random.Range(100f, 200f);
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 2f, player.position.z + distance);
        Instantiate(shield, spawnPos, transform.rotation);

        // EL BUCLE INFINITO: Llama a esta misma función otra vez entre 15 y 25 segundos después
        float tiempoAleatorio = Random.Range(15f, 25f);
        Invoke("GenerateObject", tiempoAleatorio);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}