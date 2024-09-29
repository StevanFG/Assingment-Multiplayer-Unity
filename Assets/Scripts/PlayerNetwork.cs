using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNetwork : NetworkBehaviour
{
    private PlayerInput playerInput;
    private InputAction moveAction;

    [SerializeField] 
    private float moveSpeed = 3f;

    [SerializeField] 
    private float rotationSpeed = 10f;

    private NetworkVariable<Vector3> netPosition = new NetworkVariable<Vector3>();
    private NetworkVariable<Quaternion> netRotation = new NetworkVariable<Quaternion>();

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            transform.position = Vector3.zero;
        }
    }

    private void Update()
    {
        if (IsOwner)
        {
            MovePlayer();
            SendMovementToServerRpc(transform.position, transform.rotation);
        }
        else
        {
            UpdateClient();
        }
    }

    void MovePlayer()
    {
        Vector2 inputDirection = moveAction.ReadValue<Vector2>();
        Vector3 movement = new Vector3(inputDirection.x, 0, inputDirection.y).normalized;

        transform.position += movement * moveSpeed * Time.deltaTime;

        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    [ServerRpc]
    void SendMovementToServerRpc(Vector3 position, Quaternion rotation)
    {
        netPosition.Value = position;
        netRotation.Value = rotation;
    }

    void UpdateClient()
    {
        transform.position = Vector3.Lerp(transform.position, netPosition.Value, Time.deltaTime * 10f);
        transform.rotation = Quaternion.Slerp(transform.rotation, netRotation.Value, Time.deltaTime * 10f);
    }
}
