using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (!player.HasKitchenObject())
            {
                //�����û��Я����Ʒ��״���»�����Ҷ���
                KitchenObject.SpawnKitChenObject(kitchenObjectSO, player);
                OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
            }
        }
    }

   
}
