using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerManager : MonoBehaviour
{
    public DrugType DemandedDrug;

    [HideInInspector]
    public Transform DestinationPoint;
    [HideInInspector]
    public Transform ExitPoint;
    private NavMeshAgent _navAgent;

    private void Awake()
    {
        _navAgent = GetComponentInChildren<NavMeshAgent>();
    }

    private void Start()
    {
        EnterRoom();
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
        _navAgent.SetDestination(DestinationPoint.position);
    }

    public void LeaveRoom()
    {
        _navAgent.SetDestination(ExitPoint.position);
    }
    
    public void CommitSuicide()
    {
        // TODO: Go down the Suisilde
    }
}
