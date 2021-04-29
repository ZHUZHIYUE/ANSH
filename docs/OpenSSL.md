# OpenSSL  

## 生成SSL证书  

### 1. 生成CA证书  

生成CA私钥  

```bash
#!/bin/bash
$ openssl genrsa -out ca.key 1024
```  

生成证书请求文件CSR  

```bash
#!/bin/bash
$ openssl req -new -key ca.key -out ca.csr
```  

生成证书  

```bash
#!/bin/bash
$ openssl x509 -req -in ca.csr -signkey ca.key -out ca.crt
```

### 2. 生成服务器端证书

生成服务器端私钥  
  
```bash
#!/bin/bash
$ openssl genrsa -out server.key 1024
```  

生成证书请求文件CSR  

```bash
#!/bin/bash
$ openssl req -new -key server.key -out server.csr
```  

生成证书

```bash
#!/bin/bash
$ openssl x509 -req -CA ca.crt -CAkey ca.key -CAcreateserial -in server.csr -out server.crt
```
