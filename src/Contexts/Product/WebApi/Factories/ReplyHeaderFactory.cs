using MCIO.Demos.Store.Commom.Protos.V1;

namespace MCIO.Demos.Store.Product.WebApi.Factories;

public static class ReplyHeaderFactory
{
    public static ReplyHeader Create(OutputEnvelop.OutputEnvelop outputEnvelop)
    {
        var replyHeader = new ReplyHeader();

        foreach (var outputMessage in outputEnvelop.OutputMessageCollection)
            replyHeader.ReplyMessageCollection.Add(
                new ReplyMessage()
                {
                    Code = outputMessage.Code,
                    Description = outputMessage.Description,
                    Type = outputMessage.Type switch
                    {
                        OutputEnvelop.Enums.OutputMessageType.Information => ReplyMessageType.Information,
                        OutputEnvelop.Enums.OutputMessageType.Success => ReplyMessageType.Success,
                        OutputEnvelop.Enums.OutputMessageType.Warning => ReplyMessageType.Warning,
                        OutputEnvelop.Enums.OutputMessageType.Error => ReplyMessageType.Error,
                        _ => ReplyMessageType.Information
                    }
                }
            );

        replyHeader.ReplyResultType = outputEnvelop.Type switch
        {
            OutputEnvelop.Enums.OutputEnvelopType.Success => ReplyResultType.Success,
            OutputEnvelop.Enums.OutputEnvelopType.Partial => ReplyResultType.Partial,
            OutputEnvelop.Enums.OutputEnvelopType.Error => ReplyResultType.Error,
            _ => ReplyResultType.Undefinied
        };

        return replyHeader;
    }
}
