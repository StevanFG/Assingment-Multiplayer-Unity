using Unity.Netcode;
using UnityEngine;

public class MovingTarget : NetworkBehaviour
{
    [SerializeField] 
    private float moveSpeed = 3f;

    private NetworkVariable<Vector3> netPosition = new NetworkVariable<Vector3>();

    private void Start()
    {
        if (IsServer)
        {
            netPosition.Value = transform.position;
        }
    }

    private void Update()
    {
        if (IsServer)
        {
            MoveTowardsNearestPlayer();
            UpdateServerPosition();
        }
        else
        {
            UpdateClientPosition();
        }
    }

    private void MoveTowardsNearestPlayer()
    {
        Transform nearestPlayer = FindNearestPlayer();

        if (nearestPlayer != null)
        {
            Vector3 direction = (nearestPlayer.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    private Transform FindNearestPlayer()
    {
        Transform nearest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            if (client.PlayerObject != null)
            {
                Transform clientTransform = client.PlayerObject.transform;
                float distance = Vector3.Distance(transform.position, clientTransform.position);

                if (distance < shortestDistance)
                {
                    nearest = clientTransform;
                    shortestDistance = distance;
                }
            }
        }

        return nearest;
    }

    private void UpdateServerPosition()
    {
        netPosition.Value = transform.position;
    }

    private void UpdateClientPosition()
    {
        transform.position = Vector3.Lerp(transform.position, netPosition.Value, Time.deltaTime * 10f);
    }
}
