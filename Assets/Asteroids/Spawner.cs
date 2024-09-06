using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] asteroidPrefabs;
    public float spawnIntervalMin;
    public float spawnIntervalMax;
    public float acceleration;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnAsteroid());
    }

    IEnumerator SpawnAsteroid()
    {
        while (true)
        {
            float spawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(spawnInterval);

            int asteroidIndex = Random.Range(0, asteroidPrefabs.Length);
            float spawnDirection = Random.Range(1, 4);
            Vector3 spawnPosition;
            GameObject asteroid;

            switch (spawnDirection)
            {
                case 1:
                    spawnPosition = new Vector3(-11, Random.Range(-6, 6));
                    asteroid = Instantiate(asteroidPrefabs[asteroidIndex], spawnPosition, Quaternion.identity);
                    asteroid.GetComponent<Rigidbody2D>().AddForce(new Vector2(acceleration, Random.Range(-acceleration, acceleration)));
                    break;
                case 2:
                    spawnPosition = new Vector3(11, Random.Range(-6, 6));
                    asteroid = Instantiate(asteroidPrefabs[asteroidIndex], spawnPosition, Quaternion.identity);
                    asteroid.GetComponent<Rigidbody2D>().AddForce(new Vector2(-acceleration, Random.Range(-acceleration, acceleration)));
                    break;
                case 3:
                    spawnPosition = new Vector3(Random.Range(-10, 10), -7);
                    asteroid = Instantiate(asteroidPrefabs[asteroidIndex], spawnPosition, Quaternion.identity);
                    asteroid.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-acceleration, acceleration), acceleration));
                    break;
                case 4:
                    spawnPosition = new Vector3(Random.Range(-10, 10), 7);
                    asteroid = Instantiate(asteroidPrefabs[asteroidIndex], spawnPosition, Quaternion.identity);
                    asteroid.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-acceleration, acceleration), -acceleration));
                    break;

            }
        }
    }
}
