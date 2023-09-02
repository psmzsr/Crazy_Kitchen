using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DeliveryManager : MonoBehaviour
{

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;


    public static DeliveryManager Instance { get; private set; }


    [SerializeField] private RecipeListSO recipeListSO;


    private List<RecipeSO> waitingRecipeSOList;
    private float spwanRecipeTimer;
    private float spwanRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    private int successfulRecipesAmount;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }
    private void Update()
    {
        spwanRecipeTimer -= Time.deltaTime;
        if (spwanRecipeTimer <= 0f)
        {
            spwanRecipeTimer = spwanRecipeTimerMax;
            if (KitchenGameManager.Instance.IsGamePlaying()&& waitingRecipeSOList.Count < waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                Debug.Log(waitingRecipeSO.recipeName);
                waitingRecipeSOList.Add(waitingRecipeSO);


                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
       
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
       for(int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {//有相同的数量

                bool plateCountentsMatchRecope = true;

                foreach(KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {//循环遍历菜谱成分

                    bool ingredientFound = false;

                    foreach(KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {//循环遍历盘子中的成分
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            //匹配成功
                            ingredientFound = true;
                            break;

                        }
                    }
                    if (!ingredientFound)
                    {
                        //盘子里没有菜谱中的东西
                        plateCountentsMatchRecope = false;
                    }
                }
                if (plateCountentsMatchRecope)
                {//玩家提交了正确的食品
                    //Debug.Log("玩家提交了正确的食品");
                    successfulRecipesAmount++;

                    waitingRecipeSOList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this,EventArgs.Empty);
                    return;
                }
            }
        }

        //没有匹配成功
        //玩家没有提供正确的实物
        //Debug.Log("玩家没有提供正确的实物");
        OnRecipeFailed?.Invoke(this,EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }
    public int GetsuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }
}
