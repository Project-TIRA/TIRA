﻿using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Configuration;
using ServiceProviderBot.Bot.Dialogs.UpdateOrganization.Capacity;
using ServiceProviderBot.Bot.Dialogs.UpdateOrganization.CaseManagement;
using ServiceProviderBot.Bot.Dialogs.UpdateOrganization.JobTraining;
using ServiceProviderBot.Bot.Utils;
using Shared;

namespace ServiceProviderBot.Bot.Dialogs.UpdateOrganization
{
    public class UpdateOrganizationDialog : DialogBase
    {
        public static string Name = typeof(UpdateOrganizationDialog).FullName;

        public UpdateOrganizationDialog(StateAccessors state, DialogSet dialogs, DbInterface database, IConfiguration configuration)
            : base(state, dialogs, database, configuration) { }

        public override WaterfallDialog GetWaterfallDialog()
        {
            // Define the dialog and add it to the set.
            return new WaterfallDialog(Name, new WaterfallStep[]
            {
                async (stepContext, cancellationToken) =>
                {
                    var needsUpdate = await NeedsUpdate(state, database, stepContext.Context);
                    if (!needsUpdate)
                    {
                        // Nothing to update.
                        await Messages.SendAsync(Phrases.UpdateOrganization.NothingToUpdate, stepContext.Context, cancellationToken);

                        // End this dialog to pop it off the stack.
                        return await stepContext.EndDialogAsync(cancellationToken);
                    }

                    // Create a new snapshot to be filled in by UpdateOrganization process.
                    await database.CreateSnapshot(stepContext.Context);

                    var housingNeedsUpdate = await UpdateHousingDialog.CanUpdate(state, database, stepContext.Context);
                    if(!housingNeedsUpdate)
                    {
                        // Skip this step.
                        return await stepContext.NextAsync(null, cancellationToken);
                    }

                    // Push the update capacity dialog onto the stack.
                    return await BeginDialogAsync(stepContext, UpdateCapacityDialog.Name, null, cancellationToken);
                },
                async (stepContext, cancellationToken) =>
                {
                    var caseManagementNeedsUpdate = await UpdateCaseManagementDialog.CanUpdate(state, database, stepContext.Context);
                    if (!caseManagementNeedsUpdate)
                    {
                        // Nothing to update. Skip this step.
                        return await stepContext.NextAsync(null, cancellationToken);
                    }

                    // Push the update case management dialog onto the stack.
                    return await BeginDialogAsync(stepContext, UpdateCaseManagementDialog.Name, null, cancellationToken);
                },
                async (stepContext, cancellationToken) =>
                {
                    var jobTrainingNeedsUpdate = await UpdateJobTrainingDialog.CanUpdate(state, database, stepContext.Context);
                    if(!jobTrainingNeedsUpdate)
                    {
                        return await stepContext.NextAsync(null, cancellationToken);
                    }

                    return await BeginDialogAsync(stepContext, JobTraining.UpdateJobTrainingDialog.Name, null, cancellationToken);

                },
                async (stepContext, cancellationToken) =>
                {
                    // Mark the snapshot as complete.
                    var snapshot = await database.GetSnapshot(stepContext.Context);
                    snapshot.IsComplete = true;
                    await database.Save();

                    // Send the closing message.
                    await Messages.SendAsync(Phrases.UpdateOrganization.Closing, stepContext.Context, cancellationToken);

                    // End this dialog to pop it off the stack.
                    return await stepContext.EndDialogAsync(cancellationToken);
                }
            });
        }

        private static async Task<bool> NeedsUpdate(StateAccessors state, DbInterface database, ITurnContext context)
        {
            // Check if any service needs updates
            return await UpdateHousingDialog.CanUpdate(state, database, context) ||
                await UpdateCaseManagementDialog.CanUpdate(state, database, context) ||
                await UpdateJobTrainingDialog.CanUpdate(state, database, context);
        }
    }
}
