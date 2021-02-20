using UnityEngine;

/// <summary>
/// Class that manages the main functions of the player.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5;
    bool dontMove = true;
    bool moveLeft = false;
    float maxBound = 7, minBound = -7;
    [SerializeField] AudioSource shootAudio = null;
    [SerializeField] Transform shootPoint = null;
    [SerializeField] float cadency = 1;
    bool canShoot = false;

    public float nextFire;

    private void OnEnable()
    {
        DontAllowMovement();
        DontAllowShoot();
    }

    void Update()
    {
        HandleMoving();

        Shoot();

        if (Input.GetButtonDown("Cancel"))
        {
            GameManager3.manager3.PauseGame();
        }
    }

    /// <summary>
    /// Function that manages the movement of the player.
    /// </summary>
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

    /// <summary>
    /// Function that allows the player to move.
    /// </summary>
    /// <param name="leftMovement">True if the movement is to the left, false if it is to the right.</param>
    public void AllowMovement(bool leftMovement)
    {
        dontMove = false;
        moveLeft = leftMovement;
    }

    /// <summary>
    /// Function that cancels the player's movement.
    /// </summary>
    public void DontAllowMovement()
    {
        dontMove = true;
    }

    /// <summary>
    /// Function that moves the character to the left.
    /// </summary>
    void MoveLeft()
    {
        if (transform.position.x >= minBound)
        {
            transform.Translate(Vector2.right * -speed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Function that moves the character to the right.
    /// </summary>
    void MoveRight()
    {
        if (transform.position.x <= maxBound)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Function that keeps the player in position.
    /// </summary>
    void StopMoving()
    {
        transform.Translate(Vector2.right * 0 * Time.deltaTime);
    }

    /// <summary>
    /// Function that allows the player to shoot.
    /// </summary>
    public void AllowShoot()
    {
        canShoot = true;
    }

    /// <summary>
    /// Function that cancels the possibility of shooting.
    /// </summary>
    public void DontAllowShoot()
    {
        canShoot = false;
    }

    /// <summary>
    /// Function that is called every time the player shoots.
    /// </summary>
    void Shoot()
    {
        if (canShoot && (Time.time > nextFire))
        {
            nextFire = Time.time + cadency;
            GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("Game3/BulletPlayer");
            if (bullet != null)
            {
                bullet.SetActive(true);
                bullet.transform.position = shootPoint.position;
                bullet.transform.rotation = Quaternion.identity;
            }
            shootAudio.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Game3/BulletEnemy"))
        {
            other.gameObject.SetActive(false);
            GameManager3.manager3.LoseHealth(1);
        }
    }
}