using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using Helpers;

namespace CommandModules {
    public class EventModule : ApplicationCommandModule<ApplicationCommandContext>
    {
        [SlashCommand("checkevents", "Check the current event quests going on this week!")]    
        public async Task GetEventsEmbedAsync() {
            await checkAndSendEventByWeek(0);
        }

        [SlashCommand("checknextweekevent", "Check the next weeks event quests coming soon!")]    
        public async Task CheckNextWeekEventAsync() {
            await checkAndSendEventByWeek(1);
        }

        [SlashCommand("checkweekafternextevent", "Check the week AFTER next (2 weeks from now) event quests coming soon!")]    
        public async Task CheckWeekAfterNextEventAsync() {
            await checkAndSendEventByWeek(2);
        }

        public async Task checkAndSendEventByWeek(int week) {
            try {
                // tells discord to wait, discord gives the user a "bot is thinking" message and I have roughly 15 minutes to respond
                await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());

                // generate the message list
                var messageList = await MessageHelper.GetEventMessageList<InteractionMessageProperties>(week);
                // generate the challenge message list
                var challengeList = await MessageHelper.GetChallengeMessageList<InteractionMessageProperties>(week);

                // could be multiple messages with groups of 10 embeds for discord limitations
                foreach(var message in messageList ?? new List<InteractionMessageProperties>()) {
                    await Context.Interaction.SendFollowupMessageAsync(message);
                }

                // could be multiple messages with groups of 10 embeds for discord limitations
                foreach(var challenge in challengeList ?? new List<InteractionMessageProperties>()) {
                    await Context.Interaction.SendFollowupMessageAsync(challenge);
                }

            } catch (Exception ex) {
                var errorMessage = MessageHelper.CreateMessage<InteractionMessageProperties>();
                errorMessage.Content = "Sorry, something went wrong!!";
                Console.WriteLine(ex.Message);
                await Context.Interaction.SendFollowupMessageAsync(errorMessage);
            }
        }
    }
}