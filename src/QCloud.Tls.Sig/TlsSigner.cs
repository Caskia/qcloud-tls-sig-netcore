using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace QCloud.Tls.Sig
{
    public class TlsSigner
    {
        private readonly string _appId;
        private readonly KeyChain _keyChain;
        private string _expired = (180 * 24 * 3600).ToString();

        public TlsSigner(string appId, string privateKeyPath, string publicKeyPath)
        {
            if (string.IsNullOrEmpty(appId))
            {
                throw new ArgumentNullException(nameof(appId));
            }

            _appId = appId;
            _keyChain = new KeyChain(privateKeyPath, publicKeyPath);
        }

        public string Sign(string identity)
        {
            var tls = new Tls()
            {
                AccountType = "0",
                AppId = _appId,
                AppIdAt3rd = "0",
                Expired = _expired,
                Identifier = identity,
                Time = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
            };

            var content = tls.ToSignContent();

            var signedTls = new SignedTls();
            signedTls.CopyFromTls(tls);
            signedTls.Signature = Convert.ToBase64String(ShaSigner.SignSha256ECDSA(content, _keyChain.PrivateKey));

            var signedTlsJson = JsonConvert.SerializeObject(signedTls);
            var signedTlsByte = Encoding.UTF8.GetBytes(signedTlsJson);
            var signedTlsCompressed = ZipHelper.Compress(signedTlsByte);
            return ToBase64UrlString(signedTlsCompressed);
        }

        public bool Verify(string identity, string signature)
        {
            var signedTlsCompressed = FromBase64UrlString(signature);
            var signedTlsByte = ZipHelper.Decompress(signedTlsCompressed);
            var signedTlsJson = Encoding.UTF8.GetString(signedTlsByte);

            var signedTls = JsonConvert.DeserializeObject<SignedTls>(signedTlsJson);
            if (signedTls.Identifier != identity)
            {
                return false;
            }

            if (signedTls.AppId != _appId)
            {
                return false;
            }

            var signaturedBytes = Convert.FromBase64String(signedTls.Signature);
            var tls = new Tls()
            {
                AccountType = signedTls.AccountType,
                AppId = signedTls.AppId,
                AppIdAt3rd = signedTls.AppIdAt3rd,
                Expired = signedTls.Expired,
                Identifier = signedTls.Identifier,
                Time = signedTls.Time
            };
            return ShaSigner.VerifySha256ECDSA(tls.ToSignContent(), signaturedBytes, _keyChain.PublicKey);
        }

        private byte[] FromBase64UrlString(string str)
        {
            var incoming = str.Replace('_', '=').Replace('-', '/').Replace('*', '+');
            switch (str.Length % 4)
            {
                case 2: incoming += "=="; break;
                case 3: incoming += "="; break;
            }
            return Convert.FromBase64String(incoming);
        }

        private string ToBase64UrlString(byte[] bits)
        {
            return Convert.ToBase64String(bits).Replace('+', '*').Replace('/', '-').Replace('=', '_');
        }
    }
}