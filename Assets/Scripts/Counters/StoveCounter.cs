using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class StoveCounter : BaseCounter,IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangeEventArgs> OnProgressChanged;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }


    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burninggRecipeSOArray;


    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO buringRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }
    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Frying:
                fryingTimer += Time.deltaTime;

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
                {
                    progressNormalized = fryingTimer / fryingRecipeSO.fryingtimeMax
                });

                if (fryingTimer > fryingRecipeSO.fryingtimeMax)
                {
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitChenObject(fryingRecipeSO.output, this);
                    state = State.Fried;
                    burningTimer = 0f;
                    buringRecipeSO = GetBurningRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSO());

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state=state
                    });
                }
                break;
            case State.Fried:
                burningTimer += Time.deltaTime;

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
                {
                    progressNormalized = burningTimer / buringRecipeSO.buringtimeMax
                });

                if (burningTimer > buringRecipeSO.buringtimeMax)
                {
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitChenObject(buringRecipeSO.output, this);
                    state = State.Burned;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
                    {
                        progressNormalized = 0f
                    });

                }
                break;
            case State.Burned:
                break;
        }
    }

    public override void Interact(Player player)
    {

        if (!HasKitchenObject())
        {
            //û����Ʒ������
            if (player.HasKitchenObject())
            {
                //���Я����Ʒ
                if (HasRecipeWhithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    //���Я������Ʒ���Ա����
                    player.GetKitchenObject().SetKitchenObjectParent(this);


                     fryingRecipeSO = GetFryingRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Frying;
                    fryingTimer = 0f;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingtimeMax
                    });
                }
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

                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //���Я������һ������
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();

                        state = State.Idle;


                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                }
            }
            else
            {
                //���û��Я������
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;


                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    private bool HasRecipeWhithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSoWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSoWithInput(inputKitchenObjectSO);

        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GetFryingRecipeSoWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }
    private BurningRecipeSO GetBurningRecipeSoWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO buringRecipeSO in burninggRecipeSOArray)
        {
            if (buringRecipeSO.input == inputKitchenObjectSO)
            {
                return buringRecipeSO;
            }
        }
        return null;
    }


    public bool IsFried()
    {
        return state == State.Fried;
    }
}
