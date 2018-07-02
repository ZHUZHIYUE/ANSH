# Docker
Docker 是一个开源的应用容器引擎，让开发者可以打包他们的应用以及依赖包到一个可移植的容器中，然后发布到任何流行的Linux机器上，也可以实现虚拟化，容器是完全使用沙箱机制，相互之间不会有任何接口。
## 准备
1. 为您的操作系统下载并安装[docker](https://docs.docker.com/)
2. [设置镜像加速地址](#设置镜像加速地址)
## 常用命令
### 查看镜像
```
$ docker images
```
### 搜索镜像
```
$ docker search ${镜像名称}
```
### 查看容器
```
$ docker ps --查看正在运行的容器
$ docker ps -a --所有容器
```
### 删除容器
```
$ docker rm ${容器名或ID}
$ docker rm -f ${容器名或ID} --强制删除
```
### 删除镜像
```
$ docker rmi ${镜像名称或ID}
```
### 镜像打包
```
$ docker save -o /opt/docker/backups/gitlab-ce-rc-backups.tar ${镜像ID或名称}
```
## BASH
```
$ docker exec -it ${containername} bash
```
## MSSQL
```
bash
$ docker run \
--name mssql \
-p 14330:1433 \
-v /opt/docker/mssql:/var/opt/mssql:rw \
-e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Bio*novo!' \
--restart=always \
--privileged=true \
-d microsoft/mssql-server-linux:2017-CU8
```
```
powershell
$ docker run `
--name mssql `
-p 14330:1433 `
-v /g/docker/mssql:/var/opt/mssql:rw `
-e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Bio*novo!' `
--restart=always `
--privileged=true `
-d microsoft/mssql-server-linux:2017-CU8
```
## MYSQL
```
bash
$ docker run \
--name mysql \
-p 33060:3306 \
-v /opt/docker/MySql:/var/lib/mysql:rw \
-e MYSQL_ROOT_PASSWORD=Bio*novo! \
--restart=always \
-d mysql:8
```
## CentOS7
```
bash
$ docker run \
--name centos7 \
-p 8000:80 -p 2200:22 \
-v /opt/docker/CentOS7:/sys/fs/cgroup:rw \
--restart=always \
--privileged=true -it \
-d centos:7 /usr/sbin/init
```
##Gitlab
### bash
```
$ docker run \
--name gitlab \
--hostname localhost \
-p 10080:80 -p 10022:22 -p 10443:443 \
-v /opt/docker/gitlab/data:/var/opt/gitlab:rw \
-v /opt/docker/gitlab/logs:/var/log/gitlab:rw \
-v /opt/docker/gitlab/config:/etc/gitlab:rw \
--restart=always \
--privileged=true -it \
-d gitlab/gitlab-ce:rc
```
### [GitLab SMTP](https://docs.gitlab.com/omnibus/settings/smtp.html#jangosmtp)
### [GitLab Configure](https://docs.gitlab.com/omnibus/docker/README.html#doc-nav)
### 重新加载配置信息
```
$ docker exec -it gitlab gitlab-ctl reconfigure
```
### 备份
```
$ docker exec -it gitlab gitlab-rake gitlab:backup:create
```
### 恢复
```
$ docker exec -it gitlab gitlab-rake gitlab:backup:restore BACKUP=${文件名称}
```
## 设置镜像加速地址
```
$ cd /etc/docker/daemon.json
设置成
{
    "registry-mirrors": ["https://lbpwb5di.mirror.aliyuncs.com"]
}
```