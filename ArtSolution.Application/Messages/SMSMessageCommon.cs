using RestSharp.Serializers;
using System;

namespace ArtSolution.Messages
{

    public class HeaderConst
    {
        public const string AUTHORIZATION = "Authorization";
        public const string CONTENTTYPE = "Content-Type";
        public const string CONTENTMD5 = "Content-MD5";
        public const string MQVERSION = "x-mqs-version";
        public const string HOST = "Host";
        public const string DATE = "Date";
        public const string KEEPALIVE = "Keep-Alive";
    }


    public enum Method
    {
        GET,
        PUT,
        POST,
        DELETE
    }



    public interface MQSRequest
    {
    }

    [SerializeAs(Name = "Queue")]
    public class QueueAttributeSetRequest : MQSRequest
    {
        public int VisibilityTimeout { set; get; }
        public int MaximumMessageSize { set; get; }
        public int MessageRetentionPeriod { set; get; }
        public int DelaySeconds { set; get; }
        public int PollingWaitSeconds { set; get; }
    }



    public abstract class MQSResponse
    {
        public string Code { set; get; } //错误码 
        public string Message { set; get; } //错误消息
        public string RequestId { set; get; } //请求id
        public string HostId { set; get; }
    }

    [SerializeAs(Name = "Message")]
    public class MessageSendRequest : MQSRequest
    {
        public string MessageBody { set; get; }
        public int DelaySeconds { set; get; }
        public int Priority { set; get; }
    }

    public class MQSRequestException : Exception
    {
        public MQSRequestException(string message) : base(message)
        {

        }
    }


    public class NoContentResponse : MQSResponse
    {
    }


    public class QueueAttributeGetResponse : MQSResponse
    {
        public int ActiveMessages { set; get; }
        public string CreateTime { set; get; }
        public int DelayMessages { set; get; }
        public int InactiveMessages { set; get; }
        public string LastModifyTime { set; get; }
        public int MaximumMessageSize { set; get; }
        public string MessageRetentionPeriod { set; get; }
        public int PollingWaitSeconds { set; get; }
        public string QueueName { set; get; }
        public string QueueStatus { set; get; }
        public int VisibilityTimeout { set; get; }
    }


    public class MessageReceiveResponse : MQSResponse
    {
        public string MessageId { set; get; }
        public string ReceiptHandle { set; get; }
        public string MessageBodyMD5 { set; get; }
        public string MessageBody { set; get; }
        public long EnqueueTime { set; get; }
        public long NextVisibleTime { set; get; }
        public long FirstDequeueTime { set; get; }
        public long DequeueCount { set; get; }
        public int Priority { set; get; }
    }

    public class MessageVisibilityChangeResponse : MQSResponse
    {
        public string ReceiptHandle { set; get; }
        public long NextVisibleTime { set; get; }
    }
}
