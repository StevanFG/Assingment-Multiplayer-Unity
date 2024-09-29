using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletSpawner : NetworkBehaviour
{
    [SerializeField] 
    private GameObject bullet;

    [SerializeField] 
    private Transform initialTransform;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && IsOwner)
        {
            SpawnBulletServerRPC(initialTransform.position, initialTransform.rotation);
        }
    }

    [ServerRpc] //(RequireOwnership = false)
    private void SpawnBulletServerRPC(Vector3 position, Quaternion rotation)
    {
        GameObject instantiatedBullet = Instantiate(bullet, position, rotation);
        instantiatedBullet.GetComponent<NetworkObject>().Spawn();
    }
}