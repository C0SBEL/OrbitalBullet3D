using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : MonoBehaviour
{
    //Vida enemigo
    public int maxHealth;
    private int health;

    public float speed; // Velocidad de la bala
    public GameObject center;
    private bool moving = false;

    public float shootingTime;
    private float time;

    public GameObject CrossObject;
    private Cross scriptCross;
    public Transform startPoint;

    //Animator ghostAnimator;
    private bool holding = false;

    //Barra de vida;
    public GameObject HealthBar;
    private HealthBar scriptHealthBar;

    //Detectar player
    //public float distanceRange;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        scriptHealthBar = HealthBar.GetComponent<HealthBar>();
        scriptHealthBar.setMaxHealth(maxHealth);

        /*ghostAnimator = GetComponent<Animator>();

        ghostAnimator.SetBool("Hold", false);
        ghostAnimator.SetBool("Die", false);*/
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (!moving)
        {
            moving = true;
            StartCoroutine(MoveVampire());
        }
    }

    //RECIBIR DAÑO Y MORIR
    public void TakeDamage(int damage)
    {
        Debug.Log("TAKE DAMAGE: " + damage);
        health -= damage;
        scriptHealthBar.SetHealth(health);
        if (health <= 0) Die();
    }

    private void Die()
    {
        if (scriptCross != null)
        {
            // Activar la bala
            scriptCross.DisableCross();
        }

        HealthBar.SetActive(false);

        moving = false;
        //ghostAnimator.SetBool("Die", true);

        StartCoroutine(WaitForAnimationToEnd());
        //gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    private IEnumerator WaitForAnimationToEnd()
    {
        // Espera hasta que la animación de "Die" haya terminado
        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }

    IEnumerator MoveVampire()
    {
        while (moving)
        {
            //Gira hacia la izquierda, para cambiar a la derecha poner la speed a negativo
            transform.RotateAround(center.transform.position, new(0, 1, 0), speed * Time.deltaTime);

            //Miro si el jugador está en rango y no sujetamos nada
            /*if (!holding && JugadorEnRango())
            {
                ghostAnimator.SetBool("Hold", true);
            }*/

            if (!holding /*&& ghostAnimator.GetBool("Hold")*/)
            {
                //Debug.Log("CREAR CRUZ");
                holding = true;
                time = 0;
                // Instanciar una nueva bala en el punto de disparo
                GameObject newCross = Instantiate(CrossObject, startPoint.position, startPoint.rotation);

                // Obtener el componente Bullet de la nueva bala
                scriptCross = newCross.GetComponent<Cross>();
                scriptCross.setStartPoint(startPoint);
            }

            if (time >= shootingTime) ShootCross();

            yield return null;
        }

        //transform.position = startPoint.position;
    }

    void ShootCross()
    {
        //Debug.Log("DISPARAR CRUZ");
        time = 0;
        // Verificar si el componente existe antes de activar la bala
        if (scriptCross != null && player != null)
        {
            // Activar la bala
            scriptCross.ShootCross();
        }

        //ghostAnimator.SetBool("Hold", false);
        holding = false;
    }

    //TRIGGERS Y COLISIONES//
    void OnTriggerEnter(Collider other)
    {
        // Verificar colisión con otros objetos y realizar las acciones necesarias
        if (other.CompareTag("Object") || other.CompareTag("Player"))
        {
            speed *= -1;
            transform.Rotate(0f, 180f, 0f);
        }

        if (other.CompareTag("Bullet"))
        {
            Debug.Log("VAMPIRE: Bala detectada");
            // Acceder al componente BulletC de la bala que ha colisionado
            Bullet bullet = other.GetComponent<Bullet>();

            // Verificar si el componente Bala existe en el objeto colisionado
            if (bullet != null)
            {
                // Obtener el daño de la bala y aplicarlo a la función TakeDamage
                TakeDamage(bullet.damage);
            }
        }
    }
}
