using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerController : MonoBehaviour
{
    #region Anim
    [Header("Anim")]
    [SerializeField] private Animator animator;
    [SerializeField] private AnimatorEventListner animatorEventListner;
    [SerializeField] private Rig rig;

    public readonly int animMoveX = Animator.StringToHash("MoveX");
    public readonly int animMoveY = Animator.StringToHash("MoveY");
    public readonly int animSpeed = Animator.StringToHash("Speed");
    public readonly int animJump = Animator.StringToHash("Jump");
    public readonly int animIsGround = Animator.StringToHash("IsGround");
    #endregion

    #region Move
    [Header("Move")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpSpeed;
    private Vector3 moveDir;
    bool isRun;
    #endregion

    #region Rotate
    [Header("Rotate")]
    [Range(100, 800), SerializeField] private float inputSenservity = 400;
    [SerializeField] private Transform followTransform;
    [SerializeField] private float rotationLerp;
    #endregion

    #region Aim
    [Header("Aim")]
    [SerializeField] private GameObject aimingCam;
    [SerializeField] private CanvasGroup aimCanvasGroup;
    [SerializeField] private Transform aimTarget;
    private Entity curFocusEntity;
    private Camera cam;
    private bool isAiming;
    private bool isInAir;
    private float lastAimTargetDistance;
    public bool isCanShoot { get { return !isRun && isAiming && !isInAir; } }
    #endregion

    #region HP
    private float playerHP = 10;
    public GameManager gameManager;
    #endregion

    public void Start()
    {
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Update()
    {
        Aiming();
        Move();
        Rotate();
        Animate();
    }

    private void Aiming()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit))
        {
            aimTarget.position = Vector3.Lerp(aimTarget.position, hit.point, Time.deltaTime * 7.5f);
            lastAimTargetDistance = Vector3.Distance(cam.transform.position, aimTarget.position);

            if (hit.transform.TryGetComponent<Entity>(out Entity entity))
            {
                if (entity != null && entity != curFocusEntity)
                {
                    if (curFocusEntity != null)
                        curFocusEntity.OnFocusOut?.Invoke();

                    curFocusEntity = entity;
                    curFocusEntity.OnFocusIn?.Invoke();
                }
            }
            else
            {
                if (curFocusEntity != null)
                    curFocusEntity.OnFocusOut?.Invoke();

                curFocusEntity = null;
            }
        }
        else
        {
            aimTarget.position = Vector3.Lerp(aimTarget.position, cam.transform.position + cam.transform.forward * lastAimTargetDistance, Time.deltaTime * 7.5f);

            if (curFocusEntity != null)
                curFocusEntity.OnFocusOut?.Invoke();

            curFocusEntity = null;
        }
        if (!isCanShoot)
        {
            rig.weight = Mathf.Lerp(rig.weight, 0f, Time.deltaTime * 7.5f);
            aimCanvasGroup.alpha = Mathf.Lerp(aimCanvasGroup.alpha, 0.25f, Time.deltaTime * 5f);
        }
        else if (isCanShoot)
        {
            rig.weight = Mathf.Lerp(rig.weight, 1f, Time.deltaTime * 7.5f);
            aimCanvasGroup.alpha = Mathf.Lerp(aimCanvasGroup.alpha, 1f, Time.deltaTime * 5f);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            aimingCam.gameObject.SetActive(true);
            isAiming = true;
            DOTween.To(() => aimCanvasGroup.alpha, x => aimCanvasGroup.alpha = x, 1, 0.25f);
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            aimingCam.gameObject.SetActive(false);
            isAiming = false;
            DOTween.To(() => aimCanvasGroup.alpha, x => aimCanvasGroup.alpha = x, 0, 0.25f);
        }
    }

    private void Rotate()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y")) * inputSenservity * Time.deltaTime;

        #region Follow Transform Rotation
        followTransform.rotation *= Quaternion.AngleAxis(input.x, Vector3.up);
        #endregion

        #region Vertical Rotation
        followTransform.rotation *= Quaternion.AngleAxis(input.y, Vector3.right);

        var angles = followTransform.localEulerAngles;
        angles.z = 0;

        var angle = followTransform.localEulerAngles.x;

        if (angle > 180 && angle < 305)
        {
            angles.x = 305;
        }
        else if (angle < 180 && angle > 75)
        {
            angles.x = 75;
        }

        followTransform.localEulerAngles = angles;
        #endregion

        //nextRotation = Quaternion.Lerp(followTransform.rotation, nextRotation, Time.deltaTime * rotationLerp);

        if (moveDir.x == 0 && moveDir.z == 0)
        {
            if (isAiming)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, followTransform.rotation.eulerAngles.y, 0), Time.deltaTime * rotationLerp * 2);
                followTransform.localRotation = Quaternion.Lerp(followTransform.localRotation, Quaternion.Euler(angles.x, 0, 0), Time.deltaTime * rotationLerp * 2);
            }
            return;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, followTransform.rotation.eulerAngles.y, 0), Time.deltaTime * rotationLerp);
        followTransform.localRotation = Quaternion.Lerp(followTransform.localRotation, Quaternion.Euler(angles.x, 0, 0), Time.deltaTime * rotationLerp);
    }

    private void Move()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Normalize();

        moveDir = new Vector3(input.x, moveDir.y, input.y);

        if (controller.isGrounded)
        {
            moveDir.y = 0;
        }
        else
        {
            moveDir.y -= Time.deltaTime * 16f;
        }
        animator.SetBool(animIsGround, controller.isGrounded);

        if (Input.GetKeyDown(KeyCode.Space) && !isInAir)
        {
            moveDir.y = jumpSpeed;
            animator.SetTrigger(animJump);
            isInAir = true;
        }
        if (isInAir && controller.isGrounded && Mathf.Abs(moveDir.y) < 0.2f)
        {
            isInAir = false;
        }

        isRun = Input.GetKey(KeyCode.LeftShift) && input.y > 0;

        Vector3 moveVector = transform.TransformDirection(moveDir * Time.deltaTime);
        moveVector.x *= isRun ? runSpeed : speed;
        moveVector.z *= isRun ? runSpeed : speed;
        moveVector.y *= speed;

        controller.Move(moveVector);
    }

    void Animate()
    {
        animator.SetFloat(animMoveX, Input.GetAxis("Horizontal"));
        animator.SetFloat(animMoveY, Input.GetAxis("Vertical"));
        animator.SetFloat(animSpeed, Mathf.Lerp(animator.GetFloat(animSpeed), Input.GetKey(KeyCode.LeftShift) ? 2 : 1, Time.deltaTime * 4));

        if (moveDir.x == 0 && moveDir.y != 0)
        {
            animatorEventListner.listenKey = "Vertical";
        }
        else if (moveDir.y == 0 && moveDir.x != 0)
        {
            animatorEventListner.listenKey = "Horizontal";
        }
    }

    private void FixedUpdate()
    {
        PlayerHP();

    }
    void PlayerHP()
    {
        if (playerHP <= 0)
        {
          //  Debug.Log("게임 오버");
            gameManager.GameOver();
            return;
        }

        gameManager.GetComponent<GameManager>().PlayerInit(this);
        playerHP -= Time.deltaTime;
    }
}
