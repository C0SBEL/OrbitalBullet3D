using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    //Vida enemigo
    public int maxHealth;
    private int health;

    public int maxShield;
    private int shield;

    public float speed; // Velocidad de la bala
    public GameObject center;
    private bool moving = false;

    public float shootingTime;
    private float time;

    public GameObject LanternObject;
    private Lantern scriptLantern;
    public Transform startPoint;

    //Animator ghostAnimator;
    private bool holding = false;

    //Barra de vida;
    public GameObject HealthBar;
    private HealthBar scriptHealthBar;

    public GameObject ShieldBar;
    private HealthBar scriptShieldBar;

    //Detectar player
    public float distanceRange;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        shield = maxShield;

        scriptHealthBar = HealthBar.GetComponent<HealthBar>();
        scriptHealthBar.setMaxHealth(maxHealth);
        HealthBar.SetActive(false);

        scriptShieldBar = ShieldBar.GetComponent<HealthBar>();
        scriptShieldBar.setMaxHealth(maxShield);

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
            StartCoroutine(MoveGhost());
        }
    }

    //RECIBIR DAÑO Y MORIR
    public void TakeDamage(int damage)
    {
        shield -= damage;
        if (shield > 0)
        {
            //Debug.Log("Shield");
            scriptShieldBar.SetHealth(shield);
        }
        else if (shield <= 0 && ShieldBar.activeSelf)
        {
            HealthBar.SetActive(true);
            ShieldBar.SetActive(false);
        }
        else if (shield <= 0)
        {
            //Debug.Log("Health");
            health -= damage;
            scriptHealthBar.SetHealth(health);
        }
        if (health <= 0) Die();
    }

    private void Die()
    {
        StopCoroutine(MoveGhost());

        if (scriptLantern != null)
        {
            // Activar la bala
            scriptLantern.DisableLantern();
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

    IEnumerator MoveGhost()
    {
        while (moving)
        {
            //Gira hacia la izquierda, para cambiar a la derecha poner la speed a negativo
            transform.RotateAround(center.transform.position, new(0, 1, 0), -speed * Time.deltaTime);

            //Miro si el jugador está en rango y no sujetamos nada
            /*if (!holding && JugadorEnRango())
            {
                ghostAnimator.SetBool("Hold", true);
            }*/

            if (!holding /*&& ghostAnimator.GetBool("Hold")*/)
            {
                holding = true;
                time = 0;
                // Instanciar una nueva bala en el punto de disparo
                GameObject newLantern = Instantiate(LanternObject, startPoint.position, startPoint.rotation);

                // Obtener el componente Bullet de la nueva bala
                scriptLantern = newLantern.GetComponent<Lantern>();
                scriptLantern.setStartPoint(startPoint);
            }

            if (time >= shootingTime) ShootLantern();

            yield return null;
        }

        //transform.position = startPoint.position;
    }

    bool JugadorEnRango()
    {
        // Verificar si el player está dentro de un rango de ataque
        if (player != null)
        {
            float distanciaAlJugador = Vector3.Distance(transform.position, player.position);
            return distanciaAlJugador < distanceRange;
        }

        return false;
    }

    void ShootLantern()
    {
        time = 0;
        // Verificar si el componente existe antes de activar la bala
        if (scriptLantern != null && player != null)
        {
            // Activar la bala
            scriptLantern.ShootLantern(player.transform);
        }

        //ghostAnimator.SetBool("Hold", false);
        holding = false;
    }

    //TRIGGERS Y COLISIONES//
    void OnTriggerEnter(Collider other)
    {
        // Verificar colisión con otros objetos y realizar las acciones necesarias
        if (other.CompareTag("Object"))
        {
            speed *= -1;
            //transform.Rotate(0f, 180f, 0f);
        }

        if (other.CompareTag("Bullet"))
        {
            //Debug.Log("GHOST: Bala detectada");
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
