using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;


    [SerializeField] private float _enemyCount; 
    [SerializeField] private float _spawnInterval;

    private void Start()
    {
        StartCoroutine(SpawnEnemy(_spawnInterval, _enemy));
    }

    private IEnumerator SpawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        StartCoroutine(SpawnEnemy(interval, enemy));
    }
}
