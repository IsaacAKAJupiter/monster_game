using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMouseLook : MonoBehaviour {

    Vector2 mouseLook;
    Vector2 smoothV;
    public float sensitivity = 5.0f;
    public float smoothing = 2.0f;

    GameObject player;

	// Use this for initialization
	void Start () {
        player = this.transform.parent.gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        if (StaticClasses.IsInBattle == false && StaticClasses.IsGamePaused == false && StaticClasses.IsInDialogue == false && StaticClasses.UIIsOpen == false)
        {
            var MouseDirection = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

            MouseDirection = Vector2.Scale(MouseDirection, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
            smoothV.x = Mathf.Lerp(smoothV.x, MouseDirection.x, 1f / smoothing);
            smoothV.y = Mathf.Lerp(smoothV.y, MouseDirection.y, 1f / smoothing);
            mouseLook += smoothV;
            mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);

            transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
            player.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, player.transform.up);
        }
	}
}
