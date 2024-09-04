// MIT License
//
// Copyright (c) 2024 SirRandoo
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using JetBrains.Annotations;
using SirRandoo.UX;
using Verse;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace StreamKit.Mod.Core;

/// <summary>
///     Contains the translation string for StreamKit.
/// </summary>
[PublicAPI]
[StaticConstructorOnStartup]
[TranslationNamespace("StreamKit")]
[UsedImplicitly(ImplicitUseKindFlags.Assign, ImplicitUseTargetFlags.WithMembers)]
public static class KitTranslations
{
    [Translation("Common.Text.Type")] public static readonly string CommonTextType;
    [Translation("Common.Text.Karma")] public static readonly string CommonTextKarma;
    [Translation("Common.Text.Amount")] public static readonly string CommonTextAmount;
    [Translation("Common.Text.Balance")] public static readonly string CommonTextBalance;
    [Translation("Common.Text.Username")] public static readonly string CommonTextUsername;
    [Translation("Windows.Ledger.Text.LastSeen")] public static readonly string LedgerLastSeen;
    [Translation("Windows.Ledger.Headers.CurrentLedger")] public static readonly string CurrentLedger;
    [Translation("Windows.Ledger.Text.LastSeenDays")] public static readonly string LedgerLastSeenDays;
    [Translation("Windows.Ledger.Text.LastSeenHours")] public static readonly string LedgerLastSeenHours;
    [Translation("Windows.Ledger.Tooltips.DeleteViewer")] public static readonly string DeleteViewerTooltip;
    [Translation("Windows.Ledger.Text.LastSeenMinutes")] public static readonly string LedgerLastSeenMinutes;
    [Translation("Windows.Ledger.Text.LastSeenSeconds")] public static readonly string LedgerLastSeenSeconds;
    [Translation("Dialogs.Transactions.TransactionId")] public static readonly string TransactionIdDialogText;
    [Translation("Tables.Transactions.Headers.Name")] public static readonly string TransactionTableNameHeader;
    [Translation("Tables.Transactions.Headers.Type")] public static readonly string TransactionTableTypeHeader;
    [Translation("Windows.Ledger.Buttons.ViewTransactions")] public static readonly string ViewTransactionsText;
    [Translation("Windows.Ledger.Tooltips.AddViewerKarma")] public static readonly string AddViewerKarmaTooltip;
    [Translation("Windows.Ledger.Tooltips.AddViewerPoints")] public static readonly string AddViewerPointsTooltip;
    [Translation("Tables.Transactions.Headers.Amount")] public static readonly string TransactionTableAmountHeader;
    [Translation("Tables.Transactions.Tooltips.Refunded")] public static readonly string TransactionRefundedOnTableTooltip;
    [Translation("Dialogs.Transactions.PurchaseHistory")] public static readonly string TransactionPurchaseHistoryDialogText;
    [Translation("Settings.Tabs.Poll")] public static readonly string SettingsTabPoll;
    [Translation("Settings.Tabs.Tooltips.Poll")] public static readonly string SettingsTabPollTooltip;
    [Translation("Settings.Tabs.Debug")] public static readonly string SettingsTabDebug;
    [Translation("Settings.Tabs.Tooltips.Debug")] public static readonly string SettingsTabDebugTooltip;
    [Translation("Settings.Tabs.Pawns")] public static readonly string SettingsTabPawns;
    [Translation("Settings.Tabs.Tooltips.Pawns")] public static readonly string SettingsTabPawnsTooltip;
    [Translation("Settings.Tabs.Store")] public static readonly string SettingsTabStore;
    [Translation("Settings.Tabs.Tooltips.Store")] public static readonly string SettingsTabStoreTooltip;
    [Translation("Settings.Tabs.Client")] public static readonly string SettingsTabClient;
    [Translation("Settings.Tabs.Tooltips.Client")] public static readonly string SettingsTabClientTooltip;
    [Translation("Settings.Tabs.Points")] public static readonly string SettingsTabPoints;
    [Translation("Settings.Tabs.Tooltips.Points")] public static readonly string SettingsTabPointsTooltip;
    [Translation("Settings.Tabs.Commands")] public static readonly string SettingsTabCommands;
    [Translation("Settings.Tabs.Tooltips.Commands")] public static readonly string SettingsTabCommandTooltip;
    [Translation("Settings.Tabs.Morality")] public static readonly string SettingsTabMorality;
    [Translation("Settings.Tabs.Tooltips.Morality")] public static readonly string SettingsTabMoralityTooltip;
    [Translation("Settings.DebugActions.OpenLedgerWindow")] public static readonly string OpenLedgerWindowDebugAction;
    [Translation("Errors.Input.ValueNaN")] public static readonly string NaNInputError;
    [Translation("Errors.Input.ValueToLow")] public static readonly string ValueTooLowInputError;
    [Translation("Errors.Input.ValueTooHigh")] public static readonly string ValueTooHighInputError;
    [Translation("Errors.Input.ValueOutOfRange")] public static readonly string ValueOutOfRangeInputError;
    [Translation("Common.Text.Cancel")] public static readonly string CommonTextCancel;
    [Translation("Common.Text.Confirm")] public static readonly string CommonTextConfirm;
    [Translation("Settings.Pages.Client.GizmoPuff")] public static readonly string GizmoPuff;
    [Translation("Settings.Pages.Command.Prefix")] public static readonly string CommandPrefix;
    [Translation("Settings.Pages.Command.Emoji")] public static readonly string CommandEmojis;
    [Translation("Settings.Pages.Morality.KarmaSystem")] public static readonly string KarmaSystem;
    [Translation("Settings.Pages.Morality.StartingKarma")] public static readonly string StartingKarma;
    [Translation("Settings.Pages.Morality.KarmaRange")] public static readonly string KarmaRange;
    [Translation("Settings.Pages.Pawn.Pool")] public static readonly string PawnPool;
    [Translation("Settings.Pages.Pawn.Descriptions.Pool")] public static readonly string PawnPoolDescription;
    [Translation("Settings.Pages.Pawn.PoolRestrictions")] public static readonly string PawnPoolRestrictions;
    [Translation("Settings.Pages.Pawn.Vacationing")] public static readonly string PawnVacationing;
    [Translation("Settings.Pages.Pawn.OverworkedPawns")] public static readonly string OverworkedPawns;
    [Translation("Settings.Pages.Pawn.EmergencyWorkCrisis")] public static readonly string EmergencyWorkCrisis;
    [Translation("Common.Text.Vip")] public static readonly string CommonTextVip;
    [Translation("Common.Text.Moderator")] public static readonly string CommonTextModerator;
    [Translation("Common.Text.Subscriber")] public static readonly string CommonTextSubscriber;
    [Translation("Settings.Pages.Points.InfinitePoints")] public static readonly string InfinitePoints;
    [Translation("Tables.Transactions.Tooltips.TransactionId")] public static readonly string TransactionIdTableTooltip;
    [Translation("Tables.Transactions.Tooltips.PurchasedOn")] public static readonly string TransactionPurchasedOnTableTooltip;
    [Translation("Dialogs.Transactions.TransactionDescription")] public static readonly string TransactionDescriptionDialogText;
    [Translation("Dialogs.Transactions.TransactionRefunded")] public static readonly string TransactionRefundedDialogText;
    [Translation("Windows.Ledger.Tooltips.RemoveViewerKarma")] public static readonly string RemoveViewerKarmaTooltip;
    [Translation("Windows.Ledger.Tooltips.RemoveViewerPoints")] public static readonly string RemoveViewerPointsTooltip;
    [Translation("Settings.DebugActions.OpenPlatformsWindow")] public static readonly string OpenPlatformsWindowDebugAction;

