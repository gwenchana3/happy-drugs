using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class EndScreenManager : MonoBehaviour
{
    public VideoPlayer Player;

    private void Start()
    {
        Invoke("ToMenu", (float)Player.clip.length + 0.5f);
    }

    private void ToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
