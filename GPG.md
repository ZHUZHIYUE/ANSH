### GPG
#### 什么是GPG
1991年，程序员Phil Zimmermann为了避开政府的监视，开发了加密软件PGP。因为这个软件非常好用，迅速流传开来成为许多程序员的必备工具。但是，它是商业软不能自由使用。所以，自由软件基金会决定，开发一个PGP的替代品取名为GnuPG，因此GPG就诞生了。GPG是GNU Privacy Guard的缩写，是自由软件基金会的GNU计划的一部分。它是一种基于密钥的加密方式，使用了一对密钥对消息进行加密和解密，来保证消息的安全传输。一开始，用户通过数字证书认证软件生成一对公钥和私钥。任何其他想给该用户发送加密消息的用户，需要先从证书机构的公共目录获取接收者的公钥，然后用公钥加密信息，再发送给接收者。当接收者收到加密消息后，他可以用自己的私钥来解密，而私钥是不应该被其他人拿到的。
#### 创建GPG密钥
1. 为您的操作系统下载并安装最新版本[GunPG v2.1+](https://www.gnupg.org/download/index.html)或者[GIT v2.16.2+](https://git-scm.com/downloads)。
2. GnuPG运行命令，GIT打开安装目录下面git-bash.exe。
3. 粘贴下面的文本来生成一个GPG密钥。
``` 
$ gpg --gen-key
```
4. 在提示符处，指定您想要的键类型，或者按Enter键接受默认的RSA和RSA。
``` 
$ gpg --gen-key
gpg (GnuPG) 1.4.22; Copyright (C) 2015 Free Software Foundation, Inc.
This is free software: you are free to change and redistribute it.
There is NO WARRANTY, to the extent permitted by law.

Please select what kind of key you want:
   (1) RSA and RSA (default)
   (2) DSA and Elgamal
   (3) DSA (sign only)
   (4) RSA (sign only)
Your selection?
``` 
5. 输入所需的密钥大小。我们建议最大的密钥大小为4096。
``` 
RSA keys may be between 1024 and 4096 bits long.
What keysize do you want? (2048)
``` 
6. 输入时间的长度，键应该是有效的。按Enter键指定默认选择，表示键不会过期。
``` 
Requested keysize is 4096 bits
Please specify how long the key should be valid.
         0 = key does not expire
      <n>  = key expires in n days
      <n>w = key expires in n weeks
      <n>m = key expires in n months
      <n>y = key expires in n years
Key is valid for? (0)
``` 
7. 确认您的选择是正确的。
``` 
Key does not expire at all
Is this correct? (y/N)
``` 
8. 输入您的用户ID信息。**（当您需要在GitHub使用GPG时，请确保您输入的电子邮件与GitHub中所使用的电子邮件相符，并且通过了验证。）**
``` 
You need a user ID to identify your key; the software constructs the user ID
from the Real Name, Comment and Email Address in this form:
    "Heinrich Heine (Der Dichter) <heinrichh@duesseldorf.de>"

Real name:
Email address:
Comment:
``` 
9. 输入安全密码。
``` 
You need a Passphrase to protect your secret key.
Enter passphrase:
``` 
10. 粘贴下面的文本来列出GPG密钥，您可以同时使用公钥和私钥。签署提交或标记所需的私钥。
``` 
$ gpg --list-secret-keys --keyid-format LONG
```
11. 从GPG密钥列表中，复制您想要使用的GPG密钥ID。在本例中，GPG密钥ID为3AA5C34371567BD2。
``` 
$ gpg --list-secret-keys --keyid-format LONG
/Users/hubot/.gnupg/secring.gpg
------------------------------------
sec   4096R/3AA5C34371567BD2 2016-03-10 [expires: 2017-03-10]
uid                          Hubot 
ssb   4096R/42B317FD4BA89E7A 2016-03-10
```
12. 生成公匙，粘贴下面的文本，用您想要使用的GPG密钥ID代替。在本例中，GPG密钥ID为3AA5C34371567BD2。
```
$ gpg --armor --export 3AA5C34371567BD2
# Prints the GPG key ID, in ASCII armor format
```
### 发布GPG密钥
1. 拷贝您的GPG公匙, 从 -----BEGIN PGP PUBLIC KEY BLOCK----- 到 -----END PGP PUBLIC KEY BLOCK----- 结束**公钥加密，私匙解密。实际使用中应将公匙发布给服务器** *（如果您需要发布到GitHub，则将公匙添加到GitHub账户的GPG KEY中。）*  
2. 在GIT中设置您的GPG钥匙。请粘贴下面的文本，替换为要使用的GPG密钥ID。在此示例中，GPG密钥ID为3AA5C34371567BD2
```
$ git config --global user.signingkey 3AA5C34371567BD2
```
### 签名和验证签名
#### 对GIT提交进行签名
在本地分支中提交更改时，将-S标志添加到git提交命令
```
$ git commit -S -m your commit message
# Creates a signed commit
```
>**要将Git客户端配置为在默认情况下为本地存储库签署提交，请执行以下操作**
>```
>$ git config --global commit.gpgsign true.
>```
>**要存储GPG密钥密码，使您不必在每次签署提交时输入密码，建议使用以下工具**  
>
>>对于Mac用户，GPG套件允许您将GPG密钥密码存储在Mac操作系统钥匙串中。  
>>对于Windows用户，[Gpg4win](https://www.gnupg.org/download/)与其他Windows工具集成在一起，并做相应的配置。
>>```
>>$ git config --global gpg.program "${Gpg4win}/gpg.exe"
>>```
#### 对GIT标签进行签名和验证签名
通过以下命令签名标签
```
$ git tag -s mytag
# Creates a signed tag
```
通过以下命令验证标签
```
$ git tag -v mytag
# Verifies the signed tag
```
#### 对文件签名和验证签名
通过以下命令签名文件
```
$ gpg --sign demo.txt                  --生成二进制码的签名文件
```
```
$ gpg --clearsign demo.txt             --生成ASCII码的签名文件
```
```
$ gpg --detach-sign demo.txt           --生成单独的二进制签名文件
```
```
$ gpg --armor --detach-sign demo.txt   --生成单独的ASCII码签名文件
```
我们收到别人签名后的文件，需要用对方的公钥验证签名是否为真。verify参数用来验证。
通过以下命令验证签名文件
```
$ gpg --verify demo.txt.asc demo.txt
```
