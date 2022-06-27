using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Level 1");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) SceneManager.LoadScene("Level 1"); ;
    }
}
