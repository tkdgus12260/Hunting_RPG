using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour, IControllable
{
    private Vector3 inputDir = Vector3.zero;

    public Vector3 KeyboardInputDir
    {
        set
        {
            inputDir = value;
        }
    }

    private Vector2 mouseDelta = Vector2.zero;

    public Vector2 MouseDelta
    {
        set
        {
            mouseDelta = value;
        }
    }

    public IControllable.InputActionDelegate OnAttack { get; set; }
    public IControllable.InputActionDelegate OnRolling { get; set; }
    public IControllable.InputActionDelegate OnJump { get; set; }
    public IControllable.InputActionDelegate OnInventory { get; set; }
    public IControllable.InputActionDelegate OnPause { get; set; }

    public Transform objToFollow;
    // 따라가는 카메라 속도
    public float followSpeed = 10.0f;
    // 마우스 감도
    public float sensitivity = 100.0f;
    // 제한 각도
    public float clampAngle = 70.0f;

    private float rotX;
    private float rotY;

    // 카메라 정보
    public Transform realCamera;
    // 카메라 방향
    public Vector3 dirNormalized;
    // 최종적으로 저장된 카메라 방향
    public Vector3 finalDir;
    public float minDistance;
    public float maxDistance;
    public float finalDistance;
    public float smoothness = 10.0f;

    private void Awake()
    {
        this.transform.position = DataManager.Inst.Player.playerPos;
    }

    private void Start()
    {
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        dirNormalized = realCamera.localPosition.normalized;
        finalDistance = realCamera.localPosition.magnitude;


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        rotX += -(mouseDelta.y) * sensitivity * Time.deltaTime;
        rotY += mouseDelta.x * sensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, objToFollow.position, followSpeed);

        finalDir = transform.TransformPoint(dirNormalized * maxDistance);

        RaycastHit hit;

        if(Physics.Linecast(transform.position, finalDir, out hit))
        {
            finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            finalDistance = maxDistance;
        }
        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNormalized * finalDistance, Time.deltaTime * smoothness);
    }
}
