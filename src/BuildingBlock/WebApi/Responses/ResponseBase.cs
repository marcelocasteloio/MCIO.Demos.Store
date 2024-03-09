using MCIO.OutputEnvelop;

namespace MCIO.Demos.Store.BuildingBlock.WebApi.Responses;

public class ResponseBase
{
    // Properties
    public IEnumerable<ResponseMessage>? Messages { get; }

    // Constructors
    protected ResponseBase(IEnumerable<ResponseMessage>? messages)
    {
        Messages = messages;
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
}
