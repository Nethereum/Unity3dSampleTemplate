using System;
using System.Collections;
using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;
using UnityEngine;
using Nethereum.Contracts;
using Nethereum.Web3;


public class DecodeData : MonoBehaviour {

   

    void Start ()
	{
	    
	}

   
    // Update is called once per frame
    void Update ()
    {

        var web3 = new Web3();
        Debug.Log(web3.Eth.Blocks.GetBlockNumber.SendRequestAsync().Result.Value.ToString());
    }
}
