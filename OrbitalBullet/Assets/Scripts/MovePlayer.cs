using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    //Health
    public int maxHealth;
    public int health;


        public FollowPlayer cameraScript; // Referencia a la cámara

    public float rotationSpeed, jumpSpeed, gravity;
    Vector3 startDirection;
    float speedY;

    private Animator anim;

    private bool right;
    /*public GameObject GunSystem;
    private GunSystem scriptGunSystem;*/

    //Barra de vida;
    public GameObject HealthBar;
    private HealthBar scriptHealthBar;

    Transform startPoint; // Punto inicial
    Transform endPoint;   // Punto final
    private float startTime;     // Tiempo inicial
    private float journeyLength; // Longitud del recorrido
    bool jump;


    // Start is called before the first frame update
    void Start()
    {
        jump = false;
        anim = GetComponent<Animator>(); //componente de animacion
        anim.SetBool("Walk", false);

        startDirection = transform.position - transform.parent.position;
        startDirection.y = 0.0f;
        startDirection.Normalize();

        speedY = 0;

        right = true;

        //scriptGunSystem = GunSystem.GetComponent<GunSystem>();

        health = maxHealth;
        scriptHealthBar = HealthBar.GetComponent<HealthBar>();
        scriptHealthBar.setMaxHealth(maxHealth);
    }

        //SALTAR A LA SGUIENTE PLANTA
    public void jumpFloor(float height, float movespeed, float z, string type) {
        jump = true;
        startTime = Time.time;
        Vector3 va = transform.position;
        if (type == "normal") {
            anim.SetBool("Walk", false);
            Vector3 vb = new Vector3(va.x, va.y + height, va.z + z);
            journeyLength = Vector3.Distance(va, vb);
            StartCoroutine(MoveObject(va, vb, movespeed, type));
        }
        else if (type == "elevator") {
            Quaternion targetRotation = Quaternion.Euler(0, 72, 0);
            transform.rotation = targetRotation;
            anim.SetBool("Walk", true);
            Vector3 vb = new Vector3(va.x+1.2f,va.y,va.z+0.5f);
            journeyLength = Vector3.Distance(va, vb);
            StartCoroutine(MoveObject(va, vb, movespeed, type));
        }
        else if (type == "elevator2") {
            Quaternion targetRotation = Quaternion.Euler(0, -89, 0);
            transform.rotation = targetRotation;
            anim.SetBool("Walk", true);
            Vector3 vb = new Vector3(va.x-4.136f,va.y,va.z-2.278f);
            journeyLength = Vector3.Distance(va, vb);
            StartCoroutine(MoveObject(va, vb, movespeed, type));
        }
        else if (type == "elevator3") {
            Quaternion targetRotation = Quaternion.Euler(0, 54, 0);
            transform.rotation = targetRotation;
            anim.SetBool("Walk", true);
            Vector3 vb = new Vector3(-2.531f,va.y,-4.037f);
            journeyLength = Vector3.Distance(va, vb);
            StartCoroutine(MoveObject(va, vb, movespeed, type));
        }
    }

    IEnumerator MoveObject(Vector3 start, Vector3 end, float movespeed, string type) {
        while (Vector3.Distance(transform.position, end) > 0.01f) {
            float distCovered = (Time.time - startTime) * movespeed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(start, end, fracJourney);
            cameraScript.Update();
            yield return null;
        }
        //  Debug.Log("Objeto ha llegado al punto final.");
        if (type == "normal") {
            jump = false;

        }

        else if (type == "elevator") {
            anim.SetBool("Walk", false);
            Quaternion targetRotation = Quaternion.Euler(0, 250, 0);
            transform.rotation = targetRotation;

        }

        else if (type == "elevator2") {
            anim.SetBool("Walk", false);
            jump = false;
        }

        else if (type == "elevator3") {
            anim.SetBool("Walk", false);
            jump = false;
        }
    }


    //RECIBIR DA�O Y MORIR
    public void TakeDamage(int damage)
    {
        health -= damage;
        scriptHealthBar.SetHealth(health);
        //if (health <= 0) Die();
    }

    public void GainHealth(int healthAmount)
    {
        health += healthAmount;
        if (health > maxHealth) health = maxHealth;
        scriptHealthBar.SetHealth(health);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CharacterController charControl = GetComponent<CharacterController>();
        Vector3 position;
        if (!jump) {
             /*if (Input.GetKey(KeyCode.Alpha1))
        {
            scriptGunSystem.selectGun(0, right);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            scriptGunSystem.selectGun(1, right);
        }*/

        if (Input.GetKey(KeyCode.H)) TakeDamage(25);

        // Left-right movement
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D )))
        {
            anim.SetBool("Walk", true);
            float angle;
            Vector3 direction, target;

            position = transform.position;
            angle = rotationSpeed * Time.deltaTime;
            direction = position - transform.parent.position;
            if (Input.GetKey(KeyCode.A))
            {
                right = false;
                target = transform.parent.position + Quaternion.AngleAxis(angle, Vector3.up) * direction;
                if (charControl.Move(target - position) != CollisionFlags.None)
                {
                    transform.position = position;
                    Physics.SyncTransforms();
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                right = true;
                target = transform.parent.position + Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                if (charControl.Move(target - position) != CollisionFlags.None)
                {
                    transform.position = position;
                    Physics.SyncTransforms();
                }
            }
        }

        else anim.SetBool("Walk", false);

        // Correct orientation of player
        // Compute current direction
        Vector3 currentDirection = transform.position - transform.parent.position;
        currentDirection.y = 0.0f;
        currentDirection.Normalize();
        // Change orientation of player accordingly
        Quaternion orientation;
        if (!right)
        {
            if ((startDirection - currentDirection).magnitude < 1e-3)
                orientation = Quaternion.AngleAxis(0.0f, Vector3.up);
            else if ((startDirection + currentDirection).magnitude < 1e-3)
                orientation = Quaternion.AngleAxis(180.0f, Vector3.up);
            else
                orientation = Quaternion.FromToRotation(startDirection, currentDirection);
        }
        else
        {
            if ((startDirection - currentDirection).magnitude < 1e-3)
                orientation = Quaternion.AngleAxis(180.0f, Vector3.up);
            else if ((startDirection + currentDirection).magnitude < 1e-3)
                orientation = Quaternion.AngleAxis(0.0f, Vector3.up);
            else
                orientation = Quaternion.FromToRotation(startDirection * -1.0f, currentDirection);
        }
        transform.rotation = orientation;

        // Apply up-down movement
        position = transform.position;
        if (charControl.Move(speedY * Time.deltaTime * Vector3.up) != CollisionFlags.None)
        {
            transform.position = position;
            Physics.SyncTransforms();
        }
        if (charControl.isGrounded)
        {
            if (speedY < 0.0f)
                speedY = 0.0f;
            if (Input.GetKey(KeyCode.W))
                speedY = jumpSpeed;
        }
        else
            speedY -= gravity * Time.deltaTime;
        }
    }

    /*
     * public void UnlockGun(int id)
    {
        scriptGunSystem.UnlockGun(id);
        scriptGunSystem.selectGun(id, right);
    }

    public void addBullets(int id, int num)
    {
        scriptGunSystem.addBullets(id, num);
    }*/

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cross"))
        {
            //Debug.Log("GHOST: Bala detectada");
            // Acceder al componente BulletC de la bala que ha colisionado
            Cross cross = other.GetComponent<Cross>();

            // Verificar si el componente Bala existe en el objeto colisionado
            if (cross != null)
            {
                // Obtener el da�o de la bala y aplicarlo a la funci�n TakeDamage
                TakeDamage(cross.damage);
            }
        }
    }
}
