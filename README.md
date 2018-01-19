# Unity3d Simple Sample (Environment Template)

Very simple sample of setting up an Environment for Unity3d development using Net461, it includes all the DLLs required in the asset folder.

Dlls: Nethereum, dependencies (BouncyCastle..) and System.Numerics.

The code just demonstrates output to the log the current BlockNumber.

To run a local blockchain you can just use the preconfigured [testchains](https://github.com/Nethereum/Nethereum.Workbooks/tree/master/testchain/)

```csharp
﻿using System;
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


```


