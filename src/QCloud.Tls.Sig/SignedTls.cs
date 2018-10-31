using Newtonsoft.Json;

namespace QCloud.Tls.Sig
{
    public class SignedTls : Tls
    {
        [JsonProperty("TLS.sig")]
        public string Signature { get; set; }

        [JsonProperty("TLS.version")]
        public string Version
        {
            get
            {
                return "201610110000";
            }
        }

        public void CopyFromTls(Tls tls)
        {
            AccountType = tls.AccountType;
            AppId = tls.AppId;
            AppIdAt3rd = tls.AppIdAt3rd;
            Expired = tls.Expired;
            Identifier = tls.Identifier;
            Time = tls.Time;
        }
    }
}