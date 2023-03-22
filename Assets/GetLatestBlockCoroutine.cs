using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Nethereum.Unity.Rpc;

public class GetLatestBlockCoroutine : MonoBehaviour
{
    public string Url = "http://localhost:8545";

    public InputField ResultBlockNumber;
    public InputField InputUrl;

    // Use this for initialization
    void Start()
    {
        InputUrl.text = Url;
    }

    public void GetBlockNumberRequest()
    {
        Url = InputUrl.text;
        StartCoroutine(GetBlockNumber());
    }

    public IEnumerator GetBlockNumber()
    {

       var blockNumberRequest = new EthBlockNumberUnityRequest(InputUrl.text);
 
       yield return blockNumberRequest.SendRequest();

        if (blockNumberRequest.Exception != null)
        {
            UnityEngine.Debug.Log(blockNumberRequest.Exception.Message);
        }
        else
        {
            ResultBlockNumber.text = blockNumberRequest.Result.Value.ToString();
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
