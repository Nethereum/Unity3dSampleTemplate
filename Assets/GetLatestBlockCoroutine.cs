using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;

public class GetLatestBlockCoroutine : MonoBehaviour
{
    public string Url = "https://mainnet.infura.io";

    public InputField ResultBlockNumber;
    public InputField InputUrl;

    // Use this for initialization
    void Start()
    {
        InputUrl.text = Url;
    }

    public void GetBlockNumberRequest()
    {
        StartCoroutine(GetBlockNumber());
    }

    public IEnumerator GetBlockNumber()
    {
        Url = InputUrl.text;

        var blockNumberRequest = new EthBlockNumberUnityRequest(Url);

        yield return blockNumberRequest.SendRequest();

        ResultBlockNumber.text = blockNumberRequest.Result.Value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
