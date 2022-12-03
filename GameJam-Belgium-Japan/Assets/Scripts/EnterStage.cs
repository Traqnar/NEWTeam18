using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EnterStage : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private string _playerName = "Player";
    private bool isStay;
    // Start is called before the first frame update
    void Start()
    {
        isStay = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != _playerName) return;

        isStay = true;
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != _playerName) return;

        isStay = false;

    }
    public void OnEnter(InputAction.CallbackContext context)
    {
        if (isStay)
        {   Vector3 pos =  this.gameObject.transform.position;
            InGameDate.Stage = new Vector3(pos.x, 0f, pos.z);
            SceneManager.LoadScene(_sceneName);
        }
    }
}
