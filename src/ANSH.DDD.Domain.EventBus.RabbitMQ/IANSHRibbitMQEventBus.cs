using System;

namespace ANSH.DDD.Domain.EventBus.RabbitMQ {
    /// <summary>
    /// 事件总线程
    /// </summary>
    public interface IANSHRibbitMQEventBus : IDisposable {
        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="event">事件</param>
        /// <returns>是否发布成功</returns>
        bool Publish<TModel> (params ANSHRibbitMQIntegrationEvent<TModel>[] @event);

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="func">订阅方法</param>
        /// <typeparam name="TANSHRibbitMQIntegrationEvent">事件</typeparam>
        /// <typeparam name="TModel">事件数据类型</typeparam>
        /// <typeparam name="TANSHRabbitMQIntegrationEventHandler">订阅事件</typeparam>
        /// <returns></returns>
        void Subscribe<TANSHRibbitMQIntegrationEvent, TModel, TANSHRabbitMQIntegrationEventHandler> (Func<TModel, bool> func)
        where TANSHRibbitMQIntegrationEvent : ANSHRibbitMQIntegrationEvent<TModel>, new ()
            where TANSHRabbitMQIntegrationEventHandler : ANSHRabbitMQIntegrationEventHandler<TANSHRibbitMQIntegrationEvent>, new ();
    }
}