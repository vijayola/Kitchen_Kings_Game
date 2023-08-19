using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlatesCounter : MonoBehaviour
{
    [SerializeField] private ClearCounter clearCounter;

    [SerializeField] private KitchenObjectSO plateSO;
    [SerializeField] private GameObject platesCounterTopPoint;

    private List<GameObject> spawnedPlatesList;  // stores the plates which have spawned

    private void Awake()
    {
        spawnedPlatesList = new List<GameObject>();
    }
    private void Start()
    {
        clearCounter.OnPlateSpawned += ClearCounter_OnPlateSpawned;
    }

    private void ClearCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        GameObject newPlate = Instantiate(plateSO.prefab, platesCounterTopPoint.transform);
        spawnedPlatesList.Add(newPlate);

        Vector3 newPlatePos = new Vector3(0, 0.2f * spawnedPlatesList.Count, 0);  // new plates will be kept at greater height for visibility
        newPlate.transform.localPosition = newPlatePos;
    }

    public GameObject Plate_GO()   // return the top plate GO to player
    {
        int n = spawnedPlatesList.Count;

        if (n == 0)
        {
            return null;
        }

        clearCounter.ReducePlatesSpawned();    // function to reduce the number of plates spawned as top plate is picked up

        GameObject topPlate = spawnedPlatesList[n - 1];
        spawnedPlatesList.Remove(topPlate);

        return topPlate;
    }
}
