using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;

public class GameStart : MonoBehaviour
{
    [SerializeField] private Vector3 _startPositon;
    // Start is called before the first frame update
    void Start()
    {
        BGMManager.Instance.Play(BGMPath.YURUYAKANAKAZE);
        InGameDate.Stage = _startPositon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
