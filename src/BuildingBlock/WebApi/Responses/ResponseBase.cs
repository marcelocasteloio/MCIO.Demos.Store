using MCIO.OutputEnvelop;
using MCIO.OutputEnvelop.Enums;
using MCIO.OutputEnvelop.Models;
using System.Text.Json.Serialization;

namespace MCIO.Demos.Store.BuildingBlock.WebApi.Responses;

public class ResponseBase
{
    // Properties
    [JsonPropertyName("messages")]
    public IEnumerable<ResponseMessage>? MessageCollection { get; }

    // Constructors
    protected ResponseBase(IEnumerable<ResponseMessage>? messages)
    {
        MessageCollection = messages;
    }

    // Public Methods
    public static ResponseBase Create(
        IEnumerable<ResponseMessage>? messages
    )
    {
        return new ResponseBase(messages);
    }
    public static ResponseBase FromOutputEnvelop(
        OutputEnvelop.OutputEnvelop outputEnvelop
    )
    {
        return new ResponseBase(
            messages: outputEnvelop.OutputMessageCollection.Select(ResponseMessage.FromOutputMessage)
        );
    }

    public OutputEnvelop.OutputEnvelop ToOutputEnvelop(int statusCode)
    {
        var outputEnvelopType = statusCode >= 200 && statusCode < 300
                ? OutputEnvelopType.Success
                : OutputEnvelopType.Error;

        if(MessageCollection?.Any() != true)
            OutputEnvelop.OutputEnvelop.Create(type: outputEnvelopType);

        // Copy messages
        var outputMessageCollection = new List<OutputMessage>(capacity: 5);
        foreach (var message in MessageCollection!)
        {
            outputMessageCollection.Add(
                OutputMessage.Create(
                    type: message.Type switch
                    {
                        ResponseMessageType.Information => OutputMessageType.Information,
                        ResponseMessageType.Success => OutputMessageType.Success,
                        ResponseMessageType.Warning => OutputMessageType.Warning,
                        ResponseMessageType.Error => OutputMessageType.Error,
                        _ => OutputMessageType.Information,
                    },
                    code: message.Code,
                    description: message.Description
                )
            );
        }

        return OutputEnvelop.OutputEnvelop.Create(
            type: outputEnvelopType, 
            outputMessageCollection: outputMessageCollection.ToArray(),
            exceptionCollection: null
        );
    }
}
public class ResponseBase<TData>
    : ResponseBase
{
    // Properties
    public TData? Data { get; }

    // Constructors
    private ResponseBase(
        TData? data,
        IEnumerable<ResponseMessage>? messages
    ) : base(messages)
    {
        Data = data; 
    }

    // Public Methods
    public static ResponseBase<TData?> Create(
        TData? data,
        IEnumerable<ResponseMessage>? messages
    )
    {
        return new ResponseBase<TData?>(data, messages);
    }
    public static ResponseBase<TData?> FromOutputEnvelop(
        TData? data,
        OutputEnvelop.OutputEnvelop outputEnvelop
    )
    {
        return new ResponseBase<TData?>(
            data,
            messages: outputEnvelop.OutputMessageCollection.Select(ResponseMessage.FromOutputMessage)
        );
    }
    public static ResponseBase<TData?> FromOutputEnvelop(
        OutputEnvelop<TData?> outputEnvelop
    )
    {
        return FromOutputEnvelop(
            data: outputEnvelop.Output,
            outputEnvelop
        );
    }
    public OutputEnvelop<TData?> ToOutputEnvelopWithData(int statusCode)
    {
        var outputEnvelopType = statusCode >= 200 && statusCode < 300
                ? OutputEnvelopType.Success
                : OutputEnvelopType.Error;

        if (MessageCollection?.Any() != true)
            OutputEnvelop<TData?>.Create(output: Data, type: outputEnvelopType);

        // Copy messages
        var outputMessageCollection = new List<OutputMessage>(capacity: 5);
        foreach (var message in MessageCollection!)
        {
            outputMessageCollection.Add(
                OutputMessage.Create(
                    type: message.Type switch
                    {
                        ResponseMessageType.Information => OutputMessageType.Information,
                        ResponseMessageType.Success => OutputMessageType.Success,
                        ResponseMessageType.Warning => OutputMessageType.Warning,
                        ResponseMessageType.Error => OutputMessageType.Error,
                        _ => OutputMessageType.Information,
                    },
                    code: message.Code,
                    description: message.Description
                )
            );
        }

        return OutputEnvelop<TData?>.Create(
            output: Data,
            type: outputEnvelopType,
            outputMessageCollection: outputMessageCollection.ToArray(),
            exceptionCollection: null
        );
    }
}
