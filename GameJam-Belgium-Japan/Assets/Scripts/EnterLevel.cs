using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterLevel : MonoBehaviour
{

    // Add public variable to a level object.
    public GameObject level;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown(){

        // Display level by activating the object.
        level.SetActive(!level.activeSelf);
        
    }

}
