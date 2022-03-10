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
    * Binary layout for non-historical root namespace history
    */
    [Serializable]
    public class RootNamespaceHistoryBuilder: StateHeaderBuilder {

        /* Id of the root namespace history. */
        public NamespaceIdDto id;
        /* Namespace owner address. */
        public AddressDto ownerAddress;
        /* Lifetime in blocks. */
        public NamespaceLifetimeBuilder lifetime;
        /* Root namespace alias. */
        public NamespaceAliasBuilder rootAlias;
        /* Save child sub-namespace paths. */
        public List<NamespacePathBuilder> paths;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal RootNamespaceHistoryBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                id = NamespaceIdDto.LoadFromBinary(stream);
                ownerAddress = AddressDto.LoadFromBinary(stream);
                lifetime = NamespaceLifetimeBuilder.LoadFromBinary(stream);
                rootAlias = NamespaceAliasBuilder.LoadFromBinary(stream);
                var childrenCount = stream.ReadInt64();
                paths = GeneratorUtils.LoadFromBinaryArray(NamespacePathBuilder.LoadFromBinary, stream, childrenCount, 0);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of RootNamespaceHistoryBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of RootNamespaceHistoryBuilder.
        */
        public new static RootNamespaceHistoryBuilder LoadFromBinary(BinaryReader stream) {
            return new RootNamespaceHistoryBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param version Serialization version.
        * @param id Id of the root namespace history.
        * @param ownerAddress Namespace owner address.
        * @param lifetime Lifetime in blocks.
        * @param rootAlias Root namespace alias.
        * @param paths Save child sub-namespace paths.
        */
        internal RootNamespaceHistoryBuilder(short version, NamespaceIdDto id, AddressDto ownerAddress, NamespaceLifetimeBuilder lifetime, NamespaceAliasBuilder rootAlias, List<NamespacePathBuilder> paths)
            : base(version)
        {
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(id, "id is null");
            GeneratorUtils.NotNull(ownerAddress, "ownerAddress is null");
            GeneratorUtils.NotNull(lifetime, "lifetime is null");
            GeneratorUtils.NotNull(rootAlias, "rootAlias is null");
            GeneratorUtils.NotNull(paths, "paths is null");
            this.id = id;
            this.ownerAddress = ownerAddress;
            this.lifetime = lifetime;
            this.rootAlias = rootAlias;
            this.paths = paths;
        }
        
        /*
        * Creates an instance of RootNamespaceHistoryBuilder.
        *
        * @param version Serialization version.
        * @param id Id of the root namespace history.
        * @param ownerAddress Namespace owner address.
        * @param lifetime Lifetime in blocks.
        * @param rootAlias Root namespace alias.
        * @param paths Save child sub-namespace paths.
        * @return Instance of RootNamespaceHistoryBuilder.
        */
        public static  RootNamespaceHistoryBuilder Create(short version, NamespaceIdDto id, AddressDto ownerAddress, NamespaceLifetimeBuilder lifetime, NamespaceAliasBuilder rootAlias, List<NamespacePathBuilder> paths) {
            return new RootNamespaceHistoryBuilder(version, id, ownerAddress, lifetime, rootAlias, paths);
        }

        /*
        * Gets id of the root namespace history.
        *
        * @return Id of the root namespace history.
        */
        public NamespaceIdDto GetId() {
            return id;
        }

        /*
        * Gets namespace owner address.
        *
        * @return Namespace owner address.
        */
        public AddressDto GetOwnerAddress() {
            return ownerAddress;
        }

        /*
        * Gets lifetime in blocks.
        *
        * @return Lifetime in blocks.
        */
        public NamespaceLifetimeBuilder GetLifetime() {
            return lifetime;
        }

        /*
        * Gets root namespace alias.
        *
        * @return Root namespace alias.
        */
        public NamespaceAliasBuilder GetRootAlias() {
            return rootAlias;
        }

        /*
        * Gets save child sub-namespace paths.
        *
        * @return Save child sub-namespace paths.
        */
        public List<NamespacePathBuilder> GetPaths() {
            return paths;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public new int GetSize() {
            var size = base.GetSize();
            size += id.GetSize();
            size += ownerAddress.GetSize();
            size += lifetime.GetSize();
            size += rootAlias.GetSize();
            size += 8; // childrenCount
            size +=  GeneratorUtils.GetSumSize(paths, 0);
            return size;
        }



    
        /*
        * Serializes an object to bytes.
        *
        * @return Serialized bytes.
        */
        public new byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            var superBytes = base.Serialize();
            bw.Write(superBytes, 0, superBytes.Length);
            var idEntityBytes = (id).Serialize();
            bw.Write(idEntityBytes, 0, idEntityBytes.Length);
            var ownerAddressEntityBytes = (ownerAddress).Serialize();
            bw.Write(ownerAddressEntityBytes, 0, ownerAddressEntityBytes.Length);
            var lifetimeEntityBytes = (lifetime).Serialize();
            bw.Write(lifetimeEntityBytes, 0, lifetimeEntityBytes.Length);
            var rootAliasEntityBytes = (rootAlias).Serialize();
            bw.Write(rootAliasEntityBytes, 0, rootAliasEntityBytes.Length);
            bw.Write((long)GeneratorUtils.GetSize(GetPaths()));
            paths.ForEach(entity =>
            {
                var entityBytes = entity.Serialize();
                bw.Write(entityBytes, 0, entityBytes.Length);
                GeneratorUtils.AddPadding(entityBytes.Length, bw, 0);
            });
            var result = ms.ToArray();
            return result;
        }
    }
}
