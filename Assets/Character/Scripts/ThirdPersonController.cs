using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]

public class ThirdPersonController : MonoBehaviour
{
    /*-----PlayerStatus-----*/
    [Header("Player")]
    [Tooltip("CharacterNumber")] [SerializeField] private int playerNumber;
    //�L�����N�^�[�̈ʒu
    [Tooltip("Character position")] private Vector3 position;
    //�L�����N�^�[��WalkSpeed
    [Tooltip("Move Speed of the character")] public float MoveSpeed = 2.0f;
    //�L�����N�^�[��SprintSpeed
    [Tooltip("Sprint Speed of the character")] public float SprintSpeed = 5.3f;
    //�L�����N�^�[�̃^�[�Q�b�g���̑��x
    [Tooltip("Move Speed of the Character's movement speed when targeting")] public float TargetingMoveSpeed = 2.0f;
    //�L�����N�^�[��DodgeSpeed
    [Tooltip("Dodge speed of the character")] public float DodgeSpeed = 5.3f;
    //�L�����N�^�[�̐U��������X���[�Y�ɂ��鎞��
    [Tooltip("The speed of the character's swing in the direction of movement")] [Range(0.0f, 0.3f)] public float RotationSmoothTime = 0.12f;
    //�L�����N�^�[�̉����x&�����x
    [Tooltip("Acceleration and Deceleration")] public float SpeedChangeRate = 10.0f;
    //�L�����N�^�[�̃I�[�f�B�I�N���b�v
    public AudioSource audioSource;
    public AudioClip LandingAudioClip;
    public AudioClip CatchAudioClip;
    public AudioClip ThrowAudioClip;

    public AudioClip[] FootstepAudioClips;

    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;
    [Range(0, 1)] public float CatchAudioVolume = 0.5f;
    [Range(0, 1)] public float ThrowAudioVolume = 0.5f;

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
    //�N���オ��܂ł̓���̃N�[���^�C��
    [Tooltip("Time required to wake up before begin able to wake up")] public float ResuscitableTimeout = 0.5f;
    //�Ăѓ�����悤�ɂȂ�܂ł̓���̃N�[���^�C��
    [Tooltip("Time required to move before begin able to move again")] public float ChargeTimeout = 0.3f;

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
    //�{�[����ێ�������GameObject
    [Tooltip("RightHand")] public GameObject rightHandGameObject;
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
    // �L�����N�^�[������ł邩
    [Tooltip("If the character is Dieing or not")] public bool Dieing = false;

    /*-----�v���C���[�̉�𓮍�-----*/
    [Header("Player Dodging")]
    //�L�����N�^�[�������Ă��邩
    [Tooltip("If the character is dodging or not")] public bool Dodging = false;
    //�����̃N�[���^�C��
    public float dodgingCooltime = 2.0f;
    public float dodgingCooltimeDelta = 2.0f;
    public bool isDodgeEnable = true;

    /*-----�L���b�`-----*/
    [Header("PlayerCatching")]
    //�L�����N�^�[���L���b�`���Ă��邩
    [Tooltip("If the character is catching or not")] public bool Catching = false;
    Vector3 ballCheckSpherePosition;

    /*-----�L�����N�^�[�̎u�]����-----*/
    // �L�����N�^�[������ł邩
    [Tooltip("If the character is Dieing or not")] public bool Dying = false;

    /*-----�L�����N�^�[�̑h��-----*/
    //���ȃq�[��
    public bool Charging = false;
    //�L�����N�^�[�̑h���͈�
    [Tooltip("Character resuscitation range")] private float ResuscitationRange = 1.0f;
    //�h���\�t���O
    public bool resuscitable = false;
    //�h�����Ă���
    public bool Reanimated = false;
    //�����Ȃ��悤�ɂ���
    public bool freeze = false;
    //�G�t�F�N�g
    [SerializeField]
    private ParticleSystem m_soseiParticle;
    ParticleSystem newEffect;

    /*-----�^�[�Q�b�g-----*/
    [Header("Current targets")] public GameObject CurrentTarget;

    /*-----�G���[�g-----*/
    private bool[] Emote = { false, false, false, false };
    private bool isEmote = false;

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
    //�J�����̐U������I���t���O
    private bool isFinishRotation = true;

    /*-----�G-----*/
    [Tooltip("EnemyObject")] private List<GameObject> enemyGameObject;
    int enemyNumber = 0;

    /*-----����1-----*/
    [Tooltip("AllyObject")] public GameObject allyGameObject;
    private ThirdPersonController allyTPC;

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
    private float resuscitableTimeoutDelta;
    private float chargeTimeoutDelta;

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
    private int animIDDie;
    private int animIDTarget;
    private int animIDFront;
    private int animIDSide;
    private int animIDResuscitation;
    private int animIDCharge;
    private int animIDEmote1;
    private int animIDEmote2;
    private int animIDEmote3;

    //�{�[���𓊂�����
    private bool Throwed = false;

    //�W�����v�{�^�����������u�Ԃ��璅�n����܂ő��̍s���𐧌�����t���O
    [SerializeField] private bool isJumpFlag = false;

    [SerializeField]
    private float hp;

    [SerializeField] private bool m_isOperation;

    /*-----�X�L��-----*/
    [SerializeField] private GameObject canvas;

    public CoolTime coolTime;

    [SerializeField] private Camera camera;
    [SerializeField] private GameObject virtualCamera;

    [SerializeField] private RectTransform rectTransform;

    [SerializeField] PlayerCounter playerCounter;

    /*-----�`�[��------*/
    [SerializeField]
    private LayerMask m_teamLayer;


#if ENABLE_INPUT_SYSTEM
    private PlayerInput playerInput;
#endif
    //�e�R���|�[�l���g
    private Animator animator;
    private CharacterController controller;
    private InputAsist input;
    private GameObject mainCamera;
    //private TrailRenderer tr;

    private const float threshold = 0.01f;

    //���݂̃A�j���[�V����
    private bool hasAnimator;

    /*--------------------------DUBUG-------------------------*/
    //�㉺�̑ł������f�o�b�O�ϐ�(0:�� 1:��)
    public int targetNumber = 0;
    //�ł������p�̃L�[�𒷉��������Ȃ��t���O
    private bool isHold = false;




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
        //���X�؂��ǉ�
        GameObject.Find("GameRule").GetComponent<GameRule>().AddCharacter(this.gameObject);

        //���C���J�������擾
        if (mainCamera == null)
        {
            //mainCamera = this.transform.root.gameObject.FindGameObjectWithTag("MainCamera");
            mainCamera = this.transform.root.Find("Camera").gameObject;

            controller = GetComponent<CharacterController>();

            playerCounter = GameObject.Find("PlayerManager").GetComponent<PlayerCounter>();
            playerCounter.PlayerNum++;

            this.gameObject.name = "FemaleDummy" + playerCounter.PlayerNum;
            GameObject.FindWithTag("PlayerCanvas").SetActive(false);

            if (playerCounter.PlayerNum == 1)
            {
                this.gameObject.layer = 13;
                virtualCamera.gameObject.layer = 13;

                camera.cullingMask &= ~(1 << 14 << 17 << 18);

                rectTransform.localPosition = new Vector2(-480f, 0f);

                playerNumber = 1;


            }
            else if (playerCounter.PlayerNum == 2)
            {
                this.gameObject.layer = 14;
                virtualCamera.gameObject.layer = 14;

                camera.cullingMask &= ~(1 << 13 << 17 << 18);

                rectTransform.localPosition = new Vector2(480f, 0f);

                GameObject p1 = GameObject.Find("FemaleDummy1");
                GameObject p2 = GameObject.Find("FemaleDummy2");

                //p1.GetComponent<ThirdPersonController>().SetEnemy = p2;
                //p2.GetComponent<ThirdPersonController>().SetEnemy = p1;

                /*-----Debug-----*/
                p1.GetComponent<ThirdPersonController>().SetAlly = p2;
                p2.GetComponent<ThirdPersonController>().SetAlly = p1;
                /*---------------*/

                playerNumber = 2;


            }

            else if (playerCounter.PlayerNum == 3)
            {
                this.gameObject.layer = 17;
                virtualCamera.gameObject.layer = 17;

                camera.cullingMask &= ~(1 << 13 << 14 << 18);

                rectTransform.localPosition = new Vector2(480f, 0f);

                GameObject p1 = GameObject.Find("FemaleDummy1");
                GameObject p2 = GameObject.Find("FemaleDummy2");

                //p1.GetComponent<ThirdPersonController>().SetEnemy = p2;
                //p2.GetComponent<ThirdPersonController>().SetEnemy = p1;

                /*-----Debug-----*/
                //p1.GetComponent<ThirdPersonController>().SetAlly = p2;
                //p2.GetComponent<ThirdPersonController>().SetAlly = p1;
                /*---------------*/

                playerNumber = 3;


            }

            else if (playerCounter.PlayerNum == 4)
            {
                this.gameObject.layer = 18;
                virtualCamera.gameObject.layer = 18;

                camera.cullingMask &= ~(1 << 13 << 14 << 17);

                rectTransform.localPosition = new Vector2(480f, 0f);

                GameObject p1 = GameObject.Find("FemaleDummy1");
                GameObject p2 = GameObject.Find("FemaleDummy2");

                //p1.GetComponent<ThirdPersonController>().SetEnemy = p2;
                //p2.GetComponent<ThirdPersonController>().SetEnemy = p1;

                /*-----Debug-----*/
                //p1.GetComponent<ThirdPersonController>().SetAlly = p2;
                //p2.GetComponent<ThirdPersonController>().SetAlly = p1;
                /*---------------*/

                playerNumber = 4;


            }
        }

        if (playerNumber == 1)
        {
            GameObject.FindWithTag("1PJoinUI").GetComponent<CharanterJoinUI>().JoinFlag = true;
        }
        else if (playerNumber == 2)
        {
            GameObject.FindWithTag("2PJoinUI").GetComponent<CharanterJoinUI>().JoinFlag = true;
        }
        else if (playerNumber == 3)
        {
            GameObject.FindWithTag("3PJoinUI").GetComponent<CharanterJoinUI>().JoinFlag = true;
        }
        else if (playerNumber == 4)
        {
            GameObject mr = GameObject.Find("Movement Restrictions");
            mr.SetActive(false);
            mr.SetActive(true);
            GameObject.FindWithTag("4PJoinUI").GetComponent<CharanterJoinUI>().JoinFlag = true;
        }
    }

    void Start()
    {
        //�V�l�}�V�[����Y����ݒ�
        cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

        //�e�R���|�[�l���g���擾
        hasAnimator = TryGetComponent(out animator);

        input = GetComponent<InputAsist>();

#if ENABLE_INPUT_SYSTEM
        playerInput = GetComponent<PlayerInput>();
#else
        Debug.Log("Package is missing dependencies.")
#endif
        //�A�j���[�V����ID��ݒ�
        AssignAnimationIDs();

        //�`�[���̃��C���[�ݒ�
        if (this.gameObject.layer == 13 || this.gameObject.layer == 17)         //Red�`�[��
            m_teamLayer = 19;
        else if (this.gameObject.layer == 14 || this.gameObject.layer == 18)    //Blue�`�[��
            m_teamLayer = 20;

        //�^�C���A�E�g�֘A�����Z�b�g����
        jumpTimeoutDelta = JumpTimeout;
        fallTimeoutDelta = FallTimeout;
        throwTimeoutDelta = ThrowTimeout;
        dodgeTimeoutDelta = DodgeTimeout;
        catchTimeoutDelta = CatchTimeout;
        passTimeoutDelta = PassTimeout;
        faintTimeoutDelta = FaintTimeout;
        resuscitableTimeoutDelta = ResuscitableTimeout;
        chargeTimeoutDelta = ChargeTimeout;
    }

    private void Update()
    {
        //�A�j���[�^�[���Z�b�g
        hasAnimator = TryGetComponent(out animator);

        //AddEnemy();

        JumpAndGravity();
        GroundedCheck();

        if (m_isOperation)
        {
            CreateCanvas();
            BallCheck();
            //Attention();
            NormalMove();
            TargetMove();
            Throw();
            Pass();
            Faint();
            Dodge();
            Catch();
            Die();
            ChangeMode();
            ChangeTarget();
            TargetArea();

            Revival();
            Resuscitation();
            PlayEmote();
            Charge();

            //DEBUG������
            ReturnBall();
            ResetPosition();
        }
    }

    private void LateUpdate()
    {
        CameraRotation();
        TargetCameraRotation();
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
        animIDDie = Animator.StringToHash("Die");
        animIDTarget = Animator.StringToHash("Target");
        animIDFront = Animator.StringToHash("Front");
        animIDSide = Animator.StringToHash("Side");
        animIDResuscitation = Animator.StringToHash("Resuscitation");
        animIDCharge = Animator.StringToHash("Charge");
        animIDEmote1 = Animator.StringToHash("Emote1");
        animIDEmote2 = Animator.StringToHash("Emote2");
        animIDEmote3 = Animator.StringToHash("Emote3");
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

    private void BallCheck()
    {
        Vector3 a = transform.position + playerDirection.normalized;

        //�L�����N�^�[�̃R���W�����̏����擾
        spherePosition = new Vector3(a.x, a.y - BallHitOffset, a.z);
        //�Փ˔�����擾
        HasBall = Physics.CheckSphere(spherePosition, BallCheckRadius, BallLayers, QueryTriggerInteraction.Ignore);

        if (HasBall && Grounded && !Dodging)
        {
            //photonView.RPC(nameof(PickUpBall), RpcTarget.All, this.gameObject);

            Ball ball = GameObject.FindWithTag("Ball1").GetComponent<Ball>();

            if (m_teamLayer == ball.TeamLayer || ball.TeamLayer == 0)
                PickUpBall();
        }
    }

    /// <summary>
    /// �J�����̉�](�L�����N�^�[�̓����ɍ��킹�邽�߂�Update������ɌĂ�)
    /// </summary>
    //private void CameraRotation()
    //{
    //    // ���͂�����A�J�����ʒu���Œ肳��Ă��Ȃ��ꍇ
    //    if (input.look.sqrMagnitude >= threshold && !LockCameraPosition)
    //    {
    //        //�}�E�X���͎��ɂ͂�deltaTime���|���Ȃ�
    //        float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

    //        //�J�����̉�]
    //        cinemachineTargetYaw += input.look.x * deltaTimeMultiplier;
    //        cinemachineTargetPitch += input.look.y * deltaTimeMultiplier;
    //    }

    //    // �l��360���ɐ��������悤�ɂ���
    //    cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
    //    cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);

    //    // �V�l�}�V�[�����t�H���[����^�[�Q�b�g
    //    CinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + CameraAngleOverride, cinemachineTargetYaw, 0.0f);
    //}

    private void CameraRotation()
    {
        if (CurrentTarget != null) return;

        // if there is an input and camera position is not fixed
        if (input.look.sqrMagnitude >= threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            cinemachineTargetYaw += input.look.x * deltaTimeMultiplier;
            cinemachineTargetPitch += input.look.y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + CameraAngleOverride,
            cinemachineTargetYaw, 0.0f);
    }

    private void TargetCameraRotation()
    {
        if (CurrentTarget == null) return;

        targetRotation = Mathf.Atan2(CurrentTarget.transform.position.x - transform.position.x, CurrentTarget.transform.position.z - transform.position.z) * Mathf.Rad2Deg;

        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, RotationSmoothTime);

        if (cinemachineTargetPitch < 0)
        {
            cinemachineTargetPitch += 2f;
            isFinishRotation = false;
        }
        else if (cinemachineTargetPitch > 0)
        {
            cinemachineTargetPitch -= 2f;
            isFinishRotation = false;
        }

        if (cinemachineTargetPitch <= 2f && cinemachineTargetPitch >= -2f)
        {
            cinemachineTargetPitch = 0f;
            isFinishRotation = true;
        }

        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(new Vector3(cinemachineTargetPitch, rotation, 0.0f));

        cinemachineTargetYaw = rotation;
    }


    /// <summary>
    /// �ʏ��Ԃ̈ړ�
    ///   �{�W�����v
    ///   �{�_�b�V��
    ///   �{������H
    ///   �{���
    ///   �{�L���b�`�H
    /// </summary>
    private void NormalMove()
    {
        if (CurrentTarget != null) return;

        //��𒆂͓����Ȃ�
        if (freeze) return;

        // �ړ����x��ݒ�
        float targetSpeed = SprintSpeed;

        // ���͂��Ȃ��ꍇ&��𒆂ł͂Ȃ��ꍇ
        if (input.move == Vector2.zero && !Dodging)
        {
            //���x��0�ɂ���
            targetSpeed = 0.0f;

            if (CurrentTarget != null)
            {
                //�^�[�Q�b�g�̕���
                targetRotation = Mathf.Atan2(CurrentTarget.transform.position.x - transform.position.x, CurrentTarget.transform.position.z - transform.position.z) * Mathf.Rad2Deg/* + mainCamera.transform.eulerAngles.y*/;

                //������:���ݒl�@������:�ڕW�l�@��O����:���ݑ��x���i�[����ϐ��@��l����:�ڕW�l�ɓ��B����܂ł̂����悻�̎���
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, RotationSmoothTime);

                // �J�����̌����Ă�������ɉ�]
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                //transform.Rotate(0f, targetRotation, 0f);
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
            ResetEmote();

            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;

            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        playerDirection = targetDirection;

        // �v���C���[���ړ�������
        controller.Move(targetDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

        //position = transform.position;

        if (hasAnimator)
        {
            //�A�j���[�^�[�̃p�����[�^�Ƀu�����h���, Input��������
            animator.SetFloat(animIDSpeed, animationBlend);
            animator.SetFloat(animIDMotionSpeed, inputMagnitude);

            if (!CurrentTarget)
                animator.SetBool(animIDTarget, false);
            else
                animator.SetBool(animIDTarget, true);
        }
    }

    /// <summary>
    /// �^�[�Q�b�g���̈ړ�
    /// </summary>
    private void TargetMove()
    {
        if (CurrentTarget == null) return;

        //��𒆂͓����Ȃ�
        if (freeze) return;

        // �ړ����x��ݒ�
        float targetSpeed = TargetingMoveSpeed;

        // ���͂��Ȃ��ꍇ&��𒆂ł͂Ȃ��ꍇ
        if (input.move == Vector2.zero && !Dodging)
        {
            //���x��0�ɂ���
            targetSpeed = 0.0f;
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



        //�^�[�Q�b�g�̕���
        targetRotation = Mathf.Atan2(CurrentTarget.transform.position.x - transform.position.x, CurrentTarget.transform.position.z - transform.position.z) * Mathf.Rad2Deg/* + mainCamera.transform.eulerAngles.y*/;
        //targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, RotationSmoothTime);
        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);



        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * inputDirection/* * Vector3.forward*/;

        playerDirection = targetDirection;

        // �v���C���[���ړ�������
        controller.Move(targetDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

        position = transform.position;



        if (hasAnimator)
        {
            //�A�j���[�^�[�̃p�����[�^�Ƀu�����h���, Input��������
            animator.SetFloat(animIDFront, -targetDirection.z, 0.1f, Time.deltaTime);
            animator.SetFloat(animIDSide, -targetDirection.x, 0.1f, Time.deltaTime);

            if (!CurrentTarget)
                animator.SetBool(animIDTarget, false);
            else
                animator.SetBool(animIDTarget, true);
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
            if (input.jump && jumpTimeoutDelta <= 0.0f && !freeze && !CurrentTarget)
            {
                //  H * -2 * G = �]�݂̍����ɓ��B���邽�߂ɕK�v�ȑ��x
                verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                if (hasAnimator)
                {
                    //�A�j���[�^�[�̃p�����[�^�ɃW�����v��������
                    animator.SetBool(animIDJump, true);
                    isJumpFlag = true;
                }
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
            isJumpFlag = false;
        }

        // �d�͂𑫂�
        if (verticalVelocity < terminalVelocity)
        {
            verticalVelocity += Gravity * Time.deltaTime;
        }

        if (freeze)
        {
            controller.Move(new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
        }
    }

    /// <summary>
    /// �L�����N�^�[�̓����铮��
    /// �@�{�ړ�
    /// </summary>
    private void Throw()
    {
        //������L�[�������ꂽ��
        if (input.ThrowingAndCatching && isBallHaving && Grounded && !isJumpFlag && !freeze && isFinishRotation)
        {
            if (hasAnimator)
            {
                //������
                Throwing = true;

                //�����Ȃ�����
                freeze = true;

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
            if (throwTimeoutDelta <= 0.95f && !Throwed)
            {
                ball.gameObject.transform.parent = null;
                ball.GetComponent<Rigidbody>().isKinematic = false;
                ball.GetComponent<Ball>().CheckLayer(m_teamLayer);
                ball.GetComponent<Ball>().CheckThrowObject(this.gameObject);
                ball.GetComponent<Ball>().CollisionNullification(); //�����蔻����ꎞ�I�ɏ���

                if (CurrentTarget == null)
                {
                    ball.GetComponent<Ball>().Straight(transform.forward);
                }
                else
                {
                    if(targetNumber == 0) //��
                        ball.GetComponent<Ball>().TargetStraight(CurrentTarget.transform.position);
                    else if(targetNumber == 1) //��
                        ball.GetComponent<Ball>().TargetDownward(CurrentTarget.transform.position);
                }

                isBallHaving = false;
                Throwed = true;
            }
            if (throwTimeoutDelta <= 0.9f)
            {
                ball.GetComponent<Ball>().CollisionValidation(); //�����蔻������ɖ߂�
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
                    //�����I��
                    Throwing = false;

                    //������悤�ɂ���
                    freeze = false;

                    //�A�j���[�^�[�̃p�����[�^�ɓ������������
                    animator.SetBool(animIDThrow, Throwing);

                    //�Ăщ�����悤�ɂ���
                    input.ThrowingAndCatching = false;
                }
            }
        }
        else
        {
            //������f���^�^�C�������Z�b�g
            throwTimeoutDelta = ThrowTimeout;
            Throwed = false;
        }
    }

    /// <summary>
    /// ������[�V���� 
    /// �@�{�ړ�
    /// </summary>
    private void Dodge()
    {
        if (dodgingCooltimeDelta <= 0f)
        {
            dodgingCooltimeDelta = dodgingCooltime;
            isDodgeEnable = true;
        }

        else if (!isDodgeEnable)
        {
            dodgingCooltimeDelta -= Time.deltaTime;
            return;
        }

        if (CurrentTarget != null) return;


        if (Grounded)
        {
            //������L�[�������ꂽ��
            if (input.dodge && !isBallHaving && Grounded && !isJumpFlag && !freeze)
            {
                if (hasAnimator)
                {
                    //�������
                    Dodging = true;

                    //�����Ȃ�����
                    freeze = true;

                    coolTime.CoolTimeFlag = Dodging;
                    //�A�j���[�^�̃p�����[�^�ɏ�������
                    animator.SetBool(animIDDodge, Dodging);
                }
            }
        }
        

        if (Dodging)
        {
            //���
            if (0.8f >= dodgeTimeoutDelta && dodgeTimeoutDelta >= 0.1f)
            {
                controller.Move(transform.forward * DodgeSpeed * Time.deltaTime);
            }

            //������Ԃ��c���Ă����猸�炷�i����𑱂���j
            if (dodgeTimeoutDelta >= 0.0f)
            {
                dodgeTimeoutDelta -= Time.deltaTime;

                hp -= 0.1f;
            }
            //������Ԃ��c���Ă��Ȃ�������A�j���[�^�[���X�V����
            else
            {
                if (hasAnimator)
                {
                    //����I��
                    Dodging = false;

                    //������悤�ɂ���
                    freeze = false;

                    //�A�j���[�^�̃p�����[�^�ɓ������������
                    animator.SetBool(animIDDodge, Dodging);
                }
            }
        }
        else
        {
            //������f���^�^�C�������Z�b�g
            dodgeTimeoutDelta = DodgeTimeout;

            //�Ăщ�����悤�ɂ���
            input.dodge = false;
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

        ballCheckSpherePosition.y = 1.5f;

        if (Grounded && !isJumpFlag)
        {
            //�L���b�`�L�[�������ꂽ��
            if (input.ThrowingAndCatching && !isBallHaving && !freeze)
            {
                if (hasAnimator)
                {
                    //�L���b�`����
                    Catching = true;

                    //�����Ȃ�����
                    freeze = true;

                    //�A�j���[�^�[�̃p�����[�^�ɏ�������
                    animator.SetBool(animIDCatch, Catching);
                }
            }

            if (Catching)
            {
                if (!isBallHaving)
                {
                    //�L���b�`
                    if (0.4f >= catchTimeoutDelta && catchTimeoutDelta >= 0.3f)
                    {
                        bool hasCatchBall = Physics.CheckSphere(ballCheckSpherePosition, BallCheckRadius, BallLayers, QueryTriggerInteraction.Ignore);

                        if (hasCatchBall)
                        {

                            PickUpBall();
                        }
                    }
                }


                //���Ԃ��c���Ă����猸�炷(�L���b�`�𑱂���)
                if (catchTimeoutDelta >= 0.0f)
                {
                    catchTimeoutDelta -= Time.deltaTime;
                }
                //���Ԃ��c���Ă��Ȃ�������A�j���[�^�[���X�V����
                else
                {
                    if (hasAnimator)
                    {
                        //�L���b�`�I��
                        Catching = false;

                        //������悤�ɂ���
                        freeze = false;

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
        if (Grounded)
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
            if (input.faint && !isJumpFlag && isBallHaving && !freeze)
            {
                if (hasAnimator)
                {
                    //�t�F�C���g����
                    Fainting = true;

                    //�����Ȃ�����
                    freeze = true;

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

                    hp -= 0.05f;
                }
                else
                {
                    if (hasAnimator)
                    {
                        //�t�F�C���g�I��
                        Fainting = false;

                        //������悤�ɂ���
                        freeze = false;

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

    private void DropBall()
    {
        GameObject ball = GameObject.FindWithTag("Ball1");

        ball.gameObject.transform.parent = null;
        ball.GetComponent<Rigidbody>().isKinematic = false;
        ball.GetComponent<Ball>().CheckLayer(this.gameObject.layer);
        ball.GetComponent<Ball>().CheckThrowObject(this.gameObject);
        //ball.GetComponent<Ball>().CollisionNullification(); //�����蔻����ꎞ�I�ɏ���
        ball.GetComponent<Ball>().UseGravity = true;
    }

    private void TargetArea()
    {
        //�^�[�Q�b�g���Ă��Ȃ������烊�^�[��
        if (CurrentTarget == null) return;

        //���͂���Ă��Ȃ�������isHold��false
        if (!input.lower && !input.upper) isHold = false;
        if (isHold) return;

        switch (targetNumber)
        {
            case 0:  //��

                //�����̏�����

                //����_��
                if (input.lower)
                {
                    targetNumber = 1;
                    isHold = true;
                    //input.lower = false;
                }
                break;
            case 1:  //��

                //�����̏���

                //�̂�_��
                if (input.upper)
                {
                    targetNumber = 0;
                    isHold = true;
                    //input.upper = false;
                }
                break;
        }
    }


    /// <summary>
    /// ����(HP��0�ȉ��ɂȂ����玀�S)
    /// </summary>
    public void Die()
    {
        //Hp���c���Ă����烊�^�[��
        if (hp > 0) return;

        if (hasAnimator)
        {
            CurrentTarget = null;

            //����
            Dying = true;

            hp = 0;

            //�����Ȃ�����
            freeze = true;

            //�A�j���[�^�[�̃p�����[�^�Ɏ��ʏ�������
            animator.SetBool(animIDDie, Dying);

            CinemachineCameraTarget.transform.localPosition = new Vector3(0.0f,0.2f,0.0f);

            if (isBallHaving)
                DropBall();
        }
    }

    /// <summary>
    /// ���ȃq�[��
    /// </summary>
    private void Charge()
    {
        if (Grounded && !isJumpFlag && !isBallHaving && !Reanimated)
        {
            //�q�[��
            if (input.resuscitation && hp < 100)
            {
                if (hasAnimator)
                {
                    //�q�[��
                    Charging = true;

                    //�����Ȃ��悤�ɂ���
                    freeze = true;

                    //�̗͂����X�ɉ񕜂���
                    hp += 0.5f;

                    //�A�j���[�^�[�̃p�����[�^�ɐ����Ԃ��������
                    animator.SetBool(animIDCharge, Charging);

                    if (newEffect == null)
                    {
                        //�p�[�e�B�N��
                        newEffect = Instantiate(m_soseiParticle);
                        newEffect.transform.position = transform.position;
                        newEffect.Play();
                    }
                }
            }
            else
            {
                //�h�����I������ゾ������
                if (Charging)
                {
                    //�A�j���[�^�[�̃p�����[�^�ɑh����������
                    animator.SetBool(animIDCharge, false);

                    if (newEffect != null)
                    {
                        Destroy(newEffect.gameObject);
                        newEffect.Stop();
                    }

                    //���Ԃ��c���Ă����猸�炷
                    if (chargeTimeoutDelta >= 0.0f)
                    {
                        chargeTimeoutDelta -= Time.deltaTime;
                    }
                    else
                    {
                        if (hasAnimator)
                        {
                            //�h�����I������
                            Charging = false;

                            //������悤�ɂ���
                            freeze = false;

                            //������f���^�^�C�������Z�b�g
                            chargeTimeoutDelta = ChargeTimeout;
                        }
                    }
                }
            }
        }
    }


    /// <summary>
    /// ����(�����I��HP���񕜂���&HP��MAX�ɂȂ����畜��)
    /// </summary>
    public void Revival()
    {
        //����ł��Ȃ������烊�^�[��
        if (!Dying) return;

        //HP���S�񕜂����畜��
        if (hp >= 100)
        {
            if (hasAnimator)
            {
                //����
                Dying = false;

                //������悤�ɂ���
                freeze = false;

                hp = 100;

                //�A�j���[�^�[�̃p�����[�^�ɐ����Ԃ��������
                animator.SetBool(animIDDie, Dying);

                CinemachineCameraTarget.transform.localPosition = new Vector3(0.0f, 1.0f, 0.0f);
            }
            return;
        }

        //�̗͂����X�ɉ񕜂���
        hp += 0.1f;
    }

    /// <summary>
    /// �h��(����ł��閡���̋߂��őh���{�^���������B������HP�𑁂��񕜂���B)
    /// </summary>
    public void Resuscitation()
    {
        //�h���{�^������������
        if (input.resuscitation && !isJumpFlag && !isBallHaving && allyGameObject.GetComponent<ThirdPersonController>().DIE)
        {
            //true�Ȃ�
            if (resuscitable = CheckResuscitable())
            {
                //�h������
                Reanimated = true;

                //�����Ȃ��悤�ɂ���
                freeze = true;

                if (newEffect == null)
                {
                    //�p�[�e�B�N��
                    newEffect = Instantiate(m_soseiParticle);
                    newEffect.transform.position = transform.position;
                    newEffect.Play();
                }

                //�A�j���[�^�[�̃p�����[�^�ɑh����������
                animator.SetBool(animIDResuscitation, Reanimated);

                //HP�������񕜂���
                allyGameObject.GetComponent<ThirdPersonController>().HP += 0.5f;

                //���g��HP���������񕜂���
                hp += 0.1f;
            }
        }
        else
        {
            //�h�����I������ゾ������
            if (Reanimated)
            {
                //�A�j���[�^�[�̃p�����[�^�ɑh����������
                animator.SetBool(animIDResuscitation, false);

                if (newEffect != null)
                {
                    Destroy(newEffect.gameObject);
                    newEffect.Stop();
                }

                //���Ԃ��c���Ă����猸�炷
                if (resuscitableTimeoutDelta >= 0.0f)
                {
                    resuscitableTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    if (hasAnimator)
                    {
                        //�h�����I������
                        Reanimated = false;

                        //������悤�ɂ���
                        freeze = false;

                        //������f���^�^�C�������Z�b�g
                        resuscitableTimeoutDelta = ResuscitableTimeout;
                    }
                }
            }
        }
    }

    /// <summary>
    /// �h���ł��邩�̃`�F�b�N1
    /// </summary>
    /// <returns>�h���\:true  �h���s��:false</returns>
    bool CheckResuscitable()
    {
        // ���S�Ԃ̋����̕������v�Z
        Vector3 d = transform.position - allyGameObject.transform.position;

        float dist2 = Vector3.Dot(d, d);

        // �������������������������a�̍��v�����������ꍇ�ɂ͏Փ˂��Ă���
        float radiusSum = ResuscitationRange + ResuscitationRange;

        return dist2 <= radiusSum * radiusSum;
    }

    /// <summary>
    /// ���[�h�؂�ւ�
    /// </summary>    
    private void ChangeMode()
    {
        if (input.untarget && Grounded && !freeze)
        {
            if (CurrentTarget != null)
            {
                //�G���^�[�Q�b�g����
                CurrentTarget = null;
            }
            else
            {
                //�G��ǉ�
                CurrentTarget = enemyGameObject[enemyNumber];
            }
        }
        input.untarget = false;
    }

    /// <summary>
    /// �^�[�Q�b�g�؂�ւ�
    /// </summary>
    private void ChangeTarget()
    {
        if (CurrentTarget)
        {
            if (input.left)
            {
                enemyNumber = 0;
                CurrentTarget = enemyGameObject[enemyNumber];
            }
            if (input.right)
            {
                enemyNumber = 1;
                CurrentTarget = enemyGameObject[enemyNumber];
            }
        }
    }

    private void PlayEmote()
    {
        if (isJumpFlag || freeze || isBallHaving) return;

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle Walk Run Blend") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Dance1") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Dance2") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Dance3")) return;

        if (input.emote1)
        {
            ResetEmote();

            Emote[0] = true;
            animator.SetBool(animIDEmote1, Emote[0]);

            isEmote = true;
        }
        else if (input.emote2)
        {
            ResetEmote();

            Emote[1] = true;
            animator.SetBool(animIDEmote2, Emote[1]);

            isEmote = true;
        }
        else if (input.emote3)
        {
            ResetEmote();

            Emote[2] = true;
            animator.SetBool(animIDEmote3, Emote[2]);

            isEmote = true;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dance1") ||
           animator.GetCurrentAnimatorStateInfo(0).IsName("Dance2") ||
           animator.GetCurrentAnimatorStateInfo(0).IsName("Dance3") ||
           animator.GetCurrentAnimatorStateInfo(0).IsName("Dance4")) hp -= 0.03f;
    }

    private void ResetEmote()
    {
        if (!isEmote) return;

        for (int i = 0; i < 3; i++)
        {
            Emote[i] = false;
            if (i == 0)
                animator.SetBool(animIDEmote1, Emote[i]);

            if (i == 1)
                animator.SetBool(animIDEmote2, Emote[i]);

            if (i == 2)
                animator.SetBool(animIDEmote3, Emote[i]);

            isEmote = false;
        }
    }



    /// <summary>
    /// �{�[�����E��
    /// </summary>
    private void PickUpBall()
    {
        GameObject ball = GameObject.FindWithTag("Ball1");
        Transform ballPosition = ball.GetComponent<Transform>();
        ball.GetComponent<Rigidbody>().isKinematic = true;
        ball.GetComponent<Rigidbody>().useGravity = false;
        ball.GetComponent<Ball>().UseGravity = false;
        ball.GetComponent<Ball>().HitValidity = true;
        Transform rightHand = rightHandGameObject.transform;//GameObject.FindWithTag("RightHand").transform;
        ballPosition.position = rightHand.position;
        ball.transform.parent = rightHand;
        isBallHaving = true;
        audioSource.PlayOneShot(CatchAudioClip, CatchAudioVolume);

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

    private void ReturnBall()
    {
        if (input.returned)
        {
            GameObject _ball = GameObject.FindWithTag("Ball1");
            _ball.transform.position = new Vector3(0f, 3.83f, 0f);
            _ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    private void ResetPosition()
    {
        if (this.transform.position.y < -15f)
        {
            verticalVelocity = 0f;
            controller.Move(new Vector3(0f, 55f, 0f));
        }
    }

    private void CreateCanvas()
    {
        //GameObject canvas = this.transform.root.f;
        canvas.gameObject.SetActive(true);
    }


    /// <summary>
    /// �������Ă���G��T��
    /// </summary>
    //private void AddEnemy()
    //{
    //    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

    //    foreach(GameObject enemy in enemies)
    //    {
    //        enemyList.Add(enemy);
    //    }
    //}

    /// <summary>
    /// �������Ă���G���J������
    /// </summary>
    //private void EnemyReset()
    //{
    //    enemyList.Clear();
    //}

    public bool GetDodging
    {
        get { return Dodging; }
    }

    public bool GetJumping
    {
        get { return Grounded; }
    }

    public List<GameObject> SetEnemy
    {
        set { enemyGameObject = value; }
    }

    public GameObject SetAlly
    {
        set { allyGameObject = value; }
    }


    public float HP
    {
        get { return hp; }
        set { hp = value; }
    }

    public bool DIE
    {
        get { return Dying; }
    }

    public bool IsOperation
    {
        get { return m_isOperation; }
        set { m_isOperation = value; }
    }

    public float CinemachineTargetYaw
    {
        get { return cinemachineTargetYaw; }
        set { cinemachineTargetYaw = value; }
    }

    public LayerMask TeamLayer
    {
        get { return m_teamLayer; }
        set { m_teamLayer = value; }
    }

}
