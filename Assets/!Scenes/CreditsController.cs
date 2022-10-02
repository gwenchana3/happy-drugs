using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CreditsController : MonoBehaviour
{
    public VideoPlayer VideoPlayer;
    public void ToggleWindow(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.active);

        if (!gameObject.active)
            VideoPlayer.Stop();
        else
            VideoPlayer.Play();
    }
}
