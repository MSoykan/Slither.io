using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class PlayerController : NetworkBehaviour {


    [SerializeField] private float speed = 3f;
    private Camera mainCamera;
    private Vector3 mouseInput;
    private PlayerLength playerLength;

    private void Initialize() {
        mainCamera = Camera.main;
        playerLength = GetComponent<PlayerLength>();
    }

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        Initialize();
    }

    private void Update() {

        if (!IsOwner || !Application.isFocused) { return; }

        mouseInput.x = Input.mousePosition.x;
        mouseInput.y = Input.mousePosition.y;
        mouseInput.z = mainCamera.nearClipPlane;
        Vector3 mouseWorldCoordinates = mainCamera.ScreenToWorldPoint(mouseInput);
        mouseWorldCoordinates.z = 0;
        transform.position = Vector3.MoveTowards(transform.position, mouseWorldCoordinates, Time.deltaTime * speed);

        //Rotate
        if (mouseWorldCoordinates != transform.position) {
            Vector3 targetDirection = mouseWorldCoordinates - transform.position;
            targetDirection.z = 0;
            transform.up = targetDirection;
        }
    }

    [ServerRpc]
    private void DetermineCollisionWinnerServerRpc(PlayerData player1, PlayerData player2) {

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Player Collision");
        if (!collision.gameObject.CompareTag("Player")) return;
        if (!IsOwner) return;

        //Head-on Collision
        if (collision.gameObject.TryGetComponent(out PlayerLength playerLength)) {
            var player1 = new PlayerData() {
                id = OwnerClientId,
                length = playerLength.length.Value
            };
            var player2 = new PlayerData() {
                id = playerLength.OwnerClientId,
                length = playerLength.length.Value
            };
            DetermineCollisionWinnerServerRpc(player1, player2);
        }
    }

    struct PlayerData : INetworkSerializable {
        public ulong id;
        public ushort length;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref id);
            serializer.SerializeValue(ref length);
        }
    }

}
