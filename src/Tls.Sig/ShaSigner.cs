using Org.BouncyCastle.Security;
using System;
using System.Text;

namespace Tls.Sig
{
    public static class ShaSigner
    {
        public static byte[] SignSha256ECDSA(string message, string privateKey)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (string.IsNullOrEmpty(privateKey))
            {
                throw new ArgumentNullException(nameof(privateKey));
            }

            var privateKeyParameters = PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKey));

            var signer = SignerUtilities.GetSigner("SHA-256withECDSA");
            signer.Init(true, privateKeyParameters);
            var messageBytes = Encoding.UTF8.GetBytes(message);
            signer.BlockUpdate(messageBytes, 0, messageBytes.Length);
            return signer.GenerateSignature();
        }

        public static bool VerifySha256ECDS(string message, byte[] signature, string publicKey)
        {
            if (signature == null)
            {
                throw new ArgumentNullException(nameof(signature));
            }
            if (string.IsNullOrEmpty(publicKey))
            {
                throw new ArgumentNullException(nameof(publicKey));
            }

            var publicKeyParameters = PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));

            var signer = SignerUtilities.GetSigner("SHA-256withECDSA");
            signer.Init(false, publicKeyParameters);
            var messageBytes = Encoding.UTF8.GetBytes(message);
            signer.BlockUpdate(messageBytes, 0, messageBytes.Length);
            return signer.VerifySignature(signature);
        }
    }
}