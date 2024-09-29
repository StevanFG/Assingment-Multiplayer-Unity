using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletScript : NetworkBehaviour
{
    [SerializeField]
    private float speed = 20f; 

    private void Start()
    {
        GetComponent<Rigidbody>().velocity = this.transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected with: " + other.gameObject.name);

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit!");
            other.GetComponent<NetworkObject>().Despawn(true); 
            GetComponent<NetworkObject>().Despawn(true); 
        }
    }
}