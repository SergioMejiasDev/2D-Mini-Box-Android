﻿using UnityEngine;

/// <summary>
/// Class that manages the movement of the car.
/// </summary>
public class CarMovement : MonoBehaviour
{
    [Header("Movement")]
    float force = 5.0f;
    float rotationSpeed = 100.0f;
    int nextPoint = 1;
    float h;
    float v;

    [Header("Components")]
    [SerializeField] Rigidbody2D rb = null;
    [SerializeField] AudioSource hitAudio = null;
    [SerializeField] Timer11 timer = null;

    void OnEnable()
    {
        nextPoint = 1;
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.up * force * v, ForceMode2D.Force);
    }

    private void Update()
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime * h);
        }

        if (Input.GetButtonDown("Cancel"))
        {
            GameManager11.manager.PauseGame();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hitAudio.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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

            timer.ResetTimer();
        }
    }

    /// <summary>
    /// Function called to activate the game's inputs through the buttons on the screen.
    /// </summary>
    /// <param name="input">1 up, 2 right, 3 down, 4 left, 5 & 6 disable.</param>
    public void ActivateInputs(int input)
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
}
