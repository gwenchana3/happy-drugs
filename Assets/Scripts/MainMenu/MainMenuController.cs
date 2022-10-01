using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string GameScene;

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(GameScene);
    }

    public void ToggleWindow(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.active);
    }
}
