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
        //将一个事件处理方法（GameInput_OnInterAction）添加到名为 gameInput 的事件委托 OnInterAction 的事件中。
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
                //有柜台
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
        //获取用户输入的值
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        //移动
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);


        //碰撞检测
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            //不能朝移动方向移动

            //试图只往x方向移动
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove =(moveDir.x<-.5f||moveDir.y>+.5f)&& !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                //可以只往x轴上移动
                moveDir = moveDirX;
            }
            else
            {
                //不能只往x轴移动

                //试图在z轴移动
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    //只能在z轴移动
                    moveDir = moveDirZ;
                }
                else
                {
                    //不能往任何方向移动
                }
            }
        }


        if (canMove)
        {
            transform.position += moveDir * Time.deltaTime * moveSpeed;
        }

        //判断是否行走，用于动画
        isWalking = moveDir != Vector3.zero;
        //人物转动方向
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

    //接口
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
