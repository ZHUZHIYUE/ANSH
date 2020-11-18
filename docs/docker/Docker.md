# Docker
Docker 是一个开源的应用容器引擎，让开发者可以打包他们的应用以及依赖包到一个可移植的容器中，然后发布到任何流行的Linux机器上，也可以实现虚拟化，容器是完全使用沙箱机制，相互之间不会有任何接口。
## 准备
1. 为您的操作系统下载并安装[docker](https://docs.docker.com/)
2. [设置镜像加速地址](#Docker-Daemon)
```
yum install -y yum-utils \
  device-mapper-persistent-data \
  lvm2
```
```
  yum-config-manager \
      --add-repo \
      https://download.docker.com/linux/centos/docker-ce.repo
```
```
yum install -y docker-ce docker-ce-cli containerd.io
```
```
$yum-config-manager --enable docker-ce-stable
$yum-config-manager --disable docker-ce-stable
```
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