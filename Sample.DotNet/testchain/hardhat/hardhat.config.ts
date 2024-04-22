import { task } from "hardhat/config";

import { config as dotenvConfig } from "dotenv";
import { resolve } from "path";
dotenvConfig({ path: resolve(__dirname, "./.env") });

import { HardhatUserConfig } from "hardhat/types";
import { NetworkUserConfig } from "hardhat/types";

import "@typechain/hardhat";
import "@nomiclabs/hardhat-ethers";
import "@nomiclabs/hardhat-waffle";

import "hardhat-gas-reporter";
import "@nomiclabs/hardhat-etherscan";
require("hardhat-contract-sizer");
require("hardhat-gas-reporter");
require("hardhat-tracer");
require("web3");

const chainIds = {
  ganache: 1337,
  goerli: 5,
  hardhat: 31337,
  kovan: 42,
  mainnet: 1,
  rinkeby: 4,
  ropsten: 3,
};

const MNEMONIC = process.env.MNEMONIC || "test test test test test test test test test test test test";
const ETHERSCAN_API_KEY = process.env.ETHERSCAN_API_KEY || "";
const INFURA_API_KEY = process.env.INFURA_API_KEY || "";
const ALCHEMY_KEY = process.env.ALCHEMY_KEY || "";
const PRIVATE_KEY = process.env.PRIVATE_KEY || "0xc87509a1c067bbde78beb793e6fa76530b6382a4c0241e5e4a9ec0a0f44dc0d5"; //well known private key

// This is a sample Hardhat task. To learn how to create your own go to
// https://hardhat.org/guides/create-task.html
task("accounts", "Prints the list of accounts", async (args, hre) => {
  const accounts = await hre.ethers.getSigners();

  for (const account of accounts) {
    console.log(await account.getAddress());
  }
});

function createTestnetConfig(network: keyof typeof chainIds): NetworkUserConfig {
  const url: string = "https://" + network + ".infura.io/v3/" + INFURA_API_KEY;
  return {
    accounts: {
      count: 10,
      initialIndex: 0,
      mnemonic: MNEMONIC,
      path: "m/44'/60'/0'/0",
    },
    chainId: chainIds[network],
    url,
  };
}

// You need to export an object to set up your config
// Go to https://hardhat.org/config/ to learn more

const config: HardhatUserConfig = {
  defaultNetwork: "hardhat",
  networks: {
    hardhat: {
      accounts: [{privateKey:"b5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7", balance:"10000000000000000000000"},
				 {privateKey:"686afc290b99aa8f2c2f6e5d568fc2e92fbb32349938fc3c43c2691d17433a87", balance:"10000000000000000000000"},
				 {privateKey:"e58adb8d9de87414dcc6741d31d9e84b76a54d3f416c526d8bbc1cd82b1aa2b1", balance:"10000000000000000000000"},
				 {privateKey:"aa5bc10ebf5c73004e990f773608acce451fb919073a6121c2b57043682c132b", balance:"10000000000000000000000"},
				 {privateKey:"aa5bc10ebf5c73004e990f773608acce451fb919073a6121c2b57043682c1332", balance:"10000000000000000000000"},
				 {privateKey:"aa5bc10ebf5c73004e990f773608acce451fb919073a6121c2b57043682c1334", balance:"10000000000000000000000"},
				 {privateKey:"aa5bc10ebf5c73004e990f773608acce451fb919073a6121c2b57043682c1337", balance:"10000000000000000000000"}
				 ],
      chainId: chainIds.hardhat,
      allowUnlimitedContractSize: true,
    },
    mainnet: {
      chainId: createTestnetConfig("mainnet").chainId,
      url: "https://" + "mainnet" + ".infura.io/v3/" + INFURA_API_KEY,
      accounts: [PRIVATE_KEY],
    },
    goerli: createTestnetConfig("goerli"),
    kovan: createTestnetConfig("kovan"),
    rinkeby: {
      chainId: createTestnetConfig("rinkeby").chainId,
      url: "https://" + "rinkeby" + ".infura.io/v3/" + INFURA_API_KEY,
      accounts: [PRIVATE_KEY],
    },
    ropsten: {
      chainId: createTestnetConfig("ropsten").chainId,
      url: "https://" + "ropsten" + ".infura.io/v3/" + INFURA_API_KEY,
      accounts: [PRIVATE_KEY],
    },
    mumbai: {
      chainId: 80001,
      url: "https://rpc-mumbai.maticvigil.com",
      accounts: [PRIVATE_KEY],
    },

    polygon: {
      chainId: 137,
      url: "https://polygon-rpc.com",
      accounts: [PRIVATE_KEY],
    },
  },
  mocha: {
    timeout: 100000000,
    parallel: true,
  },
  solidity: {
    compilers: [
      {
        version: "0.8.7",
        settings: {
          optimizer: {
            enabled: true,
            runs: 200,
          },
        },
      },
    ],
  },
  paths: {
	 artifacts:"../../../contracts/artifacts"
  },
  etherscan: {
    apiKey: ETHERSCAN_API_KEY,
  },
  gasReporter: {
    currency: "USD",
    coinmarketcap: "2311b1d0-dba9-467a-803c-8aa63e2dbbc9",
    enabled: process.env.REPORT_GAS ? true : false,
  },
  typechain: {
    outDir: "typechain",
    target: "ethers-v5",
  },
};

export default config;
