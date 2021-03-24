using System.Collections;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class that is responsible for spawning all objects on the screen.
/// </summary>
public class FroggerSpawners : MonoBehaviour
{
    [Header("Cars")]
    [SerializeField] Vector2[] car1initialSpawn = null;
    [SerializeField] Vector2[] car2initialSpawn = null;
    [SerializeField] Vector2[] car3initialSpawn = null;
    [SerializeField] Vector2[] car4initialSpawn = null;
    [SerializeField] Vector2[] car5initialSpawn = null;

    public void StartSpawns()
    {
        StopAllCoroutines();

        CleanScene();

        StartCoroutine(SpawnCar1());
        StartCoroutine(SpawnCar2());
        StartCoroutine(SpawnCar3());
        StartCoroutine(SpawnCar4());
        StartCoroutine(SpawnCar5());

        StartCoroutine(SpawnTrunk1());
        StartCoroutine(SpawnTrunk2());
        StartCoroutine(SpawnTrunk3());

        StartCoroutine(SpawnTurtle1());
        StartCoroutine(SpawnTurtle2());
    }

    void CleanScene()
    {
        GameObject[] car1 = GameObject.FindGameObjectsWithTag("Game8/Car1");
        GameObject[] car2 = GameObject.FindGameObjectsWithTag("Game8/Car2");
        GameObject[] car3 = GameObject.FindGameObjectsWithTag("Game8/Car3");
        GameObject[] car4 = GameObject.FindGameObjectsWithTag("Game8/Car4");
        GameObject[] car5 = GameObject.FindGameObjectsWithTag("Game8/Car5");
        GameObject[] trunk1 = GameObject.FindGameObjectsWithTag("Game8/Trunk1");
        GameObject[] trunk2 = GameObject.FindGameObjectsWithTag("Game8/Trunk2");
        GameObject[] trunk3 = GameObject.FindGameObjectsWithTag("Game8/Trunk3");
        GameObject[] crocodile = GameObject.FindGameObjectsWithTag("Game8/Crocodile");
        GameObject[] turtle1 = GameObject.FindGameObjectsWithTag("Game8/Turtle1");
        GameObject[] turtle2 = GameObject.FindGameObjectsWithTag("Game8/Turtle2");
        GameObject[] turtle3 = GameObject.FindGameObjectsWithTag("Game8/Turtle3");
        GameObject[] turtle4 = GameObject.FindGameObjectsWithTag("Game8/Turtle4");

        GameObject[] allObjects = car1.Concat(car2).Concat(car3).Concat(car4).Concat(car5).
            Concat(trunk1).Concat(trunk2).Concat(trunk3).Concat(crocodile).
            Concat(turtle1).Concat(turtle2).Concat(turtle3).Concat(turtle4).ToArray();

        for (int i = 0; i < allObjects.Length; i++)
        {
            allObjects[i].SetActive(false);
        }
    }

    #region Spawn Coroutines

    IEnumerator SpawnCar1()
    {
        foreach (Vector2 position in car1initialSpawn)
        {
            GameObject car = ObjectPooler.SharedInstance.GetPooledObject("Game8/Car1");

            if (car != null)
            {
                car.transform.position = position;
                car.transform.rotation = Quaternion.identity;
                car.SetActive(true);
            }
        }

        while (true)
        {
            GameObject car = ObjectPooler.SharedInstance.GetPooledObject("Game8/Car1");

            if (car != null)
            {
                car.transform.position = new Vector2(3.5f, -2.85f);
                car.transform.rotation = Quaternion.identity;
                car.SetActive(true);
            }

            yield return new WaitForSeconds(Random.Range(2f, 3f));
        }
    }

    IEnumerator SpawnCar2()
    {
        foreach (Vector2 position in car2initialSpawn)
        {
            GameObject car = ObjectPooler.SharedInstance.GetPooledObject("Game8/Car2");

            if (car != null)
            {
                car.transform.position = position;
                car.transform.rotation = Quaternion.identity;
                car.SetActive(true);
            }
        }

        while (true)
        {
            GameObject car = ObjectPooler.SharedInstance.GetPooledObject("Game8/Car2");

            if (car != null)
            {
                car.transform.position = new Vector2(-3.5f, -2.35f);
                car.transform.rotation = Quaternion.identity;
                car.SetActive(true);
            }

            yield return new WaitForSeconds(Random.Range(2f, 3f));
        }
    }

