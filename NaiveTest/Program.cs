using Entities;
using EquationSolver;
using EquationSolver.Contracts;
using EquationSolver.Implementation;
using LubyTransform.Decode;
using LubyTransform.Encode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace NaiveTest
{
    class Program
    {
        /*
         * TODO:
         * 1. Indentify P will not be affected as C scale down. -> Yes, we have to regress a alpha
         * 2. Write multi-thread Program. -> I write multiple batch. =.=
         * 3. Go for Prof. Zhang for paper. -> It didn't work out.
         */


        static char Random()
        {
            long tick = DateTime.Now.Ticks; 
            // 26 alphabetical + 10 number
            Random ram = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            char r = (char) ram.Next(36);
            // mapping
            // '0' - '9' remain otherwise + 'a' - '9' bias.
            if (r > 9)
            {
                r += (char) ('a' - '9' - 1);
            }
            return (char) ('0' + r);
        }

        static String initMsg(int size)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < size * 2; i++)
            {
                sb.Append(Random());
                Sleep(1);
            }
            return sb.ToString();
        }

        static int Codec(String sentMessage, int overHead)
        {
            byte[] file = Encoding.ASCII.GetBytes(sentMessage);
            IEncode encoder = new Encode(file);
            int blocksCount = encoder.NumberOfBlocks;
            #if (DEBUG)
            Console.WriteLine("Before encode: " + file.GetLength(0));
            Console.WriteLine("After encode: " + blocksCount + " + " + overHead);
            #endif
            IList<Drop> drops = new List<Drop>();
            for (int i = 0; i < blocksCount + overHead; i++)
            {
                var drop = encoder.Encode();
                drops.Add(drop);
            }
            IMatrixSolver matrixSolver = new MatrixSolver();
            IDecode target = new Decode(matrixSolver);

            //Act
            byte[] actualByte = target.Decode(drops, blocksCount, encoder.ChunkSize, encoder.FileSize);
            var receievdMessage = Encoding.ASCII.GetString(actualByte);

            int isDecode;
            if (sentMessage.Equals(receievdMessage))
            {
                #if (DEBUG)
                Console.WriteLine("Decode Successfully!");
                #endif
                isDecode = 1;
            }
            else
            {
                #if (DEBUG)
                Console.WriteLine("Decode Unsuccessfully =.=");
                #endif
                isDecode = 0;
            }
            return isDecode;
        }

        [DllImport("kernel32.dll")]
        private static extern void Sleep(int dwMilliseconds);

        static void Main(string[] args)
        {
            int overHead = 0;
            int total = 10;

            if (args.GetLength(0) >= 2)
            {
                total = int.Parse( args[0] );
                overHead = int.Parse( args[1] );
            }

            int C = 200;

            String sentMessage = initMsg(C);

            float p = 0;

            #if (DEBUG)
            DateTime Start = DateTime.Now;
            #endif

            for (int i = 0; i < total; i++)
            {
                p += (float) Codec(sentMessage, overHead);
            }

            #if (DEBUG)
            TimeSpan Elapsed = DateTime.Now - Start;
            Console.WriteLine("Time Elapsed: {0} s", Elapsed.TotalSeconds);
            #endif

            p /= ((float) total);
            Console.WriteLine("Probability {0}%", p * 100);
            #if (DEBUG)
            Console.ReadKey();
            #endif
        }
    }
}
