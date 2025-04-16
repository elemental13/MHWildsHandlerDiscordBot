using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using Helpers;
using WildsApi;
using Microsoft.Extensions.Logging;

namespace CommandModules {
    [SlashCommand("askgemma", "What do you want to ask Gemma?")]
    public class BlacksmithModule : ApplicationCommandModule<ApplicationCommandContext>
    {
        private WildsDocService _wildsService;
        private ILogger<BlacksmithModule> _logger;
        public BlacksmithModule(WildsDocService wildsService, ILogger<BlacksmithModule> logger){
            _wildsService = wildsService;
            _logger = logger;
        }

        [SubSlashCommand("armorset", "Check the armor information!!!")]    
        public async Task ArmorSetInfo(string? armorSetName = null) {
            // tells discord to wait, discord gives the user a "bot is thinking" message and I have roughly 15 minutes to respond
            await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());

            // generate the message
            var message = MessageHelper.CreateMessage<InteractionMessageProperties>();

            if (armorSetName == null) {
                message.Content = "Which armor set are you looking for more information about? ~ Gemma";
                await Context.Interaction.SendFollowupMessageAsync(message);
                return;
            }

            // not null, lets try to look it up
            try {
                var armorData = await _wildsService.GetArmorSetAsync(armorSetName);
                if (armorData == null) {
                    message.Content = "Sorry, I couldn't quite understand, which armor set was it again?";
                } else {
                    message.Content = $"This armor set has {armorData[0]?.pieces?.Count()} peices in it.";
                }
            } catch {
                message.Content = "Somethings not right, try again later! ~ Gemma";
            }
            
            await Context.Interaction.SendFollowupMessageAsync(message);
        }

        [SubSlashCommand("weapons", "Check the weapon information!!!")]    
        public async Task getWeaponInfo(string? weaponName = null) {
            // tells discord to wait, discord gives the user a "bot is thinking" message and I have roughly 15 minutes to respond
            await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());

            // generate the message
            var message = MessageHelper.CreateMessage<InteractionMessageProperties>();

            if (weaponName == null) {
                message.Content = "Which weapon are you looking for more information about? ~ Gemma";
                await Context.Interaction.SendFollowupMessageAsync(message);
                return;
            }

            // not null, lets try to look it up
            try {
                var weaponData = await _wildsService.GetWeaponAsync(weaponName);
                if (weaponData == null) {
                    message.Content = "Sorry, I couldn't quite understand, which weapon was it again? ~ Gemma";
                } else {
                    message = await MessageHelper.GetWeaponMessage<InteractionMessageProperties>(weaponData[0]);
                }
            } catch {
                message.Content = "Somethings not right, try again later! ~ Gemma";
            }            

            try {
                await Context.Interaction.SendFollowupMessageAsync(message);
            } catch (Exception ex) {
                _logger.LogError(ex, "Weapon info not sent because of error...");
                message = MessageHelper.CreateMessage<InteractionMessageProperties>();
                message.Content = "Somethings not right, try again later! ~ Gemma";
                await Context.Interaction.SendFollowupMessageAsync(message);
            }
            
        }
    }
}