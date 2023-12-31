using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public event EventHandler OnPickSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter baseCounter;
    }

    public static Player Instance { get; private set; }

    [Header("Movement Setting")]

    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotateSpeed;

    [Header("Reference")]

    [SerializeField] private GameInputManager gameInputManager;
    [SerializeField] private Transform kitchenObjectHoldPoint;


    [Header("Collision Settings")]
    [Tooltip("ColliderHeight"), SerializeField] private float playerHeight;
    [Tooltip("Collision Redius"), SerializeField] private float playerRadius;
    [Tooltip("Interaction Distance"), SerializeField] private float interactDistance;
    [Tooltip("Which Layer"), SerializeField] private LayerMask counterLayer;



    private BaseCounter baseCounter;
    private Vector3 lastMoveDir;
    private KitchenObject kitchenObject;

    private bool isWalking;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple instance of player class", this);
        }
        baseCounter = null;
    }

    private void OnEnable()
    {
        gameInputManager.OnInteractAction += GameInputManager_OnInteractAction;
        gameInputManager.OnInteractAlternateAction += GameInputManager_OnInteractAlternateAction;
    }

    private void GameInputManager_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying())
            return;

        if (baseCounter != null)
        {
            baseCounter.InteractAlternate(this);
        }
    }

    private void GameInputManager_OnInteractAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying())
            return;

        if (baseCounter != null)
        {
            baseCounter.Interact(this);
        }
    }

    private void OnDisable()
    {
        gameInputManager.OnInteractAction -= GameInputManager_OnInteractAction;
        gameInputManager.OnInteractAlternateAction -= GameInputManager_OnInteractAlternateAction;
    }

    private void Update()
    {

        Vector2 inputVector = gameInputManager.GetMovementInputNormalize();

        Vector3 moveDir = new(inputVector.x, 0, inputVector.y);

        HandleInteraction(moveDir);

        Movement(moveDir);

    }

    private void HandleInteraction(Vector3 moveDir)
    {
        if (moveDir != Vector3.zero)
        {
            lastMoveDir = moveDir;
        }
        if (Physics.Raycast(transform.position, lastMoveDir, out RaycastHit raycastHit, interactDistance, counterLayer))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != this.baseCounter)
                {
                    SetSelectedCounter(baseCounter);
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
    }

    private void Movement(Vector3 movementVector)
    {
        isWalking = movementVector != Vector3.zero;

        float moveDistance = movementSpeed * Time.deltaTime;


        ///
        /// this one is checking can we move in input dir
        /// if we are not can below condition will check
        /// if we are than we can movement and rotation function
        ///

        bool canMove = CanMove(movementVector, moveDistance);

        if (!canMove)
        {

            ///
            /// this one is checking can we move in x dir when we are doing forward (z/a,z/d);
            /// if we are not can below condition will check -> x dir
            /// if we are than we can movement and rotation function
            ///
            Vector3 movementVectorX = new Vector3(movementVector.x, 0, 0).normalized;

            canMove = (movementVectorX.x < -.5f || movementVector.x > .5f) && CanMove(movementVectorX, moveDistance);

            if (canMove)
            {
                movementVector = movementVectorX;
            }
            else
            {
                ///
                /// this one is checking can we move in x dir when we are doing forward (x/w,x/s);
                /// if we are not can below condition will check -> z dir
                /// if we are than we can movement and rotation function
                ///
                Vector3 movementVectorZ = new Vector3(0, 0, movementVector.z).normalized;

                canMove = (movementVectorZ.z < -.5f || movementVectorZ.z > .5f) && CanMove(movementVectorZ, moveDistance);

                if (canMove)
                {
                    movementVector = movementVectorZ;
                }
            }
        }

        ///
        /// at the end we will get move dir and are we able to move or not 
        ///
        MovementAndRotation();

        void MovementAndRotation()
        {
            if (canMove)
            {
                transform.position += moveDistance * movementVector;
            }
            if (movementVector != Vector3.zero)
            {
                transform.forward = Vector3.Slerp(transform.forward, movementVector, Time.deltaTime * rotateSpeed);
            }
        }

    }

    private bool CanMove(Vector3 dir, float dist)
    {
        if (!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, dir, dist))
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * playerHeight);
        Gizmos.DrawWireSphere((transform.position + transform.position + Vector3.up * playerHeight) / 2, playerRadius);

        Gizmos.DrawLine(transform.position, transform.forward * interactDistance);
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public void SetSelectedCounter(BaseCounter baseCounter)
    {

        this.baseCounter = baseCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            baseCounter = baseCounter
        });
    }

    public Transform CounterTopPointTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
            OnPickSomething?.Invoke(this, EventArgs.Empty);
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
