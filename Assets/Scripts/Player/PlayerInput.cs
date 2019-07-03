using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//在其他脚本前先执行
[DefaultExecutionOrder(-100)]
public class PlayerInput : MonoBehaviour, InputMaster.IPlayerActions
{
	InputMaster inputManager;

	[HideInInspector] public float horizontal;
	[HideInInspector] public bool jumpPressed;
	[HideInInspector] public bool jumpReleased;
	[HideInInspector] public bool attackPressed;

	void Awake()
	{
		inputManager = new InputMaster();
		inputManager.Player.SetCallbacks(this);
	}

	void Update()
	{
		ClearInput();
	}

	void OnEnable()
	{
		inputManager.Player.Enable();
	}

	void OnDisable()
	{
		inputManager.Player.Disable();
	}

	bool IsJoystickConnected()
	{
		if (Input.GetJoystickNames() != null)
			return true;

		return false;
	}

	void ClearInput()
	{
		//horizontal = 0;

		attackPressed = false;

		jumpPressed = false;
		jumpReleased = true;
	}

	public void OnAttack(InputAction.CallbackContext context)
	{
		attackPressed = true;
	}

	public void OnMovement(InputAction.CallbackContext context)
	{
		horizontal = context.ReadValue<float>();
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		jumpPressed = true;

		jumpReleased = false;
	}
}
