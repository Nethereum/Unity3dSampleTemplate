using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;
using Nethereum.Contracts;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.Extensions;
using Nethereum.HdWallet;
using Nethereum.Unity.Rpc;
using UnityEngine;
using UnityEngine.Assertions;

public partial class TokenDeployAndSendCoroutinesUnityWebRequest : MonoBehaviour {

    public partial class TestDeployment : TestDeploymentBase
    {
        public TestDeployment() : base(BYTECODE) { }
        public TestDeployment(string byteCode) : base(byteCode) { }
    }

    public class TestDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "608060405234801561001057600080fd5b5061055c806100206000396000f3fe608060405234801561001057600080fd5b506004361061002b5760003560e01c8063e4ca832814610030575b600080fd5b61004361003e36600461029e565b610059565b604051610050919061032a565b60405180910390f35b60408051600a808252610160820190925260609160009190816020015b6040805160a081018252600080825260208083018290529282015260608082018190526080820152825260001990920191018161007657905050905060005b600a8110156101915760008282815181106100d2576100d26103f9565b60209081029190910181015133815263ffffffff84169181019190915290506100fc826064610425565b604082015261010a82610198565b60405160200161011a9190610444565b60408051601f19818403018152919052606082015261013882610198565b6040516020016101489190610470565b604051602081830303815290604052816080018190525080838381518110610172576101726103f9565b60200260200101819052505080806101899061049e565b9150506100b5565b5092915050565b6060816101bc5750506040805180820190915260018152600360fc1b602082015290565b8160005b81156101e657806101d08161049e565b91506101df9050600a836104cf565b91506101c0565b60008167ffffffffffffffff811115610201576102016103e3565b6040519080825280601f01601f19166020018201604052801561022b576020820181803683370190505b5090505b8415610296576102406001836104e3565b915061024d600a866104fa565b61025890603061050e565b60f81b81838151811061026d5761026d6103f9565b60200101906001600160f81b031916908160001a90535061028f600a866104cf565b945061022f565b949350505050565b6000602082840312156102b057600080fd5b81356001600160a01b03811681146102c757600080fd5b9392505050565b60005b838110156102e95781810151838201526020016102d1565b838111156102f8576000848401525b50505050565b600081518084526103168160208601602086016102ce565b601f01601f19169290920160200192915050565b60006020808301818452808551808352604092508286019150828160051b87010184880160005b838110156103d557888303603f19018552815180516001600160a01b031684528781015163ffffffff1688850152868101518785015260608082015160a082870181905291906103a3838801826102fe565b92505050608080830151925085820381870152506103c181836102fe565b968901969450505090860190600101610351565b509098975050505050505050565b634e487b7160e01b600052604160045260246000fd5b634e487b7160e01b600052603260045260246000fd5b634e487b7160e01b600052601160045260246000fd5b600081600019048311821515161561043f5761043f61040f565b500290565b633ab9b2b960e11b8152600082516104638160048501602087016102ce565b9190910160040192915050565b651cde5b589bdb60d21b8152600082516104918160068501602087016102ce565b9190910160060192915050565b60006000198214156104b2576104b261040f565b5060010190565b634e487b7160e01b600052601260045260246000fd5b6000826104de576104de6104b9565b500490565b6000828210156104f5576104f561040f565b500390565b600082610509576105096104b9565b500690565b600082198211156105215761052161040f565b50019056fea2646970667358221220b58feec7463b9b01f3533a4cc15465e381aa46db7291598c2ee67aa5cd1e45fe64736f6c634300080a0033";
        public TestDeploymentBase() : base(BYTECODE) { }
        public TestDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class GetUserUpgradeDataFunction : GetUserUpgradeDataFunctionBase { }

    [Function("getUserUpgradeData", typeof(GetUserUpgradeDataOutputDTO))]
    public class GetUserUpgradeDataFunctionBase : FunctionMessage
    {
        [Parameter("address", "_user", 1)]
        public virtual string User { get; set; }
    }

    public partial class GetUserUpgradeDataOutputDTO : GetUserUpgradeDataOutputDTOBase { }

    [FunctionOutput]
    public class GetUserUpgradeDataOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("tuple[]", "list", 1)]
        public virtual List<UserUpgradeData> List { get; set; }
    }

    public partial class UserUpgradeData : UserUpgradeDataBase { }

    public class UserUpgradeDataBase
    {
        [Parameter("address", "id", 1)]
        public virtual string Id { get; set; }
        [Parameter("uint32", "level", 2)]
        public virtual uint Level { get; set; }
        [Parameter("uint256", "blocks", 3)]
        public virtual BigInteger Blocks { get; set; }
        [Parameter("string", "name", 4)]
        public virtual string Name { get; set; }
        [Parameter("string", "symbol", 5)]
        public virtual string Symbol { get; set; }
    }


    // Use this for initialization
    void Start () {

        Debug.Log("Starting TokenDeployAndSendCoroutinesUnityWebRequest example");
       
        //

        StartCoroutine(DeployAndGetArrayStruct());
        
    }

    

    public IEnumerator DeployAndGetArrayStruct()
    {
        var url = "http://localhost:8545";
        var privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
        var account = "0x12890d2cce102216644c59daE5baed380d84830c";
        //initialising the transaction request sender
        var transactionRequest = new TransactionSignedUnityRequest(url, privateKey, 31337);
        transactionRequest.UseLegacyAsDefault = true;


        var deployContract = new TestDeployment()
        {
            FromAddress = account
        };

        //deploy the contract and True indicates we want to estimate the gas
        yield return transactionRequest.SignAndSendDeploymentContractTransaction<TestDeployment>(deployContract);

        if (transactionRequest.Exception != null)
        {
            Debug.Log(transactionRequest.Exception.Message);
            yield break;
        }

        var transactionHash = transactionRequest.Result;

        Debug.Log("Deployment transaction hash:" + transactionHash);

        //create a poll to get the receipt when mined
        var transactionReceiptPolling = new TransactionReceiptPollingRequest(url);
        //checking every 2 seconds for the receipt
        yield return transactionReceiptPolling.PollForReceipt(transactionHash, 2);
        var deploymentReceipt = transactionReceiptPolling.Result;

        Debug.Log("Deployment contract address:" + deploymentReceipt.ContractAddress);

        //Query request using our acccount and the contracts address (no parameters needed and default values)
        var queryRequest = new QueryUnityRequest<GetUserUpgradeDataFunction, GetUserUpgradeDataOutputDTO>(url, account);
        yield return queryRequest.Query(new GetUserUpgradeDataFunction() { User = account }, deploymentReceipt.ContractAddress);

        var dtoResult = queryRequest.Result;
        Debug.Log(dtoResult.List[0].Name);
        Debug.Log(dtoResult.List[0].Id);
        Debug.Log(dtoResult.List.Count);

    }



    // Update is called once per frame
    void Update () {
		
	}
}