    [Translation("Settings.DebugActions.OpenRuntimeFlagsWindow")]
    public static readonly string OpenRuntimeFlagsWindowDebugAction;

    [Translation("Settings.DebugActions.OpenTransactionHistoryWindow")]
    public static readonly string OpenTransactionHistoryWindowDebugAction;

    [Translation("Settings.Pages.Client.Descriptions.GizmoPuff")]
    public static readonly string GizmoPuffDescription;

    [Translation("Settings.Pages.Command.Descriptions.Prefix")] public static readonly string CommandPrefixDescription;
    [Translation("Settings.Pages.Command.Descriptions.Emoji")] public static readonly string CommandEmojisDescription;

    [Translation("Settings.Pages.Morality.Descriptions.KarmaSystem")]
    public static readonly string KarmaSystemDescription;

    [Translation("Settings.Pages.Morality.Descriptions.StartingKarma")]
    public static readonly string StartingKarmaDescription;

    [Translation("Settings.Pages.Morality.Tooltips.KarmaRange")] public static readonly string KarmaRangeTooltip;
    [Translation("Settings.Pages.Morality.ReputationSystem")] public static readonly string ReputationSystem;

    [Translation("Settings.Pages.Morality.Descriptions.ReputationSystem")]
    public static readonly string ReputationSystemDescription;

    [Translation("Settings.Pages.Pawn.Descriptions.PoolRestrictions")]
    public static readonly string PawnPoolRestrictionsDescription;

    [Translation("Settings.Pages.Pawn.Descriptions.Vacationing")]
    public static readonly string PawnVacationingDescription;

    [Translation("Settings.Pages.Pawn.Descriptions.OverworkedPawns")]
    public static readonly string OverworkedPawnsDescription;

    [Translation("Settings.Pages.Pawn.Descriptions.EmergencyWorkCrisis")]
    public static readonly string EmergencyWorkCrisisDescription;

    [Translation("Settings.Pages.Points.Descriptions.InfinitePoints")]
    public static readonly string InfinitePointsDescription;

    [Translation("Settings.Pages.Points.Descriptions.DistributePoints")]
    public static readonly string DistributePointsDescription;


    [Translation("Settings.Pages.Points.DistributePoints")] public static readonly string DistributePoints;
    [Translation("Settings.Pages.Points.StartingBalance")] public static readonly string StartingBalance;
    [Translation("Settings.Pages.Points.RewardAmount")] public static readonly string PointRewardAmount;
    [Translation("Settings.Pages.Points.PointDecay")] public static readonly string PointDecay;
    [Translation("Settings.Pages.Points.RewardInterval")] public static readonly string PointRewardInterval;


