using UnityEngine;

/// <summary>
/// Class that is responsible for spawning the pieces during the game.
/// </summary>
public class TetrominoSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] tetrominos = null;

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            GameManager6.manager6.PauseGame();
        }
    }

    /// <summary>
    /// Function that spawns a new piece in the game.
    /// </summary>
    public void Spawn()
    {
        int randomValue = Random.Range(0, tetrominos.Length);

        Instantiate(tetrominos[randomValue], transform.position, Quaternion.identity);
    }
}
