using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed; // Velocidad de la bala
    private Transform startPoint; // Punto de inicio de la bala
    public GameObject center;

    private float time = 2f;
    private float lifeTime;
    public int damage;

    private bool moving = false;

    public void Start()
    {
        time = lifeTime;
    }

    public void Update()
    {
        time -= Time.deltaTime;
        /*if (Input.GetKey(KeyCode.D) && speed >= 0) speed *= -1;
        if (Input.GetKey(KeyCode.A) && speed < 0) speed *= -1;*/
    }

    public void SetDamage(int d)
    {
        damage = d;
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }

    public void SetLifeTime(float t)
    {
        lifeTime = t;
    }

    // Método que se llama cuando se presiona la tecla "G"
    public void ShootBullet(bool direction)
    {
        // Activar la bala
        gameObject.SetActive(true);

        moving = true;

        // Iniciar la corutina para mover la bala
        if (direction) StartCoroutine(MoveBulletRight());
        else StartCoroutine(MoveBulletLeft());
    }

    // Corutina para mover la bala
    IEnumerator MoveBulletRight()
    {
        while (moving)
        {
            //Gira hacia la izquierda, para cambiar a la derecha poner la speed a negativo
            transform.RotateAround(center.transform.position, new(0, 1, 0), -speed * Time.deltaTime);

            if (time <= 0) DisableBullet();

            yield return null;
        }

        transform.position = startPoint.position;
    }

    // Corutina para mover la bala
    IEnumerator MoveBulletLeft()
    {
        while (moving)
        {
            //Gira hacia la izquierda, para cambiar a la derecha poner la speed a negativo
            transform.RotateAround(center.transform.position, new(0, 1, 0), speed * Time.deltaTime);

            if (time <= 0) DisableBullet();

            yield return null;
        }

        transform.position = startPoint.position;
    }


    // Método llamado cuando la bala entra en un trigger
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Tipo objeto: " + other.tag); ;
        //Debug.Log("BalaColisionaObjeto");

        // Verificar colisión con otros objetos y realizar las acciones necesarias
        if (other.CompareTag("Object") || other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            // Desactivar la bala al colisionar con el objeto destino
            DisableBullet();
        }
    }

    // Método para desactivar la bala
    void DisableBullet()
    {
        // Desactivar la bala y restablecer el estado de disparo
        moving = false;
        gameObject.SetActive(false);

        // Destruir la bala al colisionar con cualquier objeto
        Destroy(gameObject);
    }
}
