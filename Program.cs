﻿using Abstractions;
using Microsoft.VisualBasic;
using Telegram.Bot;
using Zazagram.Services;

LoggingConfigure.ConfigureLogging();

var bot = new TelegramBotClient(Environment.GetEnvironmentVariable("ZAZAGRAM_TOKEN")!);

Subscribe.OnMessage(bot, "/bebra", async (ctx) => {
    if (ctx.RecievedMessage is not null) {
        await bot.SendMessage(ctx.RecievedMessage.Chat.Id, "бебра отправлена");
    }
});

//Subscribe.OnMessage(bot, (msg) => "penis", async (ctx) => {
//    var userId = ctx.RecievedMessage.From.Id;
//    var photos = await bot.GetUserProfilePhotos(userId);
//    var pp = photos.Photos;
//    foreach (var photo in pp) {
//        foreach (var photoPhoto in photo) {
//            var photoId = photoPhoto.FileId;
//            var file = await bot.GetFile(photoId);
//            await bot.SendPhoto(ctx.RecievedMessage.Chat.Id, file);
//        }
//    }
//});

Subscribe.OnMessage(bot, (msg) => msg.Text ?? "", async (ctx) => {
    if (ctx.RecievedMessage?.Text is not null) {
        var res = await LlmService.Recognize(ctx.RecievedMessage.Text, []);
        await bot.SendMessage(ctx.RecievedMessage.Chat.Id, res);
    }
});

Console.ReadLine();
