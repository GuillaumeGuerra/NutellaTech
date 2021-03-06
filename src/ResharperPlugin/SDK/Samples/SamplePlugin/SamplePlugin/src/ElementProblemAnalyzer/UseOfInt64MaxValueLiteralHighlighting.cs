﻿/*
 * Copyright 2007-2011 JetBrains s.r.o.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.SamplePlugin.ElementProblemAnalyzer;

[assembly: RegisterConfigurableSeverity(UseOfInt64MaxValueLiteralHighlighting.SeverityId,
  null,
  HighlightingGroupIds.CodeSmell,
  "Use of literal value instead of Int64.MaxValue contant",
  "Some fancy description",
  Severity.WARNING,
  false)]

namespace JetBrains.ReSharper.SamplePlugin.ElementProblemAnalyzer
{
  [ConfigurableSeverityHighlighting(SeverityId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.WARNING)]
  public class UseOfInt64MaxValueLiteralHighlighting : IHighlighting
  {
    public const string SeverityId = "UseOfInt64MaxValueLiteral";

    private readonly IExpression _expression;

    public UseOfInt64MaxValueLiteralHighlighting(IExpression expression)
    {
      _expression = expression;
    }

    #region IHighlighting Members

    public string ToolTip
    {
      get { return "Usage of Int64.MaxValue literal"; }
    }

    public string ErrorStripeToolTip
    {
      get { return ToolTip; }
    }

    public int NavigationOffsetPatch
    {
      get { return 0; }
    }

    public bool IsValid()
    {
      return _expression == null || _expression.IsValid();
    }

    #endregion
  }
}