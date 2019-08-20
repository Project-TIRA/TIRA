﻿using System;
using System.Threading;
using System.Threading.Tasks;
using EntityModel;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using ServiceProviderBot.Bot;
using ServiceProviderBot.Bot.Dialogs;
using ServiceProviderBot.Bot.Middleware;
using ServiceProviderBot.Bot.Prompts;
using ServiceProviderBot.Bot.Utils;
using Shared;
using Shared.ApiInterface;
using Xunit;

namespace Tests.Dialogs
{
    public abstract class DialogTestBase
    {
        protected readonly StateAccessors state;
        protected readonly DialogSet dialogs;
        protected readonly IApiInterface api;
        protected readonly TestAdapter adapter;
        private readonly IConfiguration configuration;

        protected ITurnContext turnContext;
        protected CancellationToken cancellationToken;

        // Turn context is only available on a turn, so this is only valid once the bot has executed a turn.
        protected string userToken;

        protected DialogTestBase()
        {
            this.state = StateAccessors.Create();
            this.dialogs = new DialogSet(state.DialogContextAccessor);
            this.api = new EfInterface(DbModelFactory.CreateInMemory());

            this.adapter = new TestAdapter()
                .Use(new TrimIncomingMessageMiddleware())
                .Use(new AutoSaveStateMiddleware(state.ConversationState));

            this.configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: true).Build();

            // Register prompts.
            Prompt.Register(this.dialogs);
        }

        protected TestFlow CreateTestFlow(string dialogName, User user = null)
        {
            return new TestFlow(this.adapter, async (turnContext, cancellationToken) =>
            {
                this.turnContext = turnContext;
                this.cancellationToken = cancellationToken;

                // Tests must init the user once there is a turn context.
                this.userToken = Helpers.GetUserToken(turnContext);
                await InitUserPhoneNumber(user);

                if (turnContext.Activity.Type == ActivityTypes.Message)
                {
                    // Initialize the dialog context.
                    DialogContext dialogContext = await this.dialogs.CreateContextAsync(turnContext, cancellationToken);

                    // Check if the conversation is expired.
                    var forceExpire = Phrases.Reset.ShouldReset(this.configuration, turnContext);
                    var expired = await this.api.ClearIncompleteSnapshots(this.userToken, forceExpire);

                    if (expired)
                    {
                        await dialogContext.CancelAllDialogsAsync(cancellationToken);
                        await Messages.SendAsync(forceExpire ? Phrases.Reset.Forced(user) : Phrases.Reset.Expired(user), turnContext, cancellationToken);
                        return;
                    }

                    // Create the master dialog.
                    var masterDialog = new MasterDialog(this.state, this.dialogs, this.api, this.configuration, this.userToken);

                    // Attempt to continue any existing conversation.
                    DialogTurnResult results = await masterDialog.ContinueDialogAsync(dialogContext, cancellationToken);

                    // Start a new conversation if there isn't one already.
                    if (results.Status == DialogTurnStatus.Empty)
                    {
                        // Difference for tests here is beginning the given dialog instead of master so that individual dialog flows can be tested.
                        await masterDialog.BeginDialogAsync(dialogContext, dialogName, null, cancellationToken);
                    }
                }
            });
        }

        protected Action<IActivity> StartsWith(IMessageActivity expected)
        {
            return receivedActivity =>
            {
                // Validate the received activity.
                var received = receivedActivity.AsMessageActivity();
                Assert.NotNull(received);
                Assert.StartsWith(expected.Text, received.Text);
            };
        }

        private async Task InitUserPhoneNumber(User user)
        {
            if (user != null)
            {
                user.PhoneNumber = this.userToken;
                await this.api.Update(user);
            }
        }
    }
}
