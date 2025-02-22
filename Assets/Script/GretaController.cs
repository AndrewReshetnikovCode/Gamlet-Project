using UnityEngine;
using System.Collections;
[RequireComponent(typeof(CharacterController))]
public class GretaController : MonoBehaviour
{
    public const string GRETANAME = "Greta";
    
    //-------------------------------------------------
    public Transform LeftWheel;
    public Transform RightWheel;

    private float WheelSpeed = 0;

    //-------------------------------------------------
    
    public float speed = 2.0F;                  //�t�׭���
    public float Velocity = 0;                  //�����t��    
    public float jumpSpeed = 2.0F;              //���D�t��
    public float gravity = 9.8F;                //���O(�P����t�εL��)
    public float MaxVelocity = 5.0f;            //�̤j���V�t��
    public float nMaxVelocity = -5.0f;          //�̤j�ϦV�t��    
    public float FrictionCoefficient = 0.15f;   //�����Y��
    public float HandBrakeCoefficient = 0.1f;   //��٨��Y��
    public Transform PlayCamera;                //�[�ݥD�����D��v��
    public float GretaWeight = 40;              //���q
    public bool isCanJump = false;              //�O�_�i���D
    
    private Vector3 moveDirection = Vector3.zero;   //���ʪ���V
    private Vector3 LookDirection;                  //�H���ݱo��V    
    private CharacterController controller;         //�H������
    private float FrictionForce = 0;                //�����O
    

    //------------------- flag -----------------
    private bool isMouseRButton = false;        //�O�_���U�ƹ��k��
    private bool isHandBrakeflag = false;       //�O�_���U�ƹ�����(��٨�)
    private bool isInclineRight = false;        //�O�_�b�שY�W���U�k��
    private bool isLevelRight = false;          //�O�_�b���a�W���U�k��
    public bool isDebugMessage = false;         //�O�_�}�Ұ����T��
    private bool isGameStop = false;            //�C���O�_�Ȱ�
    public bool IsGameStop()
    {
        return isGameStop;
    }
    

    //------------ incline --------------------
    private RaycastHit inclineHit;          //�׭��k�V�q
    private float inclineAngle = 0;         //�׭��P��������
    private float inclineForce = 0;         //�׭����V�O

    enum InclineDirection
    {
        level ,positive, negative
    }
    InclineDirection inclinedirection = InclineDirection.level;
    //--------------------------------------------

    public float pushPower = 2.0F;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.GetComponent<Rigidbody>();
        //


        if (body == null || body.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3F)
            return;

        //Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        Vector3 pushDir = Vector3.zero;
        RaycastHit lHit;



        if (Mathf.Abs(hit.moveDirection.x) > Mathf.Abs(hit.moveDirection.z))
        {
            if (Mathf.Abs(hit.moveDirection.x) > 0.9f)
                pushDir = new Vector3(hit.moveDirection.x, 0, 0);
            //body.velocity = pushDir * pushPower * Mathf.Abs(Velocity) * 0.2f;
            //print(pushDir);
        }
        else if (Mathf.Abs(hit.moveDirection.x) < Mathf.Abs(hit.moveDirection.z))
        {
            if (Mathf.Abs(hit.moveDirection.z) > 0.9f)
                pushDir = new Vector3(0, 0, hit.moveDirection.z);
            //body.velocity = pushDir * pushPower * Mathf.Abs(Velocity) * 0.2f;
            //print(pushDir);
        }

        //pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        //Debug.DrawRay(body.transform.position, pushDir*1, Color.green);

        if (body.SweepTest(pushDir, out lHit, 0.2f))
            if (string.Compare("Object", 0, lHit.transform.name, 0, 6, true) == 0)
                return;

        body.velocity = pushDir * pushPower * Mathf.Abs(Velocity) * 0.2f ;
    }

    public void LetGameStop(bool stop)
    {
        isGameStop = stop;
    }
    
    
    void Update()
    {
        Application.targetFrameRate = 70;
        controller = GetComponent<CharacterController>();
        
        //---------------handle mouse message-----------------
        if (Input.GetMouseButtonDown(2))
        {
            isHandBrakeflag = true;                   
        }

        if (Input.GetMouseButtonUp(2))
        {
            isHandBrakeflag = false;                   
        }

        if (Input.GetMouseButtonDown(1))
        {
            isMouseRButton = true;
        }

        if (Input.GetMouseButtonUp(1))
        {            
            isMouseRButton = false;            
        }
        
        //---------------------------------------------------

        //----------handle handbrake--------------------------
        if (isHandBrakeflag)
        {
            if (Velocity < -HandBrakeCoefficient)
                Velocity += HandBrakeCoefficient;
            else if (Velocity > HandBrakeCoefficient)
                Velocity -= HandBrakeCoefficient;
            else
                Velocity = 0;
        }
        //--------------------------------------------

        if (controller.isGrounded && isGameStop != true)                              //On the ground to move     
        {
            float Velocity_temp = Input.GetAxis("Vertical") * 0.1f;
            float Velocity_H_temp = Input.GetAxis("Mouse X") * 0.02f;
            
            if (Velocity_temp > 0)
            {
                if (!GetComponent<Animation>().IsPlaying("Walk_Forward"))
                {
                    GetComponent<Animation>()["Walk_Forward"].wrapMode = WrapMode.Once;

                    GetComponent<Animation>()["Walk_Forward"].speed = 1.4f;
                    GetComponent<Animation>().Play("Walk_Forward");
                }
            }
            if (Velocity_temp < 0)
            {
                if (!GetComponent<Animation>().IsPlaying("Walk_Back"))
                {
                    GetComponent<Animation>()["Walk_Back"].wrapMode = WrapMode.Once;
                    GetComponent<Animation>()["Walk_Back"].speed = 1.4f;
                    GetComponent<Animation>().Play("Walk_Back");
                }
            }
            if (Velocity_temp == 0 && Velocity_H_temp == 0)
            {
                //animation["idle"].wrapMode = WrapMode.Loop;
                //animation.CrossFade("idle");
                if (!GetComponent<Animation>().IsPlaying("idle") && !GetComponent<Animation>().IsPlaying("Walk_Forward") && !GetComponent<Animation>().IsPlaying("Walk_Back") && !GetComponent<Animation>().IsPlaying("TurnRight") && !GetComponent<Animation>().IsPlaying("TurnLeft"))
                {
                    GetComponent<Animation>().Play("HandOnWheel");
                    GetComponent<Animation>().CrossFade("idle");
                }
               // animation["HandOnWheel"].wrapMode = WrapMode.Loop;
               //animation.CrossFade("HandOnWheel");
            }
            if (Velocity_H_temp > 0.03f)
            {
                if (!isMouseRButton)
                {
                    if (!GetComponent<Animation>().IsPlaying("TurnRight"))
                    {
                        GetComponent<Animation>()["TurnRight"].wrapMode = WrapMode.Once;
                        GetComponent<Animation>()["TurnRight"].speed = 1.0f;
                        GetComponent<Animation>().Play("TurnRight");
                    }
                }
            }
            if (Velocity_H_temp < -0.03f)
            {
                if (!isMouseRButton)
                {
                    if (!GetComponent<Animation>().IsPlaying("TurnLeft"))
                    {
                        GetComponent<Animation>()["TurnLeft"].wrapMode = WrapMode.Once;
                        GetComponent<Animation>()["TurnLeft"].speed = 1.0f;
                        GetComponent<Animation>().Play("TurnLeft");
                    }
                }
            }

            Velocity += Velocity_temp;
           
            if (Input.GetButton("Jump") && isCanJump)            //handle jumping
                moveDirection.y = jumpSpeed;
            //���V�O    N = m*g                                inclineAngle = �׭�����
            //�׭�     Fi = m*g*sin(inclineAngle)              
            //�ʼ����O  fk = u * N * cos(inclineAngle)         u = �����Y��   �P���ʤ�V�ۤ�
            //�b�O     F = fk +fi 
            
            //�ʼ����O = FrictionCoefficient * GretaWeight * gravity * Mathf.Cos(inclineAngle * Mathf.PI / 180);
            //���YVelocity ����,�ʼ����O���t

            //�׭��O = GretaWeight * gravity * Mathf.Sin(inclineAngle * Mathf.PI / 180);
            //���Y�׭����H�����e,�O�q���t    �b�H���I��,�O�q����

            InclineForce();                 //handle InclineForce
            MoveFriction();                 //The impact of FrictionCoefficient
            Velocity += (inclineForce + FrictionForce);   //�b�OF = fk +fi
            //------------------debug message-------------------
            if (isDebugMessage)
            {
                print("inclineForce = " + inclineForce);
                print("FrictionForce = " + FrictionForce);
                print("inclinedirection = " + inclinedirection);
            }
            //--------------------------------------------------
            //-------�L�ճt��-------
            if (Velocity > MaxVelocity)
                Velocity = MaxVelocity;
            else if (Velocity < nMaxVelocity)
                Velocity = nMaxVelocity;
            else if (Velocity < 0.1f && Velocity > -0.1f)
                Velocity = 0.0f;
            //---------------------------
            
            
            
            moveDirection = new Vector3(0, 0, Velocity) * speed;
        }

        
        if (!isMouseRButton)
        {
            LookDirection = transform.position - PlayCamera.transform.position; //take the vector between camera and object 
            LookDirection.y = 0;
            //---------�ھڱ׭���V,�P�_Greta�ݱo��V---------
            if (inclinedirection == InclineDirection.positive)
                LookDirection.y = -Mathf.Sin(inclineAngle * Mathf.PI / 180) * LookDirection.magnitude;
            else if (inclinedirection == InclineDirection.negative)
                LookDirection.y = Mathf.Sin(inclineAngle * Mathf.PI / 180) * LookDirection.magnitude;
            //-----------------------------------------------   
            transform.rotation = Quaternion.LookRotation(LookDirection);    //handle greta look direction           
            
        }
        //--------------------------------����"�k��"�[��H���ɡAGreta�ݱo��V�ץ�-------------------------------------------        
        //-----------------���p�b�׭��W������k���[��B�J�쥭�a�A�ץ��H���ݪ���V------------------
        if (isInclineRight && inclinedirection == InclineDirection.level)
        {
            isInclineRight = false;
            Vector3 direction = LookDirection;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);    //handle greta look direction
        }
        //----------------------------------------------------------------------

        //-----------------���p�b�����W������k���[��B�J��W�U�Y�A�ץ��H���ݪ���V------------------
        if (isLevelRight && inclinedirection != InclineDirection.level)
        {
            isLevelRight = false;
            Vector3 direction = LookDirection;
            direction.y = 0;
            //---------�ھڱ׭���V,�P�_Greta�ݱo��V---------
            if (inclinedirection == InclineDirection.positive)
                direction.y = -Mathf.Sin(inclineAngle * Mathf.PI / 180) * direction.magnitude;
            else if (inclinedirection == InclineDirection.negative)
                direction.y = Mathf.Sin(inclineAngle * Mathf.PI / 180) * direction.magnitude;
            //-----------------------------------------------   
            transform.rotation = Quaternion.LookRotation(direction);    //handle greta look direction
        }
        //----------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------
        WheelSpeed += Velocity;
        LeftWheel.localRotation = Quaternion.Euler(-WheelSpeed, 180, 180);
        RightWheel.localRotation = Quaternion.Euler(-WheelSpeed, 180, 180);

        moveDirection.y -= gravity *0.015f;                    //The impact of gravity 
        controller.Move(transform.TransformDirection( moveDirection) * Time.deltaTime); //handle move

        
    }

    void InclineForce()
    {
        Ray ray = new Ray(transform.position, -Vector3.up);
        //---------debug normal vector ----------
        Debug.DrawRay(ray.origin, ray.direction, Color.black);

        //--------------------------get inclineAngle--------------------
        if (Physics.Raycast(ray, out inclineHit, 2))
        {
            //---------debug normal vector ----------
            Debug.DrawRay(inclineHit.point, inclineHit.normal * 5, Color.blue);

            inclineAngle = Vector3.Angle(-ray.direction, inclineHit.normal);
            if (isDebugMessage)
                print("inclineAngle = " + inclineAngle);
            if (inclineAngle == 0)
            {
                //------------�P�O�O�_�b�שY�W���k��A�p�G���}�ҭץ�---------------
                if (inclinedirection != InclineDirection.level && isMouseRButton)                
                    isInclineRight = true;
                //-----------------------------------------------------------
                inclinedirection = InclineDirection.level;
                inclineForce = 0;
                return;
            }
            //------------�P�O�O�_�b���a�W���k��A�p�G���}�ҭץ�---------------
            else
                if (inclinedirection == InclineDirection.level && isMouseRButton)
                    isLevelRight = true;
            //-----------------------------------------------------------
        }
        //----------------------------------------------------------------        
        ray = new Ray(transform.position - new Vector3(0, 0.4f, 0), transform.TransformDirection(Vector3.forward));
        Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
        
        //--------------------------handle negative incline---------------------------------------
        if (Physics.Raycast(ray, out inclineHit, 1) && inclineHit.collider.tag == "Finish")       
        {
            inclinedirection = InclineDirection.negative;
            inclineForce = -GretaWeight * gravity * Mathf.Sin(inclineAngle * Mathf.PI / 180) / 1000;
            return;
        }
        //--------------------------handle positive incline---------------------------------------           
        else if (Physics.Raycast(ray.origin, -ray.direction, out inclineHit, 1) && inclineHit.collider.tag == "Finish")
        {
            Debug.DrawRay(ray.origin, -ray.direction, Color.yellow);         
            inclinedirection = InclineDirection.positive;
            inclineForce = GretaWeight * gravity * Mathf.Sin(inclineAngle * Mathf.PI / 180) / 1000;            
        }
        
    }

    void MoveFriction()         //�B�z�����O,�P�t�׬ۤ�
    {
        if (Velocity > 0)
            FrictionForce = -FrictionCoefficient * GretaWeight * gravity * Mathf.Cos(inclineAngle * Mathf.PI / 180) / 1000;
        else if (Velocity < 0)
            FrictionForce = FrictionCoefficient * GretaWeight * gravity * Mathf.Cos(inclineAngle * Mathf.PI / 180) / 1000;
    }
}