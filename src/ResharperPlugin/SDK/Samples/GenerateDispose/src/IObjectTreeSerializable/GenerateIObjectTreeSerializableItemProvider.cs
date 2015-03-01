/*
 * Copyright 2007-2014 JetBrains
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

using System.Collections.Generic;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Generate;
using JetBrains.ReSharper.Feature.Services.Generate.Actions;
using JetBrains.ReSharper.Psi;
using JetBrains.UI.Icons;

namespace JetBrains.ReSharper.PowerToys.GenerateDispose
{
  [ActionHandler("Generate.IObjectTreeSerializable")]
    public class GenerateIObjectTreeSerializableAction : GenerateActionBase<GenerateIObjectTreeSerializableItemProvider>
  {
    protected override bool ShowMenuWithOneItem
    {
      get { return true; }
    }

    protected override UI.RichText.RichText Caption
    {
        get { return "Generate IObjectTreeSerializable"; }
    }
  } 

  [GenerateProvider]
  public class GenerateIObjectTreeSerializableItemProvider : IGenerateActionProvider
  {
    public IEnumerable<IGenerateActionWorkflow> CreateWorkflow(IDataContext dataContext)
    {
      var solution = dataContext.GetData(ProjectModel.DataContext.DataConstants.SOLUTION);
      var psiIconManager = solution.GetComponent<PsiIconManager>();
      var icon = psiIconManager.GetImage(CLRDeclaredElementType.METHOD);
      yield return new GenerateIObjectTreeSerializableActionWorkflow(icon);
    }
  }

  public class GenerateIObjectTreeSerializableActionWorkflow : StandardGenerateActionWorkflow
  {
      public GenerateIObjectTreeSerializableActionWorkflow(IconId icon)
          : base("IObjectTreeSerializable", icon, "IObjectTreeSerializable", GenerateActionGroup.CLR_LANGUAGE, "Generate IObjectTreeSerializable",
        "Generate an IObjectTreeSerializable implementation", "Generate.IObjectTreeSerializable")
    {
    }

    public override double Order
    {
      get { return 100; }
    }

    /// <summary>
    /// This method is redefined in order to get rid of the IsKindAllowed() check at the end.
    /// </summary>
    public override bool IsAvailable(IDataContext dataContext)
    {
      var solution = dataContext.GetData(ProjectModel.DataContext.DataConstants.SOLUTION);
      if (solution == null)
        return false;

      var generatorManager = GeneratorManager.GetInstance(solution);
      if (generatorManager == null)
        return false;

      var languageType = generatorManager.GetPsiLanguageFromContext(dataContext);
      if (languageType == null)
        return false;

      var generatorContextFactory = LanguageManager.Instance.TryGetService<IGeneratorContextFactory>(languageType);
      return generatorContextFactory != null;
    }
  }
}