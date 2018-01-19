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

    void Start ()
	{
        //code snippet for ssl connections
	    ServicePointManager.ServerCertificateValidationCallback += delegate (object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
	        if (sslPolicyErrors == SslPolicyErrors.RemoteCertificateChainErrors)
	        {
	            foreach (X509ChainStatus status in chain.ChainStatus)
	            {
	                if (status.Status != X509ChainStatusFlags.PartialChain)
	                {
	                    return false;
	                }
	            }
	            return true;
	        }
	        return false;
	    };

        _web3 = new Web3("https://mainnet.infura.io"); //defaults to http://localhost:8545
	}

   
    // Update is called once per frame
    void Update ()
    {
        Debug.Log(_web3.Eth.Blocks.GetBlockNumber.SendRequestAsync().Result.Value.ToString());
    }
}
