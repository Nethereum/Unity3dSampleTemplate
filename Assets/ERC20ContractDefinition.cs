using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;
using Nethereum.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


//NOTE: All Contract definitions can be code generated using the vscode solidity extension, using the .net libraries or directly in the playground using the abi produced by the solidity compiler
//
/// //********* CONTRACT DEFINITION  *******

//*** Deployment message**** //
// To deploy a contract we will create a class inheriting from the ContractDeploymentMessage, 
// here we can include our compiled byte code and other constructor parameters.
// As we can see below the StandardToken deployment message includes the compiled bytecode 
// of the ERC20 smart contract and the constructor parameter with the “totalSupply” of tokens.
// Each parameter is described with an attribute Parameter, including its name "totalSupply", type "uint256" and order.


public class StandardTokenDeployment : ContractDeploymentMessage
{
    public static string BYTECODE =
        "0x60606040526040516020806106f5833981016040528080519060200190919050505b80600160005060003373ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005081905550806000600050819055505b506106868061006f6000396000f360606040523615610074576000357c010000000000000000000000000000000000000000000000000000000090048063095ea7b31461008157806318160ddd146100b657806323b872dd146100d957806370a0823114610117578063a9059cbb14610143578063dd62ed3e1461017857610074565b61007f5b610002565b565b005b6100a060048080359060200190919080359060200190919050506101ad565b6040518082815260200191505060405180910390f35b6100c36004805050610674565b6040518082815260200191505060405180910390f35b6101016004808035906020019091908035906020019091908035906020019091905050610281565b6040518082815260200191505060405180910390f35b61012d600480803590602001909190505061048d565b6040518082815260200191505060405180910390f35b61016260048080359060200190919080359060200190919050506104cb565b6040518082815260200191505060405180910390f35b610197600480803590602001909190803590602001909190505061060b565b6040518082815260200191505060405180910390f35b600081600260005060003373ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005060008573ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020600050819055508273ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff167f8c5be1e5ebec7d5bd14f71427d1e84f3dd0314c0f7b2291e5b200ac8c7c3b925846040518082815260200191505060405180910390a36001905061027b565b92915050565b600081600160005060008673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020600050541015801561031b575081600260005060008673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005060003373ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000505410155b80156103275750600082115b1561047c5781600160005060008573ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000828282505401925050819055508273ffffffffffffffffffffffffffffffffffffffff168473ffffffffffffffffffffffffffffffffffffffff167fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef846040518082815260200191505060405180910390a381600160005060008673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008282825054039250508190555081600260005060008673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005060003373ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000828282505403925050819055506001905061048656610485565b60009050610486565b5b9392505050565b6000600160005060008373ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000505490506104c6565b919050565b600081600160005060003373ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020600050541015801561050c5750600082115b156105fb5781600160005060003373ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008282825054039250508190555081600160005060008573ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000828282505401925050819055508273ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff167fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef846040518082815260200191505060405180910390a36001905061060556610604565b60009050610605565b5b92915050565b6000600260005060008473ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005060008373ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005054905061066e565b92915050565b60006000600050549050610683565b9056";

    public StandardTokenDeployment() : base(BYTECODE)
    {
    }

    [Parameter("uint256", "totalSupply")]
    public BigInteger TotalSupply { get; set; }
}

//*** FUNCTION MESSAGES **** ///

// We can call the functions of smart contract to query the state of a smart contract or do any computation, 
// which will not affect the state of the blockchain.

// To do so,  we will need to create a class which inherits from "FunctionMessage". 
// First we will decorate the class with a "Function" attribute, including the name and return type.
// Each parameter of the function will be a property of the class, each of them decorated with the "Parameter" attribute, 
// including the smart contract’s parameter name, type and parameter order.
// For the ERC20 smart contract, the "balanceOf" function definition, 
// provides the query interface to get the token balance of a given address. 
// As we can see this function includes only one parameter "\_owner", of the type "address".


[Function("balanceOf", "uint256")]
public class BalanceOfFunction : FunctionMessage
{
    [Parameter("address", "_owner", 1)]
    public string Owner { get; set; }
}


// Another type of smart contract function will be a transaction 
// that will change the state of the smart contract (or smart contracts).
// For example The "transfer" function definition for the ERC20 smart contract, 
// includes the parameters “\_to”, which is an address parameter as a string, and the “\_value” 
// or TokenAmount we want to transfer.


// In a similar way to the "balanceOf" function, all the parameters include the solidity type, 
// the contract’s parameter name and parameter order.


// Note: When working with functions, it is very important to have the parameters types and function name correct 
//as all of these make the signature of the function.

[Function("transfer", "bool")]
public class TransferFunction : FunctionMessage
{
    [Parameter("address", "_to", 1)]
    public string To { get; set; }

    [Parameter("uint256", "_value", 2)]
    public BigInteger TokenAmount { get; set; }
}

// Finally, smart contracts also have events. Events defined in smart contracts write in the blockchain log, 
// providing a way to retrieve further information when a smart contract interaction occurs.
// To create an Event definition, we need to create a class that inherits from IEventDTO, decorated with the Event attribute.
// The Transfer Event is similar to a Function: it  also includes parameters with name, order and type. 
// But also a boolean value indicating if the parameter is indexed or not.
// Indexed parameters will allow us later on to query the blockchain for those values.


[Event("Transfer")]
public class TransferEventDTO : IEventDTO
{
    [Parameter("address", "_from", 1, true)]
    public string From { get; set; }

    [Parameter("address", "_to", 2, true)]
    public string To { get; set; }

    [Parameter("uint256", "_value", 3, false)]
    public BigInteger Value { get; set; }

    public static EventABI GetEventABI()
    {
        return EventExtensions.GetEventABI<TransferEventDTO>();
    }
}

// ### Multiple return types or complex objects
// Functions of smart contracts can return one or multiple values in a single call. To decode the returned values, we use a FunctionOutputDTO.
// Function outputs are classes which are decorated with a FunctionOutput attribute and implement the interface IFunctionOutputDTO.
// An example of this is the following implementation that can be used to return the single value of the Balance on the ERC20 smart contract.

[FunctionOutput]
public class BalanceOfOutputDTO : IFunctionOutputDTO
{
    [Parameter("uint256", "balance", 1)]
    public BigInteger Balance { get; set; }
}


