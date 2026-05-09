using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent agent;
    Animator anim;
    public Transform player;
    State currentState;

    private AudioSource enemyRunSound;

    // ADD: walking sound clip
    public AudioClip walkSound;

    // ADD: running sound clip
    public AudioClip runSound;

    void Awake() //  change Start to Awake for enemyRunSound
    {
        enemyRunSound = this.GetComponent<AudioSource>();

        // ADD: configure enemy sound source for 3D distance fading
        enemyRunSound.spatialBlend = 1f;   // full 3D sound
        enemyRunSound.loop = true;         // movement sounds loop
        enemyRunSound.playOnAwake = false; // do not play automatically
        enemyRunSound.minDistance = 2f;    // loud when near
        enemyRunSound.maxDistance = 20f;   // fade when far
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = this.GetComponent<Animator>();
        currentState = new Idle(this.gameObject, agent, anim, player);
        //Debug.Log("Checkpoints found: " + GameEnvironment.singleTon.CheckPoints.Count);
    }

    public void StartRunSound()
    {
        // ADD: switch to running sound if not already playing
        if (enemyRunSound.clip != runSound)
        {
            enemyRunSound.clip = runSound;
            enemyRunSound.Play();
        }

        if (!enemyRunSound.isPlaying)
            enemyRunSound.Play(); // ✅ start running sound
    }

    // ADD: start walking sound for patrol movement
    public void StartWalkSound()
    {
        if (enemyRunSound.clip != walkSound)
        {
            enemyRunSound.clip = walkSound;
            enemyRunSound.Play();
        }

        if (!enemyRunSound.isPlaying)
            enemyRunSound.Play(); // ✅ start walking sound
    }

    public void StopRunSound()
    {
        if (enemyRunSound.isPlaying)
            enemyRunSound.Stop(); // ✅ stop running sound
    }

    // ADD: stop any movement sound (walk/run)
    public void StopMoveSound()
    {
        if (enemyRunSound.isPlaying)
            enemyRunSound.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Process();
    }
}