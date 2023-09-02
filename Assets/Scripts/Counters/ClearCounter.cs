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
            //没有物品放在这
            if (player.HasKitchenObject())
            {
                //玩家携带物品
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                //玩家没有拿什么东西
            }
        }
        else
        {
            //有物品放在这。
            if (player.HasKitchenObject())
            {
                //玩家携带物品
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //玩家携带的是一个盘子
                   if( plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                   {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    //玩家拿的不是盘子但是拿着别的东西
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        //柜台有盘子
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                //玩家没有携带东西
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }


}