    [Translation("Settings.Pages.Points.Descriptions.StartingBalance")]
    public static readonly string StartingBalanceDescription;

    [Translation("Settings.Pages.Points.Descriptions.RewardInterval")]
    public static readonly string PointRewardIntervalDescription;

    [Translation("Settings.Pages.Points.Descriptions.RewardAmount")]
    public static readonly string PointRewardAmountDescription;

    [Translation("Settings.Pages.Points.RequireParticipation")] public static readonly string RequireParticipation;

    [Translation("Settings.Pages.Points.Descriptions.RequireParticipation")]
    public static readonly string RequireParticipationDescription;

    [Translation("Settings.Pages.Points.Descriptions.PointDecay")]
    public static readonly string PointDecayDescription;

    [Translation("Settings.Pages.Points.DecayPeriod")] public static readonly string PointDecayPeriod;

    [Translation("Settings.Pages.Points.Descriptions.DecayPeriod")]
    public static readonly string PointDecayPeriodDescription;

    [Translation("Settings.Pages.Points.DecayPercentage")] public static readonly string DecayPercentage;

    [Translation("Settings.Pages.Points.Descriptions.DecayPercentage")]
    public static readonly string DecayPercentageDescription;

    [Translation("Settings.Pages.Points.FixedDecayAmount")] public static readonly string FixedPointDecayAmount;

    [Translation("Settings.Pages.Points.Descriptions.FixedDecayAmount")]
    public static readonly string FixedPointDecayAmountDescription;

    [Translation("Settings.Pages.Points.PointTiers")] public static readonly string PointTiers;

    [Translation("Settings.Pages.Points.Descriptions.PointTiers")]
    public static readonly string PointTiersDescription;

    [Translation("Settings.Pages.Points.Rewards")] public static readonly string PointRewards;
    [Translation("Settings.Pages.Points.Descriptions.Rewards")] public static readonly string PointRewardsDescription;
    [Translation("Settings.Pages.Poll.Duration")] public static readonly string PollDuration;
    [Translation("Settings.Pages.Poll.Descriptions.Duration")] public static readonly string PollDurationDescription;
    [Translation("Settings.Pages.Poll.MaximumOptions")] public static readonly string MaximumPollOptions;

    [Translation("Settings.Pages.Poll.Descriptions.MaximumOptions")]
    public static readonly string MaximumPollOptionsDescription;

    [Translation("Settings.Pages.Poll.NativePolls")] public static readonly string NativePolls;

    [Translation("Settings.Pages.Poll.Descriptions.NativePolls")]
    public static readonly string NativePollsDescription;

    [Translation("Settings.Pages.Poll.ShowPollDialog")] public static readonly string ShowPollDialog;

    [Translation("Settings.Pages.Poll.Descriptions.ShowPollDialog")]
    public static readonly string ShowPollDialogDescription;

    [Translation("Settings.Pages.Poll.UseLargeText")] public static readonly string LargePollText;

    [Translation("Settings.Pages.Poll.Descriptions.UseLargeText")]
    public static readonly string LargePollTextDescription;

    [Translation("Settings.Pages.Poll.AnimatedVotes")] public static readonly string AnimatedPollVotes;

    [Translation("Settings.Pages.Poll.Descriptions.AnimatedVotes")]
    public static readonly string AnimatedPollVotesDescription;

    [Translation("Settings.Pages.Poll.GenerateRandomPolls")] public static readonly string GenerateRandomPolls;

    [Translation("Settings.Pages.Poll.Descriptions.GenerateRandomPolls")]
    public static readonly string GenerateRandomPollsDescriptions;

    [Translation("Settings.Pages.Poll.OptionsInChat")] public static readonly string PollOptionsInChat;

    [Translation("Settings.Pages.Poll.Descriptions.OptionsInChat")]
    public static readonly string PollOptionsInChatDescription;

    [Translation("Settings.Pages.Store.Link")] public static readonly string StoreLink;
    [Translation("Settings.Pages.Store.Descriptions.Link")] public static readonly string StoreLinkDescription;

    [Translation("Settings.Pages.Store.MinimumPurchasePrice")] public static readonly string MinimumPurchasePrice;

    [Translation("Settings.Pages.Store.Descriptions.MinimumPurchasePrice")]
    public static readonly string MinimumPurchasePriceDescription;

    [Translation("Settings.Pages.Store.PurchaseConfirmations")] public static readonly string PurchaseConfirmations;

    [Translation("Settings.Pages.Store.Descriptions.PurchaseConfirmations")]
    public static readonly string PurchaseConfirmationsDescription;

    [Translation("Settings.Pages.Store.PurchaseBuildings")] public static readonly string PurchaseBuildings;

    [Translation("Settings.Pages.Store.Descriptions.PurchaseBuildings")]
    public static readonly string PurchaseBuildingsDescription;

    static KitTranslations()
    {
        TranslationManager.Register(typeof(KitTranslations));
    }
}
