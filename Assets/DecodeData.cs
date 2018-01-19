using System;
using System.Collections;
using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;
using UnityEngine;
using Nethereum.Contracts;
using Nethereum.Web3;


public class DecodeData : MonoBehaviour {

   private Web3 _web3;

    void Start ()
	{
	    _web3 = new Web3(); //defaults to http://localhost:8545
	}

   
    // Update is called once per frame
    void Update ()
    {
        Debug.Log(_web3.Eth.Blocks.GetBlockNumber.SendRequestAsync().Result.Value.ToString());
    }
}
