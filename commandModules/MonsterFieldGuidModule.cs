using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using Helpers;
using WildsApi;
using Microsoft.Extensions.Logging;

namespace CommandModules {
    [SlashCommand("fieldguide", "Check the field guide with the help of your Handler for info on monsters!")]
    public class MonsterFieldGuideModule : ApplicationCommandModule<ApplicationCommandContext>
    {
        private WildsDocService _wildsService;
        private ILogger<MonsterFieldGuideModule> _logger;
        public MonsterFieldGuideModule(WildsDocService wildsService, ILogger<MonsterFieldGuideModule> logger){
            _wildsService = wildsService;
            _logger = logger;
        }

        [SubSlashCommand("getinfo", "Look up some basic info on monsters!")]    
        public async Task GetInfo(string? monsterName = null) {
            // tells discord to wait, discord gives the user a "bot is thinking" message and I have roughly 15 minutes to respond
            await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());

            // generate the message
            var message = MessageHelper.CreateMessage<InteractionMessageProperties>();

            if (monsterName == null) {
                message.Content = "Wait which monster are you talking about again? ~ Handler";
                await Context.Interaction.SendFollowupMessageAsync(message);
                return;
            }

            try {
                var monsterData = await _wildsService.GetMonsterAsync(monsterName);
                if (monsterData == null) {
                    message.Content = "Sorry, I couldn't quite understand, which monster was it again? ~ Handler";
                } else {
                    message = MessageHelper.GetMonsterMessage<InteractionMessageProperties>(monsterData);
                }
            } catch {
                message.Content = "Unable to aquire monster data, try again later!";
            }
            
            try {
                await Context.Interaction.SendFollowupMessageAsync(message);
            } catch (Exception ex) {
                _logger.LogError(ex, "Monster info not sent because of error...");
                message = MessageHelper.CreateMessage<InteractionMessageProperties>();
                message.Content = "Somethings not right, try again later! ~ Handler";
                await Context.Interaction.SendFollowupMessageAsync(message);
            }
        }
    }
}