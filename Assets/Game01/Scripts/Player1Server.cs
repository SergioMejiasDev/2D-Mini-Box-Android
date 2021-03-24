using UnityEngine;
using Photon.Pun;

/// <summary>
/// Class that controls the network functions of the player.
/// </summary>
public class Player1Server : MonoBehaviourPun, IPunObservable
{
    #region Variables

    [SerializeField] int playerNumber;

    [Header("Movement")]
    float h = 0;
    float speed = 4;
    float jump = 9.5f;
    [SerializeField] LayerMask groundMask = 0;

    [Header("Components")]
    [SerializeField] Rigidbody2D rb = null;
    [SerializeField] Animator anim = null;
    [SerializeField] SpriteRenderer sr = null;
    [SerializeField] AudioSource jumpSound = null;
    [SerializeField] PhotonView pv = null;

    #endregion

    /// <summary>
    /// Boolean that indicates through a Raycast if the player is touching the ground.
    /// </summary>
    /// <returns>True if the player is on the ground, false if not.</returns>
    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0, sr.bounds.extents.y + 0.01f, 0), Vector2.down, 0.1f, groundMask);

        return hit;
    }

    private void OnEnable()
    {
        if (pv.IsMine)
        {
            MobileInputs1.Button += ActivateInputs;
        }
    }

    private void OnDisable()
    {
        if (pv.IsMine)
        {
            MobileInputs1.Button -= ActivateInputs;
        }
    }
    
    void Update()
    {
        if (pv.IsMine)
        {
            Movement(h);

            Animation(h);

            if (Input.GetButtonDown("Cancel"))
            {
                OnlineManager1.onlineManager.PauseGame();
            }
        }
    }

    /// <summary>
    /// Function called to activate the game's inputs through the buttons on the screen.
    /// </summary>
    /// <param name="input">1 up, 2 right, 4 left, 5 disable.</param>
    void ActivateInputs(int input)
    { 
        switch (input)
        {
            case 1:
                Jump();
                break;
            case 2:
                h = 1;
                break;
            case 4:
                h = -1;
                break;
            case 5:
                h = 0;
                break;
        }
    }

    /// <summary>
    /// Function called to make the player move.
    /// </summary>
    /// <param name="h">Direction of movement of the player, positive if it is to the right and negative if it is to the left.</param>
    public void Movement(float h)
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime * h);

        if (h > 0)
        {
            transform.localScale = new Vector2(0.85f, 0.85f);
        }

        else if (h < 0)
        {
            transform.localScale = new Vector2(-0.85f, 0.85f);
        }
    }

    /// <summary>
    /// Function that activates character animations.
    /// </summary>
    /// <param name="h">Direction of movement of the player, positive if it is to the right and negative if it is to the left.</param>
    public void Animation(float h)
    {
        anim.SetBool("IsWalking", h != 0 && IsGrounded());
        anim.SetBool("IsJumping", !IsGrounded());
    }

    /// <summary>
    /// Function called to make the player jump.
    /// </summary>
    public void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            jumpSound.Play();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Game1/Enemy")) || (collision.gameObject.CompareTag("Game1/Missile")))
        {
            if (pv.IsMine)
            {
                OnlineManager1.onlineManager.Respawn();
                
                if (PhotonView.Find(pv.ViewID))
                {
                    PhotonNetwork.Destroy(PhotonView.Find(pv.ViewID));
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Game1/Coin"))
        {
            if (pv.IsMine)
            {
                OnlineManager1.onlineManager.DestroyCoin(collision.gameObject.GetComponent<PhotonView>().ViewID, playerNumber);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext((Vector2)transform.position);
            stream.SendNext(transform.localScale.x);
            stream.SendNext(rb.position);
            stream.SendNext(rb.rotation);
        }

        else
        {
            transform.position = (Vector2)stream.ReceiveNext();
            transform.localScale = new Vector3((float)stream.ReceiveNext(), 0.85f, 1);
            rb.position = (Vector2)stream.ReceiveNext();
            rb.rotation = (float)stream.ReceiveNext();
        }
    }
}