    IEnumerator SpawnCar3()
    {
        foreach (Vector2 position in car3initialSpawn)
        {
            GameObject car = ObjectPooler.SharedInstance.GetPooledObject("Game8/Car3");

            if (car != null)
            {
                car.transform.position = position;
                car.transform.rotation = Quaternion.identity;
                car.SetActive(true);
            }
        }

        while (true)
        {
            GameObject car = ObjectPooler.SharedInstance.GetPooledObject("Game8/Car3");

            if (car != null)
            {
                car.transform.position = new Vector2(3.5f, -1.85f);
                car.transform.rotation = Quaternion.identity;
                car.SetActive(true);
            }

            yield return new WaitForSeconds(Random.Range(2f, 3f));
        }
    }

    IEnumerator SpawnCar4()
    {
        foreach (Vector2 position in car4initialSpawn)
        {
            GameObject car = ObjectPooler.SharedInstance.GetPooledObject("Game8/Car4");

            if (car != null)
            {
                car.transform.position = position;
                car.transform.rotation = Quaternion.identity;
                car.SetActive(true);
            }
        }

        while (true)
        {
            GameObject car = ObjectPooler.SharedInstance.GetPooledObject("Game8/Car4");

            if (car != null)
            {
                car.transform.position = new Vector2(-3.5f, -1.35f);
                car.transform.rotation = Quaternion.identity;
                car.SetActive(true);
            }

            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    IEnumerator SpawnCar5()
    {
        foreach (Vector2 position in car5initialSpawn)
        {
            GameObject car = ObjectPooler.SharedInstance.GetPooledObject("Game8/Car5");

            if (car != null)
            {
                car.transform.position = position;
                car.transform.rotation = Quaternion.identity;
                car.SetActive(true);
            }
        }

        while (true)
        {
            GameObject car = ObjectPooler.SharedInstance.GetPooledObject("Game8/Car5");

            if (car != null)
            {
                car.transform.position = new Vector2(3.5f, -0.85f);
                car.transform.rotation = Quaternion.identity;
                car.SetActive(true);
            }

            yield return new WaitForSeconds(3);
        }
    }

    IEnumerator SpawnTrunk1()
    {
        while (true)
        {
            GameObject trunk = ObjectPooler.SharedInstance.GetPooledObject("Game8/Trunk1");

            if (trunk != null)
            {
                trunk.transform.position = new Vector2(-4.5f, 0.65f);
                trunk.transform.rotation = Quaternion.identity;
                trunk.SetActive(true);
            }

            yield return new WaitForSeconds(Random.Range(3f, 4f));
        }
    }

    IEnumerator SpawnTrunk2()
    {
        while (true)
        {
            GameObject trunk;

            if (Random.value < 0.75f)
            {
                trunk = ObjectPooler.SharedInstance.GetPooledObject("Game8/Trunk2");
            }

            else
            {
                trunk = ObjectPooler.SharedInstance.GetPooledObject("Game8/Crocodile");
            }

            if (trunk != null)
            {
                trunk.transform.position = new Vector2(-4.5f, 2.15f);
                trunk.transform.rotation = Quaternion.identity;
                trunk.SetActive(true);
            }

            yield return new WaitForSeconds(Random.Range(3f, 5f));
        }
    }

    IEnumerator SpawnTrunk3()
    {
        while (true)
        {
            GameObject trunk = ObjectPooler.SharedInstance.GetPooledObject("Game8/Trunk3");

            if (trunk != null)
            {
                trunk.transform.position = new Vector2(-4.5f, 1.15f);
                trunk.transform.rotation = Quaternion.identity;
                trunk.SetActive(true);
            }

            yield return new WaitForSeconds(Random.Range(3f, 6f));
        }
    }

    IEnumerator SpawnTurtle1()
    {
        while (true)
        {
            GameObject turtle;

            if (Random.value < 0.75f)
            {
                turtle = ObjectPooler.SharedInstance.GetPooledObject("Game8/Turtle1");
            }

            else
            {
                turtle = ObjectPooler.SharedInstance.GetPooledObject("Game8/Turtle3");
            }

            if (turtle != null)
            {
                turtle.transform.position = new Vector2(4.5f, 0.15f);
                turtle.transform.rotation = Quaternion.identity;
                turtle.SetActive(true);
            }

            yield return new WaitForSeconds(Random.Range(1.5f, 2f));
        }
    }

    IEnumerator SpawnTurtle2()
    {
        while (true)
        {
            GameObject turtle;

            if (Random.value < 0.75f)
            {
                turtle = ObjectPooler.SharedInstance.GetPooledObject("Game8/Turtle2");
            }

            else
            {
                turtle = ObjectPooler.SharedInstance.GetPooledObject("Game8/Turtle4");
            }

            if (turtle != null)
            {
                turtle.transform.position = new Vector2(4.5f, 1.65f);
                turtle.transform.rotation = Quaternion.identity;
                turtle.SetActive(true);
            }

            yield return new WaitForSeconds(Random.Range(1.5f, 2f));
        }
    }

    #endregion
}
