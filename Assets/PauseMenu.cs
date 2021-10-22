using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    private bool isShowingControls;

    public static bool IsMuted = false;

    public GameObject pauseMenuUI;

    public GameObject controlUI;

    public JukeBox jukebox;
    public spriteSwap audioUI;
    
    [FormerlySerializedAs("slider")] public Slider pixelSlider;
    public Slider volumeSlider;
    
    public PixelBoy cameraScript;

    public ToolManager toolManager;

    public GameObject GNA_UI;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Resume();
    }
    
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isShowingControls)
            {
                HideControls();
                return;
            }
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
            
        }
    }

    private void Pause()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        GNA_UI.SetActive(false);
        //Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        //Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void LoadMenu()
    {
        //Time.timeScale = 1f;
        SceneManager.LoadScene("Scenes/main_menu");
    }

    public void ToggleSound()
    {
        IsMuted = !IsMuted;
        
        if (IsMuted)
        {
            jukebox.PauseSong();
        }
        else
        {
            jukebox.PlaySong();
        }
        audioUI.UpdateSprite();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ShowControls()
    {
        isShowingControls = true;
        controlUI.SetActive(true);
    }

    public void UpdateCameraResolution()
    {
        cameraScript.h = (int)pixelSlider.value;
    }

    public void UpdateVolume()
    {
        jukebox.SetVolume(volumeSlider.value);
    }

    public void ToggleMinimap()
    {
        toolManager.ToggleRadarVisibility();
    }

    public void PlayNextSong()
    {
        jukebox.NextSong();
    }

    private void HideControls()
    {
        isShowingControls = false;
        controlUI.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
