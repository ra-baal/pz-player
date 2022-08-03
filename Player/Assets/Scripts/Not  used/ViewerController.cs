using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewerController : MonoBehaviour
{
	public float sensitivityKeyboard = 10f;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			transform.Translate(new Vector3(1 / sensitivityKeyboard, 0, 0));
		}
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			transform.Translate(new Vector3(-1 / sensitivityKeyboard, 0, 0));
		}
		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			transform.Translate(new Vector3(0, 0, -1 / sensitivityKeyboard));
		}
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{
			transform.Translate(new Vector3(0, 0, 1 / sensitivityKeyboard));
		}

	}
}
