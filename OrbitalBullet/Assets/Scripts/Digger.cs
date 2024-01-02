using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger : MonoBehaviour
{
    //Vida enemigo
    public int maxHealth;
    private int health;

    public float speed; // Velocidad de la bala
    public GameObject center;
    private bool moving = false;

    public float shootingTime;
    private float time;

    public GameObject PumpkinObject;
    //private Pumpkin scriptPumpkin;
    public Transform startPoint;

    //Barra de vida;
    public GameObject HealthBar;
    private HealthBar scriptHealthBar;

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
            StartCoroutine(MoveDigger());
        }
    }

    //RECIBIR DAÑO Y MORIR
    public void TakeDamage(int damage)
    {
        health -= damage;
        scriptHealthBar.SetHealth(health);
        if (health <= 0) Die();
    }

    private void Die()
    {
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

    IEnumerator MoveDigger()
    {
        while (moving)
        {
            //Gira hacia la izquierda, para cambiar a la derecha poner la speed a negativo
            transform.RotateAround(center.transform.position, new(0, 1, 0), speed * Time.deltaTime);

            if (time >= shootingTime) PlantPumpking();

            yield return null;
        }

        //transform.position = startPoint.position;
    }

    void PlantPumpking()
    {
        time = 0;

        GameObject newPumpkin = Instantiate(PumpkinObject, startPoint.position, startPoint.rotation);
        Pumpkin scriptPumpkin = newPumpkin.GetComponent<Pumpkin>();

        if (scriptPumpkin != null)
        {
            scriptPumpkin.Plant();
        }
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
            Debug.Log("GHOST: Bala detectada");
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
