
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using EnvDTE;

using EnvDTE80;

using Microsoft.VisualStudio.Shell;

namespace DialControllerTools
{
    internal static class VsHelpers
    {
        internal static async Task<TReturnType> GetServiceAsync<TServiceType, TReturnType>(this IAsyncServiceProvider provider, CancellationToken cancellationToken)
         => (TReturnType)await provider.GetServiceAsync(typeof(TServiceType));

        internal static Task<DTE2> GetDteAsync(this IAsyncServiceProvider provider, CancellationToken cancellationToken)
         => provider.GetServiceAsync<DTE, DTE2>(cancellationToken);

        public static string GetFileInVsix(string relativePath)
        {
            string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(folder, relativePath);
        }

        public static bool IsSolutionExplorer(this Window window)
        {
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
            return IsTool(window) && window.Type == vsWindowType.vsWindowTypeSolutionExplorer;
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        }

        public static bool IsErrorList(this Window window)
        {
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
            return IsTool(window) && window.ObjectKind == WindowKinds.vsWindowKindErrorList;
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        }

        public static bool IsBookmarks(this Window window)
        {
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
            return IsTool(window) && window.ObjectKind == WindowKinds.vsWindowKindBookmarks;
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        }

        public static bool IsDocument(this Window window) => window?.Kind == "Document";
        public static bool IsTool(this Window window) => window?.Kind == "Tool";

        public static bool IsFindResults(this Window window)
        {
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
            return IsTool(window) &&
                (window.Caption?.StartsWith("Find Results", StringComparison.OrdinalIgnoreCase) == true);
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        }

        public static bool ExecuteCommand(this Commands commands, string commandName)
        {
            try
            {
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
                Command command = commands.Item(commandName);

                if (command != null && command.IsAvailable)
                {
                    commands.Raise(command.Guid, command.ID, null, null);
                    return true;
                }
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex);
            }

            return false;
        }
    }
}
