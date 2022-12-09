using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]

public class ThirdPersonController : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] PhotonDataSend photonDataSend;



    /*-----PlayerStatus-----*/
    [Header("Player")]
    //�L�����N�^�[�̈ʒu
    [Tooltip("Character position")] private Vector3 position;
    //�L�����N�^�[��WalkSpeed
    [Tooltip("Move Speed of the character")] public float MoveSpeed = 2.0f;
    //�L�����N�^�[��SprintSpeed
    [Tooltip("Sprint Speed of the character")] public float SprintSpeed = 5.3f;
    //�L�����N�^�[��DodgeSpeed
    [Tooltip("Dodge speed of the character")] public float DodgeSpeed = 5.3f;
    //�L�����N�^�[�̐U��������X���[�Y�ɂ��鎞��
    [Tooltip("The speed of the character's swing in the direction of movement")] [Range(0.0f, 0.3f)] public float RotationSmoothTime = 0.12f;
    //�L�����N�^�[�̉����x&�����x
    [Tooltip("Acceleration and Deceleration")] public float SpeedChangeRate = 10.0f;
    //�L�����N�^�[�̃I�[�f�B�I�N���b�v
    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;
    //�L�����N�^�[�̃W�����v��
    [Space(10)]
    [Tooltip("Jump height of the character")] public float JumpHeight = 1.2f;
    //�L�����N�^�[�ɂ�����d��
    [Tooltip("Gravity Value of the uses character")] public float Gravity = -15.0f;
    //�ĂуW�����v�ł���悤�ɂȂ�܂ł̃N�[���^�C��
    [Space(10)]
    [Tooltip("Time required to pass before begin able to jump again")] public float JumpTimeout = 0.5f;
    //�]�|��ԂɂȂ�܂ł̎���(�K�i�𑖂��č~���Ƃ��Ɏg�p)
    [Tooltip("Time required to pass before entering the fall state")] public float FallTimeout = 0.15f;
    //�Ăѓ������悤�ɂȂ�܂ł̃N�[���^�C��
    [Tooltip("Time required to pass before begin able to throw again")] public float ThrowTimeout = 0.5f;
    //�Ăє������悤�ɂȂ�܂ł̃N�[���^�C��
    [Tooltip("Time required to pass before begin able to dodge again")] public float DodgeTimeout = 0.3f;
    //�ĂуL���b�`���삪�ł���悤�ɂȂ�܂ł̃N�[���^�C��
    [Tooltip("Time required to pass before begin able to catch again")] public float CatchTimeout = 0.6f;
    //�Ăуp�X���삪�ł���悤�ɂȂ�܂ł̃N�[���^�C��
    [Tooltip("Time required to pass before begin able to pass again")] public float PassTimeout = 0.5f;
    //�Ăуt�F�C���g���삪�ł���悤�ɂȂ�܂ł̃N�[���^�C��
    [Tooltip("Time required to pass before begin able to faint again")] public float FaintTimeout = 0.5f;

    /*-----�v���C���[�ƒn��-----*/
    [Header("Player Grounded")]
    //�L�����N�^�[���n�ʂƐڒn���Ă��邩
    [Tooltip("If the character is grounded or not")] public bool Grounded = true;
    //�s���n�Ŏg�p
    [Tooltip("Useful for rough ground")] public float GroundedOffset = -0.14f;
    //�ڒn�`�F�b�N�̔��a
    [Tooltip("The radius of the grounded check")] public float GroundedRadius = 0.28f;
    //�ǂ̃��C���[�Ɠ����蔻�����邩
    [Tooltip("What layers the character uses as ground")] public LayerMask GroundLayers;

    /*-----�v���C���[�ƃ{�[��-----*/
    [Header("Player HittingBall")]
    //�L�����N�^�[�ƃ{�[�����ڂ��Ă��邩
    [Tooltip("If the character is hit or not to ball")] public bool HasBall = false;
    //�s���n�Ŏg�p
    [Tooltip("Useful for rough balll")] public float BallHitOffset = -0.05f;
    //�{�[���`�F�b�N�̔��a
    [Tooltip("The radius of the ball check")] public float BallCheckRadius = 0.5f;
    //�ǂ̃^�O�̃I�u�W�F�N�g�Ɠ����蔻�����邩
    [Tooltip("What Tags the character use as ball")] public LayerMask BallLayers;
    //�{�[���𔻒肷��R���C�_�[�̈ʒu
    [Tooltip("The position of the ball check")] public Vector3 playerDirection = Vector3.zero;
    //�{�[���̃��C���[�ԍ�
    [Tooltip("Layer number of the ball")] public int BallLayerNumber = 11;
    //���������
    [Tooltip("Direction of the player")] private Vector3 spherePosition;
    //�L�����N�^�[���{�[�����������Ă��邩
    [Tooltip("Does the character have the character")] private bool isBallHaving = false;

    /*-----�v���C���[�̓����铮��-----*/
    [Header("Player Throwing")]
    //�L�����N�^�[�������Ă��邩
    [Tooltip("If the character is throwing or not")] public bool Throwing = false;
    //�L�����N�^�[���p�X���Ă��邩
    [Tooltip("If the character is passing or not")] public bool Passing = false;
    //�L�����N�^�[���t�F�C���g�����Ă��邩
    [Tooltip("If the character is fianting or not")] public bool Fainting = false;

    /*-----�v���C���[�̉�𓮍�-----*/
    [Header("Player Dodging")]
    //�L�����N�^�[�������Ă��邩
    [Tooltip("If the character is dodging or not")] public bool Dodging = false;

    /*-----�L���b�`-----*/
    [Header("PlayerCatching")]
    //�L�����N�^�[���L���b�`���Ă��邩
    [Tooltip("If the character is catching or not")] public bool Catching = false;
    Vector3 ballCheckSpherePosition;

    /*-----�^�[�Q�b�g-----*/
    //[Header("Target list")] private List<GameObject> enemyList = new List<GameObject>();
    [Header("Target list")] private GameObject[] enemies; 
    [Header("Current targets")] public GameObject CurrentTarget;
    /*-----�V�l�}�V�[��-----*/
    [Header("Cinemachine")]
    //�V�l�}�V�[��
    [Tooltip("the follow target set in the Cinemachine Virtual Camera that the camera will follow")] public GameObject CinemachineCameraTarget;
    //�J���������x�܂ŏ�������邩
    [Tooltip("How far in degrees can you move the camera up")] public float TopClamp = 70.0f;
    //�J���������x�܂ŉ��������邩
    [Tooltip("How far in degrees can you move the camera down")] public float BottomClamp = -30.0f;
    //���b�N���̃J�����ʒu�̔������Ɏg�p
    [Tooltip("Useful for fine tuning camera position when locked")] public float CameraAngleOverride = 0.0f;
    //�S���̃J�����ʒu���Œ�
    [Tooltip("For locking the camera posotion on all axis")] public bool LockCameraPosition = false;

    //�V�l�}�V�[��
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;

    //�v���C���[
    private float speed;
    private float animationBlend;
    private float targetRotation = 0.0f;
    private float rotationVelocity;
    private float verticalVelocity;
    private float terminalVelocity = 53.0f;

    //�^�C���A�E�g�֘A
    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;
    private float throwTimeoutDelta;
    private float dodgeTimeoutDelta;
    private float catchTimeoutDelta;
    private float passTimeoutDelta;
    private float faintTimeoutDelta;

    //�A�j���[�V����ID
    private int animIDSpeed;
    private int animIDGrounded;
    private int animIDJump;
    private int animIDFreeFall;
    private int animIDMotionSpeed;
    private int animIDThrow;
    private int animIDDodge;
    private int animIDCatch;
    private int animIDPass;
    private int animIDFaint;

    //�{�[���𓊂�����
    private bool Throwed = false;

#if ENABLE_INPUT_SYSTEM
    private PlayerInput playerInput;
#endif
    //�e�R���|�[�l���g
    private Animator animator;
    private CharacterController controller;
    private InputAsist input;
    private GameObject mainCamera;
    private TrailRenderer tr;

    private const float threshold = 0.01f;

    //���݂̃A�j���[�V����
    private bool hasAnimator;


    /// <summary>
    public float hp = 100;
    [SerializeField] Text text1 = null;
    /// </summary>
    /// 

    // ���v���C���[��HP���X�g
    //List<float> hpList = new List<float>();

    //--------------------------------------------------------

    //[SerializeField] Player player = null;
    [SerializeField] Text text;
    //public ThirdPersonController thirdPersonController;


    private bool IsCurrentDeviceMouse
    {
        get 
        {
#if ENABLE_INPUT_SYSTEM
            return playerInput.currentControlScheme == "KeyboardMouse";
#else
            return false;
#endif
        }
    }

    private void Awake()
    {
        //photonDataSend = GameObject.Find("PhotonConnect").GetComponent<PhotonDataSend>();
        //photonDataSend.thirdPersonController = this.GetComponent<ThirdPersonController>();

        //���C���J�������擾
        if(mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    void Start()
    {
        //�V�l�}�V�[����Y����ݒ�
        cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

        //�e�R���|�[�l���g���擾
        hasAnimator = TryGetComponent(out animator);
        controller = GetComponent<CharacterController>();
        input = GetComponent<InputAsist>();
        tr = GetComponent<TrailRenderer>();
#if ENABLE_INPUT_SYSTEM
        playerInput = GetComponent<PlayerInput>();
#else
        Debug.Log("Package is missing dependencies.")
#endif
        //�A�j���[�V����ID��ݒ�
        AssignAnimationIDs();

        //�^�C���A�E�g�֘A�����Z�b�g����
        jumpTimeoutDelta = JumpTimeout;
        fallTimeoutDelta = FallTimeout;
        throwTimeoutDelta = ThrowTimeout;
        dodgeTimeoutDelta = DodgeTimeout;
        catchTimeoutDelta = CatchTimeout;
        passTimeoutDelta = PassTimeout;
        faintTimeoutDelta = FaintTimeout;

        //enemyList.Add(GameObject.FindWithTag("Enemy").gameObject);
        //enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //tr.emitting = false;

        text1 = GameObject.Find("Text1").GetComponent<Text>();
        text = GameObject.Find("Text").GetComponent<Text>();
    }

    // --------------------------------------------------------------------------------------------



    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            // Transform�̒l���X�g���[���ɏ�������ő��M����
            stream.SendNext(this.hp);

        }
        else
        {
            // ��M�����X�g���[����ǂݍ����Transform�̒l���X�V����
            //thirdPersonController.hp = (int)stream.ReceiveNext();
            //text.text = (string)stream.ReceiveNext();
            Debug.Log(stream.ReceiveNext());
        }

    }

    private void Update()
    {
        

        if(!photonView.IsMine)
        {
            return;
        }
        

        //�A�j���[�^�[���Z�b�g
        hasAnimator = TryGetComponent(out animator);

        //AddEnemy();

        JumpAndGravity();
        GroundedCheck();
        BallCheck();
        Attention();
        Move();
        Throw();
        Pass();
        Faint();
        Dodge();
        Catch();
        Talk();

        DebugHP();

        //EnemyReset();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void AssignAnimationIDs()
    {
        animIDSpeed = Animator.StringToHash("Speed");
        animIDGrounded = Animator.StringToHash("Grounded");
        animIDJump = Animator.StringToHash("Jump");
        animIDFreeFall = Animator.StringToHash("FreeFall");
        animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        animIDThrow = Animator.StringToHash("Throw");
        animIDDodge = Animator.StringToHash("Dodge");
        animIDCatch = Animator.StringToHash("Catch");
        animIDPass = Animator.StringToHash("Pass");
        animIDFaint = Animator.StringToHash("Faint");
    }

    /// <summary>
    /// �n�ʂƂ̔�������
    /// </summary>
    private void GroundedCheck()
    {
        //�L�����N�^�[�̃R���W�����̏����擾
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        //�Փ˔�����擾
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

        if (hasAnimator)
        {
            //�A�j���[�^�[�̃p�����[�^�ɏՓ˔��茋�ʂ�n��
            animator.SetBool(animIDGrounded, Grounded);
        }
    }

    /// <summary>
    /// �{�[���Ƃ̔���
    /// </summary>
    private void BallCheck()
    {
        Vector3 a = transform.position + playerDirection.normalized;

        //�L�����N�^�[�̃R���W�����̏����擾
        spherePosition = new Vector3(a.x, a.y - BallHitOffset, a.z);
        //�Փ˔�����擾
        HasBall = Physics.CheckSphere(spherePosition, BallCheckRadius, BallLayers, QueryTriggerInteraction.Ignore);

        if(HasBall && Grounded && !Dodging)
        {
            //photonView.RPC(nameof(PickUpBall), RpcTarget.All, this.gameObject);
            PickUpBall();
        }
    }
    
    /// <summary>
    /// �J�����̉�](�L�����N�^�[�̓����ɍ��킹�邽�߂�Update������ɌĂ�)
    /// </summary>
    private void CameraRotation()
    {
        // ���͂�����A�J�����ʒu���Œ肳��Ă��Ȃ��ꍇ
        if (input.look.sqrMagnitude >= threshold && !LockCameraPosition)
        {
            //�}�E�X���͎��ɂ͂�deltaTime���|���Ȃ�
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            //�J�����̉�]
            cinemachineTargetYaw += input.look.x * deltaTimeMultiplier;
            cinemachineTargetPitch += input.look.y * deltaTimeMultiplier;
        }

        // �l��360���ɐ��������悤�ɂ���
        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);

        // �V�l�}�V�[�����t�H���[����^�[�Q�b�g
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + CameraAngleOverride, cinemachineTargetYaw, 0.0f);
    }

    /// <summary>
    /// �ړ�
    ///   �{�W�����v
    ///   �{�_�b�V��
    ///   �{������H
    ///   �{���
    ///   �{�L���b�`�H
    /// </summary>
    private void Move()
    {
        //��𒆂͓����Ȃ�
        if (Dodging || Throwing) return;

        // �ړ����x��ݒ�
        float targetSpeed = SprintSpeed;

        // ���͂��Ȃ��ꍇ&��𒆂ł͂Ȃ��ꍇ
        if (input.move == Vector2.zero && !Dodging)
        {
            //���x��0�ɂ���
            targetSpeed = 0.0f;

            if(CurrentTarget != null)
            {
                //�^�[�Q�b�g�̕�������
                targetRotation = Mathf.Atan2(CurrentTarget.transform.position.x - transform.position.x, CurrentTarget.transform.position.z - transform.position.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;

                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, RotationSmoothTime);

                // �J�����̌����Ă�������ɉ�]
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
            
        }

        // ���݂̐��������̑��x�̎Q��
        float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = input.analogMovement ? input.move.magnitude : 1f;

        // �����E����
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

            //�����_�ȉ��O���܂�
            speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else
        {
            //���x�̈ێ�
            speed = targetSpeed;
        }

        //�A�j���[�V�����p�����[�^�̃u�����h����
        animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (animationBlend < 0.01f) animationBlend = 0f;

        // ���͕����𐳋K��
        Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

        // ���͂�����ꍇ�Ƀv���C���[����]������
        if (input.move != Vector2.zero)
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;

            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, RotationSmoothTime);

            // �J�����̌����Ă�������ɉ�]
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        playerDirection = targetDirection;

        // �v���C���[���ړ�������
        controller.Move(targetDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

        position = transform.position;

        if (hasAnimator)
        {
            //�A�j���[�^�[�̃p�����[�^�Ƀu�����h���, Input��������
            animator.SetFloat(animIDSpeed, animationBlend);
            animator.SetFloat(animIDMotionSpeed, inputMagnitude);
        }
    }

    /// <summary>
    /// �W�����v�Əd��
    /// �@�{�ړ�
    /// �@�{������H
    /// �@�{�L���b�`�H
    /// </summary>
    private void JumpAndGravity()
    {
        //�n�ʂƐڒn���Ă���ꍇ
        if (Grounded)
        {
            // fallTimeoutTimer�����Z�b�g����
            fallTimeoutDelta = FallTimeout;

            if (hasAnimator)
            {
                //�A�j���[�^�[�̃p�����[�^�ɃX�e�[�g��������
                animator.SetBool(animIDJump, false);
                animator.SetBool(animIDFreeFall, false);
            }

            // �ڒn���ɑ��x�������ɒቺ����̂�h��
            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = -2f;
            }

            // �W�����v����ꍇ
            if (input.jump && jumpTimeoutDelta <= 0.0f && !Throwing && !Fainting && !Dodging && !Catching)
            {
                //  H * -2 * G = �]�݂̍����ɓ��B���邽�߂ɕK�v�ȑ��x
                verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                if (hasAnimator)
                {
                    //�A�j���[�^�[�̃p�����[�^�ɃW�����v��������
                    animator.SetBool(animIDJump, true);
                }

                ///��
                hp -= 1f;
                ///

                Debug.Log(hp);
            }

            // �W�����v�^�C���A�E�g���c���Ă����猸�炷
            if (jumpTimeoutDelta >= 0.0f)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // �W�����v�^�C���A�E�g�����Z�b�g
            jumpTimeoutDelta = JumpTimeout;

            // �����^�C���A�E�g���c���Ă����猸�炷
            if (fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                if (hasAnimator)
                {
                    //�A�j���[�^�[�̃p�����[�^�ɗ�����������
                    animator.SetBool(animIDFreeFall, true);
                }
            }

            //�Ăщ�����悤�ɂ���
            input.jump = false;
        }

        // �d�͂𑫂�
        if (verticalVelocity < terminalVelocity)
        {
            verticalVelocity += Gravity * Time.deltaTime;
        }
    }

    /// <summary>
    /// �L�����N�^�[�̓����铮��
    /// �@�{�ړ�
    /// </summary>
    private void Throw()
    {
        //������L�[�������ꂽ��
        if (input.throwing && !Throwing && !Passing && !Fainting && isBallHaving && Grounded)
        {

            if (hasAnimator)
            {
                Throwing = true;
                //�A�j���[�^�[�̃p�����[�^�ɓ������������
                animator.SetBool(animIDThrow, Throwing);

                GameObject.FindWithTag("Ball1").GetComponent<Rigidbody>().isKinematic = false;
            }
        }

        //�����Ă���Œ�
        if (Throwing)
        {
            GameObject ball = GameObject.FindWithTag("Ball1");

            //�{�[�������������(�v����)
            if (throwTimeoutDelta <= 0.84f && !Throwed)
            {
                ball.gameObject.transform.parent = null;
                ball.GetComponent<Rigidbody>().isKinematic = false;

                ball.GetComponent<Ball>().Straight(transform.forward);
                isBallHaving = false;
                Throwed = true;
            }

            //������f���^�^�C�����c���Ă����猸�炷
            if (throwTimeoutDelta >= 0.0f)
            {
                //ball.GetComponent<Rigidbody>().useGravity = true;
                throwTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                if (hasAnimator)
                {
                    Throwing = false;
                    //�A�j���[�^�[�̃p�����[�^�ɓ������������
                    animator.SetBool(animIDThrow, Throwing);
                }
            }
        }
        else
        {
            //������f���^�^�C�������Z�b�g
            throwTimeoutDelta = ThrowTimeout;
            Throwed = false;
        }


        //�Ăщ�����悤�ɂ���
        input.throwing = false;
    }

    /// <summary>
    /// ������[�V���� 
    /// �@�{�ړ�
    /// </summary>
    private void Dodge()
    {
        //tr.emitting = Dodging;

        if(Grounded)
        {
            //������L�[�������ꂽ��
            if(input.dodge)
            {
                if(hasAnimator)
                {
                    Dodging = true;
                    //�A�j���[�^�̃p�����[�^�ɏ�������
                    animator.SetBool(animIDDodge, Dodging);
                }
            }

            if (Dodging)
            {
                //���
                if(0.8f >= dodgeTimeoutDelta && dodgeTimeoutDelta >= 0.1f)
                {
                    controller.Move(transform.forward * DodgeSpeed * Time.deltaTime);
                }

                //������Ԃ��c���Ă����猸�炷�i����𑱂���j
                if (dodgeTimeoutDelta >= 0.0f)
                {
                    dodgeTimeoutDelta -= Time.deltaTime;
                }
                //������Ԃ��c���Ă��Ȃ�������A�j���[�^�[���X�V����
                else
                {
                    if (hasAnimator)
                    {
                        Dodging = false;
                        //�A�j���[�^�̃p�����[�^�ɓ������������
                        animator.SetBool(animIDDodge, Dodging);
                    }
                }
            }
            else
            {
                //������f���^�^�C�������Z�b�g
                dodgeTimeoutDelta = DodgeTimeout;
            }
        }
    }

    /// <summary>
    /// �L���b�`
    /// �@�{�ړ��H
    /// �@�{�W�����v�H
    /// </summary>
    private void Catch()
    {
        ballCheckSpherePosition = transform.position + transform.forward.normalized;

        ballCheckSpherePosition.y = -3.5f;

        if (Grounded)
        {
            //�L���b�`�L�[�������ꂽ��
            if(input.catching)
            {
                if(hasAnimator)
                {
                    Catching = true;
                    //�A�j���[�^�[�̃p�����[�^�ɏ�������
                    animator.SetBool(animIDCatch, Catching);
                }
            }

            if (Catching)
            {
                //�L���b�`
                if(0.4f >= catchTimeoutDelta && catchTimeoutDelta >= 0.3f)
                {
                    bool hasCatchBall = Physics.CheckSphere(ballCheckSpherePosition, BallCheckRadius, BallLayers, QueryTriggerInteraction.Ignore);

                    if(hasCatchBall)
                    {
                        photonView.RPC(nameof(PickUpBall), RpcTarget.All, this.gameObject);
                    }
                }

                //���Ԃ��c���Ă����猸�炷(�L���b�`�𑱂���)
                if(catchTimeoutDelta >= 0.0f)
                {
                    catchTimeoutDelta -= Time.deltaTime;
                }
                //���Ԃ��c���Ă��Ȃ�������A�j���[�^�[���X�V����
                else
                {
                    if(hasAnimator)
                    {
                        Catching = false;
                        //�A�j���[�^�[�̃p�����[�^�ɃL���b�`�̏�������
                        animator.SetBool(animIDCatch, Catching);
                    }
                }
            }
            else
            {
                //�L���b�`�̃f���^�^�C�������Z�b�g
                catchTimeoutDelta = CatchTimeout;
            }
        }
    }

    /// <summary>
    /// �p�X
    /// </summary>
    private void Pass()
    {
        //�n�ʂƐڒn���Ă���
        if(Grounded)
        {
            //������L�[�������ꂽ��
            if (input.pass && !Throwing && !Passing && !Fainting)
            {
                if (hasAnimator)
                {
                    Passing = true;
                    //�A�j���[�^�[�̃p�����[�^�ɓ������������
                    animator.SetBool(animIDPass, Passing);

                    GameObject.FindWithTag("Ball1").GetComponent<Rigidbody>().isKinematic = false;
                }
            }

            //�����Ă���Œ�
            if (Passing)
            {
                GameObject ball = GameObject.FindWithTag("Ball1");

                //�{�[�������������(�v����)
                if (passTimeoutDelta <= 0.84f)
                {
                    ball.gameObject.transform.parent = null;

                    ball.GetComponent<Rigidbody>().AddForce(
                        new Vector3(spherePosition.x, spherePosition.y + 5.0f, spherePosition.z), ForceMode.Impulse);
                    //ball.GetComponent<Ball>().Throwing(transform.forward);
                    isBallHaving = false;
                }

                //������f���^�^�C�����c���Ă����猸�炷
                if (passTimeoutDelta >= 0.0f)
                {
                    //ball.GetComponent<Rigidbody>().useGravity = true;
                    passTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    if (hasAnimator)
                    {
                        Passing = false;
                        //�A�j���[�^�[�̃p�����[�^�ɓ������������
                        animator.SetBool(animIDPass, Passing);
                    }
                }
            }
            else
            {
                //������f���^�^�C�������Z�b�g
                passTimeoutDelta = PassTimeout;
            }
        }

        //�Ăщ�����悤�ɂ���
        input.pass = false;
    }

    /// <summary>
    /// �t�F�C���g
    /// </summary>
    private void Faint()
    {
        //�n�ʂƐڒn���Ă���
        if (Grounded)
        {
            //������L�[�������ꂽ��
            if (input.faint && !Throwing && !Passing && !Fainting)
            {
                if (hasAnimator)
                {
                    Fainting = true;
                    //�A�j���[�^�[�̃p�����[�^�ɓ������������
                    animator.SetBool(animIDFaint, Fainting);
                }
            }

            //�����Ă���Œ�
            if (Fainting)
            {

                //������f���^�^�C�����c���Ă����猸�炷
                if (faintTimeoutDelta >= 0.0f)
                {
                    faintTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    if (hasAnimator)
                    {
                        Fainting = false;
                        //�A�j���[�^�[�̃p�����[�^�ɓ������������
                        animator.SetBool(animIDFaint, Fainting);
                    }
                }
            }
            else
            {
                //������f���^�^�C�������Z�b�g
                faintTimeoutDelta = FaintTimeout;
            }
        }

        //�Ăщ�����悤�ɂ���
        input.faint = false;
    }

    /// <summary>
    /// �^�[�Q�b�g�ύX
    /// </summary>
    private void Attention()
    {
        //if(enemies != null)
        //{
        //    if (input.right)
        //    {
        //        CurrentTargets = enemies[0].gameObject;
        //    }
        //    else if (input.left)
        //    {
        //        CurrentTargets = enemies[1].gameObject;
        //    }
        //    else if (input.untarget && CurrentTargets != null)
        //    {
        //        CurrentTargets = null;
        //    }
        //}
        if (CurrentTarget != null)
        {
            if (input.untarget)
            {
                CurrentTarget = null;
            }
        }
        else
        {
            if(input.right)
            {
                //CurrentTarget = 
            }
            else if(input.left)
            {
                //CurrentTarget
            }
        }
    }

    //[PunRPC]
    /// <summary>
    /// �{�[�����E��
    /// </summary>
    private void PickUpBall()
    {
        //�{�[���̏������
        GameObject ball = GameObject.FindWithTag("Ball1");

        //�{�[����Transform�������
        Transform ballPosition = ball.GetComponent<Transform>();

        //�{�[���̕��������̐ݒ�
        ball.GetComponent<Rigidbody>().isKinematic = true;
        ball.GetComponent<Rigidbody>().useGravity = false;
        ball.GetComponent<Ball>().UseGravity = false;

        //�E����������Ĉʒu���擾
        Transform rightHand = GameObject.FindWithTag("RightHand").transform;

        //�{�[���̈ʒu���E��̈ʒu�ōX�V
        ballPosition.position = rightHand.position;

        //�{�[�����E��̎q�ɐݒ�
        ball.transform.parent = rightHand;

        //�{�[�������t���O��true
        isBallHaving = true;
    }

    private void Talk()
    {
        if(input.talk)
        {
            RpcSendMessage("����ɂ���");

            photonView.RPC(nameof(RpcSendMessage), RpcTarget.All, "���͂悤");
        }
    }

    [PunRPC]
    private void RpcSendMessage(string message)
    {
        Debug.Log(message);
    }

    /// <summary>
    /// �J�����̉�](360��, -360���𒴂�����0�ɖ߂�)
    /// </summary>
    /// <param name="lfAngle">���݂̃J�����p�x</param>
    /// <param name="lfMin">�c�̊p�x���E(MIN)</param>
    /// <param name="lfMax">�c�̊p�x���E(MAX)</param>
    /// <returns></returns>
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    /// <summary>
    /// �M�Y���ŃR���C�_�[��\������
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (Grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
            GroundedRadius);

        //a = a.normalized;

        Gizmos.DrawSphere(
          spherePosition,
           GroundedRadius);

        Gizmos.DrawSphere(
          ballCheckSpherePosition,
           GroundedRadius);
    }

    private void OnGUI()
    {
        GUILayout.Label(hp.ToString());
    }

    private void DebugHP()
    {
        // �X�^�~�i���Q�[�W�ɔ��f����
        text1.text = hp.ToString();
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(controller.center), FootstepAudioVolume);
            }
        }
    }


    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(controller.center), FootstepAudioVolume);
        }
    }

    public bool IsBallHaving
    {
        get
        {
            return isBallHaving;
        }
    }

    public Vector3 SpherePosition
    {
        get
        {
            return spherePosition;
        }
    }

    public Vector3 PlayerPosition
    {
        get
        {
            return position;
        }
    }

    public Vector3 CurrentTargetPosition
    {
        get
        {
            return CurrentTarget.transform.position;
        }
    }
}
