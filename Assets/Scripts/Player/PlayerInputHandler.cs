using Molodoy.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Molodoy.CoreComponents
{
    public class PlayerInputHandler
    {
        private static bool canProcessInput;
        public static bool CanProcessInput { get => canProcessInput; set => canProcessInput = value; }

        public static int GetFInputDown()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                return 1;
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                return 2;
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                return 3;
            }
            else if (Input.GetKeyDown(KeyCode.F4))
            {
                return 4;
            }
            else if (Input.GetKeyDown(KeyCode.F5))
            {
                return 5;
            }
            else if (Input.GetKeyDown(KeyCode.F6))
            {
                return 6;
            }
            else if (Input.GetKeyDown(KeyCode.F7))
            {
                return 7;
            }
            else if (Input.GetKeyDown(KeyCode.F8))
            {
                return 8;
            }
            else if (Input.GetKeyDown(KeyCode.F9))
            {
                return 9;
            }
            else if (Input.GetKeyDown(KeyCode.F10))
            {
                return 10;
            }
            else if (Input.GetKeyDown(KeyCode.F11))
            {
                return 11;
            }
            else if (Input.GetKeyDown(KeyCode.F12))
            {
                return 12;
            }

            return -1;
        }

        public static int GetNumericInputDown()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                return 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                return 2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                return 3;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                return 4;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                return 5;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                return 6;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                return 7;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                return 8;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                return 9;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                return 0;
            }

            return -1;
        }

        public static bool GetEscapeInputDown()
        {
            return Input.GetKeyDown(KeyCode.Escape);
        }

        public static bool GetZInput()
        {
            return Input.GetKey(KeyCode.Z);
        }

        public static bool GetQInput()
        {
            return Input.GetKey(KeyCode.Q);
        }

        public static bool GetLShiftInput()
        {
            return Input.GetKey(KeyCode.LeftShift);
        }

        public static bool GetRightMouseInputDown()
        {
            if (CanProcessInput)
            {
                return Input.GetMouseButtonDown(1);
            }

            return false;
        }

        public static bool GetLeftMouseInputDown()
        {
            if (CanProcessInput)
            {
                return Input.GetMouseButtonDown(0);
            }

            return false;
        }

        public static bool GetRightMouseInputHeld()
        {
            if (CanProcessInput)
            {
                return Input.GetMouseButton(1);
            }

            return false;
        }

        public static bool GetLeftMouseInputHeld()
        {
            if (CanProcessInput)
            {
                return Input.GetMouseButton(0);
            }

            return false;
        }

        public static bool GetCancelInputDown()
        {
            if (CanProcessInput)
            {
                return Input.GetButtonDown(GameConstants.Key_CancelName);
            }

            return false;
        }

        /// <returns>Y axis</returns>
        public static float GetMouseAxisHorizontal()
        {
            return Input.GetAxis(GameConstants.Axis_MouseHorizontalName);
        }

        /// <returns>X axis</returns>
        public static float GetMouseAxisVertical()
        {
            return Input.GetAxis(GameConstants.Axis_MouseVerticalName);
        }

        /// <returns>Vector 3 axis</returns>
        public static Vector3 GetMouseMovementDirection()
        {
            return new Vector3(Input.GetAxis(GameConstants.Axis_MouseVerticalName), 0, Input.GetAxis(GameConstants.Axis_MouseHorizontalName));
        }

        public static bool GetJumpInputHeld()
        {
            if (CanProcessInput)
            {
                return Input.GetButton(GameConstants.Key_JumpName);
            }

            return false;
        }

        public static bool GetRunInputHeld()
        {
            return Input.GetButton(GameConstants.Key_RunName);
        }

        public static bool GetCameraSneakInputHeld()
        {
            if (CanProcessInput)
            {
                return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            }

            return false;
        }

        public static bool GetZoomInputHeld()
        {
            if (CanProcessInput)
            {
                return Input.GetButton(GameConstants.Key_ZoomName);
            }

            return false;
        }

        public static bool GetSitInputHeld()
        {
            return Input.GetButton(GameConstants.Key_SitName);
        }

        public static bool GetManagementInputDown()
        {
            return Input.GetButtonDown(GameConstants.Key_ManagemenrMenuName);
        }

        public static Vector3 GetMovementDirection()
        {
            return new Vector3(Input.GetAxis(GameConstants.Axis_HorizontalName), 0, Input.GetAxis(GameConstants.Axis_VerticalName));
        }

        public static Vector3 GetFlyingDirection()
        {
            if (CanProcessInput)
            {
                Vector3 direction = GetMovementDirection();

                if (GetZInput())
                {
                    direction.y = -1f;
                }
                else if (GetQInput())
                {
                    direction.y = 1f;
                }


                return direction;
            }

            return Vector3.zero;
        }

        public static float GetMouseScrollDeltaY()
        {
            if (CanProcessInput)
            {
                return Input.mouseScrollDelta.y;
            }

            return 0f;
        }

        #region Always
        public static float GetMouseScrollDeltaYAlways()
        {
            return Input.mouseScrollDelta.y;
        }

        public static bool GetHelpInputDownAlways()
        {
            return Input.GetButtonDown(GameConstants.Key_HelpName);
        }

        public static bool GetRightMouseInputDownAlways()
        {
            return Input.GetMouseButtonDown(1);
        }

        public static bool GetLeftMouseInputDownAlways()
        {
            return Input.GetMouseButtonDown(0);
        }

        public static bool GetZoomInputHeldAlways()
        {
            return Input.GetButton(GameConstants.Key_ZoomName);
        }

        public static bool GetLCtrlInputHeldAlways()
        {
            return Input.GetKey(KeyCode.LeftControl);
        }
        #endregion
    }
}