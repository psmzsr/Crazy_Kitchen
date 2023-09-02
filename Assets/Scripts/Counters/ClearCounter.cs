using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;


    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //û����Ʒ������
            if (player.HasKitchenObject())
            {
                //���Я����Ʒ
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                //���û����ʲô����
            }
        }
        else
        {
            //����Ʒ�����⡣
            if (player.HasKitchenObject())
            {
                //���Я����Ʒ
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //���Я������һ������
                   if( plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                   {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    //����õĲ������ӵ������ű�Ķ���
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        //��̨������
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                //���û��Я������
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }


}
