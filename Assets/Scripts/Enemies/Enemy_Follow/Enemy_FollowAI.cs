using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_FollowAI : MonoBehaviour, ILevelObject
{
    private AnimationCurve curveX;
    private AnimationCurve curveY;
    
    private bool isFollowing;
    [SerializeField] private bool isStunned;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private Enemy_FollowAI cloneAI;
 
    private float playbackTime;
    private float recordTime;

    public float timeUntilSpawn;
    [SerializeField] GameObject player;
    [SerializeField] private Animator anim;

    private void Awake()
    {
        curveX = new AnimationCurve();
        curveY = new AnimationCurve();
        isFollowing = false;

        player = GameObject.Find("Player");
    }
    private void FixedUpdate()
    {
        if (PauseManager.isGamePaused || PauseManager.isLevelPaused ) return;
        curveX.AddKey(recordTime, player.transform.position.x);
        curveY.AddKey(recordTime ,player.transform.position.y);

        Vector3 endVector = new Vector3(curveX.Evaluate(playbackTime), curveY.Evaluate(playbackTime), 0);

        if (isFollowing)
        {
            this.gameObject.transform.position = endVector;
        }
        else
        {
            
        }
    }
    private void Update()
    {
        if (PauseManager.isGamePaused || PauseManager.isLevelPaused ) return;
        recordTime += Time.deltaTime;
        if (timeUntilSpawn <= 0 && !isStunned)
        {
            isFollowing = true;
            playbackTime += Time.deltaTime;
        }
        else if (timeUntilSpawn > 0 && !isStunned)
        {
            timeUntilSpawn -= Time.deltaTime;
        }
        else if (isStunned)
        {
            isFollowing = false;
        }
    }

    private IEnumerator StunRoutine()
    {
        isStunned = true;
        anim.Play("Enemy_Stunned");
        yield return new WaitForSeconds(2);
        cloneAI = Instantiate(clonePrefab, new Vector3(-20, 15, 0), new Quaternion(0, 0, 0, 0)).GetComponent<Enemy_FollowAI>();
        cloneAI.timeUntilSpawn = 3;
        cloneAI.isStunned = false;
        cloneAI.isFollowing = false;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerContact(other.gameObject.GetComponent<PlayerController>());
        }
        else if (other.CompareTag("Projectile") && !isStunned)
        {
            StartCoroutine(StunRoutine());
        }
    }

    public virtual void OnPlayerContact(PlayerController player)
    {
        if (!player.isDead && timeUntilSpawn <= 0 && !isStunned)
        {
            StartCoroutine(player.Die());
        }
    }
}
