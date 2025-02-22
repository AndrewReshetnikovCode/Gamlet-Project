using UnityEngine;
using System.Collections;
[RequireComponent(typeof(CharacterController))]
public class testAI_1204 : MonoBehaviour {

    public Vector3[] IdleMoveMap = new Vector3[] { Vector3.right, Vector3.forward, -Vector3.right, -Vector3.forward };
    public Transform target;
    public float ThreatenDistance = 20.0f;
    public float AttackDistance = 12.0f;    
    public float ChaseSpeed = 10.0f;         //controller enemy of chase speed    

    private CharacterController controller;

    private float CurrentDistance;
    private Vector3 CurrentDirection;
    private Vector3 gravity = -Vector3.up;    

    private float PlayTime; //save current play time
    private Vector3 OriginPosition;
    private float IdleModeTime = 0;
    private float TransformActionTime = 5.0f;

    public float AddAngel = 1;
    public float MaxSearchAngel = 45;
    private float CurrentAngel = 0;

    private RaycastHit hit;
    private bool isAttackFlag = false;

    enum IdleMode                   
    {
        one = 1, two, three, four, back //�Ω�Idle���A�P�O��
    }
    private IdleMode _IdleMode = IdleMode.one;

    void Awake()
    {
        PlayTime = 0;
        OriginPosition = transform.position;
        controller = GetComponent<CharacterController>();
    }    

    // Update is called once per frame
	void Update () 
    {
        CurrentAngel += AddAngel;
        if (CurrentAngel > MaxSearchAngel)
            AddAngel = -AddAngel;                   

        else if (CurrentAngel < -MaxSearchAngel)
            AddAngel = -AddAngel;
        Debug.DrawRay(transform.position, transform.TransformDirection(new Vector3(Mathf.Sin(CurrentAngel * Mathf.PI / 180), 0, Mathf.Cos(CurrentAngel * Mathf.PI / 180))) * AttackDistance, Color.yellow);

        CurrentDirection = target.position - transform.position;
        Debug.DrawRay(transform.position, CurrentDirection);
        CurrentDistance = CurrentDirection.magnitude;
        PlayTime = Time.time;                                       //save current play time

        //-------------------------------
        if (Physics.Raycast(transform.position, transform.TransformDirection(new Vector3(Mathf.Sin(CurrentAngel * Mathf.PI / 180), 0, Mathf.Cos(CurrentAngel * Mathf.PI / 180))), out hit, AttackDistance))
            if (hit.transform.tag == "Player")
                isAttackFlag = true;
        //--------------------------------
        if (controller.isGrounded)
        {
            if (CurrentDistance > ThreatenDistance)    //not enter threaten area.
            {
                Idle();
                isAttackFlag = false;                
            }

            else if (CurrentDistance <= ThreatenDistance && !isAttackFlag)  // target enter threaten area , start threaten.
            {                
                Thraten();                
                //transform.LookAt(target);
            }

            else if (isAttackFlag) // target enter attack area , start attack.
            {
                Attack();                                
            }
        }

        controller.SimpleMove(gravity);     //handle gravity
	}

    void Attack()                   //handle Attack state
    {
        print("�o�{�ؼ�!!!");
        GetComponent<Renderer>().material.color = Color.red;
        transform.rotation = Quaternion.LookRotation(CurrentDirection - new Vector3(0, CurrentDirection.y, 0));
        controller.SimpleMove(CurrentDirection / CurrentDistance * ChaseSpeed);
    }

    void Thraten()                  //handle Thraten state
    {
        print("ĵ�٤�!!");
        GetComponent<Renderer>().material.color = Color.yellow;
        transform.rotation = Quaternion.LookRotation(CurrentDirection - new Vector3(0, CurrentDirection.y, 0));
        //�w�C�V�o�X�n�����a�貾��
        controller.SimpleMove(transform.TransformDirection(Vector3.forward) * Time.deltaTime * ChaseSpeed);

        _IdleMode = IdleMode.back;  //������back�Ҧ�,�Y���}Attack���A,�N�bIdle���A�^���l��m(OriginPosition)
    }


    void Idle()                     //handle idle state
    {
        GetComponent<Renderer>().material.color = Color.blue;

        if (PlayTime - IdleModeTime > TransformActionTime && _IdleMode != IdleMode.back)    //�@�q�ɶ�(TransformActionTime)�ഫ���U�@�Ӳ��ʼҦ�
        {
            IdleModeTime = PlayTime;            //�O���ثe���C���ɶ�(PlayTime),�Ω�P�_�O�_�����U�@�Ҧ���
            if (_IdleMode != IdleMode.four)
                _IdleMode++;
            else
                _IdleMode = IdleMode.one;            
        }

        switch (_IdleMode)      //(_IdleMode)�|�ز��ʼҦ� + �l�v��^����I�Ҧ�
        {
            case IdleMode.one:  //Mode 1:                
                //transform.eulerAngles = Vector3.RotateTowards(new Vector3(0, 0, 0), new Vector3(0,Mathf.Cos(Time.time), 0), 45*Mathf.PI/180, 1) * 100;
                transform.rotation = Quaternion.LookRotation(IdleMoveMap[0]);
                controller.SimpleMove(IdleMoveMap[0]);
                break;

            case IdleMode.two:  //Mode 2:
                transform.rotation = Quaternion.LookRotation(IdleMoveMap[1]);
                controller.SimpleMove(IdleMoveMap[1]);
                break;

            case IdleMode.three://Mode 3:
                transform.rotation = Quaternion.LookRotation(IdleMoveMap[2]);
                controller.SimpleMove(IdleMoveMap[2]);
                break;

            case IdleMode.four: //Mode 4:
                transform.rotation = Quaternion.LookRotation(IdleMoveMap[3]);
                controller.SimpleMove(IdleMoveMap[3]);
                break;

            case IdleMode.back:
                Vector3 distance = OriginPosition - transform.position;//�p��P�_�l��m(OriginPosition)�������Z��,�æb�l�v��}�l���^��
                distance.y = 0;
                transform.rotation = Quaternion.LookRotation(distance);
                controller.SimpleMove(distance * Time.deltaTime * ChaseSpeed);
                if (distance.magnitude < 3)//�Y�^����I�N�Ҧ����^�|�ز��ʼҦ�
                {
                    _IdleMode = 0;
                    IdleModeTime = 0;
                }
                break;
        }    

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ThreatenDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, AttackDistance);
    }

}
