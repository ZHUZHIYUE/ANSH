using System;
using System.Threading.Tasks;
using ANSH.MQ.RabbitMQ;

namespace ANSH.DDD.Domain.EventBus.RabbitMQ {
    /// <summary>
    /// 事件总线程
    /// </summary>
    public class ANSHRibbitMQEventBus : IANSHRibbitMQEventBus {

        ANSHMQFactoryBase ANSHMQFactoryBase { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="username">帐号</param>
        /// <param name="password">密码</param>
        /// <param name="port">端口号</param>
        /// <param name="virtualhost">虚拟地址</param>
        /// <param name="hostname">主机名称</param>
        public ANSHRibbitMQEventBus (string username, string password, int port, string virtualhost, string hostname) {
            ANSHMQFactoryBase = new ANSHMQFactoryBase (username, password, port, virtualhost, hostname);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose () => ANSHMQFactoryBase.Dispose ();

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="event">事件</param>
        /// <returns>是否发布成功</returns>
        public bool Publish<TModel> (params ANSHRibbitMQIntegrationEvent<TModel>[] @event) => ANSHMQFactoryBase.PublishMsgConfirm (@event);

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="func">订阅方法</param>
        /// <typeparam name="TANSHRibbitMQIntegrationEvent">事件</typeparam>
        /// <typeparam name="TModel">事件数据类型</typeparam>
        /// <typeparam name="TANSHRabbitMQIntegrationEventHandler">订阅事件</typeparam>
        /// <returns></returns>
        public void Subscribe<TANSHRibbitMQIntegrationEvent, TModel, TANSHRabbitMQIntegrationEventHandler> (Func<TModel, bool> func)
        where TANSHRibbitMQIntegrationEvent : ANSHRibbitMQIntegrationEvent<TModel>, new ()
        where TANSHRabbitMQIntegrationEventHandler : ANSHRabbitMQIntegrationEventHandler<TANSHRibbitMQIntegrationEvent>, new () {
            var retrievingMessages = new TANSHRabbitMQIntegrationEventHandler ();
            bool success;
            int repeat = 0;
            ANSHMQFactoryBase.RetrievingMessages (retrievingMessages, async (message) => {
                try {
                    if (repeat >= 5) {
                        repeat = 0;
                        return true;
                    }
                    if (repeat > 0) {
                        System.Threading.Thread.Sleep (1000);
                    }
                    var messageType = message.ToJsonObj<TANSHRibbitMQIntegrationEvent> ();
                    if (messageType == null || messageType.Body == null) {
                        success = func (default (TModel));
                    } else {
                        success = func (messageType.Body);
                    }
                    await Task.CompletedTask;
                    if (success) {
                        repeat = 0;
                    } else {
                        repeat++;
                    }
                } catch {
                    repeat++;
                    success = false;
                }
                return success;
            }, false, 1, true);

            ANSHMQFactoryBase.RetrievingDXMessages (retrievingMessages, async (message) => {
                try {
                    if (repeat > 0) {
                        System.Threading.Thread.Sleep (1000);
                    }

                    var messageType = message.ToJsonObj<TANSHRibbitMQIntegrationEvent> ();
                    if (messageType == null || messageType.Body == null) {
                        success = func (default (TModel));
                    } else {
                        success = func (messageType.Body);
                    }
                    await Task.CompletedTask;
                    if (success) {
                        repeat = 0;
                    } else {
                        repeat++;
                    }
                } catch {
                    repeat++;
                    success = false;
                }
                return success;
            }, true, 1, true);
        }
    }
}