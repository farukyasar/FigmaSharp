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

using FigmaSharp.Converters;
using FigmaSharp.Models;
using FigmaSharp.Services;
using FigmaSharp.Views;
using FigmaSharp.Views.Cocoa;

namespace LocalFile.Cocoa
{
    class LoadingSpinnerConverter : NodeConverter
	{
		public const string LoadingSpinnerName = "progess indicator ~dark";

		public override bool ScanChildren(FigmaNode currentNode)
		{
			return false;
		}

		public override bool CanConvert(FigmaNode currentNode)
		{
			return currentNode.name == LoadingSpinnerName;
		}

		public override IView ConvertToView (FigmaNode currentNode, ViewNode parent, ViewRenderService rendererService)
		{
			var spinner = new ProgressBar();
			//var nativeView = (FNSProgressIndicator)spinner.NativeObject;
			//nativeView.WidthAnchor.ConstraintEqualToConstant(36).Active = true;
			//nativeView.HeightAnchor.ConstraintEqualToConstant(36).Active = true;
			//loadingDialog.Size = new Size(36, 36);
			return spinner;
		}

		public override string ConvertToCode(CodeNode currentNode, CodeNode parentNode, ICodeRenderService rendererService)
		{
			return string.Empty;
		}

		public override System.Type GetControlType(FigmaNode currentNode) => typeof(AppKit.NSView);
	}
}

