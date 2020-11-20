using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ANSH.MQ.RabbitMQ {

    /// <summary>
    /// 队列操作类
    /// </summary>
    public class ANSHMQFactoryBase : IDisposable {

        ConnectionFactory _Factory;
        /// <summary>
        /// 工厂类
        /// </summary>
        ConnectionFactory Factory => _Factory = _Factory?? new ConnectionFactory () { AutomaticRecoveryEnabled = true, TopologyRecoveryEnabled = true };
        IConnection _IConnection;
        /// <summary>
        /// 连接类
        /// </summary>
        /// <returns>连接</returns>
        IConnection IConnection {
            get {
                if (_IConnection == null || !_IConnection.IsOpen) {
                    _IConnection = (HostName?.Count > 0 ? Factory.CreateConnection (HostName) : throw new ArgumentNullException ("没有设置终节点。"));
                }
                return _IConnection;
            }
        }

        /// <summary>
        /// 终节点
        /// </summary>
        List<string> HostName {
            get;
        } = new List<string> ();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="username">帐号</param>
        /// <param name="password">密码</param>
        /// <param name="port">端口号</param>
        /// <param name="virtualhost">虚拟地址</param>
        /// <param name="hostname">主机名称</param>
        public ANSHMQFactoryBase (string username, string password, int port, string virtualhost, string hostname) {
            var hostName = hostname.Split (new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (hostName?.Length > 0) {
                hostName.ToList ().ForEach (m => HostName.Add (m));
                Factory.UserName = username;
                Factory.Password = password;
                Factory.VirtualHost = virtualhost;
                Factory.Port = port;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose () {
            _IConnection?.Dispose ();
        }

        /// <summary>
        /// 创建交换机
        /// </summary>
        /// <param name="exchange">交换机名称</param>
        /// <param name="exchange_type">交换机类型</param>
        /// <param name="delivery">是否持久</param>
        /// <param name="auto_delete">是否自动删除</param>
        public void CreateDurableExchange (string exchange, string exchange_type, bool delivery, bool auto_delete) {
            using (var channel = IConnection.CreateModel ()) {
                channel.ExchangeDeclare (exchange, exchange_type, delivery, auto_delete, null);
            }
        }

        /// <summary>
        /// 创建队列
        /// </summary>
        /// <param name="queue">队列名称</param>
        /// <param name="delivery">是否持久</param>
        /// <param name="auto_delete">是否自动删除</param>
        /// <param name="exchange">队列绑定exchange</param>
        /// <param name="root_key">队列绑定routkey</param>
        /// <param name="bind_args">队列绑定参数</param>
        /// <param name="isLazy">是否为懒队列（先将消息保存到磁盘上，不放在内存中，当消费者开始消费的时候才加载到内存中）</param>
        public void CreateQueue (string queue, bool delivery, bool auto_delete, string exchange, string root_key, Dictionary<string, object> bind_args = null, bool isLazy = false) {
            using (var channel = IConnection.CreateModel ()) {
                if (isLazy) {
                    if (bind_args == null) {
                        bind_args = new Dictionary<string, object> ();
                    }
                    bind_args.Add ("x-queue-mode", "lazy");
                }

                channel.QueueDeclare (queue, delivery, false, auto_delete, bind_args);
                channel.QueueBind (queue: queue,
                    exchange: string.IsNullOrWhiteSpace (exchange) ? "amq.direct" : exchange,
                    routingKey : root_key);
            }
        }

        /// <summary>
        /// 创建队列
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <returns>队列名</returns>
        public void InitPublish<TMessage> (TMessage message) where TMessage : ANSHMQMessagePublishBase {
            CreateDurableExchange (message.Exchange, message.ExchangeType, true, false);
            if (message.QueueDelayOpen) {
                CreateDurableExchange (message.ExchangeDelay, message.ExchangeTypeDelay, true, false);
            }
        }

        /// <summary>
        /// 创建队列
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <returns>队列名</returns>
        public void InitRetrieving<TMessage> (TMessage message) where TMessage : ANSHMQMessageRetrievingBase {
            Dictionary<string, object> XDLExchange = null;
            Dictionary<string, object> DelayXDLExchange = null;

            if (message.QueueDxOpen) {
                XDLExchange = CreateParamFormDeathType (message.ExchangeDX, message.RootKey);
            }
            if (message.QueueDelayOpen) {
                DelayXDLExchange = CreateParamFormDeathType (message.Exchange, message.RootKey);
            }
            if (message.QueueDxOpen) {
                CreateDurableExchange (message.ExchangeDX, message.ExchangeTypeDX, true, false);
                CreateQueue (message.QueueDX, true, false, message.ExchangeDX, message.RootKey);
            }

            CreateDurableExchange (message.Exchange, message.ExchangeType, true, false);
            CreateQueue (message.Queue, true, false, message.Exchange, message.RootKey, XDLExchange);

            if (message.QueueDelayOpen) {
                CreateDurableExchange (message.ExchangeDelay, message.ExchangeTypeDelay, true, false);
                CreateQueue (message.QueueDelay, true, false, message.ExchangeDelay, message.RootKeyDelay, DelayXDLExchange, true);
            }
        }

        /// <summary>
        /// 创建队列
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <returns>队列名</returns>
        public void InitMessage<TMessage> (TMessage message) where TMessage : ANSHMQMessageBase => InitRetrieving (message);

        /// <summary>
        /// 创建排他队列
        /// </summary>
        /// <param name="queue">队列名称</param>
        /// <param name="root_key">队列绑定routkey</param>
        /// <param name="bind_args">队列绑定参数</param>
        public void CreateExclusiveQueue (string queue, string root_key = "", Dictionary<string, object> bind_args = null) {
            CreateExclusiveQueue (queue, "", root_key, bind_args);
        }

        /// <summary>
        /// 创建排他队列
        /// </summary>
        /// <param name="queue">队列名称</param>
        /// <param name="exchange">队列绑定exchange</param>
        /// <param name="root_key">队列绑定routkey</param>
        /// <param name="bind_args">队列绑定参数</param>
        public void CreateExclusiveQueue (string queue, string exchange, string root_key = "", Dictionary<string, object> bind_args = null) {
            using (var channel = IConnection.CreateModel ()) {
                channel.QueueDeclare (queue, false, true, false, null);
                channel.QueueBind (queue: queue,
                    exchange: exchange,
                    routingKey: root_key,
                    arguments: bind_args);
            }
        }

        /// <summary>
        /// 创建死信交换机绑定参数
        /// </summary>
        /// <param name="death_exchange">死信交换机名称</param>
        /// <param name="death_routkey">死信交换机routkey</param>
        /// <returns>死信交换机参数</returns>
        public Dictionary<string, object> CreateParamFormDeathType (string death_exchange, string death_routkey = "") {
            Dictionary<String, Object> args = new Dictionary<string, object> ();
            if (!string.IsNullOrWhiteSpace (death_exchange)) {
                args.Add ("x-dead-letter-exchange", death_exchange);
            }
            if (!string.IsNullOrWhiteSpace (death_routkey)) {
                args.Add ("x-dead-letter-routing-key", death_routkey);
            }
            return args;
        }

        /// <summary>
        /// 事务操作
        /// </summary>
        /// <param name="action">执行内容</param>
        public void Transaction (Action<IModel> action) {
            using (var model = IConnection.CreateModel ()) {
                try {
                    model.TxSelect ();
                    action (model);
                    model.TxCommit ();
                } catch (Exception ex) {
                    model.TxRollback ();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 生产者
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="delivery">消息是否持久</param>
        /// <typeparam name="TMessage">消息模型</typeparam>
        /// <returns>是否发送成功</returns>
        public virtual void PublishMsg<TMessage> (TMessage message, bool delivery = true)
        where TMessage : ANSHMQMessagePublishBase {
            PublishMsg (new TMessage[] { message }, delivery);
        }

        /// <summary>
        /// 生产者
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="delivery">消息是否持久</param>
        /// <typeparam name="TMessage">消息模型</typeparam>
        /// <returns>是否发送成功</returns>
        public virtual void PublishMsg<TMessage> (TMessage[] message, bool delivery = true)
        where TMessage : ANSHMQMessagePublishBase {
            using (var model = IConnection.CreateModel ()) {
                PublishMsg (model, message, delivery);
            }
        }

        /// <summary>
        /// 生产者
        /// </summary>
        /// <param name="model">AMQP操作模型</param>
        /// <param name="message">消息内容</param>
        /// <param name="delivery">消息是否持久</param>
        /// <param name="createExchangeAndQueue">是否创建交换机和队列</param>
        /// <typeparam name="TMessage">消息模型</typeparam>
        /// <returns>是否发送成功</returns>
        public virtual void PublishMsg<TMessage> (IModel model, TMessage[] message, bool delivery = true, bool createExchangeAndQueue = false)
        where TMessage : ANSHMQMessagePublishBase {
            var props = model.CreateBasicProperties ();
            if (message?.Length > 0) {
                foreach (var message_item in message) {
                    if (message_item.Expiration > 0) {
                        props.Expiration = (message_item.Expiration).ToString ();
                    }
                    props.DeliveryMode = (byte) (delivery ? 2 : 1);
                    props.ContentType = "application/json";
                    props.ContentEncoding = "utf-8";

                    if (createExchangeAndQueue) {
                        InitPublish (message_item);
                    }

                    string exchange = message_item.QueueDelayOpen?message_item.ExchangeDelay : string.IsNullOrWhiteSpace (message_item.Exchange) ? "amq.direct" : message_item.Exchange;

                    string rootKey = message_item.QueueDelayOpen?message_item.RootKeyDelay : message_item.RootKey;

                    model.BasicPublish (exchange,
                        rootKey,
                        props,
                        ASCIIEncoding.UTF8.GetBytes (message_item.ToJson ()));
                }
            }
        }

        /// <summary>
        /// 生产者（确认模式）
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="delivery">消息是否持久</param>
        /// <param name="createExchangeAndQueue">是否创建交换机和队列</param>
        /// <typeparam name="TMessage">消息模型</typeparam>
        /// <returns>是否发送成功</returns>
        public virtual bool PublishMsgConfirm<TMessage> (TMessage message, bool delivery = true, bool createExchangeAndQueue = false)
        where TMessage : ANSHMQMessagePublishBase {
            return PublishMsgConfirm (new TMessage[] { message }, delivery, createExchangeAndQueue);
        }

        /// <summary>
        /// 生产者（确认模式）
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="delivery">消息是否持久</param>
        /// <param name="createExchangeAndQueue">是否创建交换机和队列</param>
        /// <typeparam name="TMessage">消息模型</typeparam>
        /// <returns>是否发送成功</returns>
        public virtual bool PublishMsgConfirm<TMessage> (TMessage[] message, bool delivery = true, bool createExchangeAndQueue = false)
        where TMessage : ANSHMQMessagePublishBase {
            using (var model = IConnection.CreateModel ()) {
                if (message?.Length > 0) {
                    model.ConfirmSelect ();
                    PublishMsg (model, message, delivery, createExchangeAndQueue);
                    try {
                        model.WaitForConfirmsOrDie ();
                        return true;
                    } catch {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 消费者
        /// </summary>
        /// <param name="message">消息模型</param>
        /// <param name="received">消费处理</param>
        /// <param name="requeue">消费失败时是否重新进入队列</param>
        /// <param name="prefetchCount">同时处理几条消息</param>
        /// <param name="createExchangeAndQueue">是否创建交换机和队列</param>
        /// <param name="cancellationToken">Task取消</param>
        public void RetrievingMessages<TMessage> (TMessage message, Func<string, Task<bool>> received, bool requeue, ushort prefetchCount, bool createExchangeAndQueue = false, CancellationToken cancellationToken = default (CancellationToken)) where TMessage : ANSHMQMessageRetrievingBase {
            if (createExchangeAndQueue) {
                InitRetrieving (message);
            }
            RetrievingMessages (message.Queue, received, requeue, prefetchCount, cancellationToken);
        }

        /// <summary>
        /// 消费者死信
        /// </summary>
        /// <param name="message">消息模型</param>
        /// <param name="received">消费处理</param>
        /// <param name="requeue">消费失败时是否重新进入队列</param>
        /// <param name="prefetchCount">同时处理几条消息</param>
        /// <param name="createExchangeAndQueue">是否创建交换机和队列</param>
        /// <param name="cancellationToken">Task取消</param>
        public void RetrievingDXMessages<TMessage> (TMessage message, Func<string, Task<bool>> received, bool requeue, ushort prefetchCount, bool createExchangeAndQueue = false, CancellationToken cancellationToken = default (CancellationToken)) where TMessage : ANSHMQMessageRetrievingBase {
            if (createExchangeAndQueue) {
                InitRetrieving (message);
            }
            RetrievingMessages (message.QueueDX, received, requeue, prefetchCount, cancellationToken);
        }

        /// <summary>
        /// 消费者
        /// </summary>
        /// <param name="queue">消费队列</param>
        /// <param name="received">消费处理</param>
        /// <param name="requeue">消费失败时是否重新进入队列</param>
        /// <param name="prefetchCount">同时处理几条消息</param>
        /// <param name="cancellationToken">Task取消</param>
        public virtual void RetrievingMessages (string queue, Func<string, bool> received, bool requeue, ushort prefetchCount, CancellationToken cancellationToken = default (CancellationToken)) {
            var ItemCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource (cancellationToken);
            Task.Run (() => {
                do {
                    try {
                        using (var channel = IConnection.CreateModel ()) {
                            RetrievingMessagesTask (channel, queue, received, requeue, prefetchCount);
                            ItemCancellationTokenSource.Token.WaitHandle.WaitOne ();
                        }
                    } catch {

                    }
                } while (true);
            }, ItemCancellationTokenSource.Token);
        }

        /// <summary>
        /// 消费者
        /// </summary>
        /// <param name="queue">消费队列</param>
        /// <param name="received">消费处理</param>
        /// <param name="requeue">消费失败时是否重新进入队列</param>
        /// <param name="prefetchCount">同时处理几条消息</param>
        /// <param name="cancellationToken">Task取消</param>
        void RetrievingMessages (string queue, Func<string, Task<bool>> received, bool requeue, ushort prefetchCount, CancellationToken cancellationToken = default (CancellationToken)) => RetrievingMessages (queue, (param) => received (param).Result, requeue, prefetchCount, cancellationToken);

        /// <summary>
        /// 消费者
        /// </summary>
        /// <param name="queue">消费队列</param>
        /// <param name="model">AMQP操作模型</param>
        /// <param name="received">消费处理</param>
        /// <param name="requeue">消费失败时是否重新进入队列</param>
        /// <param name="prefetchCount">同时处理几条消息</param>
        void RetrievingMessagesTask (IModel model, string queue, Func<string, bool> received, bool requeue, ushort prefetchCount) => RetrievingMessagesTask (model, queue, (msg) => (received (msg), requeue), prefetchCount);

        /// <summary>
        /// 消费者
        /// </summary>
        /// <param name="queue">消费队列</param>
        /// <param name="received">消费处理，Item1：操作是否成功，Item2：操作失败是否重新进入队列。</param>
        /// <param name="prefetchCount">同时处理几条消息</param>
        /// <param name="cancellationToken">Task取消</param>
        void RetrievingMessages (string queue, Func < string, (bool, bool) > received, ushort prefetchCount, CancellationToken cancellationToken = default (CancellationToken)) {
            var ItemCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource (cancellationToken);
            Task.Run (() => {
                do {
                    try {
                        using (var model = IConnection.CreateModel ()) {
                            RetrievingMessagesTask (model, queue, received, prefetchCount);
                            ItemCancellationTokenSource.Token.WaitHandle.WaitOne ();
                        }
                    } catch {

                    }
                } while (true);
            }, ItemCancellationTokenSource.Token);
        }

        /// <summary>
        /// 消费者
        /// </summary>
        /// <param name="queue">消费队列</param>
        /// <param name="received">消费处理，Item1：操作是否成功，Item2：操作失败是否重新进入队列。</param>
        /// <param name="prefetchCount">同时处理几条消息</param>
        /// <param name="cancellationToken">Task取消</param>
        void RetrievingMessages (string queue, Func < string, Task < (bool, bool) >> received, ushort prefetchCount, CancellationToken cancellationToken = default (CancellationToken)) => RetrievingMessages (queue, (param) => received (param).Result, prefetchCount, cancellationToken);

        /// <summary>
        /// 消费者
        /// </summary>
        /// <param name="queue">消费队列</param>
        /// <param name="model">AMQP操作模型</param>
        /// <param name="received">消费处理，Item1：操作是否成功，Item2：操作失败是否重新进入队列。</param>
        /// <param name="prefetchCount">同时处理几条消息</param>
        void RetrievingMessagesTask (IModel model, string queue, Func < string, (bool, bool) > received, ushort prefetchCount) {
            model.BasicQos (0, prefetchCount, false);
            var consumer = new EventingBasicConsumer (model);
            consumer.Received += (obj, eArgs) => {
                try {
                    var (success, requeue) = received (ASCIIEncoding.UTF8.GetString (eArgs.Body));
                    if (success) {
                        model.BasicAck (eArgs.DeliveryTag, false);
                    } else {
                        model.BasicNack (eArgs.DeliveryTag, false, requeue);
                    }
                } catch (Exception ex) {
                    model.BasicNack (eArgs.DeliveryTag, false, true);
                    throw ex;
                }
            };
            model.BasicConsume (queue, false, consumer);
        }
    }
}