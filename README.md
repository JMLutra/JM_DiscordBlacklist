# JM_DiscordBlacklist
This is a simple Discord bot that allows you to blacklist links using lists that can be found online.
This Bot is made to be selfhosted.
## Running the Bot
### Requirements
The .Net 6.0 SDK is required to build and run the bot.

A Discord Bot with the Message Content Intent and the following pewrmissions on each guild it should be active on:
- View Channels
- Send Messages (at least to the specified log channel)
- Manage Messages
- Timeout Members (if a timeout is specified)


The bot also needs a role higher than the members whose links should be checked.

### Setup
1. Clone the repository using `git clone https://github.com/JMLutra/JM_DiscordBlacklist.git`
2. Create a json file somewhere on your system with the following syntax:

    ```javascript
    {
        "botconfig": {
            "token": "", // The token of the bot
            "activity": "", // The activity of the bot
            "adminIds": [  ] // The IDs of the users that can enable/disable the link checking
        },
    
        "lists": [
            {
            "url": "", // The url to the list of bad links
            "topic": "", // The topic of the list
            "comment": "", // Character that signifies a comment
            "prefix": "", // Possible Prefix to remove
            "suffix": "" // Possible Suffix to remove
            }
        ],
    
        "guilds": [
            {
            "guildId": , // The ID of the guild
            "channelId": , // The ID of the channel where a message should be sent when a link is found
            "timeout":  // in h; 0 for no timeout
            }
        ]
    }
    ```
    You can have as many lists and guilds as you want, just repeat the json objects.

3. Build the bot using `dotnet build -c Release`
4. Change into the `bin/Release/net6.0` directory
5. Run the bot using `dotnet JM_DiscordBlacklist.dll <path to config file>`

## Usage
The bot will start by fetching all the lists specified in your config file. 

Between infos about the discord bot, it tells you in cyan how many links it now has on it's blacklist, how long it took to fetch them and how long it takes to search for a link (typically 0ms).

After that, it will listen for messages in the guilds specified in the config file. 

If it finds a blacklisted link, the message will be deleted and a message with all the necesary info will be sent to the channel specified in the config file.

When a timeout is specified for the guild, the bot will carry it out.

The bot automatically refreshes the blacklist every 30 minutes. This only takes a few ms.

### Commands
The bot has two Slashcommands:
`/checklinks enable/disable` this enables or disables the link checking on all guilds specified as well as sending a message to all specified channels to inform of this change. This command can only be used by the users specified in the config file under `adminIds`.

`/shutdown` This command can only be used by the owner of the bot and shuts down the bot.
