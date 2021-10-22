using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject controlUI;

    private bool controlDisplayActive = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (controlDisplayActive && Input.GetKeyDown(KeyCode.Escape))
        {
            controlUI.SetActive(false);
            controlDisplayActive = false;
        }
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void DisplayControls()
    {
        controlUI.SetActive(true);
        controlDisplayActive = true;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Scenes/septune_room");
    }
}
