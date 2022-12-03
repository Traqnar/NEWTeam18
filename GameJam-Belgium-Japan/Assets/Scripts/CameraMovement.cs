using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public float moveSpeed = 10;
    private float angle = Mathf.PI/6;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(0, verticalInput*Mathf.Sin(angle), verticalInput*Mathf.Cos(angle))* moveSpeed * Time.deltaTime);
        transform.Translate(Vector3.right * horizontalInput * moveSpeed * Time.deltaTime);
    }
}
