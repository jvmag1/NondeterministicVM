using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using NondeterministicVM.BLL;

namespace NondeterministicVM.Assembler
{
    public class AsmFileCompiler : IDisposable
    {
        private readonly StreamReader _inputStream;
        private readonly FileStream _outputStream;

        public AsmFileCompiler(string inputFileName, string outputFileName)
        {
            _inputStream = new StreamReader(inputFileName);
            _outputStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write);
        }

        public void Dispose()
        {
            _inputStream.Close();
            _outputStream.Close();
        }

        public void Compile()
        {
            string line = "";
            string mode = "";
            long fileOffset = 0;

            while ((line = _inputStream.ReadLine()) != null)
            {
                if (line.StartsWith("."))
                {
                    var info = line.Split(' ');

                    mode = info[0];
                    fileOffset = Convert.ToInt64(info[1].Trim().Replace("0x", ""), 16);

                    if (fileOffset > _outputStream.Length)
                    {
                        long requiredLength = fileOffset - _outputStream.Length;

                        byte[] emptyBytes = new byte[requiredLength];
                        
                        _outputStream.Write(emptyBytes, 0, emptyBytes.Length);
                    }

                    _outputStream.Seek(fileOffset, SeekOrigin.Begin);
                }
                else
                {
                    var bytes = new byte[0];

                    if (mode == ".code")
                    {
                        bytes = ParseCode(line);
                    }
                    else if (mode == ".data")
                    {
                        bytes = ParseData(line);
                    }

                    _outputStream.Write(bytes, 0, bytes.Length);
                }
            }
        }

        private byte[] ParseCode(string line)
        {
            var result = new List<byte>();

            var match = Regex.Match(line, @"(\w+) (r\d+ ){1,3}(0x[0-9a-fA-F]+ ){0,1}(\d+)%");

            if (match.Success)
            {
                int command = 0, register1 = 0, register2 = 0, register3 = 0, probability = 0;
                int? memoryAddress = null;
                
                var commandText = match.Groups[1].Captures[0].ToString();
                command = (int) Enum.Parse(typeof (OpCodes), commandText);

                var register1Text = match.Groups[2].Captures[0].ToString();
                register1 = Int32.Parse(Regex.Match(register1Text, @"\d+").Value);

                if (match.Groups[2].Captures.Count > 1)
                {
                    var register2Text = match.Groups[2].Captures[1].ToString();
                    register2 = Int32.Parse(Regex.Match(register2Text, @"\d+").Value);
                }

                if (match.Groups[2].Captures.Count > 2)
                {
                    var register3Text = match.Groups[2].Captures[2].ToString();
                    register3 = Int32.Parse(Regex.Match(register3Text, @"\d+").Value);
                }

                if (match.Groups[3].Captures.Count > 0)
                {
                    var hex = match.Groups[3].Captures[0].ToString().Trim().Replace("0x", "");
                    memoryAddress = Convert.ToInt32(hex, 16);
                }

                var probabilityText = match.Groups[4].Captures[0].ToString();
                probability = Int32.Parse(probabilityText);

                result = new List<byte>
                {
                    (byte) command,
                    (byte) register1,
                    (byte) ((register2 << 4) | (register3)),
                    (byte) probability
                };

                if (memoryAddress.HasValue)
                {
                    var bytes = BitConverter.GetBytes(memoryAddress.Value);

                    Array.Reverse(bytes);

                    result.AddRange(bytes);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(line) && !line.StartsWith("#"))
                {
                    Console.WriteLine("Warning: ignoring line '{0}'", line);
                }
            }

            return result.ToArray();
        }

        private byte[] ParseData(string line)
        {
            var result = new byte[0];

            var match = Regex.Match(line, @"0x([0-9a-fA-F]+)");

            if (match.Success)
            {
                var hex = match.Groups[1].Captures[0].ToString();

                result = StrToByteArray(hex);
            }

            var match2 = Regex.Match(line, "\"(.{4})\"");

            if (match2.Success)
            {
                var text = match2.Groups[1].Captures[0].ToString();

                result = new byte[]
                {
                    Convert.ToByte(text[0]),
                    Convert.ToByte(text[1]),
                    Convert.ToByte(text[2]),
                    Convert.ToByte(text[3])
                };
            }

            if (!match.Success && !match2.Success)
            {
                if (!string.IsNullOrEmpty(line) && !line.StartsWith("#"))
                {
                    Console.WriteLine("Warning: ignoring line '{0}'", line);
                }
            }

            return result;
        }

        private byte[] StrToByteArray(string str)
        {
            var hexindex = new Dictionary<string, byte>();
            for (byte i = 0; i < 255; i++)
            {
                hexindex.Add(i.ToString("X2"), i);
            }

            var hexres = new List<byte>();
            for (int i = 0; i < str.Length; i += 2)
            {
                hexres.Add(hexindex[str.Substring(i, 2)]);
            }

            return hexres.ToArray();
        }
    }
}
