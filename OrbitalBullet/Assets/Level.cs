using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource audioSource;

    void Start()
    {
      audioSource = GetComponent<AudioSource>();
      audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
