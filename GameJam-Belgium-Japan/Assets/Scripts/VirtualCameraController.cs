using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCameraController : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _virtualCamera;
    [SerializeField]
    private float _position;

    private CinemachineTrackedDolly _dolly;

    void Start()
    {
        // Virtual Cameraに対してGetCinemachineComponentでCinemachineTrackedDollyを取得する
        // GetComponentではなくGetCinemachineComponentなので注意
        _dolly = _virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    void Update()
    {
        // パスの位置を更新する
        // 代入して良いのか不安になる変数名だけどこれでOK
        _dolly.m_PathPosition = _position;
    }
}
