using UnityEngine;
using System.Collections;

public class ThirdPersonMovement : MonoBehaviour {

    iniParser parser = new iniParser();

    public Transform cameraTransform;

    public AnimationClip idleAnimations;
    public AnimationClip walkAnimation;
    public AnimationClip runAnimation;
    public AnimationClip jumpPoseAnimation;
    public AnimationClip sneakAnimation;

    public float walkMaxAnimationSpeed = 0.75f;
    public float trotAnimationSpeed = 1f;
    public float runAnimationSpeed = 1f;
    public float jumpAnimationSpeed = 1.15f;
    public float landAnimationsSpeed = 1f;
    public float crouchAnimationSpeed = 1f;

    enum CharacterState
    {
        Idle = 0, Walking = 1, Trotting = 1, Running = 3, Jumping = 4, Sneaking = 5
    }

    private CharacterState characterState;

    public float walkSpeed = 1.3f;
    public float trotSpeed = 1.5f;
    public float runSpeed = 3f;
    public float sneakSpeed = 0.9f;
    public float inAirControlAcceleration = 3f;
    public float jumpHieght = 0.5f;
    public float gravity = 20.0f;
    public float speedSmoothing = 10.0f;
    public float rotateSpeed = 300.0f;
    public float trotAfterSeconds = 3.0f;

    public bool allowJump = true;
    public bool allowSneak = true;

    private float jumpRepeateTime = 0.05f;
    private float jumpTimeout = 0.15f;
    private float groundedTimeOut = 0.25f;

    private float lockCameraTimer = 0.0f;
    private Vector3 moveDirection = Vector3.zero;
    private float verticalSpeed = 0.0f;
    private float moveSpeed = 0.0f;
    private CollisionFlags collisionFlags;

    private bool jumping = false;
    private bool jumpingReachedApex = false;

    private bool movingBack = false;
    private bool isMoving = false;
    private float walkTimeStart = 0.0f;
    private float lastJumpButtonTime = -10f;
    private float lastJumpTime = -1.0f;

    private float lastJumpStartHeight = 0.0f;
    private Vector3 inAirVelocity = Vector3.zero;
    private float lastGroundedTime = 0.0f;
    private bool isControllable = true;

    private Animation _animation;

    void Start()
    {

        if (!networkView.isMine)
        {
            enabled = false;
            gameObject.GetComponent<XPmanager>().enabled = false;
        }

        parser.Load(IniFiles.CONFIG);

        moveDirection = transform.TransformDirection(Vector3.forward);

        _animation = GetComponent<Animation>();
    }

    void UpdateSmoothedMovementDirection()
    {
        bool grounded = IsGrounded();

        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        float v = 0, h = 0;

        if (bool.Parse(parser.Get("input_controller")))
        {
            v = Input.GetAxisRaw("win vertical");
            h = Input.GetAxisRaw("win horizontal");
        }
        else
        {
            v = Input.GetAxisRaw("Vertical");
            h = Input.GetAxisRaw("Horizontal");
        }
        
        if (v < -0.2)
            movingBack = true;
        else
            movingBack = false;

        bool wasMoving = isMoving;
        isMoving = Mathf.Abs(h) > 0.01f || Mathf.Abs(v) > 0.1f;

        Vector3 targetDirection = h * right + v * forward;

        if (grounded)
        {
            lockCameraTimer += Time.deltaTime;
            if (isMoving != wasMoving)
                lockCameraTimer = 0.0f;

            if (targetDirection != Vector3.zero)
            {
                if (moveSpeed < walkSpeed * 0.9 && grounded)
                {
                    moveDirection = targetDirection.normalized;
                }
                else
                {
                    moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
                    moveDirection = moveDirection.normalized;
                }
            }

            float curSmooth = speedSmoothing * Time.deltaTime;

            float targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);

            characterState = CharacterState.Idle;

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetButton("L3"))
            {
                targetSpeed *= runSpeed;
                characterState = CharacterState.Running;
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                targetSpeed *= sneakSpeed;
                characterState = CharacterState.Sneaking;
            }
            else if (Time.time - trotAfterSeconds > walkTimeStart)
            {
                targetSpeed *= trotSpeed;
                characterState = CharacterState.Trotting;
            }
            else
            {
                targetSpeed *= walkSpeed;
                characterState = CharacterState.Walking;
            }

            moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);

            if (moveSpeed < walkSpeed * 0.3)
            {
                walkTimeStart = Time.time;
            }
        }
        else
        {
            if (jumping)
                lockCameraTimer = 0.0f;

            if (isMoving)
                inAirVelocity += targetDirection.normalized * Time.deltaTime * inAirControlAcceleration;
        }
    }

    void ApplyJumping()
    {
        if (lastJumpTime + jumpRepeateTime > Time.time)
            return;

        if (IsGrounded())
        {
            if (allowJump && Time.time < lastJumpButtonTime + jumpTimeout)
            {
                verticalSpeed = CalculateJumpVerticalSpeed(jumpHieght);
                SendMessage("Didjump", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void ApplyGravity()
    {
        if (isControllable)
        {
            bool jumpButton = Input.GetButton("Jump");

            if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0f)
            {
                jumpingReachedApex = true;
                SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
            }

            if (IsGrounded())
            {
                verticalSpeed = 0.0f;
            }
            else
            {
                verticalSpeed -= gravity * Time.deltaTime;
            }
        }
    }

    float CalculateJumpVerticalSpeed(float targetJumpHeight)
    {
        return Mathf.Sqrt(2 * targetJumpHeight * gravity);
    }

    void DidJump()
    {
        jumping = true;
        jumpingReachedApex = false;
        lastJumpTime = Time.time;
        lastJumpStartHeight = transform.position.y;
        lastJumpButtonTime = -10;

        characterState = CharacterState.Jumping;
    }

    void Update()
    {
        if (!isControllable)
        {
            Input.ResetInputAxes();
        }

        if (Input.GetButtonDown("Jump"))
        {
            lastJumpButtonTime = Time.time;
        }

        UpdateSmoothedMovementDirection();

        ApplyGravity();

        ApplyJumping();

        Vector3 movement = moveDirection * moveSpeed + new Vector3(0, verticalSpeed, 0) + inAirVelocity;
        movement *= Time.deltaTime;

        CharacterController controller = GetComponent<CharacterController>();
        collisionFlags = controller.Move(movement);

        if (_animation)
        {
            if (characterState == CharacterState.Jumping)
            {

                if (!jumpingReachedApex)
                {
                    _animation[jumpPoseAnimation.name].speed = jumpAnimationSpeed;
                    _animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
                    _animation.CrossFade(jumpPoseAnimation.name);
                }
                else
                {
                    _animation[jumpPoseAnimation.name].speed = -landAnimationsSpeed;
                    _animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
                    _animation.CrossFade(jumpPoseAnimation.name);
                }
            }
            else if (characterState == CharacterState.Sneaking)
            {


            }
            else
            {
                if (controller.velocity.sqrMagnitude < 0.1)
                {
                    _animation.CrossFade(idleAnimations.name);
                }
                else
                {
                    if (characterState == CharacterState.Running)
                    {
                        _animation[runAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, runAnimationSpeed);
                        _animation.CrossFade(runAnimation.name);
                    }
                    else if (characterState == CharacterState.Trotting)
                    {
                        _animation[walkAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, trotAnimationSpeed);
                        _animation.CrossFade(walkAnimation.name);
                    }
                    else if (characterState == CharacterState.Walking)
                    {
                        _animation[walkAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, walkMaxAnimationSpeed);
                        _animation.CrossFade(walkAnimation.name);
                    }
                }
            }
        }

        if (IsGrounded())
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
        else
        {
            Vector3 xzMove = movement;
            xzMove.y = 0;
            if (xzMove.sqrMagnitude > 0.001)
            {
                transform.rotation = Quaternion.LookRotation(xzMove);
            }
        }

        if (IsGrounded())
        {
            lastGroundedTime = Time.time;
            inAirVelocity = Vector3.zero;
            if (jumping)
            {
                jumping = false;
                SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    float GetSpeed()
    {
        return moveSpeed;
    }

    bool IsJumping()
    {
        return jumping;
    }

    bool IsGrounded()
    {
        return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
    }

    Vector3 GetDirection()
    {
        return moveDirection;
    }

    bool IsMovingBackwards()
    {
        return movingBack;
    }

    float GetLockCameraTimer()
    {
        return lockCameraTimer;
    }

    bool IsMoving()
    {
        return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f;
    }

    bool HasJumpReachedApex()
    {
        return jumpingReachedApex;
    }

    bool IsGroundedWithTimeout()
    {
        return lastGroundedTime + groundedTimeOut > Time.time;
    }

    void Reset()
    {
        gameObject.tag = "Player";
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {

        Debug.Log(hit.moveDirection.y);

        if (!hit.rigidbody || hit.moveDirection.y < -0.10) { return; }

        Vector3 moveDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        hit.rigidbody.velocity = moveDir * 2.0f;
    }
}
