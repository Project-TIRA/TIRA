﻿using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using TestBot.Bot.Dialogs.NewOrganization;
using TestBot.Bot.Prompts;

namespace TestBot.Bot.Dialogs
{
    public static class MasterDialog
    {
        public static string Name = "MasterDialog";

        /// <summary>Creates a dialog for managing the conversation.</summary>
        /// <param name="state">The state accessors.</param>
        public static Dialog Create(StateAccessors state)
        {
            // Define the dialog and add it to the set.
            return new WaterfallDialog(Name, new WaterfallStep[]
            {
                async (stepContext, cancellationToken) =>
                {
                    // Prompt for the action.
                    return await stepContext.PromptAsync(
                        Utils.Prompts.ChoicePrompt,
                        new WelcomeChoicePrompt(),
                        cancellationToken);
                },
                async (stepContext, cancellationToken) =>
                {
                    var choice = (FoundChoice)stepContext.Result;

                    // Branch based on the input.
                    switch (choice.Index)
                    {
                        case WelcomeChoicePrompt.NewOrganizationChoice:
                        {
                            // Push the new organization dialog onto the stack.
                            return await stepContext.BeginDialogAsync(NewOrganizationDialog.Name, null, cancellationToken);
                        }
                        default:
                        {
                            // TODO: Update org option.
                            return await stepContext.NextAsync(null, cancellationToken);
                        }
                    }
                },
                async (stepContext, cancellationToken) =>
                {
                    // Send the closing message.
                    await Utils.Messages.SendAsync(Utils.Phrases.Greeting.GetClosing, stepContext.Context, cancellationToken);

                    // End this dialog to pop it off the stack.
                    return await stepContext.EndDialogAsync(cancellationToken);
                }
            });
        }
    }
}
