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
        // 入力値を保持しておく
        _inputMove = context.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        // 空中にいるときは、下向きに重力加速度を与えて落下させる
        _verticalVelocity -= gravity * Time.deltaTime;

        // カメラの向き（角度[deg]）取得
        var cameraAngleY = _targetCamera.transform.eulerAngles.y;

        // 操作入力と鉛直方向速度から、現在速度を計算
        var moveVelocity = new Vector3(_inputMove.x * _speed, _verticalVelocity, _inputMove.y * _speed);

        // カメラの角度分だけ移動量を回転
        moveVelocity = Quaternion.Euler(0, cameraAngleY, 0) * moveVelocity;

        // 現在フレームの移動量を移動速度から計算
        var moveDelta = moveVelocity * Time.deltaTime;

        // CharacterControllerに移動量を指定し、オブジェクトを動かす
        _characterController.Move(moveDelta);

        if (_inputMove != Vector2.zero)
        {
            // 移動入力がある場合は、振り向き動作も行う

            // 操作入力からy軸周りの目標角度[deg]を計算
            var targetAngleY = -Mathf.Atan2(_inputMove.y, _inputMove.x) * Mathf.Rad2Deg + 90;

            // カメラの角度分だけ振り向く角度を補正
            targetAngleY += cameraAngleY;

            // イージングしながら次の回転角度[deg]を計算
            var angleY = Mathf.SmoothDampAngle(_transform.eulerAngles.y, targetAngleY, ref _turnVelocity, 0.1f);

            // オブジェクトの回転を更新
            _transform.rotation = Quaternion.Euler(0, angleY, 0);
        }

    }
}
