# qcloud-tls-sig-netcore
腾讯云TLS签名工具


Example
```
var tlsSigner = new TlsSigner(appId, privateKeyPath, publicKeyPath);
var signature = tlsSigner.Sign(identity);
```