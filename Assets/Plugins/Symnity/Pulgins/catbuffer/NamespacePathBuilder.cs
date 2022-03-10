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
    * Binary layout for a namespace path
    */
    [Serializable]
    public class NamespacePathBuilder: ISerializer {

        /* Namespace path (excluding root id). */
        public List<NamespaceIdDto> path;
        /* Namespace alias. */
        public NamespaceAliasBuilder alias;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal NamespacePathBuilder(BinaryReader stream)
        {
            try {
                var pathSize = stream.ReadByte();
                path = GeneratorUtils.LoadFromBinaryArray(NamespaceIdDto.LoadFromBinary, stream, pathSize, 0);
                alias = NamespaceAliasBuilder.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of NamespacePathBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of NamespacePathBuilder.
        */
        public static NamespacePathBuilder LoadFromBinary(BinaryReader stream) {
            return new NamespacePathBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param path Namespace path (excluding root id).
        * @param alias Namespace alias.
        */
        internal NamespacePathBuilder(List<NamespaceIdDto> path, NamespaceAliasBuilder alias)
        {
            GeneratorUtils.NotNull(path, "path is null");
            GeneratorUtils.NotNull(alias, "alias is null");
            this.path = path;
            this.alias = alias;
        }
        
        /*
        * Creates an instance of NamespacePathBuilder.
        *
        * @param path Namespace path (excluding root id).
        * @param alias Namespace alias.
        * @return Instance of NamespacePathBuilder.
        */
        public static  NamespacePathBuilder Create(List<NamespaceIdDto> path, NamespaceAliasBuilder alias) {
            return new NamespacePathBuilder(path, alias);
        }

        /*
        * Gets namespace path (excluding root id).
        *
        * @return Namespace path (excluding root id).
        */
        public List<NamespaceIdDto> GetPath() {
            return path;
        }

        /*
        * Gets namespace alias.
        *
        * @return Namespace alias.
        */
        public NamespaceAliasBuilder GetAlias() {
            return alias;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += 1; // pathSize
            size +=  GeneratorUtils.GetSumSize(path, 0);
            size += alias.GetSize();
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
            bw.Write((byte)GeneratorUtils.GetSize(GetPath()));
            path.ForEach(entity =>
            {
                var entityBytes = entity.Serialize();
                bw.Write(entityBytes, 0, entityBytes.Length);
                GeneratorUtils.AddPadding(entityBytes.Length, bw, 0);
            });
            var aliasEntityBytes = (alias).Serialize();
            bw.Write(aliasEntityBytes, 0, aliasEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
