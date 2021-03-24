using UnityEngine;
using Photon.Pun;

/// <summary>
/// Class that controls the functions of the paddle in online mode.
/// </summary>
public class PaddleNetwork : MonoBehaviourPun, IPunObservable
{
    float speed = 3.5f;
    float v;
    [SerializeField] float startPosition;

    [Header("Components")]
    [SerializeField] PhotonView pv = null;
    [SerializeField] Rigidbody2D rb = null;
    [SerializeField] AudioSource hitSound = null;

    private void OnEnable()
    {
        if (pv.IsMine)
        {
            MobileInputs2.Button += ActivateInputs;
        }
    }

    private void OnDisable()
    {
        if (pv.IsMine)
        {
            MobileInputs2.Button -= ActivateInputs;
        }
    }

    void Update()
    {
        if (pv.IsMine)
        {
            rb.velocity = new Vector2(rb.velocity.x, v * speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Game2/Ball"))
        {
            hitSound.Play();
        }
    }

    /// <summary>
    /// Function called to activate the game's inputs through the buttons on the screen.
    /// </summary>
    /// <param name="input">1 up, 3 down, 5 disable.</param>
    void ActivateInputs(int input)
    {
        switch (input)
        {
            case 1:
                v = 1;
                break;
            case 3:
                v = -1;
                break;
            case 5:
                v = 0;
                break;
        }
    }

    /// <summary>
    /// Function called to reset the paddle position.
    /// </summary>
    public void ResetPosition()
    {
        v = 0;
        rb.velocity = Vector2.zero;
        transform.position = new Vector2(startPosition, 0);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position.y);
            stream.SendNext(rb.position);
        }

        else
        {
            transform.position = new Vector2(startPosition, (float)stream.ReceiveNext());
            rb.position = (Vector2)stream.ReceiveNext();
        }
    }
}
