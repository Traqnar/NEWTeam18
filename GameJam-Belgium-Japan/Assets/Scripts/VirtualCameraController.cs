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
        // Virtual Camera�ɑ΂���GetCinemachineComponent��CinemachineTrackedDolly���擾����
        // GetComponent�ł͂Ȃ�GetCinemachineComponent�Ȃ̂Œ���
        _dolly = _virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    void Update()
    {
        // �p�X�̈ʒu���X�V����
        // ������ėǂ��̂��s���ɂȂ�ϐ��������ǂ����OK
        _dolly.m_PathPosition = _position;
    }
}
