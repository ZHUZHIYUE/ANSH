# Docker Swarms  

Docker Swarms是Docker容器的集群。  
## 准备  

1. 为您的操作系统下载并安装[docker](https://docs.docker.com/)  

2. 关闭 SELinux  

    ```bash
    #!/bin/bash
    CentOS 7
    $ vi /etc/selinux/config
    修改
    SELINUX=disabled
    ```  

3. 设置启动  

    通用配置  
    ```bash
    #!/bin/bash
    vi /usr/lib/systemd/system/docker.service
    修改
    ExecStart=/usr/bin/dockerd -H tcp://0.0.0.0:2375 -H unix://var/run/docker.sock 
    或者
    vi /etc/docker/daemon.json
    {
      "hosts": ["tcp://0.0.0.0:2375", "unix:///var/run/docker.sock"],
      "bip": "10.10.0.1/16",
      "graph":"/volume1/docker",
      "userland-proxy": false,
      "registry-mirrors": [
          "https://lbpwb5di.mirror.aliyuncs.com"
        ]
    }


    systemctl start firewalld
    systemctl enable firewalld
    /usr/lib/firewalld/services/
    firewall-cmd --zone=public --add-port=3306/tcp --permanent （--permanent永久生效，没有此参数重启后失效）
    firewall-cmd --zone=public --remove-service=mysql --permanent
    firewall-cmd --zone=public --add-service=docker-swarm --permanent
    firewall-cmd --reload（重新载入）
    firewall-cmd --zone=public --list-ports（查看所有打开的端口）
    firewall-cmd --zone=public --list-all
    firewall-cmd --set-default-zone=public　　　　　设定默认域
    firewall-cmd --permanent --add-interface=eth0 --zone=public 　　　　添加指定域的网络接口

    ```  
    CentOS配置
    ```bash
    #!/bin/bash
    vi /etc/systemd/system/docker-tcp.socket
    修改
    [Unit]
    Description=Docker Socket for the API

    [Socket]
    ListenStream=2375
    BindIPv6Only=both
    Service=docker.service

    [Install]
    WantedBy=sockets.target


    $ sudo systemctl daemon-reload
    $ sudo systemctl enable docker-tcp.socket
    $ sudo systemctl stop docker
    $ sudo systemctl start docker-tcp.socket
    $ sudo systemctl start docker

    注意：这种方法必须先启动 docker-tcp.socket，再启动 Docker，一定要注意启动顺序！

    docker swarm init --force-new-cluster 是在保留配置数据的情况下重建集群
    /volume1/docker/swarm/worker/tasks.db 删除
    ```

    在docker swarm init 之前修改docker_gwbridge网段    
    ```bash
      docker network rm docker_gwbridge
      docker network create \
      --subnet 10.11.0.0/16 \
      --gateway 10.11.0.1 \
      -o com.docker.network.bridge.enable_icc=false \
      -o com.docker.network.bridge.enable_ip_masquerade=true \
      -o com.docker.network.bridge.name=docker_gwbridge \
      docker_gwbridge


      docker network rm ingress
      docker network create \
      --driver overlay \
      --ingress \
      --subnet=10.12.0.0/16 \
      --gateway=10.12.0.1 \
      ingress


      docker network create \
      --driver overlay \
      --subnet=10.13.0.0/16 \
      --gateway=10.13.0.1 \
      swarms
      ```

4. 启动端口  

    ```bash
    #!/bin/bash

    #防火墙服务
    cd /usr/lib/firewalld/services/

    #docker-swarm.xml
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

    #docker-registry.xml
    <?xml version="1.0" encoding="utf-8"?>
    <service>
      <short>Docker Registry</short>
      <description>Docker Registry is the protocol used to serve Docker images. If you plan to make your Docker Registry server publicly available, enable this option. This option is not required for developing Docker images locally.</description>
      <port protocol="tcp" port="5000"/>
    </service>


    #添加防火墙
    firewall-cmd --zone=public --add-service=docker-swarm --permanent
    firewall-cmd --reload
    ```  

5. 创建swarm  

    ```bash
    #!/bin/bash
    docker swarm init
    ```

6. 创建network  

    ```bash
    #!/bin/bash
    docker network create swarms -d overlay
    ```

7. 设置node label  

    ```bash
    #!/bin/bash
    docker node update --label-add identity=leader
    ```  

## daemon.json配置文件  

```bash
#!/bin/bash
vi /etc/docker/daemon.json
{
"insecure-registries":["172.17.106.1:5000"],
"graph":"/volume1/docker",
"userland-proxy": false,
"registry-mirrors": [
    "https://lbpwb5di.mirror.aliyuncs.com"
  ]
}

insecure-registries 安全连接  
graph docker安装地址
userland-proxy 是否启用docker守护进程
registry-mirrors docker镜像地址
```  
