using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioSource audioSource;

    void Start() {
      audioSource = GetComponent<AudioSource>();
      audioSource.Play();
    }

    public void Play(string level) // aqui hi anira es nom des nivell, important q sigui identicament es mateix
    {
        SceneManager.LoadScene(level); //string des nom de dalt identic

    }

    public void Salir()
    {
        Application.Quit();
        //Debug.Log("S ha tancat es joc"); //per veure que realment funciona es boto de sortir
    }
}
