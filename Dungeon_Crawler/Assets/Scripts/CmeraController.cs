using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmeraController : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private float mouseSensitivity; // weight to use mouse input

    // REFERENCES- references to gameobjects (example - player)
    private Transform parent; // reference to Parent object higher in hierarchy

    private void Start()
    {
        parent = transform.parent; // transform parent object (player)
        Cursor.lockState = CursorLockMode.Locked; // lock cursor to center of screen
    }
    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        // get mouse input from side to side mouse motion
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        // rotate around Y/"up" axis based on input
        parent.Rotate(Vector3.up, mouseX);
    }

}
