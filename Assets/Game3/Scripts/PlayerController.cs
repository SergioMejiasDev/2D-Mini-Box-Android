using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that manages the main functions of the player.
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

    void Update()
    {
        HandleMoving();

        Shoot();
        
        if (Input.GetButtonDown("Cancel"))
        {
            GameManager3.manager3.PauseGame();
        }
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
        if (transform.position.x >= minBound)
        {
            transform.Translate(Vector2.right * -speed * Time.deltaTime);
        }
    }

    void MoveRight()
    {
        if (transform.position.x <= maxBound)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }

    void StopMoving()
    {
        transform.Translate(Vector2.right * 0 * Time.deltaTime);
    }

    public void AllowShoot()
    {
        canShoot = true;
    }

    public void DontAllowShoot()
    {
        canShoot = false;
    }

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
