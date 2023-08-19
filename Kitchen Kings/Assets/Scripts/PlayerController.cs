using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public event EventHandler OnPickup;
    public event EventHandler OnDrop;
    public event EventHandler OnTrashed;


    public event EventHandler OnCounterChange;

    // Android touch input
    [SerializeField] private Button pick_Button;
    [SerializeField] private Button leave_Button;
    //

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private GameInput gameInput;

    [SerializeField] private PlatesCounter platesCounter;   // reference to plates counter script

    [SerializeField] private DeliveryManager deliveryManager;  // reference to DeliveryManager script

    //[SerializeField] private Plate plate;   // reference to plate script

    private bool isWalking;

    private Vector3 prev_moveDir;

    private ClearCounter selectedCounter;

    // Alternate method of calling function Interact() in clear counter using C# events
    /*private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        HandleInteractions()
    }*/

    [SerializeField] private GameObject anchorPont;

    private bool hasChild = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pick_Button.onClick.AddListener(Pickup);
        leave_Button.onClick.AddListener(Drop);
    }
    private void Update()
    {
        HandleMovement();
        HandleInteractions();
        //Pickup();   // we do not need to call it every frame as we are not checking if button is pressed, we are using the touch inputs
        //Drop();
    }

    public ClearCounter SelectedCounterClass()
    {
        if (selectedCounter != null)
        {
            return selectedCounter;
        }
        return null;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalised();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            prev_moveDir = moveDir;
        }

        float interactDist = 7f;

        // Ray 
        Ray ray = new Ray(transform.position,prev_moveDir*interactDist);
        Debug.DrawRay(transform.position, moveDir * interactDist, Color.red);
        RaycastHit hitInfo = new RaycastHit();

        if(Physics.Raycast(ray,out hitInfo, interactDist))
        {
            if(hitInfo.collider.tag == "clearCounter")
            {
                ClearCounter clearCounter = hitInfo.collider.GetComponent<ClearCounter>();
                if(selectedCounter != clearCounter)
                {
                    selectedCounter = clearCounter;
                    OnCounterChange(this, EventArgs.Empty);
                    //clearCounter.Interact();
                }
            }
            else
            {
                OnCounterChange(this, EventArgs.Empty);
                selectedCounter = null;
                Debug.Log("-");
            }
        }
        else
        {
            OnCounterChange(this, EventArgs.Empty);
            selectedCounter = null;
            Debug.Log("-");
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalised();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            prev_moveDir = moveDir;
        }

        float playerSize = 0.7f;
        float playerHeight = 2f;

        //Debug.DrawRay(transform.position, moveDir * playerSize, Color.red);

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerSize / 2, moveDir, playerSize);

        if (canMove == false)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f);
            Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z);

            if (!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerSize / 2, moveDirX, playerSize))
            {
                // can move in X
                moveDir = moveDirX.normalized;
            }
            else if (!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerSize / 2, moveDirZ, playerSize))
            {
                // can move in Z
                moveDir = moveDirZ.normalized;
            }
            else
            {
                moveDir = Vector3.zero;
            }

            transform.position += moveDir * Time.deltaTime * moveSpeed;
        }

        if (canMove)
        {
            transform.position += moveDir * Time.deltaTime * moveSpeed;
        }

        transform.forward = Vector3.Slerp(transform.forward, prev_moveDir, Time.deltaTime);

        Debug.Log(inputVector);

        if(moveDir != Vector3.zero)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void Pickup()
    {
        //if (Input.GetKeyDown(KeyCode.P) && selectedCounter!=null)   // Pick Up

        if (selectedCounter != null)  // Pick Up (Android Touch Input)

        {
            GameObject kitchenObj = selectedCounter.gameObjectOnCounter();

            if (kitchenObj != null && hasChild == false)        // pick up KO when player is not holding anything
            {
                Debug.Log("Pick up !");

                OnPickup?.Invoke(this, EventArgs.Empty);    // for playing the sound

                // player will become the parent of gameobj
                kitchenObj.transform.parent = anchorPont.transform;
                kitchenObj.transform.localPosition = Vector3.zero;
                hasChild = true;
                selectedCounter.setGameObject_null(); // so we can spawn new KO on the counter
            }

            if(selectedCounter.isPlatesCounter_Fun() == true && hasChild == false)  // pick up plate from plates counter when player is holding nothing
            {
                Debug.Log("Plate Pick up !");

                OnPickup?.Invoke(this, EventArgs.Empty);    // for playing the sound

                GameObject topPlate = platesCounter.Plate_GO();   // reference to top plate GO


                topPlate.transform.parent = anchorPont.transform;
                topPlate.transform.localPosition = Vector3.zero;
                hasChild = true;
            }

            if(kitchenObj != null && hasChild == true && anchorPont.transform.GetChild(0).CompareTag("Plate")) // if player is holding a plate, than keep the GO on plate
            {
                Plate plate = anchorPont.transform.GetChild(0).GetComponent<Plate>();

                Try_Add_GO_toPlate(kitchenObj, plate);

                selectedCounter.setGameObject_null(); // so we can spawn new KO on the counter
            }

        }
    }

    private void Drop()
    {
        //if (Input.GetKeyDown(KeyCode.L) && selectedCounter != null)    // Drop

        if(selectedCounter != null)    // Pick Up (Android Touch Input)
        {
            GameObject kitchenObj = selectedCounter.gameObjectOnCounter();

            GameObject player_holding = null;
            if (hasChild == true)    // only assign if player has a child
            {
                player_holding = anchorPont.transform.GetChild(0).gameObject;   // GO that player is holding
            }

            if (kitchenObj == null && hasChild == true)  // if there is no KO on counter and player is holding something
            {
                if (selectedCounter.isClearCounter_Fun() == true || selectedCounter.isCuttingCounter_Fun() == true || selectedCounter.isStoveCounter_Fun() == true)
                // we can keep GO on above counters only
                {
                    Debug.Log("can Keep !");

                    OnDrop?.Invoke(this, EventArgs.Empty);    // for playing the sound

                    hasChild = false;

                    anchorPont.transform.DetachChildren();  // detach the child of player(anchor point)

                    // change parent
                    player_holding.transform.parent = selectedCounter.counterTop_point().transform;
                    player_holding.transform.localPosition = Vector3.zero;
                    selectedCounter.setGameObj_onCounter(player_holding);  // so we cannot spawn as there is already obj on counter
                }

                if (selectedCounter.isTrash_Fun() == true)   // if it is trash than destroy the GO
                {
                    Debug.Log("Destroy !!");

                    OnTrashed?.Invoke(this, EventArgs.Empty);

                    hasChild = false;
                    anchorPont.transform.DetachChildren();  // detach the child of player(anchor point)

                    // destroy 
                    Destroy(player_holding);
                }

            }

            if (hasChild == true && kitchenObj != null && kitchenObj.CompareTag("Plate"))   // if player is holding something and a plate is kept on counter
            {
                if (player_holding.CompareTag("Plate"))
                {
                    return;
                }

                Plate plate = kitchenObj.GetComponent<Plate>();

                bool b = Try_Add_GO_toPlate(player_holding, plate);

                if (b == true)
                {
                    hasChild = false;
                    anchorPont.transform.DetachChildren();
                }
            }

            if (hasChild == true && player_holding.CompareTag("Plate"))               // Delivery Counter
            {
                Destroy(player_holding);
                hasChild = false;
                anchorPont.transform.DetachChildren();

                deliveryManager.DeliverRecipe(player_holding.GetComponent<Plate>().Get_KO_onPlate_list_Fun());   // call the deliver recipe function and
                                                                                                                 //  pass the KO_SOs the plate contained
            }
        }
    }


    // Function to add GO to Plate
    private bool Try_Add_GO_toPlate(GameObject GO, Plate plate)
    {
      
        List<KitchenObjectSO> valid_KOs = plate.Valid_KO_list_Fun();
        List<KitchenObjectSO> KOs_onPlate = plate.Get_KO_onPlate_list_Fun();

        int n = KOs_onPlate.Count;


        for (int i = 0; i < n; i++)        // check if we are not keeping 2 KOs of the same type on plate
        {
            if (KOs_onPlate[i].prefab.CompareTag(GO.tag))
            {
                Debug.Log("Already Kept on Plate");
                return false;
            }
        }


        int m = valid_KOs.Count;

        Debug.Log("m (valid KOs) = " + m);

        for (int i = 0; i < m; i++)     // keep valid KO on plate
        {
            if (GO.CompareTag(valid_KOs[i].prefab.tag))
            {

                Debug.Log("valid GO");

                KitchenObject kitchenObject_Script = GO.GetComponent<KitchenObject>();       // Get the KitchenObjectSO of KitchenObject
                plate.Add_KO_onPlate_Fun(kitchenObject_Script.KitchenObjectSO);

                Debug.Log(GO + " added to plate ");

                Destroy(GO);     // destroy the KO after it is added to the KOs_onPlate list

                return true;
            }
        }
        return false;
    }
}
