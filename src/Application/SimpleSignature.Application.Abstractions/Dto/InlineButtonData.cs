namespace SimpleSignature.Application.Abstractions.Dto;

public class InlineButtonData
{
    public string Text { get; }
    public string CallbackData { get; }

    public InlineButtonData(string text, string callbackData)
    {
        Text = text;
        CallbackData = callbackData;
    }
}