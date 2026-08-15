using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterScript : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed = 5f;
    [SerializeField] float shift = 2f;
    [SerializeField] float compensacionX = 0f;
    [SerializeField] float jumpForce = 8f;
    [SerializeField] Animator anim;

    [SerializeField] GameObject menu;
    [SerializeField] TMP_Text scoreText;

    // AUDIO
    [SerializeField] AudioClip itemSFX, shieldSFX, obstacleSFX, destroySFX;
    [SerializeField] AudioSource sound, music;

    // PARTÍCULAS / VFX (¡Las 3 nuevas variables!)
    [SerializeField] GameObject itemVFX, shieldVFX, obstacleVFX;

    int carrilActual = 0;
    bool isGameOver = false;
    float roundScore = 0f;

    // VARIABLES DEL ESCUDO
    bool isShield = false;
    GameObject currentShieldVFX; // Para controlar el objeto del aura del escudo

    void Update()
    {
        if (!isGameOver)
        {
            roundScore += Time.deltaTime;
            scoreText.text = "Score: " + roundScore.ToString("f0");
        }

        if (isGameOver) return;

        if (Input.GetKeyDown(KeyCode.A) && carrilActual > -1) carrilActual--;
        if (Input.GetKeyDown(KeyCode.D) && carrilActual < 1) carrilActual++;

        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rb.velocity.y) < 0.1f)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        if (rb.velocity.y > 0.1f) anim.SetBool("Jump", true);
        if (Mathf.Abs(rb.velocity.y) < 0.1f) anim.SetBool("Jump", false);
    }

    void FixedUpdate()
    {
        if (isGameOver) return;

        float objetivoX = (carrilActual * shift) + compensacionX;
        Vector3 nuevaPosicion = transform.position + transform.forward * speed * Time.fixedDeltaTime;
        nuevaPosicion.x = objetivoX;
        rb.MovePosition(nuevaPosicion);
    }

    // COLISIÓN DURA (Obstáculos)
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstaculo"))
        {
            if (isShield == true)
            {
                // Destruir obstáculo y sonido
                Destroy(other.gameObject);
                if (sound != null && destroySFX != null) sound.PlayOneShot(destroySFX);
            }
            else
            {
                // Game Over
                isGameOver = true;
                if (menu != null) menu.SetActive(true);
                rb.isKinematic = true;
                anim.speed = 0;

                // VFX de choque / Game Over
                if (obstacleVFX != null)
                {
                    GameObject vfx = Instantiate(obstacleVFX, transform.position, transform.rotation);
                    Destroy(vfx, 3f);
                }

                // Audio de muerte
                if (sound != null && obstacleSFX != null) sound.PlayOneShot(obstacleSFX);
                if (music != null) music.Stop();
            }
        }
    }

    // TRIGGER FANTASMA (Monedas y Escudo)
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Money"))
        {
            roundScore += 5f;

            // VFX de la estrella en su propia posición
            if (itemVFX != null)
            {
                GameObject vfx = Instantiate(itemVFX, other.transform.position, other.transform.rotation);
                Destroy(vfx, 3f);
            }

            // SFX de la estrella
            if (sound != null && itemSFX != null) sound.PlayOneShot(itemSFX);

            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Escudo"))
        {
            isShield = true;

            // Cancelar el apagado anterior si agarramos otro escudo antes de tiempo
            CancelInvoke("DisableShield");

            // Si ya teníamos un aura activada, la destruimos antes de crear la nueva
            if (currentShieldVFX != null)
            {
                Destroy(currentShieldVFX);
            }

            // VFX del Escudo (Pegado al cuerpo del jugador)
            if (shieldVFX != null)
            {
                currentShieldVFX = Instantiate(shieldVFX, transform.position + transform.up, Quaternion.identity);
                currentShieldVFX.transform.SetParent(this.transform);
            }

            // SFX del Escudo
            if (sound != null && shieldSFX != null) sound.PlayOneShot(shieldSFX);

            Destroy(other.gameObject);
            Invoke("DisableShield", 5f);
        }
    }

    void DisableShield()
    {
        isShield = false;

        // Destruir el efecto del aura cuando el escudo se vence
        if (currentShieldVFX != null)
        {
            Destroy(currentShieldVFX);
        }
    }
}