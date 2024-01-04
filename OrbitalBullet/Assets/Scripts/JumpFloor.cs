using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpFloor : MonoBehaviour
{

    public GameObject tape; // Asigna el objeto en el inspector de Unity
    public float height;
    public float movespeed;
    public float z;
    public string type;
    public GameObject floatingText;

    // Start is called before the first frame update
    void Start()
    {
             floatingText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.CompareTag("Player"))
        {
            floatingText.SetActive(true);
        }
    }

    void OnTriggerStay(Collider obj) {
                   
                   
        if (Input.GetKey(KeyCode.J)) StartCoroutine(JumpAndActivate(obj.GetComponent<MovePlayer>()));


    }


    IEnumerator JumpAndActivate(MovePlayer movePlayer)
    {

        tape.SetActive(false);
        if (type == "normal") {
            movePlayer.jumpFloor(height, movespeed, z, type);
            yield return new WaitForSeconds(height/movespeed);
            tape.SetActive(true);
        }
        else if (type == "elevator") {
            //esperamos primer mov (hacia dentro ascensor)
            movePlayer.jumpFloor(height, movespeed, z, type);
            yield return new WaitForSeconds(3);
            //pasamos a mov normal (hacia arriba)
            type = "normal";
            movespeed = 1f;
            movePlayer.jumpFloor(height, movespeed, z, type);
            yield return new WaitForSeconds(height/movespeed);
            //mov hacia fuera
            type = "elevator2";
            movespeed = 1f;
            movePlayer.jumpFloor(height, movespeed, z, type);
            yield return new WaitForSeconds(2);
        }

        else if (type == "elevator3") {
            Debug.Log("Objeto ha llegado al punto final.");

            movePlayer.jumpFloor(height, movespeed, z, type);
            yield return new WaitForSeconds(3);
        }
    
    }
        
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            floatingText.SetActive(false);
        }
    }

}
