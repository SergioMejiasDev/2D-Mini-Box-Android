using UnityEngine;
using Photon.Pun;

/// <summary>
/// Class that manages the movement of cars.
/// </summary>
public class CarMovement : MonoBehaviourPun, IPunObservable
{
    [Header("Movement")]
    float force = 5.0f;
    float rotationSpeed = 100.0f;
    int nextPoint = 1;
    float h;
    float v;

    [Header("Components")]
    [SerializeField] Rigidbody2D rb = null;
    [SerializeField] PhotonView pv = null;
    [SerializeField] AudioSource hitAudio = null;

    private void OnEnable()
    {
        if (pv.IsMine)
        {
            MobileInputs11.Button += ActivateInputs;

            nextPoint = 1;
        }
    }

    private void OnDisable()
    {
        if (pv.IsMine)
        {
            MobileInputs11.Button -= ActivateInputs;
        }
    }

    void FixedUpdate()
    {
        if (pv.IsMine)
        {
            rb.AddForce(transform.up * force * v, ForceMode2D.Force);
        }
    }

    private void Update()
    {
        if (pv.IsMine)
        {
            if (rb.velocity.magnitude > 0.1f)
            {
                transform.Rotate(0, 0, rotationSpeed * Time.deltaTime * h);
            }

            if (Input.GetButtonDown("Cancel"))
            {
                OnlineManager11.manager.PauseGame();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Sprite")
        {
            hitAudio.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!pv.IsMine)
        {
            return;
        }

        if (collision.gameObject.name == "Point1" && nextPoint == 1)
        {
            nextPoint = 2;
        }

        else if (collision.gameObject.name == "Point2" && nextPoint == 2)
        {
            nextPoint = 3;
        }

        else if (collision.gameObject.name == "Point3" && nextPoint == 3)
        {
            nextPoint = 4;
        }

        else if (collision.gameObject.name == "Point4" && nextPoint == 4)
        {
            nextPoint = 5;
        }

        else if (collision.gameObject.name == "Point5" && nextPoint == 5)
        {
            nextPoint = 6;
        }

        else if (collision.gameObject.name == "Point6" && nextPoint == 6)
        {
            nextPoint = 1;
            
            OnlineManager11.manager.UpdateScore(NetworkManager.networkManager.playerNumber);
        }
    }

    /// <summary>
    /// Function called to activate the game's inputs through the buttons on the screen.
    /// </summary>
    /// <param name="input">1 up, 2 right, 3 down, 4 left, 5 & 6 disable.</param>
    void ActivateInputs(int input)
    {
        switch (input)
        {
            case 1:
                v = 1;
                break;
            case 2:
                h = -1;
                break;
            case 3:
                v = -1;
                break;
            case 4:
                h = 1;
                break;
            case 5:
                h = 0;
                break;
            case 6:
                v = 0;
                break;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext((Vector2)transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(rb.position);
            stream.SendNext(rb.rotation);
        }

        else
        {
            transform.position = (Vector2)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
            rb.position = (Vector2)stream.ReceiveNext();
            rb.rotation = (float)stream.ReceiveNext();
        }
    }
}
