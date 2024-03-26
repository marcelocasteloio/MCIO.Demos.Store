using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.OutputEnvelop.Enums;
using MCIO.OutputEnvelop.Models;

namespace MCIO.Demos.Store.Gateways.General.Factories;

public static class OutputEnvelopFactory
{
    public static OutputEnvelop.OutputEnvelop Create(ReplyHeader replyHeader)
    {
        var outputEnvelopType = replyHeader.ReplyResultType switch
        {
            ReplyResultType.Undefinied => OutputEnvelopType.Success,
            ReplyResultType.Success => OutputEnvelopType.Success,
            ReplyResultType.Partial => OutputEnvelopType.Partial,
            ReplyResultType.Error => OutputEnvelopType.Error,
            _ => OutputEnvelopType.Success,
        };

        if(replyHeader.ReplyMessageCollection.Count == 0)
            return OutputEnvelop.OutputEnvelop.Create(outputEnvelopType);

        var outputMessageCollection = new OutputMessage[replyHeader.ReplyMessageCollection.Count];

        for (int i = 0; i < replyHeader.ReplyMessageCollection.Count; i++)
        {
            var replyMessage = replyHeader.ReplyMessageCollection[i];

            outputMessageCollection[i] = OutputMessage.Create(
                type: replyMessage.Type switch
                {
                    ReplyMessageType.Undefinied => OutputMessageType.Information,
                    ReplyMessageType.Information => OutputMessageType.Information,
                    ReplyMessageType.Success => OutputMessageType.Success,
                    ReplyMessageType.Warning => OutputMessageType.Warning,
                    ReplyMessageType.Error => OutputMessageType.Error,
                    _ => OutputMessageType.Information,
                },
                code: replyMessage.Code,
                description: replyMessage.Description
            );
        }

        return OutputEnvelop.OutputEnvelop.Create(
            outputEnvelopType,
            outputMessageCollection,
            exceptionCollection: null
        );
    }
    public static OutputEnvelop.OutputEnvelop<TOutput?> Create<TOutput>(TOutput output, ReplyHeader replyHeader)
    {
        var outputEnvelopType = replyHeader.ReplyResultType switch
        {
            ReplyResultType.Undefinied => OutputEnvelopType.Success,
            ReplyResultType.Success => OutputEnvelopType.Success,
            ReplyResultType.Partial => OutputEnvelopType.Partial,
            ReplyResultType.Error => OutputEnvelopType.Error,
            _ => OutputEnvelopType.Success,
        };

        if (replyHeader.ReplyMessageCollection.Count == 0)
            return OutputEnvelop.OutputEnvelop<TOutput?>.Create(output, outputEnvelopType);

        var outputMessageCollection = new OutputMessage[replyHeader.ReplyMessageCollection.Count];

        for (int i = 0; i < replyHeader.ReplyMessageCollection.Count; i++)
        {
            var replyMessage = replyHeader.ReplyMessageCollection[i];

            outputMessageCollection[i] = OutputMessage.Create(
                type: replyMessage.Type switch
                {
                    ReplyMessageType.Undefinied => OutputMessageType.Information,
                    ReplyMessageType.Information => OutputMessageType.Information,
                    ReplyMessageType.Success => OutputMessageType.Success,
                    ReplyMessageType.Warning => OutputMessageType.Warning,
                    ReplyMessageType.Error => OutputMessageType.Error,
                    _ => OutputMessageType.Information,
                },
                code: replyMessage.Code,
                description: replyMessage.Description
            );
        }

        return OutputEnvelop.OutputEnvelop<TOutput?>.Create(
            output,
            outputEnvelopType,
            outputMessageCollection,
            exceptionCollection: null
        );
    }
}
