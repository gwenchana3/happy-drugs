using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class SpawnController : MonoBehaviour
{
    private static SpawnController _instance;

    public SpawnType SpawnMode;

    public int SpawnAmmount;

    public int GameHP { get; private set; }

    private int _spawnCounter;

    public static SpawnController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SpawnController>();
            }

            return _instance;
        }
    }

    public CustomerManager[] CustomerTemplates;
    public DestinationPoint[] Destinations;

    private System.Random _random = new System.Random();
    private float _lastTime;
    public float SpawnTime;

    private void Awake()
    {
        _lastTime = Time.time - SpawnTime;
    }

    public void Spawn()
    {
        CustomerManager thisCustomer = Instantiate(CustomerTemplates[_random.Next(0, CustomerTemplates.Length)], 
            transform.position, transform.rotation);

        try { thisCustomer.DestinationPoint = Destinations.First(o => o.IsOccupied == false); }
        catch { Destroy(thisCustomer.gameObject); return; }
        thisCustomer.EnterRoom();
    }

    public void DecreaseHP(int ammount)
    {
        GameHP -= ammount;
    }

    private void EndGame(bool isWin)
    {
        switch(isWin)
        {
            case true:
                SceneManager.LoadScene("WinScreenScene");
                return;
            case false:
                SceneManager.LoadScene("LoseSceneScreen");
                return;
        }
    }

    public void Update()
    {
        if (SpawnAmmount >= _spawnCounter)
        {
            switch (SpawnMode)
            {
                case SpawnType.OneAtATime:
                    if (!FindObjectOfType<CustomerManager>())
                        Spawn();
                    break;
                case SpawnType.TimedSpawn:
                    if (Time.time >= _lastTime + SpawnTime)
                    {
                        Spawn();
                        _lastTime = Time.time;
                    }
                    break;
            }
        }
        else if(!FindObjectOfType<CustomerManager>())
        {
            EndGame(true);
        }

        if(GameHP <= 0)
        {
            EndGame(false);
        }
    }
}
