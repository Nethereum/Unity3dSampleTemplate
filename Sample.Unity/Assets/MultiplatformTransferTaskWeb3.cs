using UnityEngine;
using UnityEngine.UI;
using Nethereum.Util;
using Debug = UnityEngine.Debug;
#if UNITY_WEBGL
  using Nethereum.Unity.Metamask;
#endif
using System.Numerics;
using Nethereum.Hex.HexTypes;
using System.Threading.Tasks;
using Nethereum.Web3;
using Nethereum.Metamask;
using Nethereum.Web3.Accounts;
using System;

public class MultiplatformTransferTaskWeb3 : MonoBehaviour
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
    private MetamaskHostProvider metamaskHost;

    void Start()
    {

        if (IsWebGL())
        {
            InputUrl.enabled = false;
            InputPrivateKey.enabled = false;
            InputChainId.enabled = false;
#if UNITY_WEBGL
            metamaskHost = MetamaskWebglHostProvider.CreateOrGetCurrentInstance();
            metamaskHost.SelectedAccountChanged += MetamaskHost_SelectedAccountChanged;
#endif
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
        await TransferEtherUsingWeb3inWebGlOrNative();
    }

    public async Task TransferEtherUsingWeb3inWebGlOrNative()
    {
        var web3 = await GetWeb3Async();
        string selectedAccount = GetSelectedAccount();
        AddressTo = InputAddressTo.text;
        Amount = System.Decimal.Parse(InputAmount.text);
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
        _selectedAccountAddress = arg;
        return Task.CompletedTask;
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

    public async void SignMessageRequest()
    {
       await SignMessageAsync();
    }

    public async Task SignMessageAsync()
    {
        var web3 = await GetWeb3Async();
        try
        {
            var result = await web3.Eth.AccountSigning.PersonalSign.SendRequestAsync(new HexUTF8String(InputSignMessage.text));
            LblSignedMessage.text = result;
        }
        catch (Exception ex)
        {
            Debug.Log("Error signing message");
            DisplayError(ex.Message);
        }
    }

    private string GetSelectedAccount()
    {
        return _selectedAccountAddress;
    }

    private async Task<IWeb3> GetWeb3Async()
    {
#if UNITY_WEBGL
        await metamaskHost.EnableProviderAsync();
        _selectedAccountAddress = metamaskHost.SelectedAccount;
        return await metamaskHost.GetWeb3Async();
#else
        Url = InputUrl.text;
        PrivateKey = InputPrivateKey.text;
        ChainId = BigInteger.Parse(InputChainId.text);
        var account = new Account(PrivateKey, ChainId);
        _selectedAccountAddress = account.Address;
        return new Web3(account, Url);
#endif
    }

}
