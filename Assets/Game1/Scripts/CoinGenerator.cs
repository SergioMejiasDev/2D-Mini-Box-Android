using System.Collections;
using UnityEngine;

/// <summary>
/// Class used by the coins generator.
/// </summary>
public class CoinGenerator : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(SpawnCoins(GameManager1.manager.isMultiplayer));
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            GameManager1.manager.PauseGame();
        }
    }

    /// <summary>
    /// Function that is responsible for generating coins.
    /// </summary>
    public void GenerateCoin()
    {
        int randonNumber = Random.Range(1, 7);

        GameObject coin = ObjectPooler.SharedInstance.GetPooledObject("Game1/Coin");
        if (coin != null)
        {
            if (randonNumber == 1)
            {
                coin.transform.position = new Vector2(Random.Range(-7.65f, -3.13f), 4.5f);
            }
            else if (randonNumber == 2)
            {
                coin.transform.position = new Vector2(Random.Range(3.13f, 7.65f), 4.5f);
            }
            else if (randonNumber == 3)
            {
                coin.transform.position = new Vector2(Random.Range(-1.75f, 1.75f), 2f);
            }
            else if (randonNumber == 4)
            {
                coin.transform.position = new Vector2(Random.Range(-8.8f, -3f), -1f);
            }
            else if (randonNumber == 5)
            {
                coin.transform.position = new Vector2(Random.Range(3f, 8.8f), -1f);
            }
            else if (randonNumber == 6)
            {
                coin.transform.position = new Vector2(-1.75f, -3.5f);
            }
            else if (randonNumber == 7)
            {
                coin.transform.position = new Vector2(1.75f, -3.5f);
            }

            coin.transform.rotation = Quaternion.identity;
            coin.SetActive(true);
        }
    }

    /// <summary>
    /// Corroutine that calls the function to generate enemies after a few seconds.
    /// </summary>
    /// <param name="multiplayer">True if multiplayer is active.</param>
    /// <returns></returns>
    IEnumerator SpawnCoins(bool multiplayer)
    {
        while (true)
        {
            GenerateCoin();

            if (multiplayer == true)
            {
                yield return new WaitForSeconds(Random.Range(3, 6));
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(5, 10));
            }
        }
    }
}
