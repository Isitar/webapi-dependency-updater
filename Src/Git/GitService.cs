namespace Isitar.DependencyUpdater.Git
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common.Services;
    using Domain.Entities;

    public class GitService : IGitService
    {
        private readonly GitSettings gitSettings;
        private readonly IProcessExecutor processExecutor;
        public string WorkingDirectory { get; set; }

        public GitService(GitSettings gitSettings, IProcessExecutor processExecutor)
        {
            this.gitSettings = gitSettings;
            this.processExecutor = processExecutor;
        }

        private async Task<ProcessOutput> ExecuteGitCommandAsync(string cmd, CancellationToken cancellationToken)
        {
            return await processExecutor.Run(WorkingDirectory, gitSettings.PathToGitExecutable, cmd, true, cancellationToken);
        }

        public async Task CloneRepositoryAsync(string url, Platform platform, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(platform.PrivateKey))
            {
                var keyFile = Path.GetTempFileName();
                await File.WriteAllTextAsync(keyFile, platform.PrivateKey, Encoding.ASCII, cancellationToken);
                await processExecutor.Run(WorkingDirectory, "chmod", $"400 {keyFile}", true, cancellationToken);
                await ExecuteGitCommandAsync($" -c core.sshCommand=\"ssh -i {keyFile}\" clone {url} .", cancellationToken);
            }
            else
            {
                await ExecuteGitCommandAsync($"clone {url} .", cancellationToken);
            }
        }

        public async Task<bool> BranchExistsAsync(string name, CancellationToken cancellationToken)
        {
            var result = await ExecuteGitCommandAsync($"ls-remote --heads origin {name}", cancellationToken);
            if (!result.Success)
            {
                return false;
            }

            return result.Output.Length > 0;
        }

        public async Task CheckoutBranchAsync(string name, bool create, CancellationToken cancellationToken)
        {
            await ExecuteGitCommandAsync($"checkout {(create ? "-b " : "")} {name}", cancellationToken);
        }

        public async Task AddFilesAsync(string[] files, CancellationToken cancellationToken)
        {
            if (0 == files.Length)
            {
                await ExecuteGitCommandAsync("add -A", cancellationToken);
            }
            else
            {
                await ExecuteGitCommandAsync($"add {string.Join(" ", files)}", cancellationToken);
            }
        }

        public async Task AddFilesAsync(CancellationToken cancellationToken)
        {
            await AddFilesAsync(Array.Empty<string>(), cancellationToken);
        }

        public async Task CommitAddedFilesAsync(string message, CancellationToken cancellationToken)
        {
            await ExecuteGitCommandAsync($"commit -m \"{message}\"", cancellationToken);
        }


        public async Task PushAsync(string remote, string branch, CancellationToken cancellationToken)
        {
            await ExecuteGitCommandAsync($"push {remote} {branch}", cancellationToken);
        }

        public async Task MergeAsync(string branch, CancellationToken cancellationToken)
        {
            await ExecuteGitCommandAsync($"merge {branch}", cancellationToken);
        }

        public async Task DeleteBranchAsync(string branch, bool remote, CancellationToken cancellationToken)
        {
            await ExecuteGitCommandAsync($"branch -d {branch}", cancellationToken);
            if (remote)
            {
                await ExecuteGitCommandAsync($"push origin --delete {branch}", cancellationToken);
            }
        }

        public async Task ResetHardAsync(CancellationToken cancellationToken)
        {
            await ExecuteGitCommandAsync("reset --hard", cancellationToken);
        }
    }
}