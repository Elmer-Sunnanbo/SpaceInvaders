using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) //Start the game when the player hits space
        {
            SceneManager.LoadScene("SampleScene");
        }
        if (Input.GetKeyDown(KeyCode.Escape)) //Closes the game when the player hits esc
        {
            Application.Quit();
        }
    }
}
