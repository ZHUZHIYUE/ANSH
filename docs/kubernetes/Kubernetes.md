# Kubernetes  

Kubernetes 是一个可移植的、可扩展的开源平台，用于管理容器化的工作负载和服务，可促进声明式配置和自动化。 Kubernetes 拥有一个庞大且快速增长的生态系统。Kubernetes 的服务、支持和工具广泛可用。
名称 Kubernetes 源于希腊语，意为“舵手”或“飞行员”。Google 在 2014 年开源了 Kubernetes 项目。 Kubernetes 建立在 Google 在大规模运行生产工作负载方面拥有十几年的经验 的基础上，结合了社区中最好的想法和实践。  

## kubeadm  

本页面显示如何安装 kubeadm 工具箱。 有关在执行此安装过程后如何使用 kubeadm 创建集群的信息，请参见 使用 kubeadm 创建集群 页面

### 准备开始

* 一台或多台运行着下列系统的机器：

```bash  
Ubuntu 16.04+
Debian 9+
CentOS 7+
Red Hat Enterprise Linux (RHEL) 7+
Fedora 25+
HypriotOS v1.0.1+
Flatcar Container Linux （使用 2512.3.0 版本测试通过）
```  

* 每台机器 2 GB 或更多的 RAM （如果少于这个数字将会影响你应用的运行内存) 2 CPU 核或更多  
* 集群中的所有机器的网络彼此均能相互连接(公网和内网都可以)
* 节点之中不可以有重复的主机名、MAC 地址或 product_uuid。请参见这里了解更多详细信息。
* 开启机器上的某些端口。请参见这里 了解更多详细信息。
* 禁用交换分区。为了保证 kubelet 正常工作，你 必须 禁用交换分区

### 确保每个节点上 MAC 地址和 product_uuid 的唯一性

* 你可以使用命令 ip link 或 ifconfig -a 来获取网络接口的 MAC 地址
* 可以使用 sudo cat /sys/class/dmi/id/product_uuid 命令对 product_uuid 校验

一般来讲，硬件设备会拥有唯一的地址，但是有些虚拟机的地址可能会重复。 Kubernetes 使用这些值来唯一确定集群中的节点。 如果这些值在每个节点上不唯一，可能会导致安装 失败。

### 检查网络适配器

如果你有一个以上的网络适配器，同时你的 Kubernetes 组件通过默认路由不可达，我们建议你预先添加 IP 路由规则，这样 Kubernetes 集群就可以通过对应的适配器完成连接。

### 允许 iptables 检查桥接流量

确保 br_netfilter 模块被加载。这一操作可以通过运行 lsmod | grep br_netfilter 来完成。若要显式加载该模块，可执行 sudo modprobe br_netfilter。

为了让你的 Linux 节点上的 iptables 能够正确地查看桥接流量，你需要确保在你的 sysctl 配置中将 net.bridge.bridge-nf-call-iptables 设置为 1。例如：

```bash
cat <<EOF | sudo tee /etc/modules-load.d/k8s.conf
br_netfilter
EOF

cat <<EOF | sudo tee /etc/sysctl.d/k8s.conf
net.bridge.bridge-nf-call-ip6tables = 1
net.bridge.bridge-nf-call-iptables = 1
EOF
sudo sysctl --system
```

### 检查所需端口

#### 控制平面节点

| 协议 | 方向   | 端口范围 | 作用 | 使用者 |
| ------ | --------- | ----------- | ------- | ------- |
| TCP | 入站 | 6443 | Kubernetes API 服务器 | 所有组件|
| TCP | 入站 | 2379-2380| etcd 服务器客户端 API |kube-apiserver, etcd|
| TCP | 入站 | 10250 | Kubelet API |kubelet 自身、控制平面组件|
| TCP | 入站 | 10251| kube-scheduler |kube-scheduler 自身|
| TCP | 入站 | 10252 | kube-controller-manager |kube-controller-manager 自身|

#### 工作节点

| 协议 | 方向   | 端口范围 | 作用 | 使用者 |
| ------ | --------- | ----------- | ------- | ------- |
| TCP | 入站 | 10250 | Kubelet API | kubelet 自身、控制平面组件|
| TCP | 入站 | 30000-32767 | NodePort 服务† | 所有组件 |

### 安装 kubeadm、kubelet 和 kubectl

安装 kubeadm、kubelet 和 kubectl

你需要在每台机器上安装以下的软件包：

* kubeadm：用来初始化集群的指令。

* kubelet：在集群中的每个节点上用来启动 Pod 和容器等。

* kubectl：用来与集群通信的命令行工具。

```bash
cat <<EOF > /etc/yum.repos.d/kubernetes.repo
[kubernetes]
name=Kubernetes
baseurl=https://mirrors.aliyun.com/kubernetes/yum/repos/kubernetes-el7-x86_64/
enabled=1
gpgcheck=1
repo_gpgcheck=1
gpgkey=https://mirrors.aliyun.com/kubernetes/yum/doc/yum-key.gpg https://mirrors.aliyun.com/kubernetes/yum/doc/rpm-package-key.gpg
EOF

yum install -y yum-utils device-mapper-persistent-data lvm2
yum-config-manager --add-repo http://mirrors.aliyun.com/docker-ce/linux/centos/docker-ce.repo
sed -i 's+download.docker.com+mirrors.aliyun.com/docker-ce+' /etc/yum.repos.d/docker-ce.repo
yum makecache fast

yum install -y kubelet-1.20.2 kubeadm-1.20.2 kubectl-1.20.2 --disableexcludes=kubernetes

#yum list docker-ce --showduplicates | sort -r 查看docker版本
yum install -y docker-ce-19.03.9 docker-ce-cli-19.03.9 containerd.io
 
systemctl enable kubelet && systemctl start kubelet && systemctl enable docker && systemctl start docker
```

### 永久关闭swap

```bash
swapoff  -a
sed -ri 's/.*swap.*/#&/' /etc/fstab    永久关闭swap
```

### Docker cgroup驱动程序切换为“systemd”

```bash
vi /etc/docker/daemon.json

{
"graph":"/volume1/docker",
"exec-opts":["native.cgroupdriver=systemd"],
"storage-driver": "overlay2",
"registry-mirrors": [
    "https://lbpwb5di.mirror.aliyuncs.com"
  ]
}

systemctl restart docker
systemctl status docker
```

### 创建集群

#### 初始化集群

```bash  
#阿里云镜像仓库
kubeadm init --kubernetes-version=v1.20.2 --image-repository registry.aliyuncs.com/google_containers --service-cidr=10.125.0.0/16 --pod-network-cidr=10.126.0.0/16 
```

要使非 root 用户可以运行 kubectl，请运行以下命令， 它们也是 kubeadm init 输出的一部分：

```bash
mkdir -p $HOME/.kube
sudo cp -i /etc/kubernetes/admin.conf $HOME/.kube/config
sudo chown $(id -u):$(id -g) $HOME/.kube/config
```

或者，如果你是 root 用户，则可以运行：

```bash
export KUBECONFIG=/etc/kubernetes/admin.conf
```

#### etcd数据目录更换

```bash
vi /etc/kubernetes/manifests/etcd.yaml

cp -rf /var/lib/etcd/* /volume1/kubernetes/etcd

systemctl restart kubelet

rm -rf /var/lib/etcd

volumes:
  - hostPath:
      path: /etc/kubernetes/pki/etcd
      type: DirectoryOrCreate
    name: etcd-certs
  - hostPath:
      path: /var/lib/etcd/       # 将这个路径改为你要更改到的路径
      type: DirectoryOrCreate
    name: etcd-data
翻到最下方，编辑hostPath
```

* 删除etcd的pod，删除后pod会从列表中消失，这是正常现象
* 需把etcd的要把/var/lib/etcd/* 拷贝到更改后的目录
* 重启物理机的kubelet
* 查看状态
以上4步做完之后，etcd应该已经起来了。 注意，这里一定要先修改配置文件，在拷贝etcd数据

#### 安装网络插件

* Calico

```bash
kubectl create -f /root/tigera-operator.yaml
#kubectl create -f https://docs.projectcalico.org/manifests/tigera-operator.yaml

docker pull docker-repository.bioey.com/quay.io/tigera/operator:v1.13.4
docker tag docker-repository.bioey.com/quay.io/tigera/operator:v1.13.4 quay.io/tigera/operator:v1.13.4
```

```bash
kubectl create -f /root/custom-resources.yaml
#kubectl create -f https://docs.projectcalico.org/manifests/custom-resources.yaml
```

```bash
watch kubectl get pods -n calico-system
```

每个集群只能安装一个 Pod 网络。

安装 Pod 网络后，您可以通过在

```bash
watch kubectl get pods --all-namespaces -o wide
```

输出中检查 CoreDNS Pod 是否 Running 来确认其是否正常运行。 一旦 CoreDNS Pod 启用并运行，你就可以继续加入节点。

如果您的网络无法正常工作或CoreDNS不在“运行中”状态，请查看 kubeadm 的故障排除指南。

#### 控制平面节点隔离

默认情况下，出于安全原因，你的集群不会在控制平面节点上调度 Pod。 如果你希望能够在控制平面节点上调度 Pod， 例如用于开发的单机 Kubernetes 集群，请运行：

```bash
kubectl taint nodes --all node-role.kubernetes.io/master-
```

输出看起来像：

```bash
node "test-01" untainted
taint "node-role.kubernetes.io/master:" not found
taint "node-role.kubernetes.io/master:" not found
```

这将从任何拥有 node-role.kubernetes.io/master taint 标记的节点中移除该标记， 包括控制平面节点，这意味着调度程序将能够在任何地方调度 Pods。

#### 加入节点

节点是你的工作负载（容器和 Pod 等）运行的地方。要将新节点添加到集群，请对每台计算机执行以下操作：

* SSH 到机器
* 成为 root （例如 sudo su -）
* 运行 kubeadm init 输出的命令。例如：

```bash
kubeadm join --token <token> <control-plane-host>:<control-plane-port> --discovery-token-ca-cert-hash sha256:<hash>
```

如果没有令牌，可以通过在控制平面节点上运行以下命令来获取令牌：

```bash
kubeadm token list
```

默认情况下，令牌会在24小时后过期。如果要在当前令牌过期后将节点加入集群， 则可以通过在控制平面节点上运行以下命令来创建新令牌：

```bash
kubeadm token create
```

如果你没有 --discovery-token-ca-cert-hash 的值，则可以通过在控制平面节点上执行以下命令链来获取它：

```bash
openssl x509 -pubkey -in /etc/kubernetes/pki/ca.crt | openssl rsa -pubin -outform der 2>/dev/null | \
   openssl dgst -sha256 -hex | sed 's/^.* //'
```
