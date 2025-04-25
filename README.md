# MHWildsHandlerBot

This repo is a monster hunter wilds discord bot that will track events, give you weapon and armor information, spit out item info, and eventually monster information such as weaknesses and harvesting.

## Credit/References

* Event information
  - Parsed from the official website here: [official monster hunter wilds event quest site](https://info.monsterhunter.com/wilds/event-quest/en-us/schedule).

* API 
  - The api used for gathering game knowledge is located here: [mhdb-wilds](https://github.com/LartTyler/mhdb-wilds) (take a peek and give them some love).

* Weapon and armor pictures 
  - Web scrapped from an awesome database put together by [AtlasForge.gg/monster-hunter-wilds](https://atlasforge.gg/monster-hunter-wilds) which can help you plan out your gear, view an interactive map on another screen, and much more!  Support them by visiting their site, [discord](https://discord.gg/duAfUu45Wk_), or choosing their [ad-free subscription](https://atlasforge.gg/support).

This was built using [NetCord](https://netcord.dev/), I highly advise taking a look at that site for some cool examples.  They dont show you EXACTLY where to place things to get them to work, but I do a pretty good job here with some simple examples.

## Disclaimer!!

This is a discord bot just built for fun and to create a helpful companion for me and my friends on discord. I want to just say that I do not claim any ownership of the images, the monster hunter name, or anything like that.  Check the credits above to see where its due and how to support. I also am a novice programmer and cant garuentee this wont accidentally just SPAM your discord.  So please use at your own risk.

## Description

This bot is a web scraper that parses the [official monster hunter wilds event quest site](https://info.monsterhunter.com/wilds/event-quest/en-us/schedule) and formats it into nice to read embed messages in discord.  It will download any images it needs to attach them properly and currently it has 3 commands for events.

It also is slowly being updated to consume the [mhdb-wilds](https://github.com/LartTyler/mhdb-wilds) api with an "ask Gemma" command! (see below)

### Available Commands:

#### Event & Challenge Quests

| Slash Command | Description |
| -------- | ------- |
| /checkevents | Checks the current event for the current week, these events run from 7pm cst to 6:59pm cst on Tuesdays, it will spit out an embeded message per event quest that is available (see below for an example) |
| /checknextweekevent | Checks the NEXT weeks event and spits out nice embed messages |
| /checkweekafternextevent | You guessed it, checks 2 weeks ahead for the event quest details |

#### Example: 

![Example Embed Message](/images/examples/readmeexample1.png)

#### Ask Gemma (Weapon and Armor Info)

| Slash Command | Description |
| ------------- | ----------- |
| /askgemma armorset [name] | Get info about the armor set by looking it up by name (right now it just spits out how many peices are in the set) |
| /askgemma weapon [name] | Get info about the weapon by looking it up by name |

#### Example: `/askgemma weapon Hope Horn III`

![Example Ask Gemma for Weapon Info](/images/examples/askgemmaExample2.png)

#### Example: `/askgemma armorset Conga α`

![Example Ask Gemma for Armorsets](/images/examples/askgemmaExample1.png)

## Getting it to work

I probably wont make this publically available as a bot on discords site just yet (copyright and what not) but you are more than welcome to steal my code and take a peek at what you will need. 

### NetCord AppSettings.json (tells your server what bot to use)

I am excluding the appsettings.json, which if you read the tutorial on [NetCord](https://netcord.dev/), they will show you how to set it up.

### Dependencies if on Linux (since im running mine from a raspberry pi server)

In order to get the weapon sharpness to generate (I generate a html string and then use a tool to convert it to an image called [wkhtmltopdf](https://wkhtmltopdf.org/)) using linux, you need to install the wkhtmltopdf package:

```sh
sudo apt-get update
sudo apt-get install wkhtmltopdf
```

This should eliminate any errors coming from the server. For windows, this package comes in automatically for us.

### Starting the Bot

I used vscode to run this (hence the .vscode folder) with the c# extension installed.  Then just run it like a console exe.

Happy Hunting!!!

