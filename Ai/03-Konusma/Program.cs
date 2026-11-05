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

    Message response = await client.Messages.Create(request);

    string replyText = "";
    foreach (var block in response.Content)
    {
        if (block.TryPickText(out var textBlock))
        {
            replyText += textBlock.Text;
        }
    }

    Console.WriteLine("Claude: " + replyText);
    history.Add(new AssistantMessageParam(replyText));
}



//string ilkCevap = "";

// MessageCreateParams parameters = new()
// {
//     MaxTokens = 1024,
//     Messages =
//     [
//         new UserMessageParam("Benim adım fatih")
//     ],
//     Model = Model.ClaudeOpus5_5,
// };

// var message = await client.Messages.Create(parameters);

// foreach (var block in message.Content)
// {
//     if (block.TryPickText(out var textBlock))
//     {
//         Console.WriteLine(textBlock.Text);
//         ilkCevap = textBlock.Text;
//     }
// }

// MessageCreateParams parameters2 = new()
// {
//     MaxTokens = 1024,
//     Messages =
//     [
//         new UserMessageParam("Benim adım fatih"),
//         new AssistantMessageParam(ilkCevap),
//         new UserMessageParam("Benim adım ne?")
//     ],
//     Model = Model.ClaudeOpus5_5,
// };

// var message2 = await client.Messages.Create(parameters2);

// foreach (var block in message2.Content)
// {
//     if (block.TryPickText(out var textBlock))
//     {
//         Console.WriteLine(textBlock.Text);
//     }
// }