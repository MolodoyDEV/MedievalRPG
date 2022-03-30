using Molodoy.CoreComponents;
using UnityEngine;

public class RTSCameraController : MonoBehaviour
{
    [SerializeField] private bool invertedMouse;
    [SerializeField] private float cameraMouseHorizontalSpeed = 800f;
    [SerializeField] private float cameraKeyHorizontalSpeed = 400f;
    [SerializeField] private float cameraVerticalSpeed = 100f;
    [SerializeField] private Vector2 minMaxCameraAltitude = new Vector2(5f, 20f);
    private Transform movableTransfrom;
    private Camera myCamera;

    private void Awake()
    {
        movableTransfrom = transform.parent;
        myCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        SceneTransition.InitializeSceneProperties();
        GameProcess.UnFreezePlayer();
    }

    private void Update()
    {
        Vector3 movementDirection = Vector3.zero;

        if (PlayerInputHandler.GetRightMouseInputHeld())
        {
            if (invertedMouse)
            {
                movementDirection = PlayerInputHandler.GetMouseMovementDirection() * cameraMouseHorizontalSpeed * Time.deltaTime;
            }
            else
            {
                movementDirection = -PlayerInputHandler.GetMouseMovementDirection() * cameraMouseHorizontalSpeed * Time.deltaTime;
            }
        }
        else
        {
            movementDirection = PlayerInputHandler.GetMovementDirection() * cameraKeyHorizontalSpeed * Time.deltaTime;
        }

        float mouseScrollDelta = PlayerInputHandler.GetMouseScrollDeltaY();

        if (mouseScrollDelta > 0 && myCamera.orthographicSize > minMaxCameraAltitude.x)
        {
            myCamera.orthographicSize += -cameraVerticalSpeed * Time.deltaTime;
        }
        else if (mouseScrollDelta < 0 && myCamera.orthographicSize < minMaxCameraAltitude.y)
        {
            myCamera.orthographicSize += cameraVerticalSpeed * Time.deltaTime;
        }

        movableTransfrom.position += movementDirection;
        Vector3 correctPosition = movableTransfrom.position;

        if (movableTransfrom.position.y > minMaxCameraAltitude.y)
        {
            correctPosition.y = minMaxCameraAltitude.y;
        }
        else if (movableTransfrom.position.y < minMaxCameraAltitude.x)
        {
            correctPosition.y = minMaxCameraAltitude.x;
        }

        movableTransfrom.position = correctPosition;
    }
}
