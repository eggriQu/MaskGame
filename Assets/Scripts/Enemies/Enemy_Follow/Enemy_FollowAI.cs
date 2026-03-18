using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_FollowAI : MonoBehaviour
{
    private AnimationCurve curveX;
    private AnimationCurve curveY;
    
    private bool isFollowing;

    private float currentTime;

    [SerializeField] private float timeUntilSpawn;
    [SerializeField] GameObject player;

    private void Awake()
    {
        curveX = new AnimationCurve();
        curveY = new AnimationCurve();
        isFollowing = false;
        this.gameObject.transform.position = player.transform.position;
    }
    private void FixedUpdate()
    {
        curveX.AddKey(Time.time, player.transform.position.x);
        curveY.AddKey(Time.time ,player.transform.position.y);

        if (isFollowing)
        {
            this.gameObject.transform.position = new Vector3(curveX.Evaluate(currentTime), curveY.Evaluate(currentTime), 0);
        }
    }
    private void Update()
    {
        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn <= 0)
        {
            isFollowing = true;
            currentTime += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //TEMP---------------------------------
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
