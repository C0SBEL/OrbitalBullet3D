using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cross : MonoBehaviour
{
    public int damage;
    private Rigidbody rb;

    private bool falling;
    private Transform startPoint;

    /*void Start()
    {
        gameObject.SetActive(true);
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        falling = false;
    }*/

    void Update()
    {
        if (!falling && startPoint != null) transform.position = startPoint.position;
    }

    public void ShootCross()
    {
        falling = true;
        //rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
    }

    public void setStartPoint(Transform puntoIni)
    {
        startPoint = puntoIni;
        gameObject.SetActive(true);
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        falling = false;
        //gameObject.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto colisionado tiene el tag que esperas
        if (other.CompareTag("Object") || other.CompareTag("Player"))
        {
            Debug.Log("Colisión Obstaculo detectada");
            DisableCross();
        }
    }

    // Método para desactivar la bala
    public void DisableCross()
    {
        // Desactivar la bala y restablecer el estado de disparo
        //enMovimiento = false;
        gameObject.SetActive(false);

        // Destruir la bala al colisionar con cualquier objeto
        Destroy(gameObject);
    }
}
