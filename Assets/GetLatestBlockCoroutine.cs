using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Nethereum.Unity.Rpc;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using System;
using Nethereum.Web3;

public class GetLatestBlockCoroutine : MonoBehaviour
{
    public string Url = "http://localhost:8545";
    public string UrlFull = "https://mainnet.infura.io/v3/7238211010344719ad14a89db874158c";

    public InputField ResultBlockNumber;
    public InputField InputUrl;

    // Use this for initialization
    void Start()
    {
        InputUrl.text = Url;
    }

    public async void GetBlockNumberRequest()
    {
        Url = InputUrl.text;
        //This is to workaround issue with certificates https://forum.unity.com/threads/how-to-allow-self-signed-certificate.522183/
        //Uncomment if needed
        //ServicePointManager.ServerCertificateValidationCallback = TrustCertificate;
        var web3 = new Web3(new UnityWebRequestRpcTaskClient(new Uri(InputUrl.text)));

        var blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
        ResultBlockNumber.text = blockNumber.Value.ToString();

        //StartCoroutine(GetBlockNumber());
    }

    public IEnumerator GetBlockNumber()
    {

       var blockNumberRequest = new EthBlockNumberUnityRequest(InputUrl.text);
 
       yield return blockNumberRequest.SendRequest();

        if (blockNumberRequest.Exception != null)
        {
            UnityEngine.Debug.Log(blockNumberRequest.Exception.Message);
        }
        else
        {
            ResultBlockNumber.text = blockNumberRequest.Result.Value.ToString();
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
