using System;
using System.IO;

namespace CompilerTelegramBot
{
    public class FileBinaryChecker
    {
        public static bool IsBinaryFile(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    int bufferSize = 1024;
                    byte[] buffer = new byte[bufferSize];

                    int bytesRead = binaryReader.Read(buffer, 0, bufferSize);

                    for (int i = 0; i < bytesRead; i++)
                    {
                        if (IsControlCharacter(buffer[i]))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private static bool IsControlCharacter(byte value)
        {
            return value < 32 && value != 9 && value != 10 && value != 13;
        }
    }
}