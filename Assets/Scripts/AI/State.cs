using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    // Checks if the player is within the NPC's field of view and detection range
    public bool CanSeePlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);

        // Player must be within the set viewing angle and distance to be seen
        if (angle < visAngle && direction.magnitude < visDis)
        {
            RaycastHit hit;
            // Start raycast from eye level, not feet
            Vector3 rayOrigin = npc.transform.position + Vector3.up * 1.0f;
            Vector3 rayDirection = (player.position + Vector3.up * 0.5f) - rayOrigin;

            if (Physics.Raycast(rayOrigin, rayDirection, out hit, visDis))
            {
                if (hit.collider.CompareTag("Player"))
                    return true;
            }
        }
        return false;
    }

    // Checks if the player is close enough for the NPC to attack
    public bool CanAttackPlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        if (direction.magnitude < DisShoot)
        {
            return true;
        }
        return false;
    }

    // Defines the possible states the NPC can be in
    public enum STATE
    {
        IDLE, PATROL, PURSUE, ATTACK, SLEEP
    };

    // Defines the lifecycle stages of a specific state
    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

    // Core state machine loop: processes the current lifecycle event
    public State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState; // Transition to the new state
        }

        return this; // Remain in the current state
    }

    // Called once when first transitioning into the state
    public virtual void Enter()
    {
        stage = EVENT.UPDATE;
    }

    // Called repeatedly while the state is active
    public virtual void Update()
    {
        stage = EVENT.UPDATE;
    }

    // Called once just before transitioning out of the state
    public virtual void Exit()
    {
        stage = EVENT.EXIT;
    }

    // Constructor to initialize necessary references for the state
    public State(
        GameObject _npc, UnityEngine.AI.NavMeshAgent _agent, Animator _anim, Transform _player)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        player = _player;
        stage = EVENT.ENTER; // Always start a new state in the ENTER stage
    }

    // Core state properties and references shared across all states
    public STATE name;
    protected EVENT stage;
    protected State nextState;
    protected GameObject npc;
    protected Animator anim;
    protected Transform player;
    protected UnityEngine.AI.NavMeshAgent agent;

    // AI vision and attack threshold values
    protected float visDis = 30f;
    protected float visAngle = 60f; // wider angle for game
    protected float DisShoot = 1.0f; // closer attack range
}

// Represents the default state where the NPC is waiting
public class Idle : State
{
    public Idle(
        GameObject _npc, UnityEngine.AI.NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        anim.SetTrigger("isIdle");
        base.Enter();
    }

    public override void Update()
    {
        // If the player comes into view, switch to Pursue state
        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
        // 1% chance per frame to randomly start patrolling
        else if (Random.Range(0, 100) < 1)
        {
            nextState = new Patrol(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isIdle");
        base.Exit();
    }
}

// Represents the state where the NPC moves between predefined checkpoints
public class Patrol : State
{
    int currentIndex = 1;
    float waitTimer = 0f;
    float waitTime = 2f; // seconds to wait at each checkpoint
    bool isWaiting = false;

    public Patrol(
        GameObject _npc, UnityEngine.AI.NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        name = STATE.PATROL;
        agent.speed = 2f; // Set a slower walking speed
        if (agent.isOnNavMesh)
            agent.isStopped = false;
        agent.autoBraking = true; // Smooth stop at checkpoints
    }

    public override void Enter()
    {
        anim.SetTrigger("isWalking");
        base.Enter();
    }

    public override void Update()
    {
        // Prioritize chasing the player if they are spotted
        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
            return;
        }

        // Wait at checkpoint before moving to next one
        else if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            agent.isStopped = true;
            anim.SetTrigger("isIdle"); // play idle while waiting

            if (waitTimer >= waitTime)
            {
                isWaiting = false;
                waitTimer = 0f;
                if (agent.isOnNavMesh)
                    agent.isStopped = false;
                anim.ResetTrigger("isIdle");
                anim.SetTrigger("isWalking");
            }
            return;
        }

        // Move towards the selected checkpoint
        agent.SetDestination(GameEnvironment.singleTon.CheckPoints[currentIndex].transform.position);

        // If the NPC is close to the current target checkpoint, pick the next one
        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            currentIndex = (currentIndex + 1) % GameEnvironment.singleTon.CheckPoints.Count;
            agent.ResetPath(); // clears path completely to stop momentum
            agent.velocity = Vector3.zero; // kills remaining momentum
            isWaiting = true;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isWalking");
        base.Exit();
    }
}

// Represents the chasing state when the NPC has detected the player
public class Pursue : State
{
    float losePlayerTimer = 0f;
    float losePlayerTime = 1f; // seconds before giving up chase

    public Pursue(
        GameObject _npc, UnityEngine.AI.NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        name = STATE.PURSUE;
        agent.speed = 5f; // Set a faster running speed
        if (agent.isOnNavMesh)
            agent.isStopped = false;
    }

    public override void Enter()
    {
        anim.SetTrigger("isRunning");
        npc.GetComponent<AI>().StartRunSound(); // start running sound when entering pursue state
        base.Enter();
    }

    public override void Update()
    {
        // Continuously update destination to current player position
        agent.SetDestination(player.position);

        if (agent.hasPath)
        {
            // If the player gets close enough, transition to Attack
            if (CanAttackPlayer())
            {
                nextState = new Attack(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
            // If the player escapes vision, start timer before giving up chase
            else if (!CanSeePlayer())
            {
                //losePlayerTimer += Time.deltaTime;
                //if (losePlayerTimer >= losePlayerTime)
                //{
                    nextState = new Patrol(npc, agent, anim, player);
                    stage = EVENT.EXIT;
                //}
            }
            else
            {
                //losePlayerTimer = 0f; // reset timer if she sees player again
            }
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isRunning");
        npc.GetComponent<AI>().StopRunSound(); // stop running sound when exiting pursue state
        base.Exit();
    }
}

// Represents the state when the NPC is actively engaging the player
public class Attack : State
{
    AudioManager audioManager;
    float damageTimer = 0f;
    // AudioSource shoot;

    public Attack(
        GameObject _npc, UnityEngine.AI.NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        name = STATE.ATTACK;
        // shoot = _npc.GetComponent<AudioSource>();
    }

 
    public override void Enter()
    {
        agent.isStopped = true;
        // shoot.Play();
        agent.ResetPath();           // add this to stop the momentum of the NavMeshAgent immediately when entering attack state whichh used to cause sliding
        agent.velocity = Vector3.zero; // add this
        anim.SetTrigger("isShooting");
        base.Enter();
    }

    public override void Update()
    {
        // Calculate direction to the player and smoothly rotate to face them
        Vector3 direction = player.position - npc.transform.position;
        direction.y = 0; // Prevent tilting on the Y axis
        if (direction != Vector3.zero)
        {
            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
                Quaternion.LookRotation(direction), Time.deltaTime * 2f);
        }

        // Uncomment when PlayerHealth is ready
        //damageTimer += Time.deltaTime;
        //    if (damageTimer >= 2f)
          //  {
            //    player.GetComponent<Health>().TakeDamage();
              //  damageTimer = 0f;
            //}
            damageTimer += Time.deltaTime;
        if (damageTimer >= 1.6f)
        {
            foreach (Collider player in Physics.OverlapSphere(npc.transform.position, DisShoot))
            {
                Health health = player.GetComponent<Health>();
                if (health != null)
                {
                    health.DealDamage();
                    audioManager.PlayPunchSFX(); // ✅ play punch sound on every hit
                }
            }
            damageTimer = 0f; // ✅ reset timer
        }
            foreach(Collider player in Physics.OverlapSphere(npc.transform.position, DisShoot))
            {
               Health health = player.GetComponent<Health>();
                if (health != null)
                {
                     health.DealDamage();
                }
            }

        // If the player moves out of attack range, return to pursue until they are lost
        if (!CanAttackPlayer())
        {
            nextState = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isShooting");
        // shoot.Stop();
       
        base.Exit();
    }
}