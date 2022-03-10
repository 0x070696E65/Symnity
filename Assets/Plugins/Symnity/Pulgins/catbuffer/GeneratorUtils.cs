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
using System.Linq;
using System.Threading;

/*
 * Generator utility class.
 */
namespace Symbol.Builders {
    [Serializable]
    public class GeneratorUtils {

        /*
        * Constructor.
        */
        private GeneratorUtils() {
        }

        /*
        * Throws if the object is null.
        *
        * @param obj Object to to check.
        * @param message Format string message.
        * @param values Format values.
        * @param <T> Type of object.
        */
        public static void NotNull<T>(T obj, string message, params object[] values) {
            if (obj == null) {
                throw new ArgumentNullException(String.Format(message, values));
            }
        }

        /*
        * Throws if the value is not true.
        *
        * @param expression Expression to check.
        * @param message Format string message.
        * @param values Format values.
        */
        public static void IsTrue(bool expression, string message, params object[] values) {
            if (!expression) {
                throw new ArgumentException(String.Format(message, values));
            }
        }

        /*
        * Throws if the value is not false.
        *
        * @param expression Expression to check.
        * @param message Format string message.
        * @param values Format values.
        */
        public static void IsFalse(bool expression, string message, params object[] values) {
            IsTrue(!expression, message, values);
        }

        /*
        * Creates a bitwise representation for an Set.
        *
        * @param enumClass Enum type.
        * @param enumSet EnumSet to convert to bit representation.
        * @param <T> Type of enum.
        * @return Long value of the EnumSet.
        */
        public static long ToLong<T>(List<T> enumSet) where T : struct
        {
            List<T> values = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            IsFalse(values.Count > sizeof(long),
                "The number of enum constants is greater than " + sizeof(long));
            long result = 0;
            foreach (var value in values)
            {
                foreach (var i in enumSet)
                {
                    if ((int)(object)i == (int)(object)value)
                    {
                        result += (int)(object)value;
                    }
                }
            }
            return result;
        }

        /*
        * Creates a EnumSet from from a bit representation.
        *
        * @param enumClass Enum class.
        * @param bitMaskValue Bitmask value.
        * @param <T> Enum type.
        * @return EnumSet representing the long value.
        */
        public static List<T> ToSet<T>(int bitMaskValue) where T : struct
        {
            var values = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            var results = new List<T>();
            foreach (var value in values)
            {
                if (0 != ((int)(object)value & bitMaskValue))
                {
                    results.Add((T)value);
                }
            }
            return results;
        }

        /**
         * Gets a runtime exception to propagates from an exception.
         *
         * @param exception Exception to propagate.
         * @param wrap Function that wraps an exception in a runtime exception.
         * @param <E> Specific exception type.
         * @return RuntimeException to throw.
         */
        public static SystemException GetExceptionToPropagate<E>(Exception exception, Func<Exception, E> wrap) where E : SystemException
        {
            if ((typeof(E) == typeof(SystemException)) && (exception.InnerException != null))
            {
                return (SystemException) exception.InnerException;
            }
            if (typeof(E) == typeof(SystemException))
            {
                return (SystemException) exception;
            }
            if (typeof(E) == typeof(ThreadInterruptedException))
            {
                Thread.CurrentThread.Interrupt();
                return new ThreadInterruptedException(nameof(exception), exception);
            }
            return wrap.Invoke(exception);
        }

        /**
         * Gets a runtime exception to propagates from an exception.
         *
         * @param exception Exception to propagate.
         * @param <E> Specific exception type.
         * @return RuntimeException to throw.
         */
        public static SystemException GetExceptionToPropagate<E>(Exception exception) where E : SystemException
        {
            return GetExceptionToPropagate(exception, _ => new SystemException());
        }

        /**
         * Propagates checked exceptions as a specific runtime exception.
         *
         * @param callable Function to call.
         * @param wrap Function that wraps an exception in a runtime exception.
         * @param <T> Return type.
         * @param <E> Specific exception type.
         * @return Function result.
         */
        public static T Propagate<E, T>(Func<T> callable, Func<Exception, E> wrap) where E : SystemException
        {
            try {
                return callable.Invoke();
            } catch (Exception e) {
                throw GetExceptionToPropagate(e, wrap);
            }
        }

        /**
         * Propagates checked exceptions as a runtime exception.
         *
         * @param callable Function to call.
         * @param <T> Function return type.
         * @return Function result.
         */
        public static T Propagate<T>(Func<T> callable)
        {
            return Propagate(callable, _ => new SystemException());
        }



        /**
         * Throwing consumer interface.
         *
         * @param <T> Input type.
         * @param <E> Exception that is thrown.
         */
        public interface ThrowingConsumer<T, E>
        {

            /**
             * Performs operation on the given argument.
             *
             * @param t Input argument.
             * @throws E Exception that is thrown.
             */
            void Accept(T t);
        }

        /**
         * Serializes data using a helper function to write to the stream.
         *
         * @param consumer Helper function that writes data to MemoryStream.
         * @return Byte array of data written.
         */
        public static byte[] Serialize(ThrowingConsumer<MemoryStream, Exception> consumer)
        {
            return Propagate(() => {
                using var memoryStream = new MemoryStream();
                consumer.Accept(memoryStream);
                return memoryStream.ToArray();
            });
        }


        /*
        * It moves the output stream pointer the padding size calculated from the payload size
        *
        * @param size the payload size used to calcualted the padding
        * @param stream the input stream that will be moved the calcauted padding size
        * @param alignment Next multiple alignment
        */
        public static void SkipPadding(int size, BinaryReader dataInputStream, int alignment)
        {
            var padding = GetPadding(size, alignment);
            dataInputStream.BaseStream.Position += padding;
        }

        /*
        * This method writes 0 into the dataOutputStream. The amount of 0s is the calculated padding
        * size from provided payload size.
        *
        * @param size the payload size used to calcualted the padding
        * @param dataOutputStream used to write the 0s.
        * @param alignment Next multiple alignment
        */
        public static void AddPadding(int size, BinaryWriter dataOutputStream,
            int alignment)
        {
            var padding = GetPadding(size, alignment);
            while (padding > 0)
            {
                dataOutputStream.Write((byte)0);
                padding--;
            }
        }

        /*
        * It calcualtes the padding that needs to be added/skipped when processing inner transactions.
        *
        * @param size the size of the payload using to calculate the padding
        * @param alignment Next multiple alignment
        * @return the padding to be added/skipped.
        */
        public static int GetPadding(int size, int alignment)
        {
            if (alignment == 0)
            {
                return 0;
            }
            return 0 == size % alignment ? 0 : alignment - (size % alignment);
        }

        /*
        * It reads count elements from the stream and creates a list using the builder
        *
        * @param builder the builder
        * @param stream the stream
        * @param count the elements to be read
        * @param alignment Next multiple alignment
        * @param <T> the the type to be returned
        * @return a list of T.
        */
        public static List<T> LoadFromBinaryArray<T>(
            Func<BinaryReader, T> builder,
            BinaryReader stream,
            long count,
            int alignment
        ) where T : ISerializer
        {
            var list = new List<T>();
            for (var i = 0; i < count; i++)
            {
                var entity = builder.Invoke(stream);
                list.Add(entity);
                SkipPadding(entity.GetSize(), stream, alignment);
            }
            return list;
        }

        /*
        * It reads all the remaining entities until the end.
        *
        * @param builder the entity builder
        * @param stream the stream to read from
        * @param alignment alignment Next multiple alignment
        * @param <T> the type of the entity
        * @return a list of entities
        * @throws IOException when data cannot be loaded.
        */
        public static List<T> LoadFromBinaryArrayRemaining<T>(
            Func<BinaryReader, T> builder, BinaryReader stream,
            int alignment) where T : ISerializer
            {
                var entities = new List<T>();
                var ms = new MemoryStream(); 
                stream.BaseStream.CopyTo(ms);
                var ms2 = new MemoryStream();
                var bw = new BinaryWriter(ms2);
                var br = new BinaryReader(ms2);
                bw.Write(ms.ToArray());
                ms2.Position = 0;
                while (ms2.Position < ms2.Length){
                    var entity = builder(br);
                    entities.Add(entity);
                    SkipPadding(entity.GetSize(), br, alignment);
                }
            return entities;
            }

        /*
        * It reads all the remaining entities using the total payload size.
        *
        * @param builder the entity builder
        * @param stream the stream to read from
        * @param payloadSize the payload size
        * @param alignment alignment Next multiple alignment
        * @param <T> the type of the entity
        * @return a list of entities
        * @throws IOException when data cannot be loaded.
        */
        public static List<T> LoadFromBinaryArrayRemaining<T>(
            Func<BinaryReader, T> builder, BinaryReader stream, int payloadSize,
            int alignment) where T : ISerializer
        {
            var entities = new List<T>();
            if (payloadSize != 0)
            {
                var remainingByteSizes = payloadSize;
                while (remainingByteSizes > 0){
                    var entity = builder(stream);
                    entities.Add(entity);
                    var size = entity.GetSize();
                    var itemSize = size + GetPadding(size, alignment);
                    remainingByteSizes -= itemSize;
                    SkipPadding(size, stream, alignment);
                }
            }
            return entities;
        }
        
        /*
        * Write a list of catbuffer entities into the writer.
        *
        * @param dataOutputStream the stream to serialize into
        * @param entities the entities to be serialized
        * @param alignment alignment Next multiple alignment
        * @throws IOException when data cannot be written.
        */
        public static void WriteList<T>(BinaryWriter dataOutputStream,
        List<T> entities, int alignment) where T : ISerializer
        {
            foreach (var entity in entities)
            {
                var entityBytes = entity.Serialize();
                dataOutputStream.Write(entityBytes, 0, entityBytes.Length);
                AddPadding(entityBytes.Length, dataOutputStream, alignment);
            }
        }

        /*
        * Write a serializer into the writer.
        *
        * @param dataOutputStream the stream to serialize into
        * @param entity the entities to be serialized
        * @throws IOException when data cannot be written.
        */
        public static void WriteEntity<T>(BinaryWriter dataOutputStream, T entity) where T : ISerializer
        {
            var entityBytes = entity.Serialize();
            dataOutputStream.Write(entityBytes, 0, entityBytes.Length);
        }

        /*
        * Read a {@link BinaryReader} of the given size form the strem
        *
        * @param stream the stream
        * @param size the size of the buffer to read
        * @return the buffer
        * @throws IOException when data cannot be read
        */
        public static byte[] ReadBytes(BinaryReader stream, int size)
        {
            var buffer = stream.ReadBytes(size);
            return buffer;
        }

        /*
        * Returns the size of the buffer.
        *
        * @param buffer the buffer
        * @return its size
        */
        public static int GetSize(byte[] buffer) {
            return buffer.Length;
        }
        
        /*
        * Returns the size of the collection
        *
        * @param collection the collection
        * @return the size.
        */
        public static int GetSize<T>(List<T> collection)
        {
            return collection.Count;
        }

        /*
        * Returns the size of the collection
        *
        * @param collection the collection
        * @param alignment alignment Next multiple alignment
        * @return the size.
        */
        public static int GetSumSize<T>(List<T> collection, int alignment) where T : ISerializer
        {
            var size = 0;
            foreach (var i in collection)
            {
                var s = i.GetSize();
                size += s + GetPadding(s, alignment);
            }
            return size;
        }

        /*
        * Basic to hex function that converts a byte array to an hex
        *
        * @param bytes the bytes
        * @return the hex representation.
        */
        public static string ToHex(byte[] bytes)
        {
            var str = BitConverter.ToString(bytes);
            str = str.Replace("-", string.Empty);
            return str;
        }

        /*
        * Basic from hex to byte array function.
        *
        * @param hex the hex string
        * @return the byte array.
        */
        public static byte[] HexToBytes(string hex)
        {
            var bs = new List<byte>();
            for (var i = 0; i < hex.Length / 2; i++)
            {
                bs.Add(Convert.ToByte(hex.Substring(i * 2, 2), 16));
            }
            return bs.ToArray();
        }
    }
}

