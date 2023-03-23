using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Unity.Contracts;
#if UNITY_WEBGL
  using Nethereum.Unity.Metamask;
#endif
using Nethereum.Unity.Rpc;
using Nethereum.Util;
using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class MultiplatformErc20Coroutine : MonoBehaviour
{
    public string Url = "http://localhost:8545";
    public BigInteger ChainId = 444444444500;
    public string PrivateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
    public string AddressTo = "0xde0B295669a9FD93d5F28D9Ec85E40f4cb697BAe";
    private string _selectedAccountAddress; 
    private bool _isMetamaskInitialised = false;
    public decimal Amount = 1.1m;

    public InputField InputUrl;
    public InputField InputChainId;
    public InputField InputPrivateKey;
    public InputField InputAddressTo;
    public InputField InputAmount;

    public InputField ResultBalanceAddressTo;
    public InputField ResultTxnHash;
    public InputField ResultContractAddress;
    public Button BtnMetamaskConnect;
    public Text  LblError;

    void Start()
    {

        if (IsWebGL()) // using pk etc only on desktop as an example
        {
            InputUrl.enabled = false;
            InputPrivateKey.enabled = false;
            InputChainId.enabled = false;

        }
        else
        {
         
            InputUrl.text = Url;
            InputPrivateKey.text = PrivateKey;
            InputChainId.text = ChainId.ToString();
            BtnMetamaskConnect.enabled = false;
        }

        InputAddressTo.text = AddressTo;
        InputAmount.text = Amount.ToString();

    }

    public bool IsWebGL()
    {
#if UNITY_WEBGL
      return true;
#else
      return false;
#endif
    }

    public async void DeployRequest()
    {
        StartCoroutine(DeployERC20UsingCoroutines());
    }

    public IEnumerator DeployERC20UsingCoroutines()
    {
        var transactionRequest = GetTransactionUnityRequest();
        transactionRequest.UseLegacyAsDefault = true;


        var deployContract = new StandardTokenDeployment()
        {
            TotalSupply = Nethereum.Web3.Web3.Convert.ToWei(10000000),
            FromAddress = _selectedAccountAddress
        };

        //deploy the contract and True indicates we want to estimate the gas
        yield return transactionRequest.SignAndSendDeploymentContractTransaction(deployContract);

        if (transactionRequest.Exception != null)
        {
            Debug.Log(transactionRequest.Exception.Message);
            DisplayError(transactionRequest.Exception.Message);
            yield break;
        }

        var transactionHash = transactionRequest.Result;

        Debug.Log("Deployment transaction hash:" + transactionHash);

        //create a poll to get the receipt when mined
        var transactionReceiptPolling = new TransactionReceiptPollingRequest(GetUnityRpcRequestClientFactory());
        //checking every 2 seconds for the receipt
        yield return transactionReceiptPolling.PollForReceipt(transactionHash, 2);
        var deploymentReceipt = transactionReceiptPolling.Result;
        ResultContractAddress.text = deploymentReceipt.ContractAddress;
    }

    public async void TransferRequest()
    {
        StartCoroutine(TransferErc20AndGetBalance());
        
    }

    public IEnumerator TransferErc20AndGetBalance()
    {
      
        AddressTo = InputAddressTo.text;
        Amount = System.Decimal.Parse(InputAmount.text);
        var contractAddress = ResultContractAddress.text;

        //initialising the transaction request sender
        var transactionRequest = GetTransactionUnityRequest();
        transactionRequest.UseLegacyAsDefault = true;

        var transferFunction = new TransferFunction
        {
            To = AddressTo,
            TokenAmount = Nethereum.Web3.Web3.Convert.ToWei(Amount),
            FromAddress = _selectedAccountAddress
        };

        yield return transactionRequest.SignAndSendTransaction(transferFunction, contractAddress);

        if (transactionRequest.Exception != null)
        {
            Debug.Log(transactionRequest.Exception.Message);
            DisplayError(transactionRequest.Exception.Message);
            yield break;
        }

        var transactionTransferHash = transactionRequest.Result;

        ResultTxnHash.text = transactionTransferHash;
        Debug.Log("Transfer transaction hash:" + transactionTransferHash);

        var transactionReceiptPolling = new TransactionReceiptPollingRequest(GetUnityRpcRequestClientFactory());
        yield return transactionReceiptPolling.PollForReceipt(transactionTransferHash, 2);
        var transferReceipt = transactionReceiptPolling.Result;

        var transferEvent = transferReceipt.DecodeAllEvents<TransferEventDTO>();
        Debug.Log("Transferred amount from event: " + transferEvent[0].Event.Value);

        var getLogsRequest = new EthGetLogsUnityRequest(GetUnityRpcRequestClientFactory());

        var eventTransfer = TransferEventDTO.GetEventABI();
        yield return getLogsRequest.SendRequest(eventTransfer.CreateFilterInput(contractAddress, _selectedAccountAddress));

        var eventDecoded = getLogsRequest.Result.DecodeAllEvents<TransferEventDTO>();

        Debug.Log("Transferred amount from get logs event: " + eventDecoded[0].Event.Value);

        var queryRequest = new QueryUnityRequest<BalanceOfFunction, BalanceOfOutputDTO>(GetUnityRpcRequestClientFactory(), _selectedAccountAddress);
        yield return queryRequest.Query(new BalanceOfFunction() { Owner = AddressTo }, contractAddress);

        //Getting the dto response already decoded
        var dtoResult = queryRequest.Result;
        Debug.Log(dtoResult.Balance);

        var balanceReceiver = UnitConversion.Convert.FromWei(dtoResult.Balance);
        ResultBalanceAddressTo.text = balanceReceiver.ToString();

        Debug.Log("Balance of account:" + balanceReceiver);
    }

    public void DisplayError(string errorMessage)
    {
        LblError.text = errorMessage;
    }

    public void MetamaskConnect()
    {
#if UNITY_WEBGL
        if (IsWebGL())
        {
            if (MetamaskWebglInterop.IsMetamaskAvailable())
            {
                MetamaskWebglInterop.EnableEthereum(gameObject.name, nameof(EthereumEnabled), nameof(DisplayError));
            }
            else
            {
                DisplayError("Metamask is not available, please install it");
            }
        }
#endif

    }

    public void EthereumEnabled(string addressSelected)
    {
#if UNITY_WEBGL
        if (IsWebGL())
        {
            if (!_isMetamaskInitialised)
            {
                MetamaskWebglInterop.EthereumInit(gameObject.name, nameof(NewAccountSelected), nameof(ChainChanged));
                MetamaskWebglInterop.GetChainId(gameObject.name, nameof(ChainChanged), nameof(DisplayError));
                _isMetamaskInitialised = true;
            }
            NewAccountSelected(addressSelected);
        }
#endif
    }

    public void ChainChanged(string chainId)
    {
        print(chainId);
        ChainId = new HexBigInteger(chainId).Value;
        InputChainId.text = ChainId.ToString();
    }

    public void NewAccountSelected(string accountAddress)
    {
        _selectedAccountAddress = accountAddress;
    }




    public IUnityRpcRequestClientFactory GetUnityRpcRequestClientFactory()
    {
#if UNITY_WEBGL
        if (IsWebGL())
        {
            if (MetamaskWebglInterop.IsMetamaskAvailable())
            {
                return new MetamaskWebglCoroutineRequestRpcClientFactory(_selectedAccountAddress, null, 60000);
            }
            else
            {
                // DisplayError("Metamask is not available, please install it");
                return null;
            }
        }
        else
        {
#endif
            Url = InputUrl.text;
            return new UnityWebRequestRpcClientFactory(Url);
#if UNITY_WEBGL
        }
#endif
    }

    public IContractTransactionUnityRequest GetTransactionUnityRequest()
    {
#if UNITY_WEBGL

        if (IsWebGL())
        {
            if (MetamaskWebglInterop.IsMetamaskAvailable())
            {
                return new MetamaskTransactionCoroutineUnityRequest(_selectedAccountAddress, GetUnityRpcRequestClientFactory());
            }
            else
            {
                DisplayError("Metamask is not available, please install it");
                return null;
            }
        }
        else
        {
#endif
            Url = InputUrl.text;
            PrivateKey = InputPrivateKey.text;
            ChainId = BigInteger.Parse(InputChainId.text);
            var account = new Nethereum.Web3.Accounts.Account(InputPrivateKey.text);
            NewAccountSelected(account.Address);
            return new TransactionSignedUnityRequest(Url, PrivateKey, ChainId);
#if UNITY_WEBGL
        }
#endif
    }

}
