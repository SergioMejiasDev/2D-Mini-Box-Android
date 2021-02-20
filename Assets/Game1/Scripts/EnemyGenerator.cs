using System.Collections;
using UnityEngine;

/// <summary>
/// Class used by the enemies generator.
/// </summary>
public class EnemyGenerator : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(Spawner());
    }

    /// <summary>
    /// Function that is responsible for generating enemies.
    /// </summary>
    public void GenerateEnemy()
    {
        GameObject goomba = ObjectPooler.SharedInstance.GetPooledObject("Game1/Enemy");
        if (goomba != null)
        {
            goomba.transform.position = transform.position;
            goomba.transform.rotation = Quaternion.identity;
            goomba.SetActive(true);
        }
    }

    /// <summary>
    /// Corroutine that calls the function to generate enemies after a few seconds.
    /// </summary>
    /// <returns></returns>
    IEnumerator Spawner()
    {
        while (true)
        {
            GenerateEnemy();
            yield return new WaitForSeconds(Random.Range(3, 6));
        }
    }
}