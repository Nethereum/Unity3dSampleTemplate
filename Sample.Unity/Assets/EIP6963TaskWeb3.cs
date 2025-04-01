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
using Nethereum.Unity.EIP6963;
using Nethereum.EIP6963WalletInterop;
using System.Collections;

public class EIP6963TaskWeb3 : MonoBehaviour
{
    public BigInteger ChainId = 31337;
    public string AddressTo = "0xde0B295669a9FD93d5F28D9Ec85E40f4cb697BAe";
    private string _selectedAccountAddress;
    private bool _isInitialised = false;
    public decimal Amount = 1.1m;
    public string TransactionHash = "";
    public decimal BalanceAddressTo = 0m;

    public InputField InputAddressTo;
    public InputField InputAmount;
    public InputField InputSelectedAddress;

    public InputField InputChainId;

    public InputField ResultBalanceAddressTo;
    public InputField ResultTxnHash;
    public Button BtnGetWalletsAvailable;
    public Text LblError;


    public InputField InputSignMessage;
    public Text LblSignedMessage;
    private EIP6963WalletHostProvider eip6963Host;

    public GameObject buttonWalletConnectPrefab;
    public Transform walletSelectionContentPanel;

    void Start()
    {

        if (IsWebGL())
        {
#if UNITY_WEBGL
            try
            {
                eip6963Host = EIP6963WebglHostProvider.CreateOrGetCurrentInstance();
                eip6963Host.SelectedAccountChanged += EIP6963Host_SelectedAccountChanged;
                BtnGetWalletsAvailable.enabled = true;
            }
            catch (Exception ex)
            {
                Debug.Log("Error creating EIP6963WebglHostProvider");
                DisplayError(ex.Message);
            }

#endif
        }
        else
        {
            BtnGetWalletsAvailable.enabled = false;
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
        var service = web3.Eth.GetEtherTransferService();
        var receipt = await service.TransferEtherAndWaitForReceiptAsync(AddressTo, Amount);
        ResultTxnHash.text = receipt.TransactionHash;
        var balance = await web3.Eth.GetBalance.SendRequestAsync(AddressTo);
        BalanceAddressTo = UnitConversion.Convert.FromWei(balance.Value);
        ResultBalanceAddressTo.text = BalanceAddressTo.ToString();
    }

    private Task EIP6963Host_SelectedAccountChanged(string arg)
    {
        InputSelectedAddress.text = arg;
        _selectedAccountAddress = arg;
        return Task.CompletedTask;
    }

    public void DisplayError(string errorMessage)
    {
        LblError.text = errorMessage;
    }


    public void EthereumEnabled(string addressSelected)
    {
#if UNITY_WEBGL
        if (IsWebGL())
        {
            if (!_isInitialised)
            {
                EIP6963WebglInterop.EIP6963_EthereumInit(gameObject.name, nameof(NewAccountSelected), nameof(ChainChanged));
                EIP6963WebglInterop.EIP6963_GetChainId(gameObject.name, nameof(ChainChanged), nameof(DisplayError));
                _isInitialised = true;
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
        InputSelectedAddress.text = accountAddress;
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

    public async void GetWalletsAvailableAsync()
    {
        Debug.Log("GetWalletsAvailableAsync");
        var wallets = await eip6963Host.GetAvailableWalletsAsync();
        Debug.Log("GetWalletsAvailableAsync completed");
        if(wallets != null)
        {
            Debug.Log("Wallets found");
            Debug.Log(wallets.Length);
        }
        foreach (var wallet in wallets) {
            Debug.Log(wallet.Name);
        }
        GenerateButtons(wallets);
    }

    void GenerateButtons(EIP6963WalletInfo[] wallets)
    {
        for (int i = 0; i < wallets.Length; i++)
        {
            GameObject newButton = Instantiate(buttonWalletConnectPrefab, walletSelectionContentPanel);
            newButton.name = "Button_" + (i + 1);

            // Change button text
            newButton.GetComponentInChildren<Text>().text = wallets[i].Name;

            // Set image from Base64
            Image[] images = newButton.GetComponentsInChildren<Image>();
            foreach (Image image in images)
            {
                if (image.gameObject.name == "Icon")
                {
                    ImageHelper.ApplyBase64Image(image, wallets[i].Icon); //svg not supported, just need to create an svg helper
                }
            }
           
            // Add a click listener
            string uuid = wallets[i].Uuid;
            newButton.GetComponent<Button>().onClick.AddListener(() => ButtonSelectWalletClick(uuid));
        }
    }

    async void ButtonSelectWalletClick(string uuid)
    {
        await eip6963Host.SelectWalletAsync(uuid);
        EIP6963WebglInterop.EIP6963_EnableEthereum(gameObject.name, nameof(EthereumEnabled), nameof(DisplayError));
    }

    private string GetSelectedAccount()
    {
        return _selectedAccountAddress;
    }

    private async Task<IWeb3> GetWeb3Async()
    {
#if UNITY_WEBGL
        await eip6963Host.EnableProviderAsync();
        _selectedAccountAddress = eip6963Host.SelectedAccount;
        InputSelectedAddress.text = _selectedAccountAddress;
        return await eip6963Host.GetWeb3Async();
#endif

        return null;
    }

}
