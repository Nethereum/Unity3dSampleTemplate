using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace Sample.DotNet.Contracts.ERC20Token.ContractDefinition
{


    public partial class ERC20TokenDeployment : ERC20TokenDeploymentBase
    {
        public ERC20TokenDeployment() : base(BYTECODE) { }
        public ERC20TokenDeployment(string byteCode) : base(byteCode) { }
    }

    public class ERC20TokenDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "60806040523480156200001157600080fd5b5060405162000acb38038062000acb833981016040819052620000349162000147565b3360009081526020819052604090208490556002849055600362000059848262000265565b506004805460ff191660ff8416179055600562000077828262000265565b505050505062000331565b634e487b7160e01b600052604160045260246000fd5b600082601f830112620000aa57600080fd5b81516001600160401b0380821115620000c757620000c762000082565b604051601f8301601f19908116603f01168101908282118183101715620000f257620000f262000082565b816040528381526020925086838588010111156200010f57600080fd5b600091505b8382101562000133578582018301518183018401529082019062000114565b600093810190920192909252949350505050565b600080600080608085870312156200015e57600080fd5b845160208601519094506001600160401b03808211156200017e57600080fd5b6200018c8883890162000098565b94506040870151915060ff82168214620001a557600080fd5b606087015191935080821115620001bb57600080fd5b50620001ca8782880162000098565b91505092959194509250565b600181811c90821680620001eb57607f821691505b6020821081036200020c57634e487b7160e01b600052602260045260246000fd5b50919050565b601f8211156200026057600081815260208120601f850160051c810160208610156200023b5750805b601f850160051c820191505b818110156200025c5782815560010162000247565b5050505b505050565b81516001600160401b0381111562000281576200028162000082565b6200029981620002928454620001d6565b8462000212565b602080601f831160018114620002d15760008415620002b85750858301515b600019600386901b1c1916600185901b1785556200025c565b600085815260208120601f198616915b828110156200030257888601518255948401946001909101908401620002e1565b5085821015620003215787850151600019600388901b60f8161c191681555b5050505050600190811b01905550565b61078a80620003416000396000f3fe608060405234801561001057600080fd5b50600436106100a95760003560e01c8063313ce56711610071578063313ce567146101395780635c6581651461015857806370a082311461018357806395d89b41146101ac578063a9059cbb146101b4578063dd62ed3e146101c757600080fd5b806306fdde03146100ae578063095ea7b3146100cc57806318160ddd146100ef57806323b872dd1461010657806327e235e314610119575b600080fd5b6100b6610200565b6040516100c391906105b9565b60405180910390f35b6100df6100da366004610623565b61028e565b60405190151581526020016100c3565b6100f860025481565b6040519081526020016100c3565b6100df61011436600461064d565b6102fb565b6100f8610127366004610689565b60006020819052908152604090205481565b6004546101469060ff1681565b60405160ff90911681526020016100c3565b6100f86101663660046106ab565b600160209081526000928352604080842090915290825290205481565b6100f8610191366004610689565b6001600160a01b031660009081526020819052604090205490565b6100b66104a7565b6100df6101c2366004610623565b6104b4565b6100f86101d53660046106ab565b6001600160a01b03918216600090815260016020908152604080832093909416825291909152205490565b6003805461020d906106de565b80601f0160208091040260200160405190810160405280929190818152602001828054610239906106de565b80156102865780601f1061025b57610100808354040283529160200191610286565b820191906000526020600020905b81548152906001019060200180831161026957829003601f168201915b505050505081565b3360008181526001602090815260408083206001600160a01b038716808552925280832085905551919290917f8c5be1e5ebec7d5bd14f71427d1e84f3dd0314c0f7b2291e5b200ac8c7c3b925906102e99086815260200190565b60405180910390a35060015b92915050565b6001600160a01b038316600081815260016020908152604080832033845282528083205493835290829052812054909190831180159061033b5750828110155b6103b25760405162461bcd60e51b815260206004820152603960248201527f746f6b656e2062616c616e6365206f7220616c6c6f77616e6365206973206c6f60448201527f776572207468616e20616d6f756e74207265717565737465640000000000000060648201526084015b60405180910390fd5b6001600160a01b038416600090815260208190526040812080548592906103da90849061072e565b90915550506001600160a01b03851660009081526020819052604081208054859290610407908490610741565b909155505060001981101561044f576001600160a01b038516600090815260016020908152604080832033845290915281208054859290610449908490610741565b90915550505b836001600160a01b0316856001600160a01b03167fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef8560405161049491815260200190565b60405180910390a3506001949350505050565b6005805461020d906106de565b3360009081526020819052604081205482111561052b5760405162461bcd60e51b815260206004820152602f60248201527f746f6b656e2062616c616e6365206973206c6f776572207468616e207468652060448201526e1d985b1d59481c995c5d595cdd1959608a1b60648201526084016103a9565b336000908152602081905260408120805484929061054a908490610741565b90915550506001600160a01b0383166000908152602081905260408120805484929061057790849061072e565b90915550506040518281526001600160a01b0384169033907fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef906020016102e9565b600060208083528351808285015260005b818110156105e6578581018301518582016040015282016105ca565b506000604082860101526040601f19601f8301168501019250505092915050565b80356001600160a01b038116811461061e57600080fd5b919050565b6000806040838503121561063657600080fd5b61063f83610607565b946020939093013593505050565b60008060006060848603121561066257600080fd5b61066b84610607565b925061067960208501610607565b9150604084013590509250925092565b60006020828403121561069b57600080fd5b6106a482610607565b9392505050565b600080604083850312156106be57600080fd5b6106c783610607565b91506106d560208401610607565b90509250929050565b600181811c908216806106f257607f821691505b60208210810361071257634e487b7160e01b600052602260045260246000fd5b50919050565b634e487b7160e01b600052601160045260246000fd5b808201808211156102f5576102f5610718565b818103818111156102f5576102f561071856fea2646970667358221220edf82b9348fffb8edce1ab8c59f653f08bf9e4f4bfb477ef28aae6bd210c284264736f6c63430008130033";
        public ERC20TokenDeploymentBase() : base(BYTECODE) { }
        public ERC20TokenDeploymentBase(string byteCode) : base(byteCode) { }
        [Parameter("uint256", "_initialAmount", 1)]
        public virtual BigInteger InitialAmount { get; set; }
        [Parameter("string", "_tokenName", 2)]
        public virtual string TokenName { get; set; }
        [Parameter("uint8", "_decimalUnits", 3)]
        public virtual byte DecimalUnits { get; set; }
        [Parameter("string", "_tokenSymbol", 4)]
        public virtual string TokenSymbol { get; set; }
    }

    public partial class AllowanceFunction : AllowanceFunctionBase { }

    [Function("allowance", "uint256")]
    public class AllowanceFunctionBase : FunctionMessage
    {
        [Parameter("address", "_owner", 1)]
        public virtual string Owner { get; set; }
        [Parameter("address", "_spender", 2)]
        public virtual string Spender { get; set; }
    }

    public partial class AllowedFunction : AllowedFunctionBase { }

    [Function("allowed", "uint256")]
    public class AllowedFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
        [Parameter("address", "", 2)]
        public virtual string ReturnValue2 { get; set; }
    }

    public partial class ApproveFunction : ApproveFunctionBase { }

    [Function("approve", "bool")]
    public class ApproveFunctionBase : FunctionMessage
    {
        [Parameter("address", "_spender", 1)]
        public virtual string Spender { get; set; }
        [Parameter("uint256", "_value", 2)]
        public virtual BigInteger Value { get; set; }
    }

    public partial class BalanceOfFunction : BalanceOfFunctionBase { }

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunctionBase : FunctionMessage
    {
        [Parameter("address", "_owner", 1)]
        public virtual string Owner { get; set; }
    }

    public partial class BalancesFunction : BalancesFunctionBase { }

    [Function("balances", "uint256")]
    public class BalancesFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class DecimalsFunction : DecimalsFunctionBase { }

    [Function("decimals", "uint8")]
    public class DecimalsFunctionBase : FunctionMessage
    {

    }

    public partial class NameFunction : NameFunctionBase { }

    [Function("name", "string")]
    public class NameFunctionBase : FunctionMessage
    {

    }

    public partial class SymbolFunction : SymbolFunctionBase { }

    [Function("symbol", "string")]
    public class SymbolFunctionBase : FunctionMessage
    {

    }

    public partial class TotalSupplyFunction : TotalSupplyFunctionBase { }

    [Function("totalSupply", "uint256")]
    public class TotalSupplyFunctionBase : FunctionMessage
    {

    }

    public partial class TransferFunction : TransferFunctionBase { }

    [Function("transfer", "bool")]
    public class TransferFunctionBase : FunctionMessage
    {
        [Parameter("address", "_to", 1)]
        public virtual string To { get; set; }
        [Parameter("uint256", "_value", 2)]
        public virtual BigInteger Value { get; set; }
    }

    public partial class TransferFromFunction : TransferFromFunctionBase { }

    [Function("transferFrom", "bool")]
    public class TransferFromFunctionBase : FunctionMessage
    {
        [Parameter("address", "_from", 1)]
        public virtual string From { get; set; }
        [Parameter("address", "_to", 2)]
        public virtual string To { get; set; }
        [Parameter("uint256", "_value", 3)]
        public virtual BigInteger Value { get; set; }
    }

    public partial class ApprovalEventDTO : ApprovalEventDTOBase { }

    [Event("Approval")]
    public class ApprovalEventDTOBase : IEventDTO
    {
        [Parameter("address", "_owner", 1, true )]
        public virtual string Owner { get; set; }
        [Parameter("address", "_spender", 2, true )]
        public virtual string Spender { get; set; }
        [Parameter("uint256", "_value", 3, false )]
        public virtual BigInteger Value { get; set; }
    }

    public partial class TransferEventDTO : TransferEventDTOBase { }

    [Event("Transfer")]
    public class TransferEventDTOBase : IEventDTO
    {
        [Parameter("address", "_from", 1, true )]
        public virtual string From { get; set; }
        [Parameter("address", "_to", 2, true )]
        public virtual string To { get; set; }
        [Parameter("uint256", "_value", 3, false )]
        public virtual BigInteger Value { get; set; }
    }

    public partial class AllowanceOutputDTO : AllowanceOutputDTOBase { }

    [FunctionOutput]
    public class AllowanceOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "remaining", 1)]
        public virtual BigInteger Remaining { get; set; }
    }

    public partial class AllowedOutputDTO : AllowedOutputDTOBase { }

    [FunctionOutput]
    public class AllowedOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }



    public partial class BalanceOfOutputDTO : BalanceOfOutputDTOBase { }

    [FunctionOutput]
    public class BalanceOfOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "balance", 1)]
        public virtual BigInteger Balance { get; set; }
    }

    public partial class BalancesOutputDTO : BalancesOutputDTOBase { }

    [FunctionOutput]
    public class BalancesOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class DecimalsOutputDTO : DecimalsOutputDTOBase { }

    [FunctionOutput]
    public class DecimalsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint8", "", 1)]
        public virtual byte ReturnValue1 { get; set; }
    }

    public partial class NameOutputDTO : NameOutputDTOBase { }

    [FunctionOutput]
    public class NameOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class SymbolOutputDTO : SymbolOutputDTOBase { }

    [FunctionOutput]
    public class SymbolOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class TotalSupplyOutputDTO : TotalSupplyOutputDTOBase { }

    [FunctionOutput]
    public class TotalSupplyOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }




}
