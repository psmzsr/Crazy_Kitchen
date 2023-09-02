using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour,IKitchenObjectparent
{

    public static Player Instance{get;private set;}

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnselectedCounterChangedEventArgs> OnSelectCounterChanged;
    public class OnselectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;


    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;


    private void Awake()
    {
        if(Instance!=null)
        {
            Debug.Log("There is more than one Player instance");
        }
        Instance = this;   
    }

    private void Start()
    {
        //��һ���¼���������GameInput_OnInterAction����ӵ���Ϊ gameInput ���¼�ί�� OnInterAction ���¼��С�
        gameInput.OnInterAction += GameInput_OnInterAction;
        gameInput.OnInterAlternateAction += GameInput_OnInterAlternateAction;
    }

    private void GameInput_OnInterAlternateAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInterAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);


        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }


        float interactDistance = 2f;
        if(Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance,countersLayerMask))
        {
            if(raycastHit.transform.TryGetComponent(out BaseCounter basecounter))
            {
                //�й�̨
                if (basecounter != selectedCounter)
                {
                    SetSelectedCounter ( basecounter);
                }
            }
            else
            {
                SetSelectedCounter(null);

            }
        }
        else
        {
            SetSelectedCounter(null);
        }
        //Debug.Log(selectedCounter);
    }

    private void HandleMovement()
    {
        //��ȡ�û������ֵ
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        //�ƶ�
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);


        //��ײ���
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            //���ܳ��ƶ������ƶ�

            //��ͼֻ��x�����ƶ�
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove =(moveDir.x<-.5f||moveDir.y>+.5f)&& !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                //����ֻ��x�����ƶ�
                moveDir = moveDirX;
            }
            else
            {
                //����ֻ��x���ƶ�

                //��ͼ��z���ƶ�
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    //ֻ����z���ƶ�
                    moveDir = moveDirZ;
                }
                else
                {
                    //�������κη����ƶ�
                }
            }
        }


        if (canMove)
        {
            transform.position += moveDir * Time.deltaTime * moveSpeed;
        }

        //�ж��Ƿ����ߣ����ڶ���
        isWalking = moveDir != Vector3.zero;
        //����ת������
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectCounterChanged?.Invoke(this, new OnselectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    //�ӿ�
    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
