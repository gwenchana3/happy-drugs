using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeController : MonoBehaviour
{
    public TextMeshProUGUI Counter;
    private SpawnController _spawner;

    void Start()
    {
        _spawner = Manager.Use<PlayManager>().Spawner;
        UpdateUI();
    }

    public void UpdateUI()
    {
        Counter.text = (_spawner.SpawnAmmount - _spawner.SpawnCounter ).ToString();
    }
}
