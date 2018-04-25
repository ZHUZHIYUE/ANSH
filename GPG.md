# GPG
### 创建GPG密钥
如果您没有现有的GPG密钥，您可以生成一个新的GPG密钥来用于签署提交和标记。
1. 为您的操作系统下载并安装最新版本的[GIT工具](https://git-scm.com/downloads)。您将需要版本2.16.2或更大的版本来遵循下面的说明。
2. 打开GIT安装目录下面git-bash.exe。
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
### 设置GPG密钥
1. 拷贝您的GPG公匙, 从 -----BEGIN PGP PUBLIC KEY BLOCK----- 到 -----END PGP PUBLIC KEY BLOCK----- 结束**公钥加密，私匙解密。实际使用中应将公匙发布给服务器** *（如果您需要发布到GitHub，则将公匙添加到GitHub账户的GPG KEY中。）*  
2. 在GIT中设置您的GPG钥匙。请粘贴下面的文本，替换为要使用的GPG密钥ID。在此示例中，GPG密钥ID为3AA5C34371567BD2
```
$ git config --global user.signingkey 3AA5C34371567BD2
```
### 使用GPG密钥
使用GPG对GITCommit进行签名，在本地分支中提交更改时，将- S标志添加到git提交命令
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

