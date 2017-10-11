namespace ArtSolution.Messages
{

    public class MessageSendResponse : MQSResponse
    {
        public string MessageId { set; get; }

        public string MessageBodyMD5 { set; get; }
    }
}
