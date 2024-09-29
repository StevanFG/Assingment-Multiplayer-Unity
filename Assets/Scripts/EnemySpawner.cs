using Unity.Netcode;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] 
    private GameObject targetPrefab;

    [SerializeField]
    private int maxTargets = 10;

    [SerializeField] 
    private float spawnInterval = 5f;

    [SerializeField] 
    private Vector3 spawnAreaSize = new Vector3(20f, 0f, 20f);

    private float timer;

    private void Update()
    {
        if (!IsServer) return; 

        timer += Time.deltaTime;
        if (timer >= spawnInterval && NetworkManager.Singleton.ConnectedClients.Count > 0)
        {
            SpawnTarget();
            timer = 0f;
        }
    }

    private void SpawnTarget()
    {
        if (FindObjectsOfType<MovingTarget>().Length >= maxTargets) return;

        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
            0f,
            Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f)
        );

        GameObject newTarget = Instantiate(targetPrefab, randomPosition, Quaternion.identity);
        newTarget.GetComponent<NetworkObject>().Spawn();
    }
}
