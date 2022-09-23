﻿// Authors:
//   Jose Medrano <josmed@microsoft.com>
//
// Copyright (C) 2018 Microsoft, Corp
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the
// following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN
// NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE
// USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Threading.Tasks;

using FigmaSharp;
using FigmaSharp.Controls.Cocoa.Services;

using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui.Pads.ProjectPad;

namespace MonoDevelop.Figma.Commands
{
    class RegenerateFigmaDocumentCommandHandler : FigmaCommandHandler
    {
        protected override void OnUpdate(CommandInfo info)
        {
            if (IdeApp.ProjectOperations.CurrentSelectedItem is ProjectFolder currentFolder)
            {
                if (currentFolder.IsDocumentDirectoryBundle())
                {
                    info.Text = "Regenerate from Figma Document";
                    info.Visible = info.Enabled = true;
                    return;
                }
            }
            info.Visible = info.Enabled = false;
        }

        protected async override void OnRun()
        {
            if (IdeApp.ProjectOperations.CurrentSelectedItem is ProjectFolder currentFolder && currentFolder.IsDocumentDirectoryBundle())
            {
                var bundle = FigmaBundle.FromDirectoryPath(currentFolder.Path.FullPath);
                if (bundle == null)
                {
                    return;
                }
                var includeImages = true;

                using var monitor = IdeApp.Workbench.ProgressMonitors.GetFigmaProgressMonitor(
                    $"Regenerating ‘{bundle.Manifest.DocumentTitle}’…",
                    $"‘{bundle.Manifest.DocumentTitle}’ regenerated successfully");

                //we need to ask to figma server to get nodes as demmand
                var fileProvider = new ControlFileNodeProvider(bundle.ResourcesDirectoryPath);
                fileProvider.Load(bundle.DocumentFilePath);
                await bundle.ReloadAsync();

                var codeRendererService = new NativeViewCodeService(fileProvider)
                {
                    TranslationService = new Services.MonoDevelopTranslationService()
                };
                await bundle.SaveAllAsync(includeImages, fileProvider);

                await currentFolder.Project.IncludeBundleAsync(monitor, bundle, includeImages)
                    .ConfigureAwait(true);
            }
        }
    }
}
