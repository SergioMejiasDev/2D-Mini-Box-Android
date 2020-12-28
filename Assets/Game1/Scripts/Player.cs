using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    float speed = 4;
    float jump = 9.5f;
    bool dontMove = true;
    bool moveLeft = false;
    
    [Header("Components")]
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    AudioSource audioSource;

    [Header("Sounds")]
    [SerializeField] AudioSource coinSound = null;
    #endregion

    private void OnEnable()
    {
        transform.position = new Vector2(-8.76f, -5.4f);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandleMoving();

        Animation();
    }

    void HandleMoving()
    {
        if (dontMove)
        {
            StopMoving();
        }

        else
        {
            if (moveLeft)
            {
                MoveLeft();
            }
            else if (!moveLeft)
            {
                MoveRight();
            }
        }
    }

    public void AllowMovement(bool leftMovement)
    {
        dontMove = false;
        moveLeft = leftMovement;
    }

    public void DontAllowMovement()
    {
        dontMove = true;
    }

    void MoveLeft()
    {
        transform.Translate(Vector2.right * -speed * Time.deltaTime);
        sr.flipX = true;
    }

    void MoveRight()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        sr.flipX = false;
    }

    void StopMoving()
    {
        transform.Translate(Vector2.right * 0 * Time.deltaTime);
    }

    void Animation()
    {
        anim.SetBool("IsWalking", (!dontMove && IsGrounded()));
        anim.SetBool("IsJumping", !IsGrounded());
    }

    public bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0, sr.bounds.extents.y + 0.01f, 0), Vector2.down, 0.1f);
        return hit.collider != null;
    }

    public void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            audioSource.Play();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((other.gameObject.CompareTag("Game1/Enemy")) || (other.gameObject.CompareTag("Game1/Missile")))
        {
            if (GameManager1.manager.isMultiplayer)
            {
                GameManager1.manager.Respawn(true);
            }
            else
            {
                gameObject.SetActive(false);
                GameManager1.manager.GameOver();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Game1/Coin"))
        {
            coinSound.Play();
            other.gameObject.SetActive(false);
            GameManager1.manager.UpdateScore(true);
        }
    }
}
