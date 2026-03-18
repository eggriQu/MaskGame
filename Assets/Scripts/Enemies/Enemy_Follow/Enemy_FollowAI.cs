using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_FollowAI : MonoBehaviour, ILevelObject
{
    private AnimationCurve curveX;
    private AnimationCurve curveY;
    
    private bool isFollowing;

    private float playbackTime;
    private float recordTime;

    public float timeUntilSpawn;
    [SerializeField] GameObject player;

    private void Awake()
    {
        curveX = new AnimationCurve();
        curveY = new AnimationCurve();
        isFollowing = false;
    }
    private void FixedUpdate()
    {
        if (PauseManager.isGamePaused || PauseManager.isLevelPaused ) return;
        curveX.AddKey(recordTime, player.transform.position.x);
        curveY.AddKey(recordTime ,player.transform.position.y);

        if (isFollowing)
        {
            this.gameObject.transform.position = new Vector3(curveX.Evaluate(playbackTime), curveY.Evaluate(playbackTime), 0);
        }
    }
    private void Update()
    {
        if (PauseManager.isGamePaused || PauseManager.isLevelPaused ) return;
        recordTime += Time.deltaTime;
        if (timeUntilSpawn <= 0)
        {
            isFollowing = true;
            playbackTime += Time.deltaTime;
        }
        else
        {
            timeUntilSpawn -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerContact(other.gameObject.GetComponent<PlayerController>());
        }
    }

    public virtual void OnPlayerContact(PlayerController player)
    {
        if (!player.isDead && timeUntilSpawn <= 0)
        {
            StartCoroutine(player.Die());
        }
    }
}
