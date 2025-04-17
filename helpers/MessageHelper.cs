using System.Diagnostics;
using System.Globalization;
using CoreHtmlToImage;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using NetCord.Rest;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using WildsApi;

namespace Helpers {
    public static class MessageHelper {
        public static T CreateMessage<T>() where T : IMessageProperties, new()
        {
            return new()
            {
                Content = "",
                Components = [],
            };
        }

        public static async Task<T> GetEventMessage<T>(int week) where T: IMessageProperties, new() {
            try {
                // download the page as html and save as image
                var myEvent = getCurrentEvents(week);

                var message = CreateMessage<T>();

                if (IsEventPosted(myEvent)) {
                    
                    var tableRows = myEvent.QuerySelectorAll("tr");

                    for(int i = 1; i < tableRows.Count(); i++) {
                        var eventImage = myEvent.QuerySelector($"table > tbody > tr:nth-child({i}) > td.image > img").GetAttributeValue("src","");
                        var eventTitle = myEvent.QuerySelector($"table > tbody > tr:nth-child({i}) > td.quest > div > span").InnerText;
                        var eventDescription = myEvent.QuerySelector($"table > tbody > tr:nth-child({i}) > td.quest > p.txt").InnerText;
                        var eventDifficulty = myEvent.QuerySelector($"table > tbody > tr:nth-child({i}) > td.level > span").InnerText;
                        var questInfoFieldNames = myEvent.QuerySelectorAll($"table > tbody > tr:nth-child({i}) > td.overview > ul > li > span.overview_dt");
                        var questInfoFieldValues = myEvent.QuerySelectorAll($"table > tbody > tr:nth-child({i}) > td.overview > ul > li > span.overview_dd");

                        var fileName = $"event{i}week{week}.png";

                        await downloadImage(eventImage, fileName);

                        var embed = new EmbedProperties()
                        {
                            Title = "Monster Hunter Event Quest: " + eventTitle,
                            Description = eventDescription,
                            Url =$"https://info.monsterhunter.com/wilds/event-quest/en-us/schedule?=e{i}", // must be unique for multiple embeds
                            Timestamp = DateTimeOffset.UtcNow,
                            Color = new(0xFFA500),
                            Footer = new()
                            {
                                Text = "Happy Hunting!!"
                            },
                            Image = $"attachment://{fileName}",
                            Fields =
                            [
                                new()
                                {
                                    Name = "Difficulty",
                                    Value = eventDifficulty,
                                    Inline = true,
                                },
                            ],
                        };

                        for(int j = 0; j < questInfoFieldNames.Count(); j++) {
                            var field = new EmbedFieldProperties();
                            field.Name = questInfoFieldNames[j].InnerText.Trim().TrimStart(':');
                            field.Value = questInfoFieldValues[j].InnerText.Trim().TrimStart(':');

                            // if we find the date field, lets format it
                            if (field.Name.Contains("Date")) {
                                var temp = DateTime.ParseExact(field.Value, "MM.dd.yyyy HH:mm", CultureInfo.InvariantCulture);
                                field.Value = temp.ToString("MM/dd/yy h:mm tt");
                            }

                            field.Inline = true;
                            embed.AddFields(field);
                        }

                        var attachment = new AttachmentProperties(fileName, new MemoryStream(File.ReadAllBytes("images/" + fileName)));
                        message.AddAttachments(attachment);
                        message.AddEmbeds(embed);
                    }
                } else {
                    // else, we need to just display "comming soon"
                    var embed = new EmbedProperties()
                    {
                        Title = "Coming Soon",
                        Description = "The event quest has not been posted yet, try again later!",
                        Url =$"https://info.monsterhunter.com/wilds/event-quest/en-us/schedule?=event",
                        Timestamp = DateTimeOffset.UtcNow,
                        Color = new(0xFFA500),
                        Footer = new()
                        {
                            Text = "Happy Hunting!!"
                        },
                        Image = $"attachment://comingsoon.png",
                    };

                    var attachment = new AttachmentProperties("comingsoon.png", new MemoryStream(File.ReadAllBytes("images/comingsoon.png")));
                    message.AddAttachments(attachment);
                    message.AddEmbeds(embed);
                }
                
                // either the embedded quests or a single embed with the comming soon image
                return message;
            }
            catch
            {
                var message = CreateMessage<T>();
                message.Content = "Sorry, request failed! Try again later!";
                return message;
            }
        }

        public static async Task<T> GetChallengeMessage<T>(int week) where T: IMessageProperties, new() {
            try {
                // download the page as html and save as image
                var myChallenge = getCurrentChallenges(week);

                var message = CreateMessage<T>();

                if (IsChallengeComingSoon(myChallenge)) {
                    
                    var tableRows = myChallenge.QuerySelectorAll("tr");

                    for(int i = 1; i < tableRows.Count(); i++) {
                        var challengeImage = myChallenge.QuerySelector($"table > tbody > tr:nth-child({i}) > td.image > img").GetAttributeValue("src","");
                        var challengeTitle = myChallenge.QuerySelector($"table > tbody > tr:nth-child({i}) > td.quest > div > span").InnerText;
                        var challengeDescription = myChallenge.QuerySelector($"table > tbody > tr:nth-child({i}) > td.quest > p.txt").InnerText;
                        var challengeDifficulty = myChallenge.QuerySelector($"table > tbody > tr:nth-child({i}) > td.level > span").InnerText;
                        var questInfoFieldNames = myChallenge.QuerySelectorAll($"table > tbody > tr:nth-child({i}) > td.overview > ul > li > span.overview_dt");
                        var questInfoFieldValues = myChallenge.QuerySelectorAll($"table > tbody > tr:nth-child({i}) > td.overview > ul > li > span.overview_dd");

                        var fileName = $"challenge{i}week{week}.png";

                        await downloadImage(challengeImage, fileName);

                        var embed = new EmbedProperties()
                        {
                            Title = "Monster Hunter Challenge Quest: " + challengeTitle,
                            Description = challengeDescription,
                            Url =$"https://info.monsterhunter.com/wilds/event-quest/en-us/schedule?=c{i}", // must be unique for multiple embeds
                            Timestamp = DateTimeOffset.UtcNow,
                            Color = new(0x32a852),
                            Footer = new()
                            {
                                Text = "Happy Hunting!!"
                            },
                            Image = $"attachment://{fileName}",
                            Fields =
                            [
                                new()
                                {
                                    Name = "Difficulty",
                                    Value = challengeDifficulty,
                                    Inline = true,
                                },
                            ],
                        };

                        for(int j = 0; j < questInfoFieldNames.Count(); j++) {
                            var field = new EmbedFieldProperties();
                            field.Name = questInfoFieldNames[j].InnerText.Trim().TrimStart(':');
                            field.Value = questInfoFieldValues[j].InnerText.Trim().TrimStart(':');

                            // if we find the date field, lets format it
                            if (field.Name.Contains("Date")) {
                                var temp = DateTime.ParseExact(field.Value, "MM.dd.yyyy HH:mm", CultureInfo.InvariantCulture);
                                field.Value = temp.ToString("MM/dd/yy h:mm tt");
                            }

                            field.Inline = true;
                            embed.AddFields(field);
                        }

                        var attachment = new AttachmentProperties(fileName, new MemoryStream(File.ReadAllBytes("images/" + fileName)));
                        message.AddAttachments(attachment);
                        message.AddEmbeds(embed);
                    }
                } else if (myChallenge != null) {
                    // else, we need to just display "comming soon"
                    var embed = new EmbedProperties()
                    {
                        Title = "Coming Soon",
                        Description = "The challenge quest has not been posted yet, try again later!",
                        Url =$"https://info.monsterhunter.com/wilds/event-quest/en-us/schedule?=challenge",
                        Timestamp = DateTimeOffset.UtcNow,
                        Color = new(0x32a852),
                        Footer = new()
                        {
                            Text = "Happy Hunting!!"
                        },
                        Image = $"attachment://comingsoon.png",
                    };

                    var attachment = new AttachmentProperties("comingsoon.png", new MemoryStream(File.ReadAllBytes("images/comingsoon.png")));
                    message.AddAttachments(attachment);
                    message.AddEmbeds(embed);
                }
                
                // either the embedded quests or a single embed with the comming soon image
                return message;
            }
            catch
            {
                var message = CreateMessage<T>();
                message.Content = "Sorry, request failed! Try again later!";
                return message;
            }
        }

        public static HtmlNode? getCurrentEvents(int week = 0) {
            try
            {
                var web = new HtmlWeb();
                var url = "https://info.monsterhunter.com/wilds/event-quest/en-us/schedule?utc=-5";
                var document = web.Load(url);

                var htmlNode = document.DocumentNode.QuerySelector($"#tab{week}").QuerySelector(".table2");

                return htmlNode;
            }
            catch(Exception ex) 
            {   
                Debug.WriteLine(ex, ex.Message);
                Console.WriteLine("Request Failed");
                return null;
            }
        }

        public static HtmlNode? getCurrentChallenges(int week = 0) {
            try
            {
                var web = new HtmlWeb();
                var url = "https://info.monsterhunter.com/wilds/event-quest/en-us/schedule?utc=-5";
                var document = web.Load(url);

                var htmlNode = document.DocumentNode.QuerySelector($"#tab{week}").QuerySelector(".table3");

                return htmlNode;
            }
            catch(Exception ex) 
            {   
                Debug.WriteLine(ex, ex.Message);
                Console.WriteLine("Request Failed");
                return null;
            }
        }

        public static async Task downloadImage(string url, string filename) {
            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
                    byte[] imageBytes = await client.GetByteArrayAsync(url);
                    await File.WriteAllBytesAsync("images/" + filename, imageBytes);
                    Console.WriteLine($"Image downloaded and saved as: {filename}");
                }
                catch (Exception ex)
                {
                    // Handle any exceptions (e.g., network issues, invalid URL)
                    Console.WriteLine($"Error downloading image: {ex.Message}");
                }
            }
        }

        public static string GetRoleFromId(string roleId)
        {
            return $"<@&{roleId}>";
        }

        // Sometimes the latest info says "Coming Soon" instead of an actual post, so lets look for it
        public static bool IsEventPosted(HtmlNode? myEvent){
            if (myEvent == null) return false;

            var commingSoonText = myEvent.QuerySelector(".coming-quest_inner");

            return commingSoonText == null;
        }

        public static bool IsChallengeComingSoon(HtmlNode? myChallenge){
            if (myChallenge == null) return false;

            var commingSoonText = myChallenge.QuerySelector(".coming-quest_inner");

            return commingSoonText == null;
        }

        public static bool IsChallengePostedAtAll(HtmlNode? myChallenge){
            if (myChallenge == null) return false;

            var commingSoonText = myChallenge.QuerySelector("");

            return commingSoonText == null;
        }

        public static async Task<string?> downloadWeaponImage(string weaponName)
        {
            if (weaponName == null) return null;

            // set up the file name and see if it has already been downloaded
            var fileName = $"{weaponName.Replace(" ", "_")}.png";
            if (File.Exists($"images/{fileName}")) {
                Console.WriteLine($"Weapon image already exists as {fileName}.");
                return fileName;
            }

            using (var client = new HttpClient())
            {
                try
                {
                    var web = new HtmlWeb();
                    // Lets use atlasforge.gg/monster-hunter-wilds to pull images from when necessary
                    var document = web.Load($"https://atlasforge.gg/monster-hunter-wilds/weapons");
                    // var imgNode = document.DocumentNode.QuerySelector("img");
                    var imageElement = document.DocumentNode.SelectSingleNode($"//img[@alt='{weaponName}']");
                    var imageUrl = imageElement.GetAttributeValue("src","");

                    // setup the client to download the image
                    client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
                    byte[] imageBytes = await client.GetByteArrayAsync(imageUrl);
                    await File.WriteAllBytesAsync($"images/{fileName}", imageBytes);
                    Console.WriteLine($"Weapon image downloaded and saved as: {fileName}");
                    return fileName;
                }
                catch (Exception ex)
                {
                    // Handle any exceptions (e.g., network issues, invalid URL)
                    Console.WriteLine($"Error downloading weapon image: {ex.Message}");
                    return null;
                }
            }
        }

        public static async Task<T> GetWeaponMessage<T>(Weapon? weapon, WildsDocService wildsDocService) where T: IMessageProperties, new() {
            try {
                
                if (weapon == null || weapon.name == null) {
                    var failedMessage = CreateMessage<T>();
                    failedMessage.Content = "Sorry, request failed! Try again later!";
                    return failedMessage;
                }

                // download the weapon image for the embed message
                var fileName = await downloadWeaponImage(weapon.name);
                var sharpnessFileName = GetWeaponSharpnessImage(weapon, fileName);

                var message = CreateMessage<T>();

                var embed = new EmbedProperties()
                {
                    Title = weapon?.name ?? "No Name Found",
                    Thumbnail = (fileName != null) ? $"attachment://{fileName}" : "",
                    Description = $"Rarity {weapon?.rarity} {weapon?.kind?.Replace("-", " ")}",
                    Url =$"https://atlasforge.gg/monster-hunter-wilds/weapons/{weapon?.name.Replace(" ", "-")}", // must be unique for multiple embeds
                    Timestamp = DateTimeOffset.UtcNow,
                    Color = new(0x52521F),
                    Footer = new()
                    {
                        Text = "Happy Hunting!!",
                    },
                    Image = (sharpnessFileName != null) ? $"attachment://{sharpnessFileName}" : "",
                    Fields =
                    [
                        new()
                        {
                            Name = "Damage",
                            Value = weapon?.damage?.display.ToString(),
                            Inline = true,
                        },
                        new()
                        {
                            Name = "Affinity",
                            Value = $"{weapon?.affinity.ToString()}%",
                            Inline = true,
                        },
                    ],
                };

                // calculate the weapon element or status type from the specials list
                var weaponElement = weapon?.specials?.FirstOrDefault(x => x?.kind == "element", null);
                var weaponStatus = weapon?.specials?.FirstOrDefault(x => x?.kind == "status", null);

                if (weaponElement != null) {
                    embed.AddFields(new EmbedFieldProperties(){
                        Name = weaponElement?.element?.ToString().FirstCharToUpper(),
                        Value = weaponElement?.damage?.display.ToString(),
                        Inline = true,
                    });
                }

                if (weaponStatus != null) {
                    embed.AddFields(new EmbedFieldProperties(){
                        Name = weaponStatus?.status?.ToString().FirstCharToUpper(),
                        Value = weaponStatus?.damage?.display.ToString(),
                        Inline = true,
                    });
                }

                // continue adding fields
                embed.AddFields(
                    [
                        new EmbedFieldProperties(){
                            Name = "Defense",
                            Value = weapon?.defenseBonus?.ToString(),
                            Inline = true,
                        },
                    ]
                );

                string? slotString = null;
                foreach (var slot in weapon?.slots ?? new List<int>()) {
                    slotString += string.Format("[Lv {0}] ", slot);
                }

                // slots
                embed.AddFields(new EmbedFieldProperties(){
                    Name = "Slots",
                    Value = slotString ?? "-",
                    Inline = true,
                });

                // skills
                foreach (var skills in weapon?.skills ?? new List<WeaponSkills>()) {
                    embed.AddFields(new EmbedFieldProperties(){
                        Name = "Skill",
                        Value = $"{skills.level}x {skills.skill?.name ?? "N/A"}",
                        Inline = false,
                    });  
                }

                // if we have a filename to attach, add it to the message!
                if (fileName != null){
                    var attachment = new AttachmentProperties(fileName, new MemoryStream(File.ReadAllBytes("images/" + fileName)));
                    message.AddAttachments(attachment);
                }

                // if we created a sharpness image, add it to the message!
                if (sharpnessFileName != null){
                    var attachment2 = new AttachmentProperties(sharpnessFileName, new MemoryStream(File.ReadAllBytes("images/" + sharpnessFileName)));
                    message.AddAttachments(attachment2);
                }
                
                message.AddEmbeds(embed);

                return message;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                var message = CreateMessage<T>();
                message.Content = "Sorry, request failed! Try again later!";
                return message;
            }
        }

        public static string? GetWeaponSharpnessImage(Weapon? weapon, string? fileName) {
            if (weapon == null || fileName == null) return null;

            var sharpnessFileName = $"{fileName?.Replace(".png", "")}_sharpness.png";

            // we only need to calculate this once and then we can just reuse the image
            if (File.Exists("images/" + sharpnessFileName)) {
                return sharpnessFileName;
            }

            // looking at the weapon's sharpness int array, we can calculate the percentage of the sharpness color
            // handicraft will tell us when the color will change if that skill is applied 
            float redSharpness = ((float)((weapon?.sharpness?.red) ?? 0f)) / 400f * 100f;
            float orangeSharpness = ((float)((weapon?.sharpness?.orange) ?? 0f)) / 400f * 100f;
            float yellowSharpness = ((float)((weapon?.sharpness?.yellow) ?? 0f)) / 400f * 100f;;
            float greenSharpness = ((float)((weapon?.sharpness?.green) ?? 0f)) / 400f * 100f;
            float blueSharpness = ((float)((weapon?.sharpness?.blue) ?? 0f)) / 400f * 100f;
            float whiteSharpness = ((float)((weapon?.sharpness?.white) ?? 0f)) / 400f * 100f;
            float purpleSharpness = ((float)((weapon?.sharpness?.purple) ?? 0f)) / 400f * 100f;

            string sharpnessHtml = $@"
            <html>
            <head></head>
            <body>
                <div style=""width:214px;height:6px;font-size:0;"">
                    <span style=""background-color:#E63A3A;width:{redSharpness}%;height:100%;display:inline-block;""></span>
                    <span style=""background-color:#E68A45;width:{orangeSharpness}%;height:100%;display:inline-block;""></span>
                    <span style=""background-color:#D9C541;width:{yellowSharpness}%;height:100%;display:inline-block;""></span>
                    <span style=""background-color:#68CC52;width:{greenSharpness}%;height:100%;display:inline-block;""></span>
                    <span style=""background-color:#41A2D9;width:{blueSharpness}%;height:100%;display:inline-block;""></span>
                    <span style=""background-color:#D9D9D9;width:{whiteSharpness}%;height:100%;display:inline-block;""></span>
                    <span style=""background-color:transparent;width:{purpleSharpness}%;height:100%;display:inline-block;""></span>
                </div>
            </body>
            </html>
            ";

            var converter = new HtmlConverter();
            var bytes = converter.FromHtmlString(sharpnessHtml, 214, ImageFormat.Png);
            File.WriteAllBytes("images/" + sharpnessFileName, bytes);

            ImageHelper.MakeTransparent(sharpnessFileName);

            return sharpnessFileName;
        }

        public static T GetMonsterMessage<T>(Monster? monster) where T: IMessageProperties, new() {
            try {
                
                if (monster == null || monster.name == null) {
                    var failedMessage = CreateMessage<T>();
                    failedMessage.Content = "Sorry, request failed! Try again later!";
                    return failedMessage;
                }

                // download the weapon image for the embed message
                var fileName = "rathian.png";
                ImageHelper.MakeTransparent("images/monstericons/" + fileName, Color.Black);

                var message = CreateMessage<T>();

                var embed = new EmbedProperties()
                {
                    Title = monster?.name ?? "No Name Found",
                    Thumbnail = (fileName != null) ? $"attachment://{fileName}" : "",
                    Description = $"{monster?.description ?? "No Description Found"}",
                    Url =$"https://mhwilds.kiranico.com/data/monsters/{monster?.name.Replace(" ", "-")}", // must be unique for multiple embeds
                    Timestamp = DateTimeOffset.UtcNow,
                    Color = new(0x52521F),
                    Footer = new()
                    {
                        Text = "Happy Hunting!!",
                    },
                    // Image = (sharpnessFileName != null) ? $"attachment://{sharpnessFileName}" : "",
                    Fields =
                    [
                        new()
                        {
                            Name = "Kind",
                            Value = monster?.kind.ToString(),
                            Inline = true,
                        },
                        new()
                        {
                            Name = "Species",
                            Value = monster?.species.ToString(),
                            Inline = true,
                        },
                    ],
                };

                string? locationString = null;
                foreach (var location in monster?.locations ?? new List<Location>()) {
                    locationString += location.name + ", ";
                }

                // slots
                embed.AddFields(new EmbedFieldProperties(){
                    Name = "Location(s)",
                    Value = (locationString ?? "-").Trim().TrimEnd(','),
                    Inline = true,
                });

                // if we have a filename to attach, add it to the message!
                if (fileName != null){
                    var attachment = new AttachmentProperties(fileName, new MemoryStream(File.ReadAllBytes("images/monstericons/" + fileName)));
                    message.AddAttachments(attachment);
                }
                
                message.AddEmbeds(embed);

                return message;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                var message = CreateMessage<T>();
                message.Content = "Sorry, request failed! Try again later!";
                return message;
            }
        }
    }

    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
            };
    }

    public static class ImageHelper
    {
        public static void MakeTransparent(string fileName)
        {
            using var image = Image.Load<Rgba32>(new MemoryStream(File.ReadAllBytes("images/" + fileName)));

            float threshold = 0.005F;
            Color sourceColor = Color.White;
            Color targetColor = Color.Transparent;
            RecolorBrush brush = new RecolorBrush(sourceColor, targetColor, threshold);

            image.Mutate(context => context.Clear(brush));


            image.SaveAsPng("images/" + fileName);
        }

        public static void MakeTransparent(string fileFullPath, Color sourceColor)
        {
            using var image = Image.Load<Rgba32>(new MemoryStream(File.ReadAllBytes(fileFullPath)));

            float threshold = 0.005F;
            Color targetColor = Color.Transparent;
            RecolorBrush brush = new RecolorBrush(sourceColor, targetColor, threshold);

            image.Mutate(context => context.Clear(brush));


            image.SaveAsPng(fileFullPath);
        }
    }
}