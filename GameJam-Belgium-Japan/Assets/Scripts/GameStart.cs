using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    [SerializeField] private Vector3 _startPositon;
    // Start is called before the first frame update
    void Start()
    {
        InGameDate.Stage = _startPositon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
