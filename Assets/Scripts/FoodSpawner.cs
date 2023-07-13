using System.Collections;
using System.Collections.Generic;
using Unity.BossRoom.Infrastructure;
using Unity.Netcode;
using UnityEngine;

public class FoodSpawner : MonoBehaviour {

    [SerializeField] private GameObject foodPrefab;

    private void Start() {
        NetworkManager.Singleton.OnServerStarted += NetworkManager_OnServerStarted;
    }



    private void NetworkManager_OnServerStarted() {
        NetworkManager.Singleton.OnServerStarted -= NetworkManager_OnServerStarted;
        NetworkObjectPool.Singleton.OnNetworkSpawn();
        for (int i = 0; i < 30; i++) {
            SpawnFood();
        }

        StartCoroutine(SpawnOverTime());
    }

    private void SpawnFood() {
        NetworkObject obj = NetworkObjectPool.Singleton.GetNetworkObject(foodPrefab, GetRandomPositionOnMap(), Quaternion.identity);
        obj.GetComponent<Food>().foodPrefab = foodPrefab;
        obj.Spawn(true);
    }

    private Vector3 GetRandomPositionOnMap() {

        return new Vector3(Random.Range(-17f, 17f), Random.Range(-9f, 9f), 0);
    }

    private IEnumerator SpawnOverTime() {
        while (NetworkManager.Singleton.ConnectedClients.Count > 0) {
            yield return new WaitForSeconds(2f);
            SpawnFood();
        }
    }
}
