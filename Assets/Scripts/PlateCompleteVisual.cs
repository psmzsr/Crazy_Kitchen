using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitcehnObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitcehnObjectSO_GameObject> kitcehnObjectSO_GameObjectList;

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
        foreach (KitcehnObjectSO_GameObject kitcehnObjectSO_GameObject in kitcehnObjectSO_GameObjectList)
        {
                kitcehnObjectSO_GameObject.gameObject.SetActive(false);

        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach(KitcehnObjectSO_GameObject kitcehnObjectSO_GameObject in kitcehnObjectSO_GameObjectList)
        {
            if (kitcehnObjectSO_GameObject.kitchenObjectSO == e.kitchenObjectSO)
            {
                kitcehnObjectSO_GameObject.gameObject.SetActive(true);
            }
        }
    }
}
