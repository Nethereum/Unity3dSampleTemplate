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

public class TokenDeployAndSendCoroutinesUnityWebRequest : MonoBehaviour {

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


    //Deployment contract object definition

    public partial class EIP20Deployment : EIP20DeploymentBase
    {
        public EIP20Deployment() : base(BYTECODE) { }

        public EIP20Deployment(string byteCode) : base(byteCode) { }
    }

    public class EIP20DeploymentBase : ContractDeploymentMessage
    {

        public static string BYTECODE = "608060405234801561001057600080fd5b50610565806100206000396000f3fe608060405234801561001057600080fd5b506004361061002b5760003560e01c8063e4ca832814610030575b600080fd5b61004361003e3660046102a7565b610059565b6040516100509190610333565b60405180910390f35b60408051600a808252610160820190925260609160009190816020015b6040805160a081018252600080825260208083018290529282015260608082018190526080820152825260001990920191018161007657905050905060005b600a81101561019a5760008282815181106100d2576100d2610402565b6020908102919091018101516001600160a01b038716815263ffffffff841691810191909152905061010582606461042e565b6040820152610113826101a1565b604051602001610123919061044d565b60408051601f198184030181529190526060820152610141826101a1565b6040516020016101519190610479565b60405160208183030381529060405281608001819052508083838151811061017b5761017b610402565b6020026020010181905250508080610192906104a7565b9150506100b5565b5092915050565b6060816101c55750506040805180820190915260018152600360fc1b602082015290565b8160005b81156101ef57806101d9816104a7565b91506101e89050600a836104d8565b91506101c9565b60008167ffffffffffffffff81111561020a5761020a6103ec565b6040519080825280601f01601f191660200182016040528015610234576020820181803683370190505b5090505b841561029f576102496001836104ec565b9150610256600a86610503565b610261906030610517565b60f81b81838151811061027657610276610402565b60200101906001600160f81b031916908160001a905350610298600a866104d8565b9450610238565b949350505050565b6000602082840312156102b957600080fd5b81356001600160a01b03811681146102d057600080fd5b9392505050565b60005b838110156102f25781810151838201526020016102da565b83811115610301576000848401525b50505050565b6000815180845261031f8160208601602086016102d7565b601f01601f19169290920160200192915050565b60006020808301818452808551808352604092508286019150828160051b87010184880160005b838110156103de57888303603f19018552815180516001600160a01b031684528781015163ffffffff1688850152868101518785015260608082015160a082870181905291906103ac83880182610307565b92505050608080830151925085820381870152506103ca8183610307565b96890196945050509086019060010161035a565b509098975050505050505050565b634e487b7160e01b600052604160045260246000fd5b634e487b7160e01b600052603260045260246000fd5b634e487b7160e01b600052601160045260246000fd5b600081600019048311821515161561044857610448610418565b500290565b633ab9b2b960e11b81526000825161046c8160048501602087016102d7565b9190910160040192915050565b651cde5b589bdb60d21b81526000825161049a8160068501602087016102d7565b9190910160060192915050565b60006000198214156104bb576104bb610418565b5060010190565b634e487b7160e01b600052601260045260246000fd5b6000826104e7576104e76104c2565b500490565b6000828210156104fe576104fe610418565b500390565b600082610512576105126104c2565b500690565b6000821982111561052a5761052a610418565b50019056fea2646970667358221220c65c5c49e7a6b944821499135957f2039daf851cb2134aa0a0227c93186c319564736f6c634300080a0033";

        public EIP20DeploymentBase() : base(BYTECODE) { }

        public EIP20DeploymentBase(string byteCode) : base(byteCode) { }

        [Parameter("uint256", "_initialAmount", 1)]
        public BigInteger InitialAmount { get; set; }
        [Parameter("string", "_tokenName", 2)]
        public string TokenName { get; set; }
        [Parameter("uint8", "_decimalUnits", 3)]
        public byte DecimalUnits { get; set; }
        [Parameter("string", "_tokenSymbol", 4)]
        public string TokenSymbol { get; set; }

    }

    [Function("transfer", "bool")]
    public class TransferFunctionBase : FunctionMessage
    {
        [Parameter("address", "_to", 1)]
        public string To { get; set; }
        [Parameter("uint256", "_value", 2)]
        public BigInteger Value { get; set; }
    }

    public partial class TransferFunction : TransferFunctionBase
    {

    }

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunction : FunctionMessage
    {
        [Parameter("address", "_owner", 1)]
        public string Owner { get; set; }
    }

    [FunctionOutput]
    public class BalanceOfFunctionOutput : IFunctionOutputDTO
    {
        [Parameter("uint256", 1)]
        public int Balance { get; set; }
    }

    [Event("Transfer")]
    public class TransferEventDTOBase : IEventDTO
    {

        [Parameter("address", "_from", 1, true)]
        public virtual string From { get; set; }
        [Parameter("address", "_to", 2, true)]
        public virtual string To { get; set; }
        [Parameter("uint256", "_value", 3, false)]
        public virtual BigInteger Value { get; set; }
    }

    public partial class TransferEventDTO : TransferEventDTOBase
    {
        public static EventABI GetEventABI()
        {
            return EventExtensions.GetEventABI<TransferEventDTO>();
        }
    }
   

    // Use this for initialization
    void Start () {

        Debug.Log("Starting TokenDeployAndSendCoroutinesUnityWebRequest example");
       
        //

        //StartCoroutine(DeployAndTransferToken());
        StartCoroutine(DeployAndGetArrayStruct());
    }

    

    public IEnumerator DeployAndGetArrayStruct()
    {
        var url = "http://localhost:8545";
        var privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
        var account = "0x12890d2cce102216644c59daE5baed380d84830c";
        //initialising the transaction request sender
        var transactionRequest = new TransactionSignedUnityRequest(url, privateKey, 444444444500);
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


    //Sample of new features / requests
    public IEnumerator DeployAndTransferToken()
    {
        var url = "http://localhost:8545";
        var privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
        var account = "0x12890d2cce102216644c59daE5baed380d84830c";
        //initialising the transaction request sender
        var transactionRequest = new TransactionSignedUnityRequest(url, privateKey, 444444444500);
        transactionRequest.UseLegacyAsDefault = true;


        var deployContract = new EIP20Deployment()
        {
            InitialAmount = 10000,
            FromAddress = account,
            TokenName = "TST",
            TokenSymbol = "TST"
        };

        //deploy the contract and True indicates we want to estimate the gas
        yield return transactionRequest.SignAndSendDeploymentContractTransaction<EIP20DeploymentBase>(deployContract);

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
        var queryRequest = new QueryUnityRequest<BalanceOfFunction, BalanceOfFunctionOutput>(url, account);
        yield return queryRequest.Query(new BalanceOfFunction(){Owner = account}, deploymentReceipt.ContractAddress);

        //Getting the dto response already decoded
        var dtoResult = queryRequest.Result;
        Debug.Log(dtoResult.Balance);


        var transactionTransferRequest = new TransactionSignedUnityRequest(url, privateKey, 444444444500);
        transactionTransferRequest.UseLegacyAsDefault = true;

        var newAddress = "0xde0B295669a9FD93d5F28D9Ec85E40f4cb697BAe";

        var transactionMessage = new TransferFunction
        {
            FromAddress = account,
            To = newAddress,
            Value = 1000,

        };

        yield return transactionTransferRequest.SignAndSendTransaction(transactionMessage, deploymentReceipt.ContractAddress);

        var transactionTransferHash = transactionTransferRequest.Result;

        Debug.Log("Transfer txn hash:" + transactionHash);

        transactionReceiptPolling = new TransactionReceiptPollingRequest(url);
        yield return transactionReceiptPolling.PollForReceipt(transactionTransferHash, 2);
        var transferReceipt = transactionReceiptPolling.Result;

        var transferEvent = transferReceipt.DecodeAllEvents<TransferEventDTO>();
        Debug.Log("Transferd amount from event: " + transferEvent[0].Event.Value);

        var getLogsRequest = new EthGetLogsUnityRequest(url);

        var eventTransfer = TransferEventDTO.GetEventABI();
        yield return getLogsRequest.SendRequest(eventTransfer.CreateFilterInput(deploymentReceipt.ContractAddress, account));

        var eventDecoded = getLogsRequest.Result.DecodeAllEvents<TransferEventDTO>();

        Debug.Log("Transferd amount from get logs event: " + eventDecoded[0].Event.Value);
    }



    // Update is called once per frame
    void Update () {
		
	}
}
