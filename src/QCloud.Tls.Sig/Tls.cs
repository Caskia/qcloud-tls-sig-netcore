using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Tls.Sig
{
    public class Tls
    {
        [JsonProperty("TLS.account_type", Order = 2)]
        public string AccountType { get; set; }

        [JsonProperty("TLS.sdk_appid", Order = 4)]
        public string AppId { get; set; }

        [JsonProperty("TLS.appid_at_3rd", Order = 1)]
        public string AppIdAt3rd { get; set; }

        [JsonProperty("TLS.expire_after", Order = 6)]
        public string Expired { get; set; }

        [JsonProperty("TLS.identifier", Order = 3)]
        public string Identifier { get; set; }

        [JsonProperty("TLS.time", Order = 5)]
        public string Time { get; set; }

        public string ToSignContent()
        {
            var json = JsonConvert.SerializeObject(this);
            var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return string.Join("", dic.Select(d => $"{d.Key}:{d.Value}\n"));
        }
    }
}