using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace QCloud.Tls.Sig
{
    public class KeyChain
    {
        private string _privateKey;
        private string _publicKey;

        public KeyChain(string privateKeyPath, string publicKeyPath)
        {
            if (string.IsNullOrEmpty(privateKeyPath))
            {
                throw new ArgumentNullException(nameof(privateKeyPath));
            }

            if (string.IsNullOrEmpty(publicKeyPath))
            {
                throw new ArgumentNullException(nameof(publicKeyPath));
            }

            _privateKey = ReadFileContent(privateKeyPath);
            _publicKey = ReadFileContent(publicKeyPath);
        }

        public string PrivateKey
        {
            get
            {
                if (string.IsNullOrEmpty(_privateKey))
                {
                    throw new Exception("need load private key config first. ");
                }

                return _privateKey;
            }
        }

        public string PublicKey
        {
            get
            {
                if (string.IsNullOrEmpty(_publicKey))
                {
                    throw new Exception("need load public key config first. ");
                }

                return _publicKey;
            }
        }

        private string ReadFileContent(string filePath)
        {
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath, Encoding.Default);

                if (lines.Length == 0)
                {
                    throw new Exception($"file[{filePath}] do not have any content");
                }

                var newLines = new List<string>();
                foreach (var line in lines)
                {
                    if (line.StartsWith("-") && line.EndsWith("-"))
                    {
                        continue;
                    }
                    newLines.Add(line);
                }

                return string.Join("\n", newLines);
            }

            return null;
        }
    }
}