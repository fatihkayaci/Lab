using System;
using Anthropic;
using Anthropic.Models.Messages;

AnthropicClient client = new();
List<MessageParam> history = [];

while (true)
{
    Console.Write("Sen: ");
    string? input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input))
        break;

    history.Add(new UserMessageParam(input));

    MessageCreateParams request = new()
    {
        MaxTokens = 1024,
        Messages = history,
        Model = Model.ClaudeOpus5_5,
    };


    string replyText = "";
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

    Console.WriteLine("");
    history.Add(new AssistantMessageParam(replyText));
}

