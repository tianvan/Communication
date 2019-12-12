using System;
using System.Collections.Generic;

namespace CommunicationSample
{
    public class Resolver
    {
        private const int Limit = 489;
        private const int TypeIdSize = 1;
        private const int ObjectIdSize = 16;
        private const int ObjectSize = 16;
        private const int StartIndexSize = 2;
        private const int EndIndexSize = 2;

        private readonly IDictionary<Guid, byte[]> _objectMap = new Dictionary<Guid, byte[]>();
        private readonly IDictionary<Guid, Types> _typeMap = new Dictionary<Guid, Types>();
        private readonly IDictionary<Guid, long> _receivedMap = new Dictionary<Guid, long>();

        public void FillObjct(byte[] input)
        {
            if (input.Length < Limit)
            {
                return;
            }

            var typeId = (Types)input.SubArray(0, TypeIdSize)[0];
            var guid = new Guid(input.SubArray(TypeIdSize, ObjectIdSize));
            var totalSize = BitConverter.ToInt32(input.SubArray(TypeIdSize + ObjectIdSize, ObjectSize), 0);

            if (!_objectMap.ContainsKey(guid))
            {
                _objectMap.Add(guid, new byte[totalSize]);
                _typeMap.Add(guid, typeId);
                _receivedMap.Add(guid, 0);
                FillOjectInteral(input, guid, totalSize);
            }
            else
            {
                FillOjectInteral(input, guid, totalSize);
            }
        }

        private void FillOjectInteral(byte[] input, Guid guid, long totalSize)
        {
            var startIndex = BitConverter.ToUInt16(input.SubArray(TypeIdSize + ObjectIdSize + ObjectSize, StartIndexSize), 0);
            var endIndex = BitConverter.ToUInt16(input.SubArray(TypeIdSize + ObjectIdSize + ObjectSize + StartIndexSize, EndIndexSize), 0);
            var payload = input.SubArray(TypeIdSize + ObjectIdSize + ObjectSize + StartIndexSize + EndIndexSize, endIndex - startIndex);
            Array.Copy(payload, 0, _objectMap[guid], startIndex, payload.Length);
            _receivedMap[guid] += payload.Length;

            if (_receivedMap[guid] == totalSize)
            {
                //对象数据已接收完毕
            }
        }
    }
}
