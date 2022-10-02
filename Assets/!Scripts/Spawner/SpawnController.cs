using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnController : MonoBehaviour
{
    private static SpawnController _instance;

    public SpawnType SpawnMode;

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
    public Transform ExitPoint;

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
        thisCustomer.ExitPoint = ExitPoint;
        thisCustomer.EnterRoom();
    }

    public void Update()
    {
        switch(SpawnMode)
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
}
