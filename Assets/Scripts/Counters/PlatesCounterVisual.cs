using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform PlateVisualPrefab;


    private List<GameObject> plateVisualGmaeObjectList;

    private void Awake()
    {
        plateVisualGmaeObjectList = new List<GameObject>();
    }
    private void Start()
    {
        platesCounter.OnplateSpawned += PlatesCounter_OnplateSpawned;
        platesCounter.OnplateRemoved += PlatesCounter_OnplateRemoved;
    }

    private void PlatesCounter_OnplateRemoved(object sender, System.EventArgs e)
    {
        GameObject plateGameObject = plateVisualGmaeObjectList[plateVisualGmaeObjectList.Count-1];
        plateVisualGmaeObjectList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }

    private void PlatesCounter_OnplateSpawned(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(PlateVisualPrefab, counterTopPoint);

        float plateOffsetY = .1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGmaeObjectList.Count, 0);

        plateVisualGmaeObjectList.Add(plateVisualTransform.gameObject);
    }

   
}
