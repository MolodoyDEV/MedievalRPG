using Molodoy.CoreComponents;
using Molodoy.Inspector.Extentions;
using UnityEngine;

[DisallowMultipleComponent]
public class GameHacks : MonoBehaviour
{
    [InspectorButton("Freeze Game")]
    public void FreezeGameByInspector()
    {
        GameProcess.FreezeGame(GetHashCode());
    }

    [InspectorButton("Return previous speed")]
    public void ReturnPreviousGameSpeedByInspector()
    {
        GameProcess.ReturnPreviousSpeed(GetHashCode());
    }

    [InspectorButton("Maximum Game Speed")]
    public void SetMaximumGameSpeedByInspector()
    {
        GameProcess.SetMaximumGameSpeed(GetHashCode());
    }

    [InspectorButton("+1 Game Speed")]
    public void IncreaseGameSpeedByInspector()
    {
        GameProcess.ModifyGameSpeed(+1f, GetHashCode());
    }

    [InspectorButton("-1 Game Speed")]
    public void ReduceGameSpeedByInspector()
    {
        GameProcess.ModifyGameSpeed(-1f, GetHashCode());
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                GameProcess.ModifyGameSpeed(+1f, GetHashCode());
            }
            else if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                GameProcess.ModifyGameSpeed(-1f, GetHashCode());
            }
            else if (Input.GetKeyDown(KeyCode.KeypadDivide))
            {
                GameProcess.SetDefaultGameSpeed(GetHashCode());
            }
            else if (Input.GetKeyDown(KeyCode.KeypadMultiply))
            {
                GameProcess.SetMaximumGameSpeed(GetHashCode());
            }
        }
    }
}
