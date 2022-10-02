using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerManager : MonoBehaviour
{
    public DrugType DemandedDrug;

    [HideInInspector]
    public DestinationPoint DestinationPoint;
    [HideInInspector]
    public Transform ExitPoint;
    private NavMeshAgent _navAgent;
    private bool _leaving;

    private void Awake()
    {
        _navAgent = GetComponentInChildren<NavMeshAgent>();
    }

    private void Start()
    {
        EnterRoom();
    }

    private void Update()
    {
        if (_leaving && !_navAgent.hasPath)
            Destroy(this.gameObject);
    }

    public void ReceiveDrug(Drug drug)
    {
        if(drug.drugType != DemandedDrug)
        {
            CommitSuicide();
        }

        else
        {
            LeaveRoom();
        }
    }

    public void EnterRoom()
    {
        _navAgent.SetDestination(DestinationPoint.transform.position);
        DestinationPoint.IsOccupied = true;
    }

    public void LeaveRoom()
    {
        _navAgent.SetDestination(ExitPoint.position);
        DestinationPoint.IsOccupied = false;
        _leaving = true;
    }
    
    public void CommitSuicide()
    {
        // TODO: Go down the Suisilde
    }
}
