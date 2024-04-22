using System.Numerics;
using Nethereum.Signer;
using Nethereum.Util;
using Nethereum.Web3.Accounts;
using Nethereum.XUnitEthereumClients;
using Xunit;
using Nethereum.Web3;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Nethereum.BlockchainProcessing.ProgressRepositories;
using Sample.DotNet.Contracts.ERC20Token.ContractDefinition;
using Sample.DotNet.Contracts.ERC20Token;

namespace Sample.DotNet.Tests
{
    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_DEFAULT)]
    public class Erc20TokenTests
    {
        private readonly EthereumClientIntegrationFixture _ethereumClientIntegrationFixture;

        public Erc20TokenTests(EthereumClientIntegrationFixture ethereumClientIntegrationFixture)
        {
            _ethereumClientIntegrationFixture = ethereumClientIntegrationFixture;
        }

        [Fact]
        public async void ShouldTransferToken()
        {

            var destinationAddress = "0x6C547791C3573c2093d81b919350DB1094707011";
            //You can connect to Infura directly if wanted using GetInfuraWeb3
            //var web3 = _ethereumClientIntegrationFixture.GetInfuraWeb3(InfuraNetwork.Ropsten);
            var web3 = _ethereumClientIntegrationFixture.GetWeb3();

            //Example of using Legacy instead of 1559
            if(_ethereumClientIntegrationFixture.EthereumClient == EthereumClient.Ganache)
            {
                web3.TransactionManager.UseLegacyAsDefault = true;
            }

            var erc20TokenDeployment = new ERC20TokenDeployment() { DecimalUnits = 18, TokenName = "TST", TokenSymbol = "TST", InitialAmount = Web3.Convert.ToWei(10000) };

            //Deploy our custom token
            var tokenDeploymentReceipt = await ERC20TokenService.DeployContractAndWaitForReceiptAsync(web3, erc20TokenDeployment);
            
            //Creating a new service
            var tokenService = new ERC20TokenService(web3, tokenDeploymentReceipt.ContractAddress);

            //using Web3.Convert.ToWei as it has 18 decimal places (default)
            var transferReceipt = await tokenService.TransferRequestAndWaitForReceiptAsync(destinationAddress, Web3.Convert.ToWei(10, 18));
            
            //validate the current balance
            var balance = await tokenService.BalanceOfQueryAsync(destinationAddress);
            Assert.Equal(10, Web3.Convert.FromWei(balance));

            //retrieving the event from the receipt
            var eventTransfer = transferReceipt.DecodeAllEvents<TransferEventDTO>()[0];

            Assert.Equal(10, Web3.Convert.FromWei(eventTransfer.Event.Value));
            Assert.True(destinationAddress.IsTheSameAddress(eventTransfer.Event.To));

        }


        [Fact]
        public async void ShouldGetTransferEventLogs()
        {

            var destinationAddress = "0x6C547791C3573c2093d81b919350DB1094707011";
            //Using ropsten infura if wanted for only a single test, as opposed to configuration
            //var web3 = _ethereumClientIntegrationFixture.GetInfuraWeb3(InfuraNetwork.Ropsten);
            var web3 = _ethereumClientIntegrationFixture.GetWeb3();

            //Example of using Legacy instead of 1559
            if (_ethereumClientIntegrationFixture.EthereumClient == EthereumClient.Ganache)
            {
                web3.TransactionManager.UseLegacyAsDefault = true;
            }

            var erc20TokenDeployment = new ERC20TokenDeployment() { DecimalUnits = 18, TokenName = "TST", TokenSymbol = "TST", InitialAmount = Web3.Convert.ToWei(10000) };

            //Deploy our custom token
            var tokenDeploymentReceipt = await ERC20TokenService.DeployContractAndWaitForReceiptAsync(web3, erc20TokenDeployment);
            
            //Creating a new service
            var tokenService = new ERC20TokenService(web3, tokenDeploymentReceipt.ContractAddress);

            //using Web3.Convert.ToWei as it has 18 decimal places (default)
            var transferReceipt1 = await tokenService.TransferRequestAndWaitForReceiptAsync(destinationAddress, Web3.Convert.ToWei(10, 18));
            var transferReceipt2 = await tokenService.TransferRequestAndWaitForReceiptAsync(destinationAddress, Web3.Convert.ToWei(10, 18));

            var transferEvent = web3.Eth.GetEvent<TransferEventDTO>();
            var transferFilter = transferEvent.GetFilterBuilder().AddTopic(x => x.To, destinationAddress).Build(tokenService.ContractHandler.ContractAddress,
                new BlockRange(transferReceipt1.BlockNumber, transferReceipt2.BlockNumber));
         
            var transferEvents = await transferEvent.GetAllChangesAsync(transferFilter);

            Assert.Equal(2, transferEvents.Count); 

        }


        [Fact]
        public async void ShouldGetTransferEventLogsUsingProcessorAndStoreThem()
        {

            var destinationAddress = "0x6C547791C3573c2093d81b919350DB1094707011";
            var web3 = _ethereumClientIntegrationFixture.GetWeb3();

            //Example of using Legacy instead of 1559
            if (_ethereumClientIntegrationFixture.EthereumClient == EthereumClient.Ganache)
            {
                web3.TransactionManager.UseLegacyAsDefault = true;
            }

            var erc20TokenDeployment = new ERC20TokenDeployment() { DecimalUnits = 18, TokenName = "TST", TokenSymbol = "TST", InitialAmount = Web3.Convert.ToWei(10000) };

            //Deploy our custom token
            var tokenDeploymentReceipt = await ERC20TokenService.DeployContractAndWaitForReceiptAsync(web3, erc20TokenDeployment);

            //Creating a new service
            var tokenService = new ERC20TokenService(web3, tokenDeploymentReceipt.ContractAddress);

            //using Web3.Convert.ToWei as it has 18 decimal places (default)
            var transferReceipt1 = await tokenService.TransferRequestAndWaitForReceiptAsync(destinationAddress, Web3.Convert.ToWei(10, 18));
            var transferReceipt2 = await tokenService.TransferRequestAndWaitForReceiptAsync(destinationAddress, Web3.Convert.ToWei(10, 18));


            //We are storing in a database the logs
            var storedMockedEvents = new List<EventLog<TransferEventDTO>>();
            //storage action mock
            Task StoreLogAsync(EventLog<TransferEventDTO> eventLog)
            {
                storedMockedEvents.Add(eventLog);
                return Task.CompletedTask;
            }

            //progress repository to restart processing (simple in memory one, use the other adapters for other storage possibilities)
            var blockProgressRepository = new InMemoryBlockchainProgressRepository(transferReceipt1.BlockNumber.Value - 1);

            //create our processor to retrieve transfers
            //restrict the processor to Transfers for a specific contract address
            var processor = web3.Processing.Logs.CreateProcessorForContract<TransferEventDTO>(
                tokenService.ContractHandler.ContractAddress, //the contract to monitor
                StoreLogAsync, //action to perform when a log is found
                minimumBlockConfirmations: 0,  // number of block confirmations to wait
                blockProgressRepository: blockProgressRepository //repository to track the progress
                );

            //if we need to stop the processor mid execution - call cancel on the token
            var cancellationToken = new CancellationToken();

            //crawl the required block range
            await processor.ExecuteAsync(
                cancellationToken: cancellationToken,
                toBlockNumber: transferReceipt2.BlockNumber.Value,
                startAtBlockNumberIfNotProcessed: transferReceipt1.BlockNumber.Value);

            Assert.Equal(2, storedMockedEvents.Count);

        }

    }
}