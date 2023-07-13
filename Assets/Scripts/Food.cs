using System;
using Unity.BossRoom.Infrastructure;
using Unity.Netcode;
using UnityEngine;

public class Food : NetworkBehaviour {

    public GameObject foodPrefab;

    private void OnTriggerEnter2D(Collider2D collision) {

        if (!collision.CompareTag("Player")) return;

        if (!NetworkManager.Singleton.IsServer) return;

        if (collision.TryGetComponent(out PlayerLength playerLength)) {
            playerLength.AddLength();
        }

        else if (collision.TryGetComponent(out Tail tail)) {
            tail.networkOwner.GetComponent<PlayerLength>().AddLength();
        }
        //NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, foodPrefab);
        NetworkObject.Despawn();

    }
}
