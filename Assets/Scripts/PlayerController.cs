using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour {


    [SerializeField] private float speed = 3f;
    private Camera mainCamera;
    private Vector3 mouseInput;


    private void Initialize() {
        mainCamera = Camera.main;
    }

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        Initialize();
    }

    private void Update() {

        mouseInput.x = Input.mousePosition.x;
        mouseInput.y = Input.mousePosition.y;
        mouseInput.z = mainCamera.nearClipPlane;
        Vector3 mouseWorldCoordinates = mainCamera.ScreenToWorldPoint(mouseInput);
        transform.position = Vector3.MoveTowards(transform.position, mouseWorldCoordinates, Time.deltaTime * speed);

        //Rotate
        if(mouseWorldCoordinates != transform.position) {
            Vector3 targetDirection = mouseWorldCoordinates - transform.position;
            transform.up = targetDirection;
            targetDirection.z = 0;
        }
    }

}