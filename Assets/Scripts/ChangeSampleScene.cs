using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class ChangeSampleScene : MonoBehaviour
{
	public bool start;

#if ENABLE_INPUT_SYSTEM


	public void OnStart(InputAction.CallbackContext context)
	{
		start = context.ReadValueAsButton();
	}


#endif
	private void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}
}