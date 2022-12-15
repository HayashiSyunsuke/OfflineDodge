using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class InputAsist : MonoBehaviour
{
	[Header("Character Input Values")]
	public Vector2 move;
	public Vector2 look;
	public bool jump;
	public bool sprint;
	public bool throwing;
	public bool left;
	public bool right;
	public bool dodge;
	public bool catching;
	public bool pass;
	public bool faint;
	public bool untarget;
	public bool returned;

	[Header("Movement Settings")]
	public bool analogMovement;

	[Header("Mouse Cursor Settings")]
	public bool cursorLocked = true;
	public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
	public void OnMove(InputAction.CallbackContext context)
	{
		//MoveInput(value.Get<Vector2>());
		move = context.ReadValue<Vector2>();
	}

	public void OnLook(InputAction.CallbackContext context)
	{
		if (cursorInputForLook)
		{
			//LookInput(value.Get<Vector2>());
			look = context.ReadValue<Vector2>();
		}
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		//JumpInput(value.isPressed);
		jump = context.ReadValueAsButton();
		Debug.Log("ジャンプ");
	}

	public void OnSprint(InputAction.CallbackContext context)
	{
		//SprintInput(value.isPressed);
		sprint = context.ReadValueAsButton();
		Debug.Log("走る");
	}

	public void OnThrowing(InputAction.CallbackContext context)
	{
		//ThrowingInput(value.isPressed);
		throwing = context.ReadValueAsButton();
		Debug.Log("投げる");
	}
	public void OnLeftArrowState(InputAction.CallbackContext context)
	{
		//LeftArrowInput(value.isPressed);
		left = context.ReadValueAsButton();
		Debug.Log("左");
	}
	public void OnRightArrowState(InputAction.CallbackContext context)
	{
		//RightArrowInput(value.isPressed);
		right = context.ReadValueAsButton();
		Debug.Log("右");
	}
	public void OnDodge(InputAction.CallbackContext context)
	{
		dodge = context.ReadValueAsButton();
		Debug.Log("回避");
	}

	public void OnCatch(InputAction.CallbackContext context)
	{
		catching = context.ReadValueAsButton();
		Debug.Log("キャッチ");
	}

	public void OnPass(InputAction.CallbackContext context)
	{
		pass = context.ReadValueAsButton();
		Debug.Log("パス");
	}

	public void OnFaint(InputAction.CallbackContext context)
	{
		faint = context.ReadValueAsButton();
		Debug.Log("フェイント");
	}

	public void OnUntargeted(InputAction.CallbackContext context)
	{
		untarget = context.ReadValueAsButton();
	}

	public void OnReturn(InputAction.CallbackContext context)
	{
		returned = context.ReadValueAsButton();
	}


#endif
	public void MoveInput()
	{
		//move = newMoveDirection;
	}

	public void LookInput()
	{
		//look = newLookDirection;
	}

	public void JumpInput()
	{
		//jump = newJumpState;
	}

	public void SprintInput()
	{
		//sprint = newSprintState;
	}

	public void ThrowingInput()
	{
		//throwing = newThrowState;
	}
	public void LeftArrowInput()
	{
		//left = newLeftArrowState;
	}
	public void RightArrowInput()
	{
		//right = newRightArrowState;
	}

	public void DodgeInput()
	{
		//away = newAwayState;
	}

	public void CatchInput()
	{
		//catch = newCatchState;
	}
	//private void OnApplicationFocus()
	//{
	//    SetCursorState(cursorLocked);
	//}

	private void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}
}