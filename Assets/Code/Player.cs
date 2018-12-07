using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] bool isCircle;
    [SerializeField] float changeLerpMultiplier = 1;
    [SerializeField] float changeSpeed = 1;
    [SerializeField, Range(0, 10)] float circleGravity = 1;
    [SerializeField, Range(0, 10)] float squareGravity = 1;
    [SerializeField] float torque = 1;
    [SerializeField] float respawnDelay = 2.5f;
    [SerializeField] float bounceSpeed = 1;
    [SerializeField] ParticleSystem dieEffect;
    [SerializeField] ParticleSystem endEffect;
    [SerializeField] AudioSource deathSound;
    [SerializeField] AudioSource jumpSound;
    [SerializeField] AudioSource collisionSound;

    TrailRenderer trail;
    ScaleAnimator scaleAnimator;
    Rigidbody2D rb;
    Shape shape;
    PolygonCollider2D polygonCollider;
    CircleCollider2D circleCollider;
    Checkpoint lastCheckpoint;
    bool alive;
    public static Player Ref { get; private set; }
    float TargetCirclePercent
    {
        get { return isCircle ? 1 : 0; }
    }

    private void Awake()
    {
        Ref = this;
        trail = GetComponentInChildren<TrailRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        scaleAnimator = GetComponent<ScaleAnimator>();
        shape = GetComponent<Shape>();
        shape.CirclePercent = TargetCirclePercent;
    }

    private void OnEnable()
    {
        alive = true;
    }

    void Update()
    {
        if (alive && Input.GetMouseButtonDown(0))
        {
            rb.AddTorque((isCircle ? -1 : 1) * torque, ForceMode2D.Force);
            isCircle ^= true;
            scaleAnimator.Bump();
        }

        shape.CirclePercent = Mathf.Lerp(shape.CirclePercent, TargetCirclePercent, changeLerpMultiplier * Time.deltaTime);
        shape.CirclePercent = Mathf.MoveTowards(shape.CirclePercent, TargetCirclePercent, changeSpeed * Time.deltaTime);
        circleCollider.enabled = isCircle;// shape.CirclePercent >= 1;
        polygonCollider.enabled = !circleCollider.enabled;
        rb.gravityScale = Mathf.Lerp(squareGravity, circleGravity, shape.CirclePercent);


        if (Input.GetKey(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    //private void OnBecameInvisible()
    //{
    //    if (gameObject.activeInHierarchy)
    //        Die();
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent(typeof(Checkpoint)))
        {
            lastCheckpoint = collision.GetComponent<Checkpoint>();
        }
        else if (collision.GetComponent(typeof(Obstacle)))
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCircle && alive)
        {
            var velocity = rb.velocity;
            velocity.y += bounceSpeed;
            rb.velocity = velocity;
            PlaySound(jumpSound);
        }
        else if (collisionSound.isPlaying == false)
        {
            PlaySound(collisionSound);
        }
    }

    void Die()
    {
        if (alive == false) return;
        dieEffect.Play();
        trail.emitting = alive = false;
        scaleAnimator.Shrink();
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        Invoke("Respawn", respawnDelay);
        PlaySound(deathSound);
    }

    void Respawn()
    {
        if (lastCheckpoint)
        {
            transform.position = lastCheckpoint.transform.position;
        }
        rb.isKinematic = false;
        scaleAnimator.ResetScale();
        trail.emitting = alive = true;
    }

    void PlaySound(AudioSource source)
    {
        //source.pitch = Random.Range(.9f, 1.1f);
        source.volume = Random.Range(.9f, 1.1f);
        source.Play();
    }

    public void PlayEnd()
    {
        endEffect.Play();
    }
}
