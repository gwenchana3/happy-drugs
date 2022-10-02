using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenImageManager : MonoBehaviour
{
    private void Start()
    {
        Invoke("ToMenu", 4);
    }

    private void ToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
