using UnityEngine;

/// <summary>
/// Class that is in charge of the movement of the game pieces.
/// </summary>
public class Tetromino : MonoBehaviour
{
    [SerializeField] string letter;
    int rotationMode = 1;
    float lastFall = 0;

    AudioSource fallingSound;

    /// <summary>
    /// Boolean that is true if the piece is within the game grid.
    /// </summary>
    /// <returns>Positive if the piece is within the game grid, false if isn't.</returns>
    bool IsValidPosition()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = GameManager6.manager6.RoundVec2(child.position);

            if (!GameManager6.manager6.InsideBorder(v))
            {
                return false;
            }

            if (GameManager6.grid[(int)v.x, (int)v.y] != null && GameManager6.grid[(int)v.x, (int)v.y].parent != transform)
            {
                return false;
            }
        }

        return true;
    }

    private void OnEnable()
    {
        MobileInputs6.Button += ActivateInputs;
    }

    private void OnDisable()
    {
        MobileInputs6.Button -= ActivateInputs;
    }

    void Start()
    {
        if (!IsValidPosition())
        {
            GameManager6.manager6.GameOver();
            Destroy(gameObject);
        }

        else
        {
            fallingSound = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (Time.time - lastFall >= 1)
        {
            ActivateInputs(3);
        }
    }

    /// <summary>
    /// Function called to activate the game's Inputs through the buttons on the screen.
    /// </summary>
    /// <param name="direction">1 up, 2 right, 3 down, 4 left.</param>
    void ActivateInputs(int direction)
    {
        if (direction == 4)
        {
            transform.position += new Vector3(-1, 0, 0);

            if (IsValidPosition())
            {
                UpdateGrid();
            }

            else
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }

        else if (direction == 2)
        {
            transform.position += new Vector3(1, 0, 0);

            if (IsValidPosition())
            {
                UpdateGrid();
            }
            else
            {
                transform.position += new Vector3(-1, 0, 0);
            }
        }

        else if (direction == 1)
        {
            if ((letter == "I") || (letter == "S") || (letter == "Z"))
            {
                if (rotationMode == 1)
                {
                    transform.Rotate(0, 0, -90);

                    if (IsValidPosition())
                    {
                        UpdateGrid();
                    }

                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }

                    rotationMode = 2;
                }

                else if (rotationMode == 2)
                {
                    transform.Rotate(0, 0, 90);

                    if (IsValidPosition())
                    {
                        UpdateGrid();
                    }

                    else
                    {
                        transform.Rotate(0, 0, -90);
                    }

                    rotationMode = 1;
                }
            }

            else if ((letter == "T") || (letter == "O"))
            {
                if ((rotationMode == 1) || (rotationMode == 2))
                {
                    transform.Rotate(0, 0, -90);
                    transform.localScale = new Vector2(1, 1);

                    if (IsValidPosition())
                    {
                        UpdateGrid();
                    }

                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }

                    switch (rotationMode)
                    {
                        case 1:
                            rotationMode = 2;
                            break;
                        case 2:
                            rotationMode = 3;
                            break;
                    }
                }

                else if ((rotationMode == 3) || (rotationMode == 4))
                {
                    transform.Rotate(0, 0, -90);
                    transform.localScale = new Vector2(-1, 1);

                    if (IsValidPosition())
                    {
                        UpdateGrid();
                    }

                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }

                    switch (rotationMode)
                    {
                        case 3:
                            rotationMode = 4;
                            break;
                        case 4:
                            rotationMode = 1;
                            break;
                    }
                }
            }

            else if ((letter == "L") || (letter == "J"))
            {
                if ((rotationMode == 1) || (rotationMode == 2))
                {
                    transform.Rotate(0, 0, -90);

                    if (IsValidPosition())
                    {
                        UpdateGrid();
                    }

                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }

                    switch (rotationMode)
                    {
                        case 1:
                            rotationMode = 2;
                            break;
                        case 2:
                            rotationMode = 3;
                            break;
                    }
                }

                else if ((rotationMode == 3) || (rotationMode == 4))
                {
                    transform.Rotate(0, 0, -90);

                    if (IsValidPosition())
                    {
                        UpdateGrid();
                    }

                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }

                    switch (rotationMode)
                    {
                        case 3:
                            rotationMode = 4;
                            break;
                        case 4:
                            rotationMode = 1;
                            break;
                    }
                }
            }
        }

        else if (direction == 3)
        {
            transform.position += new Vector3(0, -1, 0);

            if (IsValidPosition())
            {
                UpdateGrid();
            }

            else
            {
                transform.position += new Vector3(0, 1, 0);

                fallingSound.Play();

                GameManager6.manager6.UpdateScore(1);

                GameManager6.manager6.DeleteFullRows();

                GameObject.FindGameObjectWithTag("Game6/Spawner").GetComponent<TetrominoSpawner>().Spawn();

                enabled = false;
            }

            lastFall = Time.time;
        }

    }

    /// <summary>
    /// Function that updates the grid data with those of the new position of the piece.
    /// </summary>
    void UpdateGrid()
    {
        for (int y = 0; y < GameManager6.height; ++y)
        {
            for (int x = 0; x < GameManager6.width; ++x)
            {
                if (GameManager6.grid[x, y] != null)
                {
                    if (GameManager6.grid[x, y].parent == transform)
                    {
                        GameManager6.grid[x, y] = null;
                    }
                }
            }
        }

        foreach (Transform child in transform)
        {
            Vector2 v = GameManager6.manager6.RoundVec2(child.position);
            GameManager6.grid[(int)v.x, (int)v.y] = child;
        }
    }
}
