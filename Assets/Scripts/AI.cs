using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent agent;
    Animator anim;
    public Transform player;
    State currentState;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = this.GetComponent<Animator>();
        currentState = new Idle(this.gameObject, agent, anim, player);
        Debug.Log("Checkpoints found: " + GameEnvironment.singleTon.CheckPoints.Count);
    }

    public float DamagePoints = 10f;
    public bool isAttacking = false;
    private void OnTriggerStay(Collider other)
    {
        if (!isAttacking) return;
        Health H = other.GetComponent<Health>();
        if (H == null) return; 
        H.HealthPoints -= DamagePoints * Time.deltaTime; //damage over time while player is in range of the enemy
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Process();
    }
}
