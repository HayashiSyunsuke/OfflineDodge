using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class pickUpTheBall : MonoBehaviour
{
    public HitWallMaria hitWallMaria;

    private bool isHavingBall = false;

    [SerializeField] Transform target;

    private float speed = 10.0f;
    private Transform ballPosition;
    //ボールを保持する手のGameObject
    [Tooltip("takasiRightHand")] public GameObject rightHandGameObject;

    [SerializeField]
    [Tooltip("追いかける対象")]
    private GameObject ball;

    private NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        Debug.Log(HitWallMaria.pickUpBallTakasi);
    }

    // Update is called once per frame
    void Update()
    {
        if (HitWallMaria.pickUpBallTakasi == true)
        {
            if (isHavingBall == false)
            {
                navMeshAgent.destination = ball.transform.position;
            }

            //Debug.Log(HitWallMaria.pickUpBallTakasi);

        }

        if (ballPosition == null)
        {
            return;
        }

        Vector3 vecBallPosition = new Vector3(ballPosition.transform.position.x, ballPosition.transform.position.y, ballPosition.transform.position.z);

        if (isHavingBall == true)
        {
            ballPosition.position = Vector3.MoveTowards(vecBallPosition, target.position, speed * Time.deltaTime);

            if (ballPosition.position == target.position)
            {
                isHavingBall = false;
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        //接触したオブジェクトのタグが"Player"のとき
        if (other.CompareTag("Ball1"))
        {
            Debug.Log("ata");
            GameObject ball = GameObject.FindWithTag("Ball1");
            ballPosition = ball.GetComponent<Transform>();
            ball.GetComponent<Rigidbody>().isKinematic = true;
            ball.GetComponent<Rigidbody>().useGravity = false;
            ball.GetComponent<Ball>().UseGravity = false;
            ball.GetComponent<Ball>().HitValidity = true;
            Transform rightHand = rightHandGameObject.transform;//GameObject.FindWithTag("RightHand").transform;
            ballPosition.position = rightHand.position;
            ball.transform.parent = rightHand;
            isHavingBall = true;

            if (isHavingBall == true)
            {
                ball.transform.parent = null;
            }
        }



    }

}
