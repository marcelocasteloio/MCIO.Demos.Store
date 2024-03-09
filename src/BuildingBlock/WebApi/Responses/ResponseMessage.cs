using MCIO.OutputEnvelop.Models;

namespace MCIO.Demos.Store.BuildingBlock.WebApi.Responses;
public readonly record struct ResponseMessage
{
    // Properties
    public ResponseMessageType Type { get; }
    public string Code { get; }
    public string Description { get; }

    // Constructors
    private ResponseMessage(
        ResponseMessageType type,
        string code,
        string description
    )
    {
        Type = type;
        Code = code;
        Description = description;
    }

    // Public Methods
    public static ResponseMessage Create(
        ResponseMessageType type,
        string code,
        string description
    )
    {
        return new ResponseMessage(type, code, description);
    }
    public static ResponseMessage FromOutputMessage(OutputMessage outputMessage)
    {
        return new ResponseMessage(
            type: outputMessage.Type switch
            {
                OutputEnvelop.Enums.OutputMessageType.Information => ResponseMessageType.Information,
                OutputEnvelop.Enums.OutputMessageType.Success => ResponseMessageType.Success,
                OutputEnvelop.Enums.OutputMessageType.Warning => ResponseMessageType.Warning,
                OutputEnvelop.Enums.OutputMessageType.Error => ResponseMessageType.Error,
                _ => ResponseMessageType.Information,
            }, 
            code: outputMessage.Code, 
            description: outputMessage.Description
        );
    }
}
