using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    const float gravity = 9.80665f;
    [SerializeField] private float _speed;

    [SerializeField]private Camera _targetCamera;

    private CharacterController _characterController;

    private Vector2 _inputMove;
    private float _verticalVelocity;

    private float _turnVelocity;
    private Transform _transform;
    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _transform = transform;
        if (_targetCamera == null) _targetCamera = Camera.main;
        Debug.Log("set:"+InGameDate.Stage);
        //_transform.position = InGameDate.Stage;
        _characterController.Move(InGameDate.Stage);
        Debug.Log("now:"+_transform.position);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // ���͒l��ێ����Ă���
        _inputMove = context.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        // �󒆂ɂ���Ƃ��́A�������ɏd�͉����x��^���ė���������
        _verticalVelocity -= gravity * Time.deltaTime;

        // �J�����̌����i�p�x[deg]�j�擾
        var cameraAngleY = _targetCamera.transform.eulerAngles.y;

        // ������͂Ɖ����������x����A���ݑ��x���v�Z
        var moveVelocity = new Vector3(_inputMove.x * _speed, _verticalVelocity, _inputMove.y * _speed);

        // �J�����̊p�x�������ړ��ʂ���]
        moveVelocity = Quaternion.Euler(0, cameraAngleY, 0) * moveVelocity;

        // ���݃t���[���̈ړ��ʂ��ړ����x����v�Z
        var moveDelta = moveVelocity * Time.deltaTime;

        // CharacterController�Ɉړ��ʂ��w�肵�A�I�u�W�F�N�g�𓮂���
        _characterController.Move(moveDelta);

        if (_inputMove != Vector2.zero)
        {
            // �ړ����͂�����ꍇ�́A�U�����������s��

            // ������͂���y������̖ڕW�p�x[deg]���v�Z
            var targetAngleY = -Mathf.Atan2(_inputMove.y, _inputMove.x) * Mathf.Rad2Deg + 90;

            // �J�����̊p�x�������U������p�x��␳
            targetAngleY += cameraAngleY;

            // �C�[�W���O���Ȃ��玟�̉�]�p�x[deg]���v�Z
            var angleY = Mathf.SmoothDampAngle(_transform.eulerAngles.y, targetAngleY, ref _turnVelocity, 0.1f);

            // �I�u�W�F�N�g�̉�]���X�V
            _transform.rotation = Quaternion.Euler(0, angleY, 0);
        }

    }
}
