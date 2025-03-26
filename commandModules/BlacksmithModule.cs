using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using Helpers;
using WildsApi;

namespace CommandModules {
    public class BlacksmithModule : ApplicationCommandModule<ApplicationCommandContext>
    {
        private WildsDocService _wildsService;
        public BlacksmithModule(WildsDocService wildsService){
            _wildsService = wildsService;
        }

        [SlashCommand("armorinfo", "Check the armor information!!!")]    
        public async Task ArmorInfo() {
            // tells discord to wait, discord gives the user a "bot is thinking" message and I have roughly 15 minutes to respond
            await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());

            // generate the message
            var message = MessageHelper.CreateMessage<InteractionMessageProperties>();

            try {
                var testArmor = await _wildsService.GetArmorAsync();
                message.Content = testArmor?.name ?? "Unable to aquire data, try again later!";
            } catch {
                message.Content = "Unable to aquire data, try again later!";
            }
            

            await Context.Interaction.SendFollowupMessageAsync(message);
        }
    }
}