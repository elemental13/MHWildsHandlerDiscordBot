using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using Helpers;
using WildsApi;

namespace CommandModules {
    public class MonsterFieldGuideModule : ApplicationCommandModule<ApplicationCommandContext>
    {
        private WildsDocService _wildsService;
        public MonsterFieldGuideModule(WildsDocService wildsService){
            _wildsService = wildsService;
        }

        [SlashCommand("fieldguide", "Check the monster hunter feild guide for information!!!")]    
        public async Task FieldGuide() {
            // tells discord to wait, discord gives the user a "bot is thinking" message and I have roughly 15 minutes to respond
            await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());

            // generate the message
            var message = MessageHelper.CreateMessage<InteractionMessageProperties>();

            try {
                var testMonster = await _wildsService.GetMonsterAsync();
                message.Content = testMonster?.name ?? "Unable to aquire data, try again later!";
            } catch {
                message.Content = "Unable to aquire data, try again later!";
            }
            

            await Context.Interaction.SendFollowupMessageAsync(message);
        }
    }
}