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
    //キャラクターの位置
    [Tooltip("Character position")] private Vector3 position;
    //キャラクターのWalkSpeed
    [Tooltip("Move Speed of the character")] public float MoveSpeed = 2.0f;
    //キャラクターのSprintSpeed
    [Tooltip("Sprint Speed of the character")] public float SprintSpeed = 5.3f;
    //キャラクターのターゲット時の速度
    [Tooltip("Move Speed of the Character's movement speed when targeting")] public float TargetingMoveSpeed = 2.0f;
    //キャラクターのDodgeSpeed
    [Tooltip("Dodge speed of the character")] public float DodgeSpeed = 5.3f;
    //キャラクターの振り向きをスムーズにする時間
    [Tooltip("The speed of the character's swing in the direction of movement")] [Range(0.0f, 0.3f)] public float RotationSmoothTime = 0.12f;
    //キャラクターの加速度&減速度
    [Tooltip("Acceleration and Deceleration")] public float SpeedChangeRate = 10.0f;
    //キャラクターのオーディオクリップ
    public AudioSource audioSource;
    public AudioClip LandingAudioClip;
    public AudioClip CatchAudioClip;
    public AudioClip ThrowAudioClip;

    public AudioClip[] FootstepAudioClips;

    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;
    [Range(0, 1)] public float CatchAudioVolume = 0.5f;
    [Range(0, 1)] public float ThrowAudioVolume = 0.5f;

    //キャラクターのジャンプ力
    [Space(10)]
    [Tooltip("Jump height of the character")] public float JumpHeight = 1.2f;
    //キャラクターにかかる重力
    [Tooltip("Gravity Value of the uses character")] public float Gravity = -15.0f;
    //再びジャンプできるようになるまでのクールタイム
    [Space(10)]
    [Tooltip("Time required to pass before begin able to jump again")] public float JumpTimeout = 0.5f;
    //転倒状態になるまでの時間(階段を走って降りるときに使用)
    [Tooltip("Time required to pass before entering the fall state")] public float FallTimeout = 0.15f;
    //再び投げれるようになるまでのクールタイム
    [Tooltip("Time required to pass before begin able to throw again")] public float ThrowTimeout = 0.5f;
    //再び避けれるようになるまでのクールタイム
    [Tooltip("Time required to pass before begin able to dodge again")] public float DodgeTimeout = 0.3f;
    //再びキャッチ動作ができるようになるまでのクールタイム
    [Tooltip("Time required to pass before begin able to catch again")] public float CatchTimeout = 0.6f;
    //再びパス動作ができるようになるまでのクールタイム
    [Tooltip("Time required to pass before begin able to pass again")] public float PassTimeout = 0.5f;
    //再びフェイント動作ができるようになるまでのクールタイム
    [Tooltip("Time required to pass before begin able to faint again")] public float FaintTimeout = 0.5f;
    //起き上がるまでの動作のクールタイム
    [Tooltip("Time required to wake up before begin able to wake up")] public float ResuscitableTimeout = 0.5f;
    //再び動けるようになるまでの動作のクールタイム
    [Tooltip("Time required to move before begin able to move again")] public float ChargeTimeout = 0.3f;

    /*-----プレイヤーと地面-----*/
    [Header("Player Grounded")]
    //キャラクターが地面と接地しているか
    [Tooltip("If the character is grounded or not")] public bool Grounded = true;
    //不整地で使用
    [Tooltip("Useful for rough ground")] public float GroundedOffset = -0.14f;
    //接地チェックの半径
    [Tooltip("The radius of the grounded check")] public float GroundedRadius = 0.28f;
    //どのレイヤーと当たり判定を取るか
    [Tooltip("What layers the character uses as ground")] public LayerMask GroundLayers;

    /*-----プレイヤーとボール-----*/
    [Header("Player HittingBall")]
    //キャラクターとボールが接しているか
    [Tooltip("If the character is hit or not to ball")] public bool HasBall = false;
    //不整地で使用
    [Tooltip("Useful for rough balll")] public float BallHitOffset = -0.05f;
    //ボールチェックの半径
    [Tooltip("The radius of the ball check")] public float BallCheckRadius = 0.5f;
    //どのタグのオブジェクトと当たり判定を取るか
    [Tooltip("What Tags the character use as ball")] public LayerMask BallLayers;
    //ボールを保持する手のGameObject
    [Tooltip("RightHand")] public GameObject rightHandGameObject;
    //ボールを判定するコライダーの位置
    [Tooltip("The position of the ball check")] public Vector3 playerDirection = Vector3.zero;
    //ボールのレイヤー番号
    [Tooltip("Layer number of the ball")] public int BallLayerNumber = 11;
    //投げる方向
    [Tooltip("Direction of the player")] private Vector3 spherePosition;
    //キャラクターがボールを所持しているか
    [Tooltip("Does the character have the character")] private bool isBallHaving = false;

    /*-----プレイヤーの投げる動作-----*/
    [Header("Player Throwing")]
    //キャラクターが投げているか
    [Tooltip("If the character is throwing or not")] public bool Throwing = false;
    //キャラクターがパスしているか
    [Tooltip("If the character is passing or not")] public bool Passing = false;
    //キャラクターがフェイントをしているか
    [Tooltip("If the character is fianting or not")] public bool Fainting = false;
    // キャラクターが死んでるか
    [Tooltip("If the character is Dieing or not")] public bool Dieing = false;

    /*-----プレイヤーの回避動作-----*/
    [Header("Player Dodging")]
    //キャラクターが避けているか
    [Tooltip("If the character is dodging or not")] public bool Dodging = false;
    //避けのクールタイム
    public float dodgingCooltime = 2.0f;
    public float dodgingCooltimeDelta = 2.0f;
    public bool isDodgeEnable = true;

    /*-----キャッチ-----*/
    [Header("PlayerCatching")]
    //キャラクターがキャッチしているか
    [Tooltip("If the character is catching or not")] public bool Catching = false;
    Vector3 ballCheckSpherePosition;

    /*-----キャラクターの志望動作-----*/
    // キャラクターが死んでるか
    [Tooltip("If the character is Dieing or not")] public bool Dying = false;

    /*-----キャラクターの蘇生-----*/
    //自己ヒール
    public bool Charging = false;
    //キャラクターの蘇生範囲
    [Tooltip("Character resuscitation range")] private float ResuscitationRange = 1.0f;
    //蘇生可能フラグ
    public bool resuscitable = false;
    //蘇生している
    public bool Reanimated = false;
    //動けないようにする
    public bool freeze = false;
    //エフェクト
    [SerializeField]
    private ParticleSystem m_soseiParticle;
    ParticleSystem newEffect;

    /*-----ターゲット-----*/
    [Header("Current targets")] public GameObject CurrentTarget;

    /*-----エモート-----*/
    private bool[] Emote = { false, false, false, false };
    private bool isEmote = false;

    /*-----シネマシーン-----*/
    [Header("Cinemachine")]
    //シネマシーン
    [Tooltip("the follow target set in the Cinemachine Virtual Camera that the camera will follow")] public GameObject CinemachineCameraTarget;
    //カメラが何度まで上を向けるか
    [Tooltip("How far in degrees can you move the camera up")] public float TopClamp = 70.0f;
    //カメラが何度まで下を向けるか
    [Tooltip("How far in degrees can you move the camera down")] public float BottomClamp = -30.0f;
    //ロック時のカメラ位置の微調整に使用
    [Tooltip("Useful for fine tuning camera position when locked")] public float CameraAngleOverride = 0.0f;
    //全軸のカメラ位置を固定
    [Tooltip("For locking the camera posotion on all axis")] public bool LockCameraPosition = false;
    //カメラの振り向き終了フラグ
    private bool isFinishRotation = true;

    /*-----敵-----*/
    [Tooltip("EnemyObject")] private List<GameObject> enemyGameObject;
    int enemyNumber = 0;

    /*-----味方1-----*/
    [Tooltip("AllyObject")] public GameObject allyGameObject;
    private ThirdPersonController allyTPC;

    //シネマシーン
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;

    //プレイヤー
    private float speed;
    private float animationBlend;
    private float targetRotation = 0.0f;
    private float rotationVelocity;
    private float verticalVelocity;
    private float terminalVelocity = 53.0f;

    //タイムアウト関連
    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;
    private float throwTimeoutDelta;
    private float dodgeTimeoutDelta;
    private float catchTimeoutDelta;
    private float passTimeoutDelta;
    private float faintTimeoutDelta;
    private float resuscitableTimeoutDelta;
    private float chargeTimeoutDelta;

    //アニメーションID
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

    //ボールを投げたか
    private bool Throwed = false;

    //ジャンプボタンを押した瞬間から着地するまで他の行動を制限するフラグ
    [SerializeField] private bool isJumpFlag = false;

    [SerializeField]
    private float hp;

    [SerializeField] private bool m_isOperation;

    /*-----スキル-----*/
    [SerializeField] private GameObject canvas;

    public CoolTime coolTime;

    [SerializeField] private Camera camera;
    [SerializeField] private GameObject virtualCamera;

    [SerializeField] private RectTransform rectTransform;

    [SerializeField] PlayerCounter playerCounter;

    /*-----チーム------*/
    [SerializeField]
    private LayerMask m_teamLayer;


#if ENABLE_INPUT_SYSTEM
    private PlayerInput playerInput;
#endif
    //各コンポーネント
    private Animator animator;
    private CharacterController controller;
    private InputAsist input;
    private GameObject mainCamera;
    //private TrailRenderer tr;

    private const float threshold = 0.01f;

    //現在のアニメーション
    private bool hasAnimator;

    /*--------------------------DUBUG-------------------------*/
    //上下の打ち分けデバッグ変数(0:体 1:足)
    public int targetNumber = 0;
    //打ち分け用のキーを長押しさせないフラグ
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
        //佐々木が追加
        GameObject.Find("GameRule").GetComponent<GameRule>().AddCharacter(this.gameObject);

        //メインカメラを取得
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
        //シネマシーンのY軸を設定
        cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

        //各コンポーネントを取得
        hasAnimator = TryGetComponent(out animator);

        input = GetComponent<InputAsist>();

#if ENABLE_INPUT_SYSTEM
        playerInput = GetComponent<PlayerInput>();
#else
        Debug.Log("Package is missing dependencies.")
#endif
        //アニメーションIDを設定
        AssignAnimationIDs();

        //チームのレイヤー設定
        if (this.gameObject.layer == 13 || this.gameObject.layer == 17)         //Redチーム
            m_teamLayer = 19;
        else if (this.gameObject.layer == 14 || this.gameObject.layer == 18)    //Blueチーム
            m_teamLayer = 20;

        //タイムアウト関連をリセットする
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
        //アニメーターをセット
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

            //DEBUG↓↓↓
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
    /// 地面との判定を取る
    /// </summary>
    private void GroundedCheck()
    {
        //キャラクターのコリジョンの情報を取得
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        //衝突判定を取得
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

        if (hasAnimator)
        {
            //アニメーターのパラメータに衝突判定結果を渡す
            animator.SetBool(animIDGrounded, Grounded);
        }
    }

    private void BallCheck()
    {
        Vector3 a = transform.position + playerDirection.normalized;

        //キャラクターのコリジョンの情報を取得
        spherePosition = new Vector3(a.x, a.y - BallHitOffset, a.z);
        //衝突判定を取得
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
    /// カメラの回転(キャラクターの動きに合わせるためにUpdateよりも後に呼ぶ)
    /// </summary>
    //private void CameraRotation()
    //{
    //    // 入力があり、カメラ位置が固定されていない場合
    //    if (input.look.sqrMagnitude >= threshold && !LockCameraPosition)
    //    {
    //        //マウス入力時にはにdeltaTimeを掛けない
    //        float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

    //        //カメラの回転
    //        cinemachineTargetYaw += input.look.x * deltaTimeMultiplier;
    //        cinemachineTargetPitch += input.look.y * deltaTimeMultiplier;
    //    }

    //    // 値が360°に制限されるようにする
    //    cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
    //    cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);

    //    // シネマシーンがフォローするターゲット
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
    /// 通常状態の移動
    ///   ＋ジャンプ
    ///   ＋ダッシュ
    ///   ＋投げる？
    ///   ＋回避
    ///   ＋キャッチ？
    /// </summary>
    private void NormalMove()
    {
        if (CurrentTarget != null) return;

        //回避中は動けない
        if (freeze) return;

        // 移動速度を設定
        float targetSpeed = SprintSpeed;

        // 入力がない場合&回避中ではない場合
        if (input.move == Vector2.zero && !Dodging)
        {
            //速度を0にする
            targetSpeed = 0.0f;

            if (CurrentTarget != null)
            {
                //ターゲットの方向
                targetRotation = Mathf.Atan2(CurrentTarget.transform.position.x - transform.position.x, CurrentTarget.transform.position.z - transform.position.z) * Mathf.Rad2Deg/* + mainCamera.transform.eulerAngles.y*/;

                //第一引数:現在値　第二引数:目標値　第三引数:現在速度を格納する変数　第四引数:目標値に到達するまでのおおよその時間
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, RotationSmoothTime);

                // カメラの向いている方向に回転
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                //transform.Rotate(0f, targetRotation, 0f);
            }

        }

        // 現在の水平方向の速度の参照
        float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = input.analogMovement ? input.move.magnitude : 1f;

        // 加速・減速
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

            //小数点以下三桁まで
            speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else
        {
            //速度の維持
            speed = targetSpeed;
        }

        //アニメーションパラメータのブレンド加減
        animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (animationBlend < 0.01f) animationBlend = 0f;

        // 入力方向を正規化
        Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

        // 入力がある場合にプレイヤーを回転させる
        if (input.move != Vector2.zero)
        {
            ResetEmote();

            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;

            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        playerDirection = targetDirection;

        // プレイヤーを移動させる
        controller.Move(targetDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

        //position = transform.position;

        if (hasAnimator)
        {
            //アニメーターのパラメータにブレンド情報, Input情報を入れる
            animator.SetFloat(animIDSpeed, animationBlend);
            animator.SetFloat(animIDMotionSpeed, inputMagnitude);

            if (!CurrentTarget)
                animator.SetBool(animIDTarget, false);
            else
                animator.SetBool(animIDTarget, true);
        }
    }

    /// <summary>
    /// ターゲット時の移動
    /// </summary>
    private void TargetMove()
    {
        if (CurrentTarget == null) return;

        //回避中は動けない
        if (freeze) return;

        // 移動速度を設定
        float targetSpeed = TargetingMoveSpeed;

        // 入力がない場合&回避中ではない場合
        if (input.move == Vector2.zero && !Dodging)
        {
            //速度を0にする
            targetSpeed = 0.0f;
        }

        // 現在の水平方向の速度の参照
        float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = input.analogMovement ? input.move.magnitude : 1f;

        // 加速・減速
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

            //小数点以下三桁まで
            speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else
        {
            //速度の維持
            speed = targetSpeed;
        }

        //アニメーションパラメータのブレンド加減
        animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (animationBlend < 0.01f) animationBlend = 0f;

        // 入力方向を正規化
        Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;



        //ターゲットの方向
        targetRotation = Mathf.Atan2(CurrentTarget.transform.position.x - transform.position.x, CurrentTarget.transform.position.z - transform.position.z) * Mathf.Rad2Deg/* + mainCamera.transform.eulerAngles.y*/;
        //targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, RotationSmoothTime);
        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);



        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * inputDirection/* * Vector3.forward*/;

        playerDirection = targetDirection;

        // プレイヤーを移動させる
        controller.Move(targetDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

        position = transform.position;



        if (hasAnimator)
        {
            //アニメーターのパラメータにブレンド情報, Input情報を入れる
            animator.SetFloat(animIDFront, -targetDirection.z, 0.1f, Time.deltaTime);
            animator.SetFloat(animIDSide, -targetDirection.x, 0.1f, Time.deltaTime);

            if (!CurrentTarget)
                animator.SetBool(animIDTarget, false);
            else
                animator.SetBool(animIDTarget, true);
        }
    }

    /// <summary>
    /// ジャンプと重力
    /// 　＋移動
    /// 　＋投げる？
    /// 　＋キャッチ？
    /// </summary>
    private void JumpAndGravity()
    {
        //地面と接地している場合
        if (Grounded)
        {
            // fallTimeoutTimerをリセットする
            fallTimeoutDelta = FallTimeout;

            if (hasAnimator)
            {
                //アニメーターのパラメータにステート情報を入れる
                animator.SetBool(animIDJump, false);
                animator.SetBool(animIDFreeFall, false);
            }

            // 接地時に速度が無限に低下するのを防ぐ
            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = -2f;
            }

            // ジャンプする場合
            if (input.jump && jumpTimeoutDelta <= 0.0f && !freeze && !CurrentTarget)
            {
                //  H * -2 * G = 望みの高さに到達するために必要な速度
                verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                if (hasAnimator)
                {
                    //アニメーターのパラメータにジャンプ情報を入れる
                    animator.SetBool(animIDJump, true);
                    isJumpFlag = true;
                }
            }

            // ジャンプタイムアウトが残っていたら減らす
            if (jumpTimeoutDelta >= 0.0f)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // ジャンプタイムアウトをリセット
            jumpTimeoutDelta = JumpTimeout;

            // 落下タイムアウトが残っていたら減らす
            if (fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                if (hasAnimator)
                {
                    //アニメーターのパラメータに落下情報を入れる
                    animator.SetBool(animIDFreeFall, true);
                }
            }

            //再び押せるようにする
            input.jump = false;
            isJumpFlag = false;
        }

        // 重力を足す
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
    /// キャラクターの投げる動作
    /// 　＋移動
    /// </summary>
    private void Throw()
    {
        //投げるキーが押されたら
        if (input.ThrowingAndCatching && isBallHaving && Grounded && !isJumpFlag && !freeze && isFinishRotation)
        {
            if (hasAnimator)
            {
                //投げる
                Throwing = true;

                //動けなくする
                freeze = true;

                //アニメーターのパラメータに投げる情報を入れる
                animator.SetBool(animIDThrow, Throwing);

                GameObject.FindWithTag("Ball1").GetComponent<Rigidbody>().isKinematic = false;
            }
        }

        //投げている最中
        if (Throwing)
        {
            GameObject ball = GameObject.FindWithTag("Ball1");

            //ボールを手放す時間(要調整)
            if (throwTimeoutDelta <= 0.95f && !Throwed)
            {
                ball.gameObject.transform.parent = null;
                ball.GetComponent<Rigidbody>().isKinematic = false;
                ball.GetComponent<Ball>().CheckLayer(m_teamLayer);
                ball.GetComponent<Ball>().CheckThrowObject(this.gameObject);
                ball.GetComponent<Ball>().CollisionNullification(); //当たり判定を一時的に消す

                if (CurrentTarget == null)
                {
                    ball.GetComponent<Ball>().Straight(transform.forward);
                }
                else
                {
                    if(targetNumber == 0) //体
                        ball.GetComponent<Ball>().TargetStraight(CurrentTarget.transform.position);
                    else if(targetNumber == 1) //足
                        ball.GetComponent<Ball>().TargetDownward(CurrentTarget.transform.position);
                }

                isBallHaving = false;
                Throwed = true;
            }
            if (throwTimeoutDelta <= 0.9f)
            {
                ball.GetComponent<Ball>().CollisionValidation(); //当たり判定を元に戻す
            }

            //投げるデルタタイムが残っていたら減らす
            if (throwTimeoutDelta >= 0.0f)
            {
                //ball.GetComponent<Rigidbody>().useGravity = true;
                throwTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                if (hasAnimator)
                {
                    //投げ終了
                    Throwing = false;

                    //動けるようにする
                    freeze = false;

                    //アニメーターのパラメータに投げる情報を入れる
                    animator.SetBool(animIDThrow, Throwing);

                    //再び押せるようにする
                    input.ThrowingAndCatching = false;
                }
            }
        }
        else
        {
            //投げるデルタタイムをリセット
            throwTimeoutDelta = ThrowTimeout;
            Throwed = false;
        }
    }

    /// <summary>
    /// 回避モーション 
    /// 　＋移動
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
            //避けるキーが押されたら
            if (input.dodge && !isBallHaving && Grounded && !isJumpFlag && !freeze)
            {
                if (hasAnimator)
                {
                    //回避する
                    Dodging = true;

                    //動けなくする
                    freeze = true;

                    coolTime.CoolTimeFlag = Dodging;
                    //アニメータのパラメータに情報を入れる
                    animator.SetBool(animIDDodge, Dodging);
                }
            }
        }
        

        if (Dodging)
        {
            //回避
            if (0.8f >= dodgeTimeoutDelta && dodgeTimeoutDelta >= 0.1f)
            {
                controller.Move(transform.forward * DodgeSpeed * Time.deltaTime);
            }

            //回避時間が残っていたら減らす（回避を続ける）
            if (dodgeTimeoutDelta >= 0.0f)
            {
                dodgeTimeoutDelta -= Time.deltaTime;

                hp -= 0.1f;
            }
            //回避時間が残っていなかったらアニメーターを更新する
            else
            {
                if (hasAnimator)
                {
                    //回避終了
                    Dodging = false;

                    //動けるようにする
                    freeze = false;

                    //アニメータのパラメータに投げる情報を入れる
                    animator.SetBool(animIDDodge, Dodging);
                }
            }
        }
        else
        {
            //避けるデルタタイムをリセット
            dodgeTimeoutDelta = DodgeTimeout;

            //再び押せるようにする
            input.dodge = false;
        }
    }

    /// <summary>
    /// キャッチ
    /// 　＋移動？
    /// 　＋ジャンプ？
    /// </summary>
    private void Catch()
    {
        ballCheckSpherePosition = transform.position + transform.forward.normalized;

        ballCheckSpherePosition.y = 1.5f;

        if (Grounded && !isJumpFlag)
        {
            //キャッチキーが押されたら
            if (input.ThrowingAndCatching && !isBallHaving && !freeze)
            {
                if (hasAnimator)
                {
                    //キャッチする
                    Catching = true;

                    //動けなくする
                    freeze = true;

                    //アニメーターのパラメータに情報を入れる
                    animator.SetBool(animIDCatch, Catching);
                }
            }

            if (Catching)
            {
                if (!isBallHaving)
                {
                    //キャッチ
                    if (0.4f >= catchTimeoutDelta && catchTimeoutDelta >= 0.3f)
                    {
                        bool hasCatchBall = Physics.CheckSphere(ballCheckSpherePosition, BallCheckRadius, BallLayers, QueryTriggerInteraction.Ignore);

                        if (hasCatchBall)
                        {

                            PickUpBall();
                        }
                    }
                }


                //時間が残っていたら減らす(キャッチを続ける)
                if (catchTimeoutDelta >= 0.0f)
                {
                    catchTimeoutDelta -= Time.deltaTime;
                }
                //時間が残っていなかったらアニメーターを更新する
                else
                {
                    if (hasAnimator)
                    {
                        //キャッチ終了
                        Catching = false;

                        //動けるようにする
                        freeze = false;

                        //アニメーターのパラメータにキャッチの情報を入れる
                        animator.SetBool(animIDCatch, Catching);
                    }
                }
            }
            else
            {
                //キャッチのデルタタイムをリセット
                catchTimeoutDelta = CatchTimeout;
            }
        }
    }

    /// <summary>
    /// パス
    /// </summary>
    private void Pass()
    {
        //地面と接地している
        if (Grounded)
        {
            //投げるキーが押されたら
            if (input.pass && !Throwing && !Passing && !Fainting)
            {
                if (hasAnimator)
                {
                    Passing = true;
                    //アニメーターのパラメータに投げる情報を入れる
                    animator.SetBool(animIDPass, Passing);

                    GameObject.FindWithTag("Ball1").GetComponent<Rigidbody>().isKinematic = false;
                }
            }

            //投げている最中
            if (Passing)
            {
                GameObject ball = GameObject.FindWithTag("Ball1");

                //ボールを手放す時間(要調整)
                if (passTimeoutDelta <= 0.84f)
                {
                    ball.gameObject.transform.parent = null;

                    ball.GetComponent<Rigidbody>().AddForce(
                        new Vector3(spherePosition.x, spherePosition.y + 5.0f, spherePosition.z), ForceMode.Impulse);
                    //ball.GetComponent<Ball>().Throwing(transform.forward);
                    isBallHaving = false;
                }

                //投げるデルタタイムが残っていたら減らす
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
                        //アニメーターのパラメータに投げる情報を入れる
                        animator.SetBool(animIDPass, Passing);
                    }
                }
            }
            else
            {
                //投げるデルタタイムをリセット
                passTimeoutDelta = PassTimeout;
            }
        }

        //再び押せるようにする
        input.pass = false;
    }

    /// <summary>
    /// フェイント
    /// </summary>
    private void Faint()
    {
        //地面と接地している
        if (Grounded)
        {
            //投げるキーが押されたら
            if (input.faint && !isJumpFlag && isBallHaving && !freeze)
            {
                if (hasAnimator)
                {
                    //フェイントする
                    Fainting = true;

                    //動けなくする
                    freeze = true;

                    //アニメーターのパラメータに投げる情報を入れる
                    animator.SetBool(animIDFaint, Fainting);
                }
            }

            //投げている最中
            if (Fainting)
            {

                //投げるデルタタイムが残っていたら減らす
                if (faintTimeoutDelta >= 0.0f)
                {
                    faintTimeoutDelta -= Time.deltaTime;

                    hp -= 0.05f;
                }
                else
                {
                    if (hasAnimator)
                    {
                        //フェイント終了
                        Fainting = false;

                        //動けるようにする
                        freeze = false;

                        //アニメーターのパラメータに投げる情報を入れる
                        animator.SetBool(animIDFaint, Fainting);
                    }
                }
            }
            else
            {
                //投げるデルタタイムをリセット
                faintTimeoutDelta = FaintTimeout;
            }
        }

        //再び押せるようにする
        input.faint = false;
    }

    private void DropBall()
    {
        GameObject ball = GameObject.FindWithTag("Ball1");

        ball.gameObject.transform.parent = null;
        ball.GetComponent<Rigidbody>().isKinematic = false;
        ball.GetComponent<Ball>().CheckLayer(this.gameObject.layer);
        ball.GetComponent<Ball>().CheckThrowObject(this.gameObject);
        //ball.GetComponent<Ball>().CollisionNullification(); //当たり判定を一時的に消す
        ball.GetComponent<Ball>().UseGravity = true;
    }

    private void TargetArea()
    {
        //ターゲットしていなかったらリターン
        if (CurrentTarget == null) return;

        //入力されていなかったらisHoldをfalse
        if (!input.lower && !input.upper) isHold = false;
        if (isHold) return;

        switch (targetNumber)
        {
            case 0:  //体

                //投げの処理↓

                //足を狙う
                if (input.lower)
                {
                    targetNumber = 1;
                    isHold = true;
                    //input.lower = false;
                }
                break;
            case 1:  //足

                //投げの処理

                //体を狙う
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
    /// 死ぬ(HPが0以下になったら死亡)
    /// </summary>
    public void Die()
    {
        //Hpが残っていたらリターン
        if (hp > 0) return;

        if (hasAnimator)
        {
            CurrentTarget = null;

            //死ぬ
            Dying = true;

            hp = 0;

            //動けなくする
            freeze = true;

            //アニメーターのパラメータに死ぬ情報を入れる
            animator.SetBool(animIDDie, Dying);

            CinemachineCameraTarget.transform.localPosition = new Vector3(0.0f,0.2f,0.0f);

            if (isBallHaving)
                DropBall();
        }
    }

    /// <summary>
    /// 自己ヒール
    /// </summary>
    private void Charge()
    {
        if (Grounded && !isJumpFlag && !isBallHaving && !Reanimated)
        {
            //ヒール
            if (input.resuscitation && hp < 100)
            {
                if (hasAnimator)
                {
                    //ヒール
                    Charging = true;

                    //動けないようにする
                    freeze = true;

                    //体力を徐々に回復する
                    hp += 0.5f;

                    //アニメーターのパラメータに生き返る情報を入れる
                    animator.SetBool(animIDCharge, Charging);

                    if (newEffect == null)
                    {
                        //パーティクル
                        newEffect = Instantiate(m_soseiParticle);
                        newEffect.transform.position = transform.position;
                        newEffect.Play();
                    }
                }
            }
            else
            {
                //蘇生が終わった後だけ入る
                if (Charging)
                {
                    //アニメーターのパラメータに蘇生情報を入れる
                    animator.SetBool(animIDCharge, false);

                    if (newEffect != null)
                    {
                        Destroy(newEffect.gameObject);
                        newEffect.Stop();
                    }

                    //時間が残っていたら減らす
                    if (chargeTimeoutDelta >= 0.0f)
                    {
                        chargeTimeoutDelta -= Time.deltaTime;
                    }
                    else
                    {
                        if (hasAnimator)
                        {
                            //蘇生を終了する
                            Charging = false;

                            //動けるようにする
                            freeze = false;

                            //投げるデルタタイムをリセット
                            chargeTimeoutDelta = ChargeTimeout;
                        }
                    }
                }
            }
        }
    }


    /// <summary>
    /// 復活(自動的にHPが回復する&HPがMAXになったら復活)
    /// </summary>
    public void Revival()
    {
        //死んでいなかったらリターン
        if (!Dying) return;

        //HPが全回復したら復活
        if (hp >= 100)
        {
            if (hasAnimator)
            {
                //復活
                Dying = false;

                //動けるようにする
                freeze = false;

                hp = 100;

                //アニメーターのパラメータに生き返る情報を入れる
                animator.SetBool(animIDDie, Dying);

                CinemachineCameraTarget.transform.localPosition = new Vector3(0.0f, 1.0f, 0.0f);
            }
            return;
        }

        //体力を徐々に回復する
        hp += 0.1f;
    }

    /// <summary>
    /// 蘇生(死んでいる味方の近くで蘇生ボタンを押す。味方のHPを早く回復する。)
    /// </summary>
    public void Resuscitation()
    {
        //蘇生ボタンを押したら
        if (input.resuscitation && !isJumpFlag && !isBallHaving && allyGameObject.GetComponent<ThirdPersonController>().DIE)
        {
            //trueなら
            if (resuscitable = CheckResuscitable())
            {
                //蘇生する
                Reanimated = true;

                //動けないようにする
                freeze = true;

                if (newEffect == null)
                {
                    //パーティクル
                    newEffect = Instantiate(m_soseiParticle);
                    newEffect.transform.position = transform.position;
                    newEffect.Play();
                }

                //アニメーターのパラメータに蘇生情報を入れる
                animator.SetBool(animIDResuscitation, Reanimated);

                //HPを高速回復する
                allyGameObject.GetComponent<ThirdPersonController>().HP += 0.5f;

                //自身のHPも少しずつ回復する
                hp += 0.1f;
            }
        }
        else
        {
            //蘇生が終わった後だけ入る
            if (Reanimated)
            {
                //アニメーターのパラメータに蘇生情報を入れる
                animator.SetBool(animIDResuscitation, false);

                if (newEffect != null)
                {
                    Destroy(newEffect.gameObject);
                    newEffect.Stop();
                }

                //時間が残っていたら減らす
                if (resuscitableTimeoutDelta >= 0.0f)
                {
                    resuscitableTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    if (hasAnimator)
                    {
                        //蘇生を終了する
                        Reanimated = false;

                        //動けるようにする
                        freeze = false;

                        //投げるデルタタイムをリセット
                        resuscitableTimeoutDelta = ResuscitableTimeout;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 蘇生できるかのチェック1
    /// </summary>
    /// <returns>蘇生可能:true  蘇生不可:false</returns>
    bool CheckResuscitable()
    {
        // 中心間の距離の平方を計算
        Vector3 d = transform.position - allyGameObject.transform.position;

        float dist2 = Vector3.Dot(d, d);

        // 平方した距離が平方した半径の合計よりも小さい場合には衝突している
        float radiusSum = ResuscitationRange + ResuscitationRange;

        return dist2 <= radiusSum * radiusSum;
    }

    /// <summary>
    /// モード切り替え
    /// </summary>    
    private void ChangeMode()
    {
        if (input.untarget && Grounded && !freeze)
        {
            if (CurrentTarget != null)
            {
                //敵をターゲット解除
                CurrentTarget = null;
            }
            else
            {
                //敵を追加
                CurrentTarget = enemyGameObject[enemyNumber];
            }
        }
        input.untarget = false;
    }

    /// <summary>
    /// ターゲット切り替え
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
    /// ボールを拾う
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
    /// カメラの回転(360°, -360°を超えたら0に戻す)
    /// </summary>
    /// <param name="lfAngle">現在のカメラ角度</param>
    /// <param name="lfMin">縦の角度限界(MIN)</param>
    /// <param name="lfMax">縦の角度限界(MAX)</param>
    /// <returns></returns>
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    /// <summary>
    /// ギズモでコライダーを表示する
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
    /// 生存している敵を探す
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
    /// 生存している敵を開放する
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
