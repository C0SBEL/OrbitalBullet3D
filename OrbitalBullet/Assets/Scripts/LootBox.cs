using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBox : MonoBehaviour
{
    public int value;
    public string type;

    public GameObject floatingText;
    public GameObject lid;

    public bool empty;
    // Start is called before the first frame update
    void Start()
    {
        floatingText.SetActive(false);
        lid.SetActive(true);

        empty = false;
    }

    void Update()
    {
        floatingText.transform.forward = Camera.main.transform.forward;
    }

    public void setType(string t)
    {
        type = t;
    }

    public void setValue(int v)
    {
        value = v;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !empty)
        {
            floatingText.SetActive(true);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !empty)
        {
            if (Input.GetKey(KeyCode.Y))
            {
                empty = true;
                lid.SetActive(false);
                floatingText.SetActive(false);
                if (type == "Health") other.GetComponent<MovePlayer>().GainHealth(value);
                //else if (type == "Gun") other.GetComponent<MovePlayer>().UnlockGun(value);
                //else if (type == "ShortBullets") other.GetComponent<MovePlayer>().addBullets(0, value);
                //else if (type == "LongBullets") other.GetComponent<MovePlayer>().addBullets(1, value);
            }
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
