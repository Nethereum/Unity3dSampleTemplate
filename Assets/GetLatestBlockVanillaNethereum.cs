using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Nethereum.ABI.FunctionEncoding.Attributes;
using UnityEngine;
using UnityEngine.UI;
using Nethereum.Contracts;
using Nethereum.Web3;
using Nethereum.JsonRpc.Client;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using static Nethereum.JsonRpc.Client.UserAuthentication;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Nethereum.JsonRpc.Client.RpcMessages;
using Nethereum.Unity.Rpc;
using System.Runtime.CompilerServices;
using AOT;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC;
using Nethereum.Unity.Metamask;
using System.Collections.Concurrent;
using Nethereum.Metamask;

public class GetLatestBlockVanillaNethereum : MonoBehaviour {

    private static bool TrustCertificate(object sender, X509Certificate x509Certificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors)
    {
        // all certificates are accepted
        return true;
    }

    public string Url = "http://localhost:8545";
    public string UrlFull = "https://mainnet.infura.io/v3/7238211010344719ad14a89db874158c";

    public InputField ResultBlockNumber;
    public InputField InputUrl;
    private MetamaskHostProvider metamaskWebGlHostProvider;

    // Use this for initialization
    async void Start()
    {
        InputUrl.text = Url;
        await CheckBlockNumber();
        if(MetamaskWebglHostProvider.Current == null)
        {
            metamaskWebGlHostProvider = new MetamaskWebglHostProvider();
        }
        else
        {
            metamaskWebGlHostProvider = (MetamaskHostProvider)MetamaskWebglHostProvider.Current;
        }
    }
    public async Task CheckBlockNumber()
    {
        var wait = 1000;
        while (true)
        {
           await Task.Delay(wait);
            wait = 1000;
            var web3 = new Web3(new UnityWebRequestRpcTaskClient(new Uri(InputUrl.text)));
            
            var blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            ResultBlockNumber.text = blockNumber.Value.ToString();
        }
    }

    public async void GetBlockNumber()
	{
        Url = InputUrl.text;
        //This is to workaround issue with certificates https://forum.unity.com/threads/how-to-allow-self-signed-certificate.522183/
        //Uncomment if needed
        //ServicePointManager.ServerCertificateValidationCallback = TrustCertificate;
        var web3 = await metamaskWebGlHostProvider.GetWeb3Async();

        var blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
        ResultBlockNumber.text = blockNumber.Value.ToString();
    }

   
    // Update is called once per frame
    void Update ()
    {
        
    }


}






