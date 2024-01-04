using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    //Health
    public int maxHealth;
    public int health;

    public float rotationSpeed, jumpSpeed, gravity;
    Vector3 startDirection;
    float speedY;
    private float speed;

    private Animator anim;

    private bool right;

    //GunSystem
    //private int gunId;
    /*public GameObject GunSystem;
    private GunSystem scriptGunSystem;*/

    //Barra de vida;
    public GameObject HealthBar;
    private HealthBar scriptHealthBar;

    private bool godMode = false;

    //Dodge
    private int originalLayer;
    private int dodgeLayer;
    public float dodgeTime;
    public float dodgeWaitTime;
    private bool isDodging = false;

    //Trap
    private bool onTrap = false;
    public int graveTrapDamage;
    public int graveTrapSpeed;

    // Start is called before the first frame update
    void Start()
    {
        originalLayer = gameObject.layer;
        dodgeLayer = 8; //Dodge Layer

        anim = GetComponent<Animator>(); //componente de animacion
        anim.SetBool("Walk", false);

        startDirection = transform.position - transform.parent.position;
        startDirection.y = 0.0f;
        startDirection.Normalize();

        speedY = 0;
        speed = rotationSpeed;

        right = true;

        //scriptGunSystem = GunSystem.GetComponent<GunSystem>();

        health = maxHealth;
        scriptHealthBar = HealthBar.GetComponent<HealthBar>();
        scriptHealthBar.setMaxHealth(maxHealth);
    }

    //RECIBIR DA�O Y MORIR
    public void TakeDamage(int damage)
    {
        if (!godMode)
        {
            health -= damage;
            scriptHealthBar.SetHealth(health);
        }
        //if (health <= 0) Die();
    }

    public void GainHealth(int healthAmount)
    {
        health += healthAmount;
        if (health > maxHealth) health = maxHealth;
        scriptHealthBar.SetHealth(health);
    }

    public void Jump()
    {
        speedY = jumpSpeed;
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.P))
        {
            if (gunId == 0)
            {
                if (scriptGunSystem.selectGun(1, right)) gunId = 1;
            }
            else if (gunId == 1)
            {
                if (scriptGunSystem.selectGun(0, right)) gunId = 0;
            }
        }*/

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (godMode) godMode = false;
            else godMode = true;
        }

        if (Input.GetKey(KeyCode.T))
        {
            if (!isDodging) StartCoroutine(DoDodge());
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CharacterController charControl = GetComponent<CharacterController>();
        Vector3 position;

        // Left-right movement
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            anim.SetBool("Walk", true);
            float angle;
            Vector3 direction, target;

            position = transform.position;
            angle = speed * Time.deltaTime;
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

    /*
     * public void UnlockGun(int id)
    {
        scriptGunSystem.UnlockGun(id);
        if (scriptGunSystem.selectGun(id, right)) gunId = id;
    }

    public void addBullets(int id, int num)
    {
        scriptGunSystem.addBullets(id, num);
    }*/

    private IEnumerator DoDodge()
    {
        isDodging = true;
        godMode = true;
        gameObject.layer = dodgeLayer;
        speed = rotationSpeed + 100;

        yield return new WaitForSeconds(dodgeTime);

        gameObject.layer = originalLayer;
        speed = rotationSpeed;

        yield return new WaitForSeconds(dodgeWaitTime);
        isDodging = false;
    }

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

        if (other.CompareTag("Grave"))
        {
            speed = graveTrapSpeed;
            StartCoroutine(DamageOnTrap());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grave"))
        {
            onTrap = false;
            speed = rotationSpeed;
        }
    }

    private IEnumerator DamageOnTrap()
    {
        onTrap = true;
        while (onTrap)
        {
            TakeDamage(graveTrapDamage);
            yield return new WaitForSeconds(1f);
        }
    }
}
