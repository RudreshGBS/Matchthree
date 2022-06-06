using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.JsonRpc.UnityClient;
using System;
using System.Collections;
using System.Linq;
using System.Numerics;
using TMPro;
using UnityEngine;
using WalletConnectSharp.Unity;

public class GetBalance : WalletConnectActions
{
    public TMP_Text resultText;
    public TMP_Text accountText;
    public int bal;
    public Action<string> onGettingBalance;
    public Action<string> onGettingDB;

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunction : FunctionMessage
    {
        [Parameter("address", "_owner", 1)]
        public string Owner { get; set; }
    }

    [FunctionOutput]
    public class BalanceOfFunctionOutput : IFunctionOutputDTO
    {
        [Parameter("uint256", 1)]
        public BigInteger Balance { get; set; }
    }

    // Start is called before the first frame update
    void OnEnable()
    {
    }

    public void Balance()
    {
        GameDataStore.WalletId = WalletConnect.ActiveSession.Accounts[0];
        accountText.text = $"ID:{WalletConnect.ActiveSession.Accounts[0]}";
      
        StartCoroutine(getAccountBalance(WalletConnect.ActiveSession.Accounts[0], (balance) =>
        {
            // When the callback is called, we are just going print the balance of the account
            bal = Mathf.FloorToInt((float)balance);
            GameDataStore.WalletBal = bal;
            resultText.text = $"your account balance is  :{bal}";
            resultText.gameObject.SetActive(true);
            Debug.Log(balance);
            var id = WalletConnect.ActiveSession.Accounts[0].ToString();
            onGettingBalance?.Invoke(id);
        }));

    }

  

    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    if (WalletConnect.ActiveSession.Accounts == null)
    //        return;

    //    accountText.text = "\nConnected to Chain " + WalletConnect.ActiveSession.ChainId + ":\n" + WalletConnect.ActiveSession.Accounts[0];
    //}


    public void OnClickDisconnectAndConnect()
    {
        bool shouldConnect = !WalletConnect.Instance.createNewSessionOnSessionDisconnect;
        CloseSession(shouldConnect);
    }

    public static IEnumerator getAccountBalance(string address, System.Action<decimal> callback)
    {


        var balanceOfMessage = new BalanceOfFunction();
        balanceOfMessage.Owner = address;

        var queryRequest = new QueryUnityRequest<BalanceOfFunction, BalanceOfFunctionOutput>("https://bsc-dataseed.binance.org/", address);
        yield return queryRequest.Query(balanceOfMessage, "0x2ca25319e2e63719f87221d8bf3646f8f5de5ded");

        var dtoResult = queryRequest.Result;
        callback(Nethereum.Util.UnitConversion.Convert.FromWei(dtoResult.Balance, 9));


    }
}
