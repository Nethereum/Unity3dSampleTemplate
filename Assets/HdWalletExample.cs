using UnityEngine;
using UnityEngine.UI;
using Nethereum.HdWallet;

public class HdWalletExample : MonoBehaviour
{
    public const string Words =
       "ripple scissors kick mammal hire column oak again sun offer wealth tomorrow wagon turn fatal";
   

    public InputField ResultAccountAddress;
    public InputField InputWords;
    public InputField ResultPrivateKey;

    // Use this for initialization
    void Start()
    {
        InputWords.text = Words;
    }

    public void GetHdWalletAccoutnsRequest()
    {
       GetHdWalletAccounts();
    }

    public void GetHdWalletAccounts()
    {
        var wallet = new Wallet(Words, null);
        var account = wallet.GetAccount(0);
        ResultAccountAddress.text = account.Address;
        ResultPrivateKey.text = account.PrivateKey;
        Debug.Log(account.Address);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
