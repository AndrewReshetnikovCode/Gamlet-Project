using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour
{
    //---------------public--------------------
    public CharacterController Person;          //�H�����
    public float ElevatorSpeed = 0.2f;          //�q�貾�ʪ��t��
    public float ElevatorHeight = 4.4f;         //�q�貾�ʪ��d��(�ӤU��ӤW���Z��)
    
    //---------------private-------------------
    private float addVaule = 0;                             //�W�[�ܶq�A�Ω�վ�q�誺����
    private float ElevatorSmoothMove = 0;                   //�Q�ΤT���禡�Ϲq��X�{���Ʋ���   
    private Vector3 ElevatorOriginPosition;                 //�q��_�l��m
    private Vector3 PersonCurrentPosition = Vector3.zero;    //�H�����U�q��˸m���誺��m
    private float PersonOrigin_y = 0;
    private float ElevatorSearcharea = 2.5f;                //�H�q�誫�󬰤��ߡA�j���d�򤺪����a�A�]���}�ҹq����

    //------------------- flag ----------------- 
    private bool isOpenMenu = false;                        //�O�_�}�ҹq�豱����
    public bool isDebugMessage = false;                     //�O�_�}�Ұ����T��

    enum ElevatorState
    {
        Up , Down , Stop
    }
    private ElevatorState Elevatorstate = ElevatorState.Stop;   //�P�_�{�b�q�説�A

    void OnGUI()
    {
        if (isOpenMenu)
        {
            if (GUI.Button(new Rect(Screen.width * 0.5f, Screen.height * 0.4f, Screen.width * 0.15f, Screen.height * 0.1f), "Up") && Elevatorstate == ElevatorState.Stop)
            {
                PersonOrigin_y = Person.transform.position.y;
                Elevatorstate = ElevatorState.Up;
                if(isDebugMessage)
                    print("Elevator is up !!");
            }
            if (GUI.Button(new Rect(Screen.width * 0.5f, Screen.height * 0.6f, Screen.width * 0.15f, Screen.height * 0.1f), "Down") && Elevatorstate == ElevatorState.Stop)
            {
                PersonOrigin_y = Person.transform.position.y - ElevatorHeight;
                Elevatorstate = ElevatorState.Down;
                if (isDebugMessage)
                    print("Elevator is down !!");
            }
            
        }
    }

    void Start()
    {
        ElevatorOriginPosition = transform.position;        
    }

	void Update ()
    {
        //-------------------�i�J�q��P���d��}�ҿ��------------------------
        if (Vector3.Distance(Person.transform.position, GetComponent<Collider>().transform.position) <= Person.radius + ElevatorSearcharea)
            isOpenMenu = true;

        else
            isOpenMenu = false;
        //----------------------------------------------------------------

        //----------�Q�ΤT���禡�A�q OriginPosition ���ʨ�OriginPosition + ElevatorHeight----------------        
        ElevatorHandle();     
        //---------------------------------------------------------------------------
	}

    void ElevatorHandle()
    {        
        if (Elevatorstate != ElevatorState.Stop)
        {
            PersonCurrentPosition = Person.transform.position;
            PersonCurrentPosition.y = PersonOrigin_y;
            if (Elevatorstate == ElevatorState.Up)
            {
                ElevatorSmoothMove = Mathf.Sin(addVaule * Mathf.PI / 180);
                if (addVaule < 90)
                    addVaule += ElevatorSpeed;
                else
                {
                    Elevatorstate = ElevatorState.Stop;
                    return;
                }
            }

            else if (Elevatorstate == ElevatorState.Down)
            {
                ElevatorSmoothMove = Mathf.Sin(addVaule * Mathf.PI / 180);
                if (addVaule > 0)
                    addVaule -= ElevatorSpeed;
                else
                {
                    Elevatorstate = ElevatorState.Stop;
                    return;
                }
            }

            Person.transform.position = PersonCurrentPosition + new Vector3(0, ElevatorHeight * ElevatorSmoothMove, 0);
            transform.position = ElevatorOriginPosition + new Vector3(0, ElevatorHeight * ElevatorSmoothMove, 0);
        }        
    }

}
