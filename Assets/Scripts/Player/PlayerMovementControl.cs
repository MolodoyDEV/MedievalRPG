using Molodoy.CoreComponents;
using Molodoy.Extensions;
using Molodoy.Inspector.Extentions;
using System;
using UnityEngine;

namespace Molodoy.Characters.Player
{
    public class PlayerMovementControl : MonoBehaviour
    {
        private float X, Y;
        [SerializeField] private float mouseSpeed = 180;
        [SerializeField] private float speed = 50F; //Скорость передвижения
        [SerializeField] private float runSpeedMultiplier = 1.5f; //Множитель скорости при беге
        [SerializeField] private float jumpPower = 15F; //Начальная скорость прыжка
        [SerializeField] private float gravity = 30F; //Сила гравитации
        [SerializeField] private int fov = 70;
        [SerializeField] private float zoomMultiplier = 6;
        //[SerializeField] private Camera playerCamera;
        //Changed by animation
        [ReadOnlyInspector] [SerializeField] private bool sitFlag;
        private bool zoomFlag;
        private Vector3 moveDirection = Vector3.zero; //Направление персонажа
        private Transform playerTransform;
        private Transform playerCameraTransform;
        private CharacterController characterController;
        private readonly float maxStandUpRayDistance = 1.50f;
        private Vector3 playerPositionAtStart;
        private IntRange xClamp = new IntRange(-85, 85);
        private Animation myAnimation;

        private void Awake()
        {
            myAnimation = GetComponent<Animation>();
            playerPositionAtStart = transform.position;
            characterController = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            sitFlag = false;
            playerTransform = GetComponent<Transform>();
            //playerCamera = Camera.main;
        }

        private void Start()
        {
            playerCameraTransform = PlayerCore.PlayerCamera.transform;
            PlayerCore.PlayerCamera.fieldOfView = fov;
        }

        public void ProcessMovement()
        {
            CorrectSpeed();

            if (PlayerInputHandler.CanProcessInput && characterController.isGrounded) //Проверяем стоит ли CharacterController на земле
            { //В нем есть переменная, которая дает знать, стоит ли он на земле. И если он стоит...
                moveDirection = PlayerInputHandler.GetMovementDirection(); //Берем значения кнопок WASD или стрелочек и создаем вектор, указывающий в нужную сторону
                moveDirection = playerTransform.TransformDirection(moveDirection); //Переводим локальные координаты в соответствующие глобальные
                moveDirection *= speed / 10; //moveDirection - это коэффициент скорости. Поэтому мы умножаем его на максимальную скорость, чтобы получить текущую

                if (sitFlag == false)
                {
                    if (PlayerInputHandler.GetRunInputHeld())
                    {
                        moveDirection *= runSpeedMultiplier;
                    }

                    if (PlayerInputHandler.GetJumpInputHeld()) //Если мы нажимаем пробел
                    {
                        moveDirection.y = jumpPower; //Мы придаем скорости прыжка
                    }
                }

                if (PlayerInputHandler.GetSitInputHeld())  //Поприседай
                {
                    if (sitFlag == false) // && myAnimation.isPlaying == false)
                    {
                        //myAnimation.PlayQueued("PlayerSitDown");
                        sitFlag = true;
                    }
                }
                else
                {
                    TryStandUp();
                }
                // /Вот так вот
            }
            else
            {
                moveDirection.y -= gravity.SmoothMovement(); //Это действие гравитации

                if (GameProcess.ThisFallenOutInWorld(transform))
                {
                    gameObject.transform.position = playerPositionAtStart;
                    return;
                }
            }

            characterController.Move(moveDirection.SmoothMovement());

            if (PlayerInputHandler.CanProcessInput)
            {
                Y += PlayerInputHandler.GetMouseAxisVertical() * mouseSpeed.SmoothMovement();// mouseSpeed * (Time.deltaTime);
                X += PlayerInputHandler.GetMouseAxisHorizontal() * mouseSpeed.SmoothMovement();// mouseSpeed * (Time.deltaTime);

                //if (Input.GetKey(KeyCode.E))
                //    X += 1f * mouseSpeed * Time.deltaTime;
                //if (Input.GetKey(KeyCode.Q))
                //    X -= 1f * mouseSpeed * Time.deltaTime;

                X = Mathf.Clamp(X, xClamp.Start, xClamp.End);

                playerCameraTransform.rotation = Quaternion.Euler(-X, Y, 0);
                playerTransform.rotation = Quaternion.Euler(0, Y, 0);
            }

            ChangeZoom(PlayerInputHandler.GetZoomInputHeld());
        }

        public Vector3 GetCurrentPosition() => transform.position;

        private void CorrectSpeed()
        {
            //if (GameProcess.ThisSpeedIsAvailable(characterController.velocity) == false)
            //{
            //    moveDirection = Vector3.zero;
            //    moveDirection.y -= gravity.SmoothMovement();
            //    characterController.Move(moveDirection);
            //}
        }

        public void SetMouseSpeed(int value)
        {
            mouseSpeed = value;
        }

        public void SetRotation(Vector3 newRotation)
        {
            X = newRotation.y;
            Y = newRotation.x;
        }

        public void TryStandUp()
        {
            if (sitFlag)
            {
                Ray ray = new Ray(PlayerCore.PlayerCamera.transform.position, transform.up * maxStandUpRayDistance);
                //Debug.DrawRay(transform.position, transform.up * maxRayDistance, Color.red);

                if (Physics.Raycast(ray, maxStandUpRayDistance)) //, out RaycastHit hit
                {
                    Debug.Log("Здесь невозможно встать!");
                }
                else// if (myAnimation.isPlaying == false)
                {
                    //myAnimation.PlayQueued("PlayerStandUp");
                    
                    sitFlag = false;
                }
            }
        }
        private void ChangeZoom(bool zoom)
        {
            if (zoom && zoomFlag == false)
            {
                zoomFlag = true;
                PlayerCore.PlayerCamera.fieldOfView = fov / zoomMultiplier;
                mouseSpeed /= zoomMultiplier / 3;
            }
            else if (zoom == false && zoomFlag)
            {
                zoomFlag = false;
                PlayerCore.PlayerCamera.fieldOfView = fov;
                mouseSpeed *= zoomMultiplier / 3;
            }
        }

        //private void OnControllerColliderHit(ControllerColliderHit hit)
        //{
        //    Rigidbody body = hit.gameObject.GetComponent<Rigidbody>();
        //    if (body != null)
        //    {
        //        body.AddForce(hit.moveDirection * playerMass);
        //    }
        //}

        //public void AttachPlayerToRoot(Rigidbody root, float maxDistance)
        //{
        //    springJoint.connectedBody = root;
        //    springJoint.maxDistance = maxDistance;
        //}
        //public void LetFree()
        //{
        //    springJoint.maxDistance = 0;
        //    springJoint.connectedBody = null;
        //}
    }
}