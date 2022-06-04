using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    Vector2 mouseClickPos;
    Vector2 mouseCurrentPos;
    bool panning = false;
    private void Start()
    {
        if(GameDataStore.CurrentLevel <= 3)
        {
            setCameraPosition(-15f);
        }
        else if(GameDataStore.CurrentLevel <= 6)
        {
            setCameraPosition(-7.5f);
        }
        else if (GameDataStore.CurrentLevel <= 9)
        {
            setCameraPosition(0f);
        }
        else if (GameDataStore.CurrentLevel <= 12)
        {
            setCameraPosition(7.5f);
        }
        else if (GameDataStore.CurrentLevel <= 15)
        {
            setCameraPosition(15f);
        }
    }
    private void Update()
    {
        // When LMB clicked get mouse click position and set panning to true
        if (Input.GetKeyDown(KeyCode.Mouse0) && !panning)
        {
            mouseClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            panning = true;
        }
        // If LMB is already clicked, move the camera following the mouse position update
        if (panning)
        {
            mouseCurrentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var distance = mouseCurrentPos - mouseClickPos;
            transform.position += new Vector3(0, -distance.y, 0);
        }
        transform.position = new Vector3(0, Mathf.Clamp(transform.position.y, -15f, 15f), -10f);
        // If LMB is released, stop moving the camera
        if (Input.GetKeyUp(KeyCode.Mouse0))
            panning = false;
    }

    public void setCameraPosition(float pos)
    {
        transform.position = new Vector3(0, Mathf.Clamp(pos, -15f, 15f), -10f);
    }
}
