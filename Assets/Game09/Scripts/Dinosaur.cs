using UnityEngine;

/// <summary>
/// Class that controls the main functions of the dinosaur.
/// </summary>
public class Dinosaur : MonoBehaviour
{
    [Header("Movement")]
    float jumpForce = 18;
    [SerializeField] LayerMask groundMask = 0;
    bool isDie = false;

    [Header("Colliders")]
    [SerializeField] BoxCollider2D standingCollider = null;
    [SerializeField] BoxCollider2D crouchCollider = null;

    [Header("Components")]
    bool isCrouch;
    [SerializeField] Rigidbody2D rb = null;
    [SerializeField] Animator anim = null;
    [SerializeField] AudioSource jumpSound = null;

    private void OnEnable()
    {
        GameManager9.StopMovement += Hit;
    }

    private void OnDisable()
    {
        GameManager9.StopMovement -= Hit;
    }

    void Update()
    {
        if (isDie)
        {
            return;
        }

        if (Input.GetButtonDown("Cancel"))
        {
            GameManager9.manager.PauseGame();
        }
        
        Animation();
    }

    /// <summary>
    /// Function that makes the dinosaur jump.
    /// </summary>
    public void Jump()
    {
        if (IsGrounded() && !isCrouch)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            jumpSound.Play();
        }
    }

    /// <summary>
    /// Function that makes the dinosaur crouch.
    /// </summary>
    /// <param name="enable">Activate or deactivate the crouch.</param>
    public void Crouch(bool enable)
    {
        if (enable)
        {
            isCrouch = true;

            standingCollider.enabled = false;
            crouchCollider.enabled = true;
        }

        else
        {
            isCrouch = false;

            standingCollider.enabled = true;
            crouchCollider.enabled = false;
        }
    }

    /// <summary>
    /// Function that activates the dinosaur animations.
    /// </summary>
    void Animation()
    {
        anim.SetBool("Jump", !IsGrounded());
        anim.SetBool("Crouch", isCrouch);
    }

    /// <summary>
    /// Function that resets the parameters of the dinosaur to restart the game.
    /// </summary>
    public void ResetValues()
    {
        transform.position = new Vector2(-5.66f, -1.53f);

        anim.SetBool("Hit", false);

        rb.gravityScale = 5;

        Crouch(false);

        isDie = false;
    }

    /// <summary>
    /// Function that is activated through the delegate when we hit an obstacle.
    /// </summary>
    void Hit(float speed)
    {
        anim.SetBool("Hit", true);

        isDie = true;

        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
    }

    /// <summary>
    /// Boolean to know if we are on the ground through a Raycast.
    /// </summary>
    /// <returns>True if we are on the ground, false if not.</returns>
    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0, 0.7f, 0), Vector2.down, 0.1f, groundMask);

        return hit;
    }
}
