using System;
using System.Collections.ObjectModel;
using System.Linq;

using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;

namespace DialControllerTools
{
    public static class IntellisenseShifter
    {
        public static bool TryShift(ITextView view, ICompletionBroker broker, RotationDirection direction)
        {
            try
            {
                if (!broker.IsCompletionActive(view))
                {
                    ICompletionSession session = broker.TriggerCompletion(view);

                    if (session == null || !session.IsStarted)
                        return false;

                    Completion active = session.CompletionSets?[0]?.SelectionStatus?.Completion;

                    if (active != null)
                    {
                        SelectAdjacentCompletion(session, direction);
                        session.Commit();
                        return true;
                    }
                }
                else
                {
                    ReadOnlyCollection<ICompletionSession> sessions = broker.GetSessions(view);
                    if (sessions.Count > 0)
                    {
                        SelectAdjacentCompletion(sessions[0], direction);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                var outputPane = ThreadHelper.JoinableTaskContext.Factory.Run(DialPackage.GetOutputPaneAsync);
                outputPane.WriteLine("Intellisense shifter failed");
                outputPane.WriteLine(ex.ToString());
            }

            return false;
        }

        private static void SelectAdjacentCompletion(ICompletionSession session, RotationDirection direction)
        {
            CompletionSet completionSet = session.CompletionSets?.FirstOrDefault();
            if (completionSet == null) return;

            var completions = completionSet.Completions;
            if (completions == null || completions.Count == 0) return;

            Completion current = completionSet.SelectionStatus?.Completion;
            int currentIndex = current != null ? completions.IndexOf(current) : -1;

            int newIndex;
            if (direction == RotationDirection.Right)
                newIndex = Math.Min(currentIndex + 1, completions.Count - 1);
            else
                newIndex = Math.Max(currentIndex - 1, 0);

            completionSet.SelectionStatus = new CompletionSelectionStatus(completions[newIndex], true, true);
        }
    }
}
