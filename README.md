# MHWildsHandlerBot

This repo is a monster hunter wilds discord bot that will track events, give you weapon and armor information, spit out item info, and eventually monster information such as weaknesses and harvesting.

Event information is parsed from the official website here: [official monster hunter wilds event quest site](https://info.monsterhunter.com/wilds/event-quest/en-us/schedule).

Currently the API used for gathering game knowledge is located here: [mhdb-wilds](https://github.com/LartTyler/mhdb-wilds) (take a peek and give them some love).

This was built using [NetCord](https://netcord.dev/), I highly advise taking a look at that site for some cool examples.  They dont show you EXACTLY where to place things to get them to work, but I do a pretty good job here with some simple examples.

## Disclaimer!!

This is a discord bot just built for fun and to create a helpful companion for me and my friends on discord. I want to just say that I do not claim any ownership of the images, the monster hunter name, or anything like that.  I also am a novice programmer and cant garuentee this wont accidentally just SPAM your discord.  So please use at your own risk.

## Description

This bot is a web scraper that parses the [official monster hunter wilds event quest site](https://info.monsterhunter.com/wilds/event-quest/en-us/schedule) and formats it into nice to read embed messages in discord.  It will download any images it needs to attach them properly and currently it has 3 commands.

### Available Commands:

#### Event & Challenge Quests

| Slash Command | Description |
| -------- | ------- |
| /checkevents | Checks the current event for the current week, these events run from 7pm cst to 6:59pm cst on Tuesdays, it will spit out an embeded message per event quest that is available (see below for an example) |
| /checknextweekevent | Checks the NEXT weeks event and spits out nice embed messages |
| /checkweekafternextevent | You guessed it, checks 2 weeks ahead for the event quest details |

#### Example: 

![Example Embed Message](/images/readmeexample1.png)

## Getting it to work

I probably wont make this publically available as a bot on discords site just yet (copyright and what not) but you are more than welcome to steal my code and take a peek at what you will need. I am excluding the appsettings.json, which if you read the tutorial on [NetCord](https://netcord.dev/), they will show you how to set it up.  I used vscode to run this (hence the .vscode folder) with the c# extension installed.  Then just run it like a console exe.  Happy Hunting!!!

