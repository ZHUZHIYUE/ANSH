# Docker Swarms  
Docker Swarms是Docker容器的集群。  
## 准备  
1. 为您的操作系统下载并安装[docker](https://docs.docker.com/)  
2. 关闭 SELinux  
```
CentOS 7
$ vi /etc/selinux/config
修改
SELINUX=disabled
```
3. 设置启动  
```
CentOS 7
vi /usr/lib/systemd/system/docker.service
修改
ExecStart=/usr/bin/dockerd -H tcp://0.0.0.0:2375 -H unix://var/run/docker.sock 
或者
vi /etc/docker/daemon.json
{
  "hosts": ["tcp://0.0.0.0:2375", "unix:///var/run/docker.sock"]
}
```
4. 启动端口  
```
cd /usr/lib/firewalld/services/
docker-swarm.xml
<?xml version="1.0" encoding="utf-8"?>
<service>
  <short>docker-swarm</short>
  <description>docker swarm.</description>
  <port protocol="tcp" port="2377"/>
  <port protocol="tcp" port="7946 "/>
  <port protocol="udp" port="7946 "/>
  <port protocol="tcp" port="4789 "/>
  <port protocol="udp" port="4789 "/>
</service>

docker-registry.xml
<?xml version="1.0" encoding="utf-8"?>
<service>
  <short>Docker Registry</short>
  <description>Docker Registry is the protocol used to serve Docker images. If you plan to make your Docker Registry server publicly available, enable this option. This option is not required for developing Docker images locally.</description>
  <port protocol="tcp" port="5000"/>
</service>
```
## 安装管理工具
```
docker run  \
--name portainer  \
--rm  \
-p 9000:9000  \
-v /var/run/docker.sock:/var/run/docker.sock  \
-v portainer:/data  \
-v /etc/localtime:/etc/localtime:ro  \
portainer/portainer:1.19.2  

# manager.yml

version: '3.6'

services:
  # portainer集群管理工具
  portainer:
    image: portainer/portainer:1.19.2
    ports:
      - "9000:9000"
    command: -H unix:///var/run/docker.sock
    networks: 
      swarms:
        aliases:
          - portainer
    volumes:
      - portainer:/data
      - /etc/localtime:/etc/localtime:ro
      - /var/run/docker.sock:/var/run/docker.sock
    logging:
      driver: "json-file"
      options:
        max-size: "1m"
        max-file: "10"
    deploy:
      restart_policy:
        condition: on-failure
      update_config:
        parallelism: 1
        delay: 10s
      replicas: 1
      placement:
        constraints: 
          - node.role == manager
          - node.labels.identity == leader



  registry:
    # docker仓库
    image: registry:2.6.2
    ports:
      - "5000:5000"
    networks: 
      swarms:
        aliases:
          - registry
    volumes:
      - registry:/var/lib/registry
      - /etc/localtime:/etc/localtime:ro
    logging:
      driver: "json-file"
      options:
        max-size: "1m"
        max-file: "10"
    deploy:
      restart_policy:
        condition: on-failure
      update_config:
        parallelism: 1
        delay: 10s
      replicas: 1
      placement:
        constraints: 
          - node.role == manager
          - node.labels.identity == leader

networks:
  swarms:
    external: true 

volumes:
  portainer:
    external: true 
  registry:
    external: true 
```