﻿using ResultSharp.Errors;
using ResultSharp.Extensions.FunctionalExtensions.Sync;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Zazagram.Abstractions;
using Zazagram.Services;

LoggingConfigure.ConfigureLogging();

var bot = new TelegramBotClient(Environment.GetEnvironmentVariable("ZAZAGRAM_TOKEN")!);

Subscribe.OnMessage(bot, "/bebra", async (ctx) => {
    if (ctx.RecievedMessage is not null) {
        await bot.SendMessage(ctx.RecievedMessage.Chat.Id, String.Join(" ", ctx.UpdateHistory.FindAll(u => u.Message?.Text is not null).Select(u => u.Message!.Text)));
        await bot.SendMessage(ctx.RecievedMessage.Chat.Id, "бебра отправлена");
    }
});

Subscribe.OnMessage(bot, (msg) => msg.Text is String msgText ? msgText : Error.Failure(""), async (ctx) => {
    if (ctx.RecievedMessage?.Text is not null) {
        (await LlmService.Recognize(ctx.RecievedMessage.Text, [])).Match(
            async ok => await bot.SendMessage(ctx.RecievedMessage.Chat.Id, ok),
            async err => await bot.SendMessage(ctx.RecievedMessage.Chat.Id, "Ошибка сосите")
        );
    }
});

Subscribe.SubscribeAll(bot);

Console.ReadLine();
