using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Nethereum.Unity.Rpc;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Debug = UnityEngine.Debug;
//#if UNITY_WEBGL
//  using Nethereum.Unity.Metamask;
//#endif
using Nethereum.Unity.FeeSuggestions;
using Nethereum.Unity.Contracts;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.Signer;
using Nethereum.HdWallet;
using System.Threading.Tasks;
using Nethereum.Web3;
using Nethereum.Metamask;
using Nethereum.Unity.Metamask;
using Nethereum.RPC.Fee1559Suggestions;
using System.Net;
using Nethereum.Web3.Accounts;

public class MultiplatformTransfer : MonoBehaviour
{
    public string Url = "http://localhost:8545";
    public BigInteger ChainId = 444444444500;
    public string PrivateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
    public string AddressTo = "0xde0B295669a9FD93d5F28D9Ec85E40f4cb697BAe";
    private string _selectedAccountAddress; 
    private bool _isMetamaskInitialised = false;
    public decimal Amount = 1.1m;
    public string TransactionHash = "";
    public decimal BalanceAddressTo = 0m;

    public InputField InputUrl;
    public InputField InputChainId;
    public InputField InputPrivateKey;
    public InputField InputAddressTo;
    public InputField InputAmount;

    public InputField ResultBalanceAddressTo;
    public InputField ResultTxnHash;
    public Button BtnMetamaskConnect;
    public Text  LblError;


    public InputField InputSignMessage;
    public Text LblSignedMessage;

    void Start()
    {

        if (IsWebGL())
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

    public async void TransferRequest()
    {
        //StartCoroutine(TransferEtherUsingCoroutines());
        await TransferEtherUsingWeb3inWebGlOrNative();
    }

    public async Task TransferEtherUsingWeb3inWebGlOrNative()
    {
        IWeb3 web3 = null;
        string selectedAccount = string.Empty;
        AddressTo = InputAddressTo.text;
        Amount = System.Decimal.Parse(InputAmount.text);
        var receivingAddress = AddressTo;
#if UNITY_WEBGL
        if (IsWebGL())
        {
           
            var metamaskHost = MetamaskWebGlHostProvider.CreateOrGetCurrentInstance();
            metamaskHost.SelectedAccountChanged += MetamaskHost_SelectedAccountChanged;
            await metamaskHost.EnableProviderAsync();
            web3 = await metamaskHost.GetWeb3Async();
            selectedAccount = metamaskHost.SelectedAccount;
        }
        else
        {
#endif
            Url = InputUrl.text;
            PrivateKey = InputPrivateKey.text;
            ChainId = BigInteger.Parse(InputChainId.text);
            var account = new Account(PrivateKey, ChainId);
            web3 = new Web3(account, Url);
            selectedAccount = account.Address;
#if UNITY_WEBGL
        }
#endif

      
        
        var timePreferenceFeeSuggesionStrategy = web3.FeeSuggestion.GetTimePreferenceFeeSuggestionStrategy();
        var fee = await timePreferenceFeeSuggesionStrategy.SuggestFeeAsync();
        var nonce = await web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(selectedAccount);
        var service = web3.Eth.GetEtherTransferService();
        var receipt = await service.TransferEtherAndWaitForReceiptAsync(AddressTo, Amount, fee.MaxPriorityFeePerGas.Value, fee.MaxFeePerGas.Value, 
            null, nonce.Value);
        ResultTxnHash.text = receipt.TransactionHash;
        var balance = await web3.Eth.GetBalance.SendRequestAsync(AddressTo);
        BalanceAddressTo = UnitConversion.Convert.FromWei(balance.Value);
        ResultBalanceAddressTo.text = BalanceAddressTo.ToString();
    }

    private Task MetamaskHost_SelectedAccountChanged(string arg)
    {
        ResultTxnHash.text = arg;
        return Task.CompletedTask;
    }

    public IEnumerator TransferEtherUsingCoroutines()
    {
      
        AddressTo = InputAddressTo.text;
        Amount = System.Decimal.Parse(InputAmount.text);

        //initialising the transaction request sender
        var ethTransfer = new EthTransferUnityRequest(GetTransactionUnityRequest());

        var receivingAddress = AddressTo;

        var timePreferenceFeeSuggestion = new TimePreferenceFeeSuggestionUnityRequestStrategy(Url);

        yield return timePreferenceFeeSuggestion.SuggestFees();

        if (timePreferenceFeeSuggestion.Exception != null)
        {
            Debug.Log(timePreferenceFeeSuggestion.Exception.Message);
            yield break;
        }

        //lets get the first one so it is higher priority
        Debug.Log(timePreferenceFeeSuggestion.Result.Length);
        if (timePreferenceFeeSuggestion.Result.Length > 0)
        {
            Debug.Log(timePreferenceFeeSuggestion.Result[0].MaxFeePerGas);
            Debug.Log(timePreferenceFeeSuggestion.Result[0].MaxPriorityFeePerGas);
        }
        var fee = timePreferenceFeeSuggestion.Result[0];

        yield return ethTransfer.TransferEther(receivingAddress, Amount, fee.MaxPriorityFeePerGas.Value, fee.MaxFeePerGas.Value);
        if (ethTransfer.Exception != null)
        {
            Debug.Log("Error transferring Ether using Time Preference Fee Estimation Strategy: " + ethTransfer.Exception.Message);
            DisplayError(ethTransfer.Exception.Message);
            yield break;
        }
       

        TransactionHash = ethTransfer.Result;
        ResultTxnHash.text = TransactionHash;
        Debug.Log("Transfer transaction hash:" + TransactionHash);

        //create a poll to get the receipt when mined
        var transactionReceiptPolling = new TransactionReceiptPollingRequest(Url);
        //checking every 2 seconds for the receipt
        yield return transactionReceiptPolling.PollForReceipt(TransactionHash, 2);

        Debug.Log("Transaction mined");

        var balanceRequest = new EthGetBalanceUnityRequest(Url);
        yield return balanceRequest.SendRequest(receivingAddress, BlockParameter.CreateLatest());

        BalanceAddressTo = UnitConversion.Convert.FromWei(balanceRequest.Result.Value);
        ResultBalanceAddressTo.text = BalanceAddressTo.ToString();

        Debug.Log("Balance of account:" + BalanceAddressTo);
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
            if (MetamaskInterop.IsMetamaskAvailable())
            {
                MetamaskInterop.EnableEthereum(gameObject.name, nameof(EthereumEnabled), nameof(DisplayError));
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
                MetamaskInterop.EthereumInit(gameObject.name, nameof(NewAccountSelected), nameof(ChainChanged));
                MetamaskInterop.GetChainId(gameObject.name, nameof(ChainChanged), nameof(DisplayError));
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

    public void SignMessageRequest()
    {
        StartCoroutine(SignMessage());
    }

    public IEnumerator SignMessage()
    {
#if UNITY_WEBGL
        var personalSignRequest = new EthPersonalSignUnityRequest(GetUnityRpcRequestClientFactory());
        yield return personalSignRequest.SendRequest(new HexUTF8String(InputSignMessage.text));
        if (personalSignRequest.Exception != null)
        {
            Debug.Log("Error signing message");
            DisplayError(personalSignRequest.Exception.Message);
            yield break;
        }
        LblSignedMessage.text = personalSignRequest.Result;
#else
       
        PrivateKey = InputPrivateKey.text;
        var signer = new EthereumMessageSigner();
        LblSignedMessage.text = signer.EncodeUTF8AndSign(InputSignMessage.text, new EthECKey(PrivateKey));
        yield break;

#endif
    }


    public IUnityRpcRequestClientFactory GetUnityRpcRequestClientFactory()
    {
#if UNITY_WEBGL
        if (IsWebGL())
        {
            if (MetamaskInterop.IsMetamaskAvailable())
            {
                return new MetamaskRequestRpcClientFactory(_selectedAccountAddress, null, 60000);
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
            if (MetamaskInterop.IsMetamaskAvailable())
            {
                return new MetamaskTransactionUnityRequest(_selectedAccountAddress, GetUnityRpcRequestClientFactory());
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
            return new TransactionSignedUnityRequest(Url, PrivateKey, ChainId);
#if UNITY_WEBGL
        }
#endif
    }

}
