using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private GameObject counterTop;    

    [SerializeField] private bool isClearCounter;
    [SerializeField] private bool isContainerCounter;
    [SerializeField] private bool isTrash;
    [SerializeField] private bool isCuttingCounter;
    [SerializeField] private bool isStoveCounter;
    [SerializeField] private bool isPlatesCounter;
    [SerializeField] private bool isDeliveryCounter;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSO_Array;
    [SerializeField] private FryingRecipeSO[] fryingRecipeSO_Array;

    //public event EventHandler OnContainerCounterSpawnObj;  not working (don't know why, so I will just call the function)
    [SerializeField] private ContainerCounterAnimator containerCounterAnim;

    private GameObject kitchenObject;

    // for cutting counter
    private int maxCuttingProgress;
    private int currentProgress;

    [SerializeField] private ProgressBarUI progressBarUI;  // for accessing the ProgressbarUI script

    [SerializeField] private CuttingCounterAnimator cuttingCounterAnim;

    [SerializeField] private StoveCounterVisual StoveCounterVisual;

    // for stove counter
    private float currentTime;

    public event EventHandler OnStateChange;
    
    public bool isCooking;
    private bool hasFried;
    private bool hasBurned;

    // for plate counter
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawned;
    private int platesSpawnedMax = 4;

    public event EventHandler OnPlateSpawned;


    [SerializeField] private AudioClipsSO audioClipsSO;

    private void Start()
    {
        currentProgress = 0;   // for cutting counter
     
        // for stove counter
        currentTime = 0;

        isCooking = false;
        hasFried = false;
        hasBurned = false;

        // for plate counter
        spawnPlateTimer = 0;
        platesSpawned = 0;
    }
    private void Update()
    {
        if(KitchenGameManager.Instance.IsPlaying() == false)   // do not do anything if player is not playing
        {
            return;
        }

        // Plates Counter Code
        
        if (platesSpawned < platesSpawnedMax)
        {
            spawnPlateTimer += Time.deltaTime;
            if (spawnPlateTimer >= spawnPlateTimerMax)
            {

                platesSpawned++;

                // spawn plates on top of plates counter
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);

                spawnPlateTimer = 0;
            }
        }


         // Stove Counter Code
        if(isStoveCounter == true && kitchenObject != null)
        {
            if (kitchenObject.CompareTag(fryingRecipeSO_Array[0].input.prefab.tag) || kitchenObject.CompareTag(fryingRecipeSO_Array[0].friedOutput.prefab.tag)) 
                // check if the GO on stove is either uncooked or fried
            {
                float fryingTime = fryingRecipeSO_Array[0].fryTime;
                float burnedTime = fryingRecipeSO_Array[0].burnTime;

                currentTime += Time.deltaTime;


                if(isCooking == false)
                {
                    isCooking = true;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                

                Debug.Log("current Time : " + currentTime);

                StoveCounterVisual.Visuals_ON();  // turn on the visuals (particle effect and stove glow)

                progressBarUI.Show();   // show progress bar

                if (hasFried == false)
                {
                    progressBarUI.progress_Fun(currentTime / fryingTime);
                }

                if(currentTime >= fryingTime && hasFried == false)    // after fryingTime destroy the uncooked patty and spawn fried meat patty
                {
                    Debug.Log("fried !!");

                    Destroy(kitchenObject);
                    kitchenObject = Instantiate(fryingRecipeSO_Array[0].friedOutput.prefab, counterTop.transform);

                    hasFried = true;
                }

                if (hasFried == true)
                {
                    progressBarUI.progress_Fun((currentTime - fryingTime) / (burnedTime - fryingTime));
                }

                if(currentTime >= burnedTime && hasBurned == false)   // after burnedTime destroy fried patty and spawn burned meat patty 
                {
                    Debug.Log("burned !!");

                    isCooking = false;

                    OnStateChange?.Invoke(this, EventArgs.Empty);

                    Destroy(kitchenObject);
                    kitchenObject = Instantiate(fryingRecipeSO_Array[0].burnedOutput.prefab, counterTop.transform);

                    hasBurned = true;

                    StoveCounterVisual.Visuals_OFF();
                    progressBarUI.Hide();
                }

            }
        }
        if(isStoveCounter == true && kitchenObject == null) // it is either not stove counter or there is no KO kept on stove counter, so we turn off the visuals 
        {
            currentTime = 0;
            StoveCounterVisual.Visuals_OFF();
            progressBarUI.Hide();

            hasFried = false;
            hasBurned = false;
        }
    }
    public void Interact()
    {
        if (KitchenGameManager.Instance.IsPlaying() == false)   // do not do anything if player is not playing
        {
            return;
        }

        // Container Counter Code
        if (kitchenObject == null && isContainerCounter == true)
        {
            Debug.Log("interact !!");

            kitchenObject = Instantiate(kitchenObjectSO.prefab, counterTop.transform);
            
            // play the "OpenClose" animation when the object is spawned from a Container Counter
            containerCounterAnim.PlayAnimation();

            //kitchenObject = kitchenObjectSO.prefab.GetComponent<KitchenObject>();

            Debug.Log(kitchenObjectSO.name);
        }


        // Cutting Counter Code
        if(kitchenObject != null && isCuttingCounter == true)
        {
            int n = cuttingRecipeSO_Array.Length;
            for(int i = 0; i < n; i++)
            {
                if (kitchenObject.CompareTag(cuttingRecipeSO_Array[i].input.prefab.tag))  // used tags to compare different GO(game objects)             
                {
                    maxCuttingProgress = cuttingRecipeSO_Array[i].cuttingTime;
                    if (currentProgress >= maxCuttingProgress)
                    {
                        progressBarUI.Hide(); // hide the progress bar when the GO is cut

                        currentProgress = 0;

                        Destroy(kitchenObject); // destroyed the unsliced KO
                        kitchenObject = Instantiate(cuttingRecipeSO_Array[i].output.prefab, counterTop.transform); // instantiated sliced KO
                        break;
                    }
                    else
                    {
                        currentProgress++;
                        progressBarUI.Show(); // start showing thr progress bar
                        cuttingCounterAnim.PlayAnimation(); // play animation
                        progressBarUI.progress_Fun(((float)currentProgress/maxCuttingProgress));

                        // cutting sound
                        SoundManager.instance.PlaySound(audioClipsSO.chop, counterTop.transform.position, SoundManager.instance.GetVolume());
                    }
                }
            }
        }
    }

    public GameObject gameObjectOnCounter()
    {
        return kitchenObject;
    }

    public void setGameObject_null()
    {
        kitchenObject = null;
    }

    public GameObject counterTop_point()
    {
        return counterTop;
    }

    public void setGameObj_onCounter(GameObject obj)
    {
        kitchenObject = obj;
    }


    // check which counter is it
    public bool isTrash_Fun()
    {
        return isTrash;
    }

    public bool isClearCounter_Fun()
    {
        return isClearCounter;
    }
    public bool isContainerCounter_Fun()
    {
        return isContainerCounter;
    }

    public bool isCuttingCounter_Fun()
    {
        return isCuttingCounter;    
    }

    public bool isStoveCounter_Fun()
    {
        return isStoveCounter;
    }

    public bool isPlatesCounter_Fun()
    {
        return isPlatesCounter;
    }

    public bool isDeliveryCounter_Fun()
    {
        return isDeliveryCounter;
    }

    //

    public void ReducePlatesSpawned()
    {
        platesSpawned--;
    }


    public Vector3 TopPoint()
    {
        return counterTop.transform.position;
    }
}