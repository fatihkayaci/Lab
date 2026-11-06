using System;
using Anthropic;
using Anthropic.Models.Messages;
using Anthropic.Exceptions;

AnthropicClient client = new();
List<MessageParam> history = [];
while (true)
{
    string replyText = "";
    Console.Write("Sen: ");
    string? input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input))
        break;

    history.Add(new UserMessageParam(input));
    try
    {
        MessageCreateParams request = new()
        {
            MaxTokens = 1024,
            Messages = history,
            Model = Model.ClaudeOpus5_5,
        };


        Console.Write("Claude: ");
        await foreach (var streamEvent in client.Messages.CreateStreaming(request))
        {
            if (streamEvent.TryPickContentBlockDelta(out var deltaEvent))
            {
                if (deltaEvent.Delta.TryPickText(out var textDelta))
                {
                    replyText += textDelta.Text;
                    Console.Write(textDelta.Text);
                }
            }
        }

    }
    catch (AnthropicUnauthorizedException)
    {
        Console.WriteLine("API anahtarı geçersiz");
        history.RemoveAt(history.Count - 1);
        break;
    }
    
    Console.WriteLine("");
    history.Add(new AssistantMessageParam(replyText));
}

