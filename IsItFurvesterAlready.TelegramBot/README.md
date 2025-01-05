# IsItFurvesterAlready Telegram Bot
Telegram Bot for the Furvester Furry Convention that acts on the same config file as the Web Version, hosted on https://isitfurvesteralready.info

The bot can be accessed via [@isitfurvesteralready_bot](https://telegram.me/isitfurvesteralready_bot)

## Setup
To set up, create a config.json in a D:\Furvester folder with the following structure:
```
{
    "Dates": {
        "StartDate": "2025-12-29",
        "EndDate": "2026-01-02"
    },
    "TelegramBotToken": "<REDACTED>",
    "ThrottleTimeOutSeconds": 15
}
```

Enter the start and end date of the convention in the ISO format in the Config JSON.
Create a new Telegram Bot using the BotFather and enter the bot token.
You can change the throttle time out (used in Groups, not in Private Chats) to any value you want.

## Hosting
This application is a standard Windows Console Application and cannot natively be hosted as a Windows Service.
The easiest way to do so, is to use [NSSM](https://nssm.cc/).
To create a log file from the bot, use the option to redirect stdout to a file.