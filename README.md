# Unity3d Simple Sample (Environment Template)

Simple sample for Unity3d development using Net461, it includes all the DLLs required in the asset folder.

Dlls: Nethereum, dependencies (BouncyCastle..) and System.Numerics.

The code just demonstrates:

* Output to the log the current BlockNumber using Unity.UI both in Async and coroutines.
* Ether transfer using Unity.UI and coroutines
* Smart contract deployment (ERC20), Transactions (Transfer) and Querying (Balance)


To run a local blockchain you can just use the preconfigured [testchains](https://github.com/Nethereum/Nethereum.Workbooks/tree/master/testchain/)

### BlockNumber query Async (vanilla Nethereum)
```csharp
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


public class GetLatestBlockVanillaNethereum : MonoBehaviour {

    private static bool TrustCertificate(object sender, X509Certificate x509Certificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors)
    {
        // all certificates are accepted
        return true;
    }

    public string Url = "https://mainnet.infura.io";
   
    public InputField ResultBlockNumber;
    public InputField InputUrl;

    // Use this for initialization
    void Start()
    {
        InputUrl.text = Url;
    }

    public async void GetBlockNumber()
	{
        Url = InputUrl.text;
        //This is to workaround issue with certificates https://forum.unity.com/threads/how-to-allow-self-signed-certificate.522183/
        //Uncomment if needed
        // ServicePointManager.ServerCertificateValidationCallback = TrustCertificate;
        var web3 = new Web3(Url);
        
        var blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
        ResultBlockNumber.text = blockNumber.Value.ToString();
    }
}
```
### BlockNumber query Coroutines
```csharp
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

    public async void GetBlockNumberRequest()
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
}

```




