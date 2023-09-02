using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO KitchenObjectSO;


    private IKitchenObjectparent kitchenObjectParent;


    public KitchenObjectSO GetKitchenObjectSO()
    {
        return KitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectparent KitchenObjectParent)
    {
        if(this.kitchenObjectParent!= null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        this.kitchenObjectParent = KitchenObjectParent;
        
        if (KitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("KitchenObjectParent already has a KintchenObject!");
        }

        KitchenObjectParent.SetKitchenObject(this);

        

        transform.parent = KitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectparent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }
    public void DestroySelf()
    {kitchenObjectParent.ClearKitchenObject();

        Destroy(gameObject);
    }


    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }




    public static KitchenObject SpawnKitChenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectparent kitchenObjectparent)
    {
        Transform kitchenObjectTransfrorm = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransfrorm.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectparent);
        return kitchenObject;
    }
}
