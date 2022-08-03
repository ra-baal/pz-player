using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour 
{
	public float speed = 5.0f;

	public PauseMenuManager pauseMenuManager;
	public Transform viewerBody;
	public float mouseSensitivity = 100f;
	float xRotation = 0f;

	public float sensitivityKeyboard = 10f;

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

    public void Move()
    {
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			viewerBody.Translate(new Vector3(1 / sensitivityKeyboard, 0, 0));
		}
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			viewerBody.Translate(new Vector3(-1 / sensitivityKeyboard, 0, 0));
		}
		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			viewerBody.Translate(new Vector3(0, 0, -1 / sensitivityKeyboard));
		}
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{
			viewerBody.Translate(new Vector3(0, 0, 1 / sensitivityKeyboard));
		}

	}

	public void LookAround()
    {
		float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -90f, 90f);

		transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
		viewerBody.Rotate(Vector3.up * mouseX);
	}

    void Update ()
	{
		if (!pauseMenuManager.isPaused)
		{
			Cursor.lockState = CursorLockMode.Locked;
			LookAround();
			Move();
		}
		else Cursor.lockState = CursorLockMode.None;
	}
}