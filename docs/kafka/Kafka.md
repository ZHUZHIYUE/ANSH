# 常用命令

## topic

```bash
# 查看所有topic
kafka-topics.sh --zookeeper bsc.zookeeper:2181/kafka --list 
# 查看指定topic详情
kafka-topics.sh --zookeeper bsc.zookeeper:2181/kafka --topic zipkin --describe
```

## consumer groups

```bash
# 查看所有groups
kafka-consumer-groups.sh --bootstrap-server bsc.kafka.node:9092 --list
# 查看指定groups详情
kafka-consumer-groups.sh --bootstrap-server bsc.kafka.node:9092 --group zipkin --describe
```

## config

```bash
# 查看指定topic配置
kafka-configs.sh --bootstrap-server bsc.kafka.node:9092 --entity-name zipkin --entity-type topics --describe
```
