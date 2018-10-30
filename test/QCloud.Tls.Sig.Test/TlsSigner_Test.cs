using Shouldly;
using Xunit;

namespace Tls.Sig.Test
{
    public class TlsSigner_Test
    {
        [Fact]
        public void Signer_Test()
        {
            //Arrange
            var privateKeyPath = "private_key";
            var publicKeyPath = "public_key";
            var identity = "test";
            var appId = "140013494912";
            var tlsSigner = new TlsSigner(appId, privateKeyPath, publicKeyPath);

            //Act
            var signature = tlsSigner.Sign(identity);
            var verify = tlsSigner.Verify(identity, signature);

            //Assert
            verify.ShouldBeTrue();
        }
    }
}