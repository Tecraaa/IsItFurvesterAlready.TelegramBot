using IsItFurvesterAlready.TelegramBot;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

// Prepare config.json (also used for the website)
string jsonContent = System.IO.File.ReadAllText(@"D:\Furvester\config.json");
Config? appConfig = JsonSerializer.Deserialize<Config>(jsonContent);
string botToken = appConfig.TelegramBotToken;
int throttleTimeOutSeconds = appConfig.ThrottleTimeOutSeconds;

// Create Telegram bot and dictionary for throttling (TKey = Chat ID, TValue = Message DateTime)
Dictionary<long, DateTime> lastSentDictionary = new();
TelegramBotClient telegramBotClient = new(botToken);

// Start processing
Console.WriteLine($"[{DateTime.UtcNow}] Started listening to messages...");
telegramBotClient.OnMessage += async (msg, type) =>
{
    // Log incoming message and ignore if empty
    Console.WriteLine($"[{DateTime.UtcNow}] Received message by @{msg.From?.Username} from {msg.Chat.Id} ({msg.Chat.Type}): {msg.Text}");
    if (msg.Text == null) return;

    // Check the incoming message for a corresponding command and reply
    var lowerCaseMsgText = msg.Text.ToLower();
    string answer = "";
    switch (lowerCaseMsgText)
    {
        // Introductory message, only sent in Private chats
        case "/start":
            if (msg.Chat.Type == ChatType.Private)
            {
                answer = "Hi! Thank you for messaging this stupid bot.\nIdk what I'm doing here, but you can message me and ask whether it is Furvester already or not.\n\nExample: /isitfurvesteralready or /isitfurvesteryet.\n\nAlso check: https://isitfurvesteralready.info\n";
            }
            break;

        // Default use case
        case "/isitfurvesteralready":
            answer = Functions.YesOrNo(appConfig);
            break;

        // Default use case
        case "/isitfurvesteryet":
            answer = Functions.YesOrNo(appConfig);
            break;

        // ... yeah.
        case "/isitfurvesterthisyear":
            answer = "No. :(";
            break;

        // Not so surprising easter egg, only sent in Private chats 
        case "/xcq":
            if (msg.Chat.Type == ChatType.Private)
            {
                answer = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";
            }
            break;

        // Message for unknown commands, only sent in Private chats 
        default:
            if (msg.Chat.Type == ChatType.Private)
            {
                answer = "Unknown command!";
            }
            break;
    }

    // Set up parameters to reply directly to the user messaging
    var replyParameters = new ReplyParameters() {
        MessageId = msg.MessageId,
        ChatId = msg.Chat.Id
    };
    
    // Throttling mechanism for anything that isn't a Private chat
    if (msg.Chat.Type != ChatType.Private)
    {
        // Create entry in dictionary, if it doesn't already exist
        if (!lastSentDictionary.ContainsKey(msg.Chat.Id))
        {
            lastSentDictionary.Add(msg.Chat.Id, msg.Date);
        }
        else
        {
            // Check if enough time passed to reply again.
            if (!Functions.CanSendMessage(lastSentDictionary, msg.Chat.Id, throttleTimeOutSeconds))
            {
                // Ignore and return
                Console.WriteLine($"[{DateTime.UtcNow}] Last message in chat this was sent less than {throttleTimeOutSeconds} seconds ago! Ignoring...");
                return;
            }
            else
            {
                // Write last sent DateTime
                lastSentDictionary[msg.Chat.Id] = msg.Date;
            }
        } 
    }

    // Finally send the message and log it
    await telegramBotClient.SendMessage(msg.Chat.Id, answer, replyParameters: replyParameters);
    Console.WriteLine($"[{DateTime.UtcNow}] Replied: {answer}");
};

// Used to close the application when run interactively
Console.WriteLine("Press Enter to exit...");
Console.ReadLine();
Console.ReadLine();