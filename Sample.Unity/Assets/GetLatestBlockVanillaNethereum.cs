using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using Nethereum.Web3;
using System.Threading.Tasks;
using Nethereum.Unity.Rpc;

public class GetLatestBlockVanillaNethereum : MonoBehaviour {

    private static bool TrustCertificate(object sender, X509Certificate x509Certificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors)
    {
        // all certificates are accepted
        return true;
    }

    public string Url = "http://localhost:8545";
    public InputField ResultBlockNumber;
    public InputField ResultBlockNumberConstant;
    
    public InputField InputUrl;

    // Use this for initialization
    async void Start()
    {
        InputUrl.text = Url;
        await CheckBlockNumberPeriodically();
    }
    public async Task CheckBlockNumberPeriodically()
    {
        var wait = 1000;
        while (true)
        {
           await Task.Delay(wait);
           wait = 1000;
           var web3 = new Web3(new UnityWebRequestRpcTaskClient(new Uri(InputUrl.text)));
            
           var blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
           ResultBlockNumberConstant.text = blockNumber.Value.ToString();
        }
    }

    public async void GetBlockNumber()
	{
        Url = InputUrl.text;
        //This is to workaround issue with certificates https://forum.unity.com/threads/how-to-allow-self-signed-certificate.522183/
        //Uncomment if needed
        //ServicePointManager.ServerCertificateValidationCallback = TrustCertificate;
        var web3 = new Web3(new UnityWebRequestRpcTaskClient(new Uri(InputUrl.text)));
        var blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
        ResultBlockNumber.text = blockNumber.Value.ToString();
    }

   
    // Update is called once per frame
    void Update ()
    {
        
    }


}






