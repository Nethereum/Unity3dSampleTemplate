using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Nethereum.ABI.FunctionEncoding.Attributes;
using UnityEngine;
using Nethereum.Contracts;
using Nethereum.Web3;


public class DecodeData : MonoBehaviour {

   private Web3 _web3;

    private static bool TrustCertificate(object sender, X509Certificate x509Certificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors)
    {
        // all certificates are accepted
        return true;
    }

    void Start ()
	{
        //This is to workaround issue with certificates https://forum.unity.com/threads/how-to-allow-self-signed-certificate.522183/

        ServicePointManager.ServerCertificateValidationCallback = TrustCertificate;
  // Note: in this sample, a special INFURA API key is used: `7238211010344719ad14a89db874158c`. If you wish to use this sample in your own project you’ll need to [sign up on INFURA](https://infura.io/register) and use your own key.
        _web3 = new Web3("https://
goerli.infura.io/v3/7238211010344719ad14a89db874158c
"); //defaults to http://localhost:8545
        //_web3 = new Web3(); //defaults to http://localhost:8545
	}

   
    // Update is called once per frame
    void Update ()
    {
        Debug.Log(_web3.Eth.Blocks.GetBlockNumber.SendRequestAsync().Result.Value.ToString());
    }
}
