﻿namespace LubyTransform.Encode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Entities;

    public class Encode : IEncode
    {
        #region Member Variables

        readonly IEnumerable<byte[]> blocks;
        readonly int degree;
        readonly Random rand;
        readonly int fileSize;
        const int chunkSize = 2;

        #endregion

        #region Constructor

        public Encode(byte[] file)
        {
            rand = new Random();
            fileSize = file.Length;
            blocks = CreateBlocks(file);
            degree = blocks.Count() / 2;
            degree += 2;
        }

        #endregion

        #region IEncode

        Drop IEncode.Encode()
        {
            int[] selectedParts = GetSelectedParts();
            byte[] data;

            if (selectedParts.Count() > 1)
            {
                data = CreateDropData(selectedParts, blocks, chunkSize);
            }
            else
            {
                data = blocks.ElementAt(selectedParts[0]);
            }

            return new Drop { SelectedParts = selectedParts, Data = data };
        }

        int IEncode.NumberOfBlocks
        {
            get { return blocks.Count(); }
        }

        int IEncode.ChunkSize
        {
            get { return chunkSize; }
        }

        int IEncode.FileSize
        {
            get { return fileSize; }
        }

        #endregion

        #region Private Methods

        private IEnumerable<byte[]> CreateBlocks(byte[] file)
        {
            var size = chunkSize;
            var blocksCount = Math.Ceiling((decimal)file.Length / size);
            var remainingSize = file.Length;

            for (int i = 0; i < blocksCount; i++)
            {
                if (remainingSize > size)
                {
                    remainingSize -= size;
                }
                else
                {
                    size = remainingSize;
                }

                var block = file.Skip(i * chunkSize).Take(size).ToArray();

                if (block.Length >= chunkSize)
                {
                    yield return block;
                }
                else
                {
                    var chunk = new byte[chunkSize];
                    Array.Copy(block, 0, chunk, 0, block.Length);
                    yield return chunk;
                }
            }
        }

        private int[] GetSelectedParts()
        {
            int length = rand.Next(1, degree);
            var selectedParts = new Dictionary<int, int>();
            for (int j = 0; j < length; j++)
            {
                while (true)
                {
                    var part = rand.Next(blocks.Count());
                    if (!selectedParts.ContainsKey(part))
                    {
                        selectedParts.Add(part, part);
                        break;
                    }
                }
            }
            return selectedParts.Keys.ToArray();
        }

        private byte[] CreateDropData(IList<int> selectedParts, IEnumerable<byte[]> blocks, int chunkSize)
        {
            var data = new byte[chunkSize];

            for (int i = 0; i < chunkSize; i++)
            {
                data[i] = XOROperation(i, selectedParts, blocks);
            }

            return data;
        }

        private byte XOROperation(int idx, IList<int> selectedParts, IEnumerable<byte[]> blocks)
        {
            var selectedBlock = blocks.ElementAt(selectedParts[0]);
            byte result = selectedBlock[idx];

            for (int i = 1; i < selectedParts.Count; i++)
            {
                result ^= blocks.ElementAt(selectedParts[i])[idx];
            }

            return result;
        }


        #endregion
    }
}