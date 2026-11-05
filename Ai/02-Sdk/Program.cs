using System;
using Anthropic;
using Anthropic.Models.Messages;

AnthropicClient client = new();

MessageCreateParams parameters = new()
{
    MaxTokens = 1024,
    Messages =
    [
        new UserMessageParam("Hello, Claude")
    ],
    Model = Model.ClaudeOpus5_5,
};

var message = await client.Messages.Create(parameters);
foreach (var block in message.Content)
{
    if (block.TryPickText(out var textBlock))
    {
        Console.WriteLine(textBlock.Text);
    }
}