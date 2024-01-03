using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    public int damage;
    public float speed = 5f;
    public float movingTime;

    private bool moving = false;
    private Transform startPoint;

    public float explosionRange = 5f;

    private Vector3 target;

    void Update()
    {
        if (!moving && startPoint != null) transform.position = startPoint.position;
    }

    IEnumerator MoveLantern()
    {
        //Debug.Log("MoveLantern()");
        moving = true;

        float startTime = Time.time;
        Vector3 startPosition = transform.position;

        while (Time.time - startTime <= movingTime) // Duración total del movimiento
        {
            float t = (Time.time - startTime) / movingTime; // Normaliza el tiempo entre 0 y 1
            transform.position = Vector3.Slerp(startPosition, target, t * speed);

            yield return null; // Espera hasta el siguiente frame
        }

        DisableLantern();
    }

    public void setStartPoint(Transform puntoIni)
    {
        startPoint = puntoIni;
        gameObject.SetActive(true);
    }

    public void ShootLantern(Transform newTarget)
    {
        //Debug.Log("ShootLantern()");
        // Activar la bala
        //gameObject.SetActive(true);

        // Configura el objetivo
        target = newTarget.position;

        StartCoroutine(MoveLantern());
    }

    void Blow()
    {
        // Desactiva la bala y restablece el estado de disparo
        gameObject.SetActive(false);
        // Destruye la bala al colisionar con cualquier objeto
        Destroy(gameObject);

        // Encuentra los objetos en un radio alrededor del lugar de la explosión
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRange);

        // Aplica daño a los objetos encontrados
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                // Aquí puedes aplicar el daño al jugador o enemigo
                // Puedes acceder a los scripts correspondientes y llamar a funciones de daño
                // Ejemplo: col.GetComponent<JugadorScript>().RecibirDanio(damage);
                col.GetComponent<MovePlayer>().TakeDamage(damage);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Verificar colisión con otros objetos y realizar las acciones necesarias
        if (other.CompareTag("Object") || other.CompareTag("Player"))
        {
            //Debug.Log("LANTERN: Colisión Obstaculo detectada");
            DisableLantern();
        }
    }

    public void DisableLantern()
    {
        Blow();
        // Desactivar la bala y restablecer el estado de disparo
        gameObject.SetActive(false);
        // Destruir la bala al colisionar con cualquier objeto
        Destroy(gameObject);
    }
}
