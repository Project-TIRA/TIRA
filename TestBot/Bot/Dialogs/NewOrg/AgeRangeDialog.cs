﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using TestBot.Bot.Models;
using TestBot.Bot.Utils;

namespace TestBot.Bot.Dialogs.NewOrg
{
    public static class AgeRangeDialog
    {
        public static string Name = "AgeRangeDialog";

        /// <summary>Creates a dialog for adding a new organization.</summary>
        /// <param name="state">The state accessors.</param>
        public static Dialog Create(StateAccessors state)
        {
            // Define the dialog and add it to the set.
            return new WaterfallDialog(Name, new WaterfallStep[]
            {
                async (stepContext, cancellationToken) =>
                {
                    return await stepContext.PromptAsync(Utils.Prompts.ConfirmPrompt, new PromptOptions
                    {
                        Prompt = MessageFactory.Text("Does your organization work with an age range?")
                    },
                    cancellationToken);
                },
                async (stepContext, cancellationToken) =>
                {
                    if (!(bool)stepContext.Result)
                    {
                        // Works with all ages.
                        var profile = await state.GetOrganizationProfile(stepContext.Context, cancellationToken);
                        profile.Demographic.AgeRange.SetToAll();

                        // End this dialog to pop it off the stack.
                        return await stepContext.EndDialogAsync(cancellationToken);
                    }

                    return await stepContext.PromptAsync(Utils.Prompts.IntPrompt, new PromptOptions
                    {
                        Prompt = MessageFactory.Text("What is the youngest age your organization works with?")
                    },
                    cancellationToken);
                },
                async (stepContext, cancellationToken) =>
                {
                    // Update the profile with the result of the previous step.
                    var profile = await state.GetOrganizationProfile(stepContext.Context, cancellationToken);
                    profile.Demographic.AgeRange.Start = (int)stepContext.Result;

                    return await stepContext.PromptAsync(Utils.Prompts.IntPrompt, new PromptOptions
                    {
                        Prompt = MessageFactory.Text("What is the oldest age your organization works with?")
                    },
                    cancellationToken);
                },
                async (stepContext, cancellationToken) =>
                {
                    var profile = await state.GetOrganizationProfile(stepContext.Context, cancellationToken);

                    // Validate the numbers.
                    var end = (int)stepContext.Result;
                    if (end < profile.Demographic.AgeRange.Start)
                    {
                        profile.Demographic.AgeRange.SetToAll();

                        // Repeat the dialog.
                        await Utils.Messages.SendAsync("Oops, the oldest age must be greater than youngest age.", stepContext.Context, cancellationToken);
                        return await stepContext.ReplaceDialogAsync(Name, null, cancellationToken);
                    }

                    // Update the profile with the result of the previous step.
                    profile.Demographic.AgeRange.End = (int)stepContext.Result;

                    // End this dialog to pop it off the stack.
                    return await stepContext.EndDialogAsync(cancellationToken);
                }
            });
        }
    }
}
