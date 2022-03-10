/**
*** Copyright (c) 2016-2019, Jaguar0625, gimre, BloodyRookie, Tech Bureau, Corp.
*** Copyright (c) 2020-present, Jaguar0625, gimre, BloodyRookie.
*** All rights reserved.
***
*** This file is part of Catapult.
***
*** Catapult is free software: you can redistribute it and/or modify
*** it under the terms of the GNU Lesser General Public License as published by
*** the Free Software Foundation, either version 3 of the License, or
*** (at your option) any later version.
***
*** Catapult is distributed in the hope that it will be useful,
*** but WITHOUT ANY WARRANTY; without even the implied warranty of
*** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
*** GNU Lesser General Public License for more details.
***
*** You should have received a copy of the GNU Lesser General Public License
*** along with Catapult. If not, see <http://www.gnu.org/licenses/>.
**/

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Symbol.Builders {
    /*
    * Binary layout for an importance block footer
    */
    [Serializable]
    public class ImportanceBlockFooterBuilder: ISerializer {

        /* Number of voting eligible accounts. */
        public int votingEligibleAccountsCount;
        /* Number of harvesting eligible accounts. */
        public long harvestingEligibleAccountsCount;
        /* Total balance eligible for voting. */
        public AmountDto totalVotingBalance;
        /* Previous importance block hash. */
        public Hash256Dto previousImportanceBlockHash;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal ImportanceBlockFooterBuilder(BinaryReader stream)
        {
            try {
                votingEligibleAccountsCount = stream.ReadInt32();
                harvestingEligibleAccountsCount = stream.ReadInt64();
                totalVotingBalance = AmountDto.LoadFromBinary(stream);
                previousImportanceBlockHash = Hash256Dto.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of ImportanceBlockFooterBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of ImportanceBlockFooterBuilder.
        */
        public static ImportanceBlockFooterBuilder LoadFromBinary(BinaryReader stream) {
            return new ImportanceBlockFooterBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param votingEligibleAccountsCount Number of voting eligible accounts.
        * @param harvestingEligibleAccountsCount Number of harvesting eligible accounts.
        * @param totalVotingBalance Total balance eligible for voting.
        * @param previousImportanceBlockHash Previous importance block hash.
        */
        internal ImportanceBlockFooterBuilder(int votingEligibleAccountsCount, long harvestingEligibleAccountsCount, AmountDto totalVotingBalance, Hash256Dto previousImportanceBlockHash)
        {
            GeneratorUtils.NotNull(votingEligibleAccountsCount, "votingEligibleAccountsCount is null");
            GeneratorUtils.NotNull(harvestingEligibleAccountsCount, "harvestingEligibleAccountsCount is null");
            GeneratorUtils.NotNull(totalVotingBalance, "totalVotingBalance is null");
            GeneratorUtils.NotNull(previousImportanceBlockHash, "previousImportanceBlockHash is null");
            this.votingEligibleAccountsCount = votingEligibleAccountsCount;
            this.harvestingEligibleAccountsCount = harvestingEligibleAccountsCount;
            this.totalVotingBalance = totalVotingBalance;
            this.previousImportanceBlockHash = previousImportanceBlockHash;
        }
        
        /*
        * Creates an instance of ImportanceBlockFooterBuilder.
        *
        * @param votingEligibleAccountsCount Number of voting eligible accounts.
        * @param harvestingEligibleAccountsCount Number of harvesting eligible accounts.
        * @param totalVotingBalance Total balance eligible for voting.
        * @param previousImportanceBlockHash Previous importance block hash.
        * @return Instance of ImportanceBlockFooterBuilder.
        */
        public static  ImportanceBlockFooterBuilder Create(int votingEligibleAccountsCount, long harvestingEligibleAccountsCount, AmountDto totalVotingBalance, Hash256Dto previousImportanceBlockHash) {
            return new ImportanceBlockFooterBuilder(votingEligibleAccountsCount, harvestingEligibleAccountsCount, totalVotingBalance, previousImportanceBlockHash);
        }

        /*
        * Gets number of voting eligible accounts.
        *
        * @return Number of voting eligible accounts.
        */
        public int GetVotingEligibleAccountsCount() {
            return votingEligibleAccountsCount;
        }

        /*
        * Gets number of harvesting eligible accounts.
        *
        * @return Number of harvesting eligible accounts.
        */
        public long GetHarvestingEligibleAccountsCount() {
            return harvestingEligibleAccountsCount;
        }

        /*
        * Gets total balance eligible for voting.
        *
        * @return Total balance eligible for voting.
        */
        public AmountDto GetTotalVotingBalance() {
            return totalVotingBalance;
        }

        /*
        * Gets previous importance block hash.
        *
        * @return Previous importance block hash.
        */
        public Hash256Dto GetPreviousImportanceBlockHash() {
            return previousImportanceBlockHash;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += 4; // votingEligibleAccountsCount
            size += 8; // harvestingEligibleAccountsCount
            size += totalVotingBalance.GetSize();
            size += previousImportanceBlockHash.GetSize();
            return size;
        }



    
        /*
        * Serializes an object to bytes.
        *
        * @return Serialized bytes.
        */
        public byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(GetVotingEligibleAccountsCount());
            bw.Write(GetHarvestingEligibleAccountsCount());
            var totalVotingBalanceEntityBytes = (totalVotingBalance).Serialize();
            bw.Write(totalVotingBalanceEntityBytes, 0, totalVotingBalanceEntityBytes.Length);
            var previousImportanceBlockHashEntityBytes = (previousImportanceBlockHash).Serialize();
            bw.Write(previousImportanceBlockHashEntityBytes, 0, previousImportanceBlockHashEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
