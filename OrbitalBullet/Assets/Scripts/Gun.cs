using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int maxCapacity;
    private int numBullets;

    public GameObject Bullet;
    public Transform startPoint;
    private bool direction;

    //public GameObject GunDisplayObject;
    //private GunDisplay scriptGunDisplay;

    public float bulletSpeed;
    public float bulletLifeTime;
    public int bulletDamage;

    public bool available = false;

    public void Start()
    {
        direction = true;
        numBullets = maxCapacity;

        /*scriptGunDisplay = GunDisplayObject.GetComponent<GunDisplay>();
        scriptGunDisplay.UpdateBulletsCount(numBullets);*/
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            direction = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction = false;
        }
        // Verificar si se presiona la tecla "G"
        if (Input.GetKeyDown(KeyCode.G))
        {
            //Debug.Log("G apretada");
            if (numBullets > 0)
            {
                // Instanciar una nueva bala en el punto de disparo
                GameObject newBullet = Instantiate(Bullet, startPoint.position, startPoint.rotation);

                // Obtener el componente Bullet de la nueva bala
                Bullet scriptBullet = newBullet.GetComponent<Bullet>();

                // Verificar si el componente existe antes de activar la bala
                if (scriptBullet != null)
                {
                    // Activar la bala
                    scriptBullet.SetDamage(bulletDamage);
                    scriptBullet.SetSpeed(bulletSpeed);
                    scriptBullet.SetLifeTime(bulletLifeTime);

                    scriptBullet.ShootBullet(direction);
                }
                --numBullets;
                //scriptGunDisplay.UpdateBulletsCount(numBullets);
            }
        }
    }

    public void addBullets(int num)
    {
        numBullets += num;
        if (numBullets > maxCapacity) numBullets = maxCapacity;
        //scriptGunDisplay.UpdateBulletsCount(numBullets);
    }

    public void setStartDirection(bool d)
    {
        direction = d;
    }

    public void setAvailable()
    {
        available = true;
        //GunDisplayObject.SetActive(true);
    }
}
