# Git  

Git 是用于 Linux内核开发的版本控制工具。与常用的版本控制工具 CVS, Subversion 等不同，它采用了分布式版本库的方式，不必服务器端软件支持（wingeddevil注：这得分是用什么样的服务端，使用http协议或者git协议等不太一样。并且在push和pull的时候和服务器端还是有交互的。），使源代码的发布和交流极其方便。 Git 的速度很快，这对于诸如 Linux kernel 这样的大项目来说自然很重要。 Git 最为出色的是它的合并跟踪（merge tracing）能力。  

## 准备  

1. 为您的操作系统下载并安装[Git.exe v2.18.0+](https://git-scm.com/downloads)
2. [初始化您的Git仓库](#导航至项目目录并创建Git仓库)  

## 常用操作命令  

### 导航至项目目录并创建Git仓库  

```bash
#!/bin/bash
$ cd ${projects}
$ git init
```  

### Git撤销Commit  

```bash
#!/bin/bash
$ git reset --soft <版本号>
使用 --hard 参数会抛弃当前工作区的修改
使用 --soft 参数的话会回退到之前的版本，但是保留当前工作区的修改，可以重新提交。
```  

### Git Branch  

#### 查看分支  

```bash
#!/bin/bash
$ git branch -a
```  

#### 删除本地分支  

```bash
#!/bin/bash
$ git branch -d ${BranchName}
```  

#### 删除远程分支  

```bash
#!/bin/bash
$ git push origin --delete ${BranchName}
```  

## Git-SSH 配置和使用  

### 设置Git的username和email  

```bash
#!/bin/bash
$ git config --global user.name "${username}"
$ git config --global user.email "${email}"
```  

### 生成密钥  

在Git安装目录中以管理员方式打开git-bash.exe。

```bash
#!/bin/bash
$ ssh-keygen -t rsa -C "${email}"
```  

默认在~/.ssh/下生成两个文件：id_rsa和id_rsa.pub。  
**${email}地址应与global中的$(email)地址一致。**  

## Git查看config配置信息  

### config配置指令  

```bash
#!/bin/bash
$ git config
```  

config 配置有system级别 global（用户级别） 和local（当前仓库）三个 设置先从system-》global-》local  底层配置会覆盖顶层配置 分别使用--system/global/local 可以定位到配置文件  

### 查看系统config  

```bash
#!/bin/bash
$ git config --system --list
```  

### 查看当前用户（global）配置  

```bash
#!/bin/bash
$ git config --global  --list
```  

### 查看当前仓库配置信息  

```bash
#!/bin/bash
$ git config --local  --list
```  

## 管理多个SSH-KEY  

1. 首先查看代理

    ```bash
    #!/bin/bash
    $ ssh-add -l
    ```  

    若提示
    Could not open a connection to your authentication agent.
    则系统代理里没有任何key，直接进行第3步操作。
2. 若系统已经有ssh-key代理 ,可以删除。

    ```bash
    #!/bin/bash
    ssh-add -D
    ```  

3. 添加ssh代理

    ```bash
    #!/bin/bash
    $exec ssh-agent bash
    ```  

4. 添加多个ssh-key

    ```bash
    #!/bin/bash
    $ssh-add ~/.ssh/${ssh-key1}
    $ssh-add ~/.ssh/${ssh-key2}
    ```  

5. 添加和编辑配置文件config
在 ~/.ssh 目录下新建一个config文件

    ```bash
    #!/bin/bash
    $ touch ~/.ssh/config
    or
    $ nano ~/.ssh/config
    ```  

    添加内容

    ```bash
    #!/bin/bash
    # GitLab.com server
    Host 192.168.1.89
    PreferredAuthentications publickey
    IdentityFile ~/.ssh/id_rsa_zhuxi@bioey_com
    User ZHUXI
    Port 10022


    # GitHub.com server
    Host github.com
    PreferredAuthentications publickey
    IdentityFile ~/.ssh/id_rsa_myprecious@vip_qq_com
    User ZHUXI
    Port 22
    ```  

## 测试ssh连接  

```bash
#!/bin/bash
$ ssh -T git@${address}
```  