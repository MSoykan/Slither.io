using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerLength : NetworkBehaviour {

    [SerializeField] private GameObject tailPrefab;

    public NetworkVariable<ushort> length = new(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);


    public static event System.Action<ushort> OnLengthChanged;

    private Collider2D collider2D;
    private List<GameObject> tails;
    private Transform lastTail;

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        tails = new List<GameObject>();
        lastTail = transform;
        collider2D = GetComponent<Collider2D>();
        if (!IsServer) {
            length.OnValueChanged += LengthChanged;
        }
    }

    // This will be called by the server.
    [ContextMenu("Add Length")]
    public void AddLength() {
        length.Value += 1;
        InstantiateTail();
    }

    private void LengthChanged(ushort previousValue, ushort newValue) {
        Debug.Log("LengthChanged callback");
        InstantiateTail();

        if (!IsOwner) {
            return;
        }
        OnLengthChanged?.Invoke(length.Value);
    }


    private void InstantiateTail() {
        GameObject tailGameobject = Instantiate(tailPrefab, transform.position, Quaternion.identity);
        tailGameobject.GetComponent<SpriteRenderer>().sortingOrder = -length.Value;
        if (tailGameobject.TryGetComponent(out Tail tail)) {
            tail.networkOwner = transform;
            tail.followTransform = lastTail;
            lastTail = tailGameobject.transform;
            Physics2D.IgnoreCollision(tailGameobject.GetComponent<Collider2D>(), collider2D);
        }
        tails.Add(tailGameobject);
    }
}
