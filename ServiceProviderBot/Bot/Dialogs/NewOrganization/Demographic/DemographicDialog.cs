﻿using EntityModel;
using Microsoft.Bot.Builder.Dialogs;
using ServiceProviderBot.Bot.Utils;

namespace ServiceProviderBot.Bot.Dialogs.NewOrganization.Demographic
{
    public class DemographicDialog : DialogBase
    {
        public static string Name = typeof(DemographicDialog).FullName;

        public override WaterfallDialog Init(StateAccessors state, DialogSet dialogs, DbInterface database)
        {
            return new WaterfallDialog(Name, new WaterfallStep[]
            {
                async (stepContext, cancellationToken) =>
                {
                    // Prompt for working with men.
                    return await stepContext.PromptAsync(
                        Utils.Prompts.ConfirmPrompt,
                        new PromptOptions { Prompt = Utils.Phrases.Demographic.GetHasDemographicMen },
                        cancellationToken);
                },
                async (stepContext, cancellationToken) =>
                {
                    // Update the organization with the male demographic.
                    var organization = await database.GetOrganization(stepContext.Context);
                    organization.UpdateGender(Gender.Male, (bool)stepContext.Result);
                    await database.Save();

                    // Prompt for working with women.
                    return await stepContext.PromptAsync(
                        Utils.Prompts.ConfirmPrompt,
                        new PromptOptions { Prompt = Utils.Phrases.Demographic.GetHasDemographicWomen },
                        cancellationToken);
                },
                async (stepContext, cancellationToken) =>
                {
                    // Update the organization with the female demographic.
                    var organization = await database.GetOrganization(stepContext.Context);
                    organization.UpdateGender(Gender.Female, (bool)stepContext.Result);
                    await database.Save();

                    // Prompt for the age range.
                    return await stepContext.PromptAsync(Utils.Prompts.ConfirmPrompt, new PromptOptions
                    {
                        Prompt = Utils.Phrases.Demographic.GetHasDemographicAgeRange
                    },
                    cancellationToken);
                },
                async (stepContext, cancellationToken) =>
                {
                    if ((bool)stepContext.Result)
                    {
                        // Push the age range dialog onto the stack.
                        return await Utils.Dialogs.BeginDialogAsync(state, dialogs, database, stepContext, AgeRangeDialog.Name, null, cancellationToken);
                    }

                    // Update the organization with the default age range.
                    var organization = await database.GetOrganization(stepContext.Context);
                    organization.SetDefaultAgeRange();
                    await database.Save();

                    // Skip this step.
                    return await stepContext.NextAsync(null, cancellationToken);
                },
                async (stepContext, cancellationToken) =>
                {
                    // End this dialog to pop it off the stack.
                    return await stepContext.EndDialogAsync(cancellationToken);
                }
            });
        }
    }
}
