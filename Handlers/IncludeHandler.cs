using BotFramework;
using BotFramework.Attributes;
using BotFramework.Enums;
using BotFramework.Setup;
using Telegram.Bot.Types.Enums;

namespace CompilerTelegramBot.Handlers
{
    class IncludeHandler : BotEventHandler
    {
        [Message(InChat.Private, MessageFlag.HasDocument)]
        public async Task IncludeHandle()
        {
            try
            {
                var fileName = RawUpdate.Message?.Document?.FileName;

                if (string.IsNullOrEmpty(fileName) || !Path.GetExtension(fileName).Equals(".inc"))
                {
                    return;
                }

                var userTelegramId = RawUpdate.Message?.From?.Id;

                if (userTelegramId == null)
                {
                    return;
                }

                var documentId = RawUpdate?.Message?.Document.FileId;

                if (documentId == null)
                {
                    return;
                }

                var document = await Bot.GetFileAsync(documentId);
                var documentSize = document.FileSize / 1000.0;

                if (documentSize > 160.0)
                {
                    await Bot.SendDiceAsync(Chat);
                    return;
                }

                var directoryPath = $"users/amxmodx/{userTelegramId}/scripting/include";

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var message = await Bot.SendTextMessageAsync(Chat, "Downloading...");

                using (var fileStream = new FileStream($"{directoryPath}/{fileName}", FileMode.Create))
                {
                    await Bot.DownloadFileAsync(document.FilePath, fileStream);
                }

                if (FileBinaryChecker.IsBinaryFile($"{directoryPath}/{fileName}"))
                {
                    File.Delete($"{directoryPath}/{fileName}");
                    await Bot.EditMessageTextAsync(Chat, message.MessageId, "Binary files not allowed.");

                    return;
                }

                await Bot.EditMessageTextAsync(Chat, message.MessageId, $"Include <code>{fileName}</code>({documentSize}Kb) success downloaded.", parseMode: ParseMode.Html);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await Bot.SendTextMessageAsync(Chat, "Something went wrong... Please try again later.");
            }
        }
    }
}