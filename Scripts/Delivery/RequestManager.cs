using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestManager : MonoBehaviour
{
    public RequestTemplate request;

    public ItemProduct[] allItemP;

    public GameObject[] uiPannels;
    public GameObject[] requestManagerGO;

    public bool fullyHidden;
    public bool hidden;

    public GameObject requestGOui;

    public int maxItemAmount;
    public int minItemAmount;

    public bool twoItems;

    public float requestTime = 100f;

    public int numberdRequest;

    public int baseWorthForItem;

    public int itemWorthOne;
    public int itemWorthTwo;
    public int requestWorth;

    public GameObject shopManagerGO;

    public GameObject playerCam;

    public GameObject randomPointChoose;

    private void Start()
    {
        hidden = true;

        CheckIfAcceptable(0);
        CheckIfAcceptable(1);
        CheckIfAcceptable(2);

        RandomItemSelector(0);
    }

    public void Update()
    {
        HideRequestUI();
    }

    /// <summary>
    /// Calculates the reward you will get for the request
    /// </summary>
    /// <param name="requestNumberButton"></param>
    public void RewardCalculations(int requestNumberButton)
    {
        CheckIfAcceptable(0);
        CheckIfAcceptable(1);
        CheckIfAcceptable(2);

        baseWorthForItem = uiPannels[requestNumberButton].GetComponent<RequestSlot>().itemOne.waarde;

        baseWorthForItem *= uiPannels[requestNumberButton].GetComponent<RequestSlot>().itemOneAmount;

        itemWorthOne = baseWorthForItem;

        requestWorth = itemWorthOne;

        print(requestWorth);

        if (uiPannels[requestNumberButton].GetComponent<RequestSlot>().doubleOrder == true)
        {
            baseWorthForItem = uiPannels[requestNumberButton].GetComponent<RequestSlot>().itemTwo.waarde;

            baseWorthForItem *= uiPannels[requestNumberButton].GetComponent<RequestSlot>().itemTwoAmount;

            itemWorthTwo = baseWorthForItem;

            requestWorth = itemWorthOne += itemWorthTwo;

            print(requestWorth);

            uiPannels[requestNumberButton].GetComponent<RequestSlot>().FillRewardText(requestWorth);

        }
        else
        {
            uiPannels[requestNumberButton].GetComponent<RequestSlot>().FillRewardText(requestWorth);
        }
    }

    /// <summary>
    /// Randomly selects an item to add to a new quest
    /// </summary>
    /// <param name="slotIndex"></param>
    public void RandomItemSelector(int slotIndex)
    {
        uiPannels[slotIndex].GetComponent<RequestSlot>().requestNumber = slotIndex;
        if (twoItems == false)
        {
            uiPannels[slotIndex].GetComponent<RequestSlot>().itemTwo = null;
            uiPannels[slotIndex].GetComponent<RequestSlot>().itemTwoAmount = 0;
        }

        int newRandomTwoItems = Random.Range(0, 2);
        if (newRandomTwoItems == 0)
        {
            twoItems = true;
        }

        int newRandomItem = Random.Range(0, allItemP.Length);
        int newRandomAmount = Random.Range(minItemAmount, maxItemAmount);

        uiPannels[slotIndex].GetComponent<RequestSlot>().itemOne = allItemP[newRandomItem];
        uiPannels[slotIndex].GetComponent<RequestSlot>().itemOneAmount = newRandomAmount;

        uiPannels[numberdRequest].SetActive(true);

        if (twoItems == true)
        {
            int newRandomItemTwo = Random.Range(0, allItemP.Length);
            int newRandomAmountTwo = Random.Range(minItemAmount, maxItemAmount);
            if (newRandomItemTwo != newRandomItem)
            {
                uiPannels[slotIndex].GetComponent<RequestSlot>().itemTwo = allItemP[newRandomItemTwo];
                uiPannels[slotIndex].GetComponent<RequestSlot>().itemTwoAmount = newRandomAmountTwo;

                uiPannels[numberdRequest].SetActive(true);
            }
        }

        twoItems = false;

        CheckIfAcceptable(0);
        CheckIfAcceptable(1);
        CheckIfAcceptable(2);

        RewardCalculations(numberdRequest);
        StartCoroutine(MakeNewRequestOverTime());
    }

    /// <summary>
    /// Delete request 
    /// </summary>
    /// <param name="requestButtonNumber"></param>
    public void DeleteRequest(int requestButtonNumber)
    {
        numberdRequest = requestButtonNumber;

        uiPannels[numberdRequest].SetActive(false);

        uiPannels[requestButtonNumber].GetComponent<RequestSlot>().itemOne = null;

        uiPannels[requestButtonNumber].GetComponent<RequestSlot>().itemTwo = null;

        uiPannels[requestButtonNumber].GetComponent<RequestSlot>().itemOneAmount = 0;

        uiPannels[requestButtonNumber].GetComponent<RequestSlot>().itemTwoAmount = 0;

        uiPannels[requestButtonNumber].GetComponent<RequestSlot>().doubleOrder = false;

        uiPannels[requestButtonNumber].GetComponent<RequestSlot>().AcceptRequest(false);

        StartCoroutine(MakeNewRequestOverTime());
    }

    /// <summary>
    /// Hide the ui for the request panel
    /// </summary>
    public void HideRequestUI()
    {
        if (Input.GetButtonDown("Tab"))
        {
            if (hidden == true)
            {
                requestManagerGO[0].SetActive(true);
                requestManagerGO[1].SetActive(true);

                Cursor.lockState = CursorLockMode.None;
                playerCam.GetComponent<Cams>().inMenu = true;

                hidden = false;
                return;
            }
            else if (hidden == false)
            {
                requestManagerGO[0].SetActive(false);
                requestManagerGO[1].SetActive(false);

                Cursor.lockState = CursorLockMode.Locked;

                playerCam.GetComponent<Cams>().inMenu = false;

                hidden = true;
                return;
            }
        }
    }

    /// <summary>
    /// Get rewarded for the completed request
    /// </summary>
    /// <param name="acceptButtonNumber"></param>
    public void GiveMoney(int acceptButtonNumber)
    {
        shopManagerGO.GetComponent<ShopManager>().AddCoins(uiPannels[acceptButtonNumber].GetComponent<RequestSlot>().moneyForthisRequest);

        DeleteRequest(acceptButtonNumber);
    }

    /// <summary>
    /// Check if the request is acceptable
    /// </summary>
    /// <param name="requestNumber"></param>
    public void CheckIfAcceptable(int requestNumber)
    {
        for (int i = 0; i < GetComponent<InventoryManager>().inventorySlots.Length; i++)
        {
            if (uiPannels[requestNumber].GetComponent<RequestSlot>().itemOne == GetComponent<InventoryManager>().inventorySlots[i].itemP)
            {
                if (uiPannels[requestNumber].GetComponent<RequestSlot>().itemOneAmount <= GetComponent<InventoryManager>().inventorySlots[i].itemAmount)
                {
                    uiPannels[requestNumber].GetComponent<RequestSlot>().AcceptRequest(true);

                    if (uiPannels[requestNumber].GetComponent<RequestSlot>().doubleOrder == true)
                    {
                        for (int y = 0; y < GetComponent<InventoryManager>().inventorySlots.Length; y++)
                        {
                            if (uiPannels[requestNumber].GetComponent<RequestSlot>().itemTwo == GetComponent<InventoryManager>().inventorySlots[y].itemP)
                            {
                                if ((uiPannels[requestNumber].GetComponent<RequestSlot>().itemTwoAmount <= GetComponent<InventoryManager>().inventorySlots[y].itemAmount))
                                {
                                    uiPannels[requestNumber].GetComponent<RequestSlot>().AcceptRequest(true);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Accept a request and remove the required items for the request
    /// </summary>
    /// <param name="buttonnumberindex"></param>
    public void AcceptRequest(int buttonnumberindex)
    {
        randomPointChoose.GetComponent<RandomPointChoose>().PickPoints();
        randomPointChoose.GetComponent<RandomPointChoose>().indexNumber = buttonnumberindex;

        for (int i = 0; i < GetComponent<InventoryManager>().inventorySlots.Length; i++)
        {
            if (uiPannels[buttonnumberindex].GetComponent<RequestSlot>().itemOne == GetComponent<InventoryManager>().inventorySlots[i].itemP)
            {
                int b = uiPannels[buttonnumberindex].GetComponent<RequestSlot>().itemOneAmount;

                GetComponent<InventoryManager>().slotIndex = i;
                GetComponent<InventoryManager>().RemoveItem(b);
            }
        }
        if (uiPannels[buttonnumberindex].GetComponent<RequestSlot>().doubleOrder == true)
        {
            for (int i = 0; i < GetComponent<InventoryManager>().inventorySlots.Length; i++)
            {

                if (uiPannels[buttonnumberindex].GetComponent<RequestSlot>().itemTwo == GetComponent<InventoryManager>().inventorySlots[i].itemP)
                {

                    int b = uiPannels[buttonnumberindex].GetComponent<RequestSlot>().itemTwoAmount;

                    GetComponent<InventoryManager>().slotIndex = i;
                    GetComponent<InventoryManager>().RemoveItem(b);
                }
            }
        }

        uiPannels[0].GetComponent<RequestSlot>().AcceptRequest(false);
        uiPannels[1].GetComponent<RequestSlot>().AcceptRequest(false);
        uiPannels[2].GetComponent<RequestSlot>().AcceptRequest(false);

        CheckIfAcceptable(0);
        CheckIfAcceptable(1);
        CheckIfAcceptable(2);
    }

    /// <summary>
    ///  Create a new random requests after a random amount of time
    /// </summary>
    /// <returns></returns>
    public IEnumerator MakeNewRequestOverTime()
    {
        yield return new WaitForSeconds(requestTime);

        for (numberdRequest = 0; numberdRequest < uiPannels.Length; numberdRequest++)
        {
            if (uiPannels[numberdRequest].GetComponent<RequestSlot>().itemOne == null)
            {
                RandomItemSelector(numberdRequest);

                RewardCalculations(numberdRequest);
                yield return new WaitForSeconds(requestTime);
                requestTime = Random.Range(10, 15);
            }
        }
    }
}
