using System.Collections;
using UnityEngine;

/// <summary>
/// Class used by the missiles generator.
/// </summary>
public class MissileGenerator : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine (SpawnMissiles());
    }

    /// <summary>
    /// Function that is responsible for generating missiles.
    /// </summary>
    void GenerateMissiles()
    {
        int randonNumber = Random.Range(1, 4);

        GameObject missile = ObjectPooler.SharedInstance.GetPooledObject("Game1/Missile");
        if (missile != null)
        {
            if (randonNumber == 1)
            {
                missile.transform.position = new Vector2(11f, 2.23f);
            }
            else if (randonNumber == 2)
            {
                missile.transform.position = new Vector2(11f, -3.5f);
            }
            else if (randonNumber == 3)
            {
                missile.transform.position = new Vector2(-11f, 2.23f);
            }
            else if (randonNumber == 4)
            {
                missile.transform.position = new Vector2(-11f, -3.5f);
            }

            missile.transform.rotation = Quaternion.identity;
            missile.SetActive(true);
        }
    }

    /// <summary>
    /// Corroutine that calls the function to generate missiles after a few seconds.
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnMissiles()
    {
        yield return new WaitForSeconds(3);

        while (true)
        {
            GenerateMissiles();
            yield return new WaitForSeconds(Random.Range(4, 7));
        }
    }

}
