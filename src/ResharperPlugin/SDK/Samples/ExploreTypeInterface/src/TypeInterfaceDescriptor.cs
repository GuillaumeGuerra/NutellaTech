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

using System.Drawing;
using JetBrains.ActionManagement;
using JetBrains.Application.Interop.NativeHook;
using JetBrains.CommonControls;
using JetBrains.IDE.TreeBrowser;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Tree;
using JetBrains.ReSharper.Feature.Services.Util;
using JetBrains.ReSharper.Psi;
using JetBrains.TreeModels;
using JetBrains.UI.Application;
using JetBrains.UI.Tooltips;
using JetBrains.UI.TreeView;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.ExploreTypeInterface
{
  /// <summary>
  /// Provides information for TreeModelBrowser about what and how to display
  /// </summary>
  internal class TypeInterfaceDescriptor : TreeModelBrowserDescriptorPsi
  {
    // Hide static and protected members for root, if true
    private readonly bool myInstanceOnly;

    // Root type element envoy

    // Presentation provider
    private readonly TreeModelBrowserPresenter myPresenter;

    // Cached title

    // Model
    private TypeInterfaceModel myModel;
    private DeclaredElementEnvoy<ITypeElement> myTypeElementEnvoy;

    public TypeInterfaceDescriptor(ITypeElement typeElement, bool instanceOnly, ISolution solution, IUIApplication uiApplication, ITooltipManager tooltipManager, IWindowsHookManager windowsHookManager, IActionManager actionManager)
      : base(solution, uiApplication, tooltipManager, windowsHookManager, actionManager)
    {
      AutoExpandSingleChild = true;
      myInstanceOnly = instanceOnly;

      // We use standard presenter, but emphasize root element using adorements
      myPresenter = new TypeInterfacePresenter
                      {
                        DrawElementExtensions = true,
                        ShowOccurenceCount = false,
                        PostfixTypeQualification = true
                      };
      myPresenter.PresentAdorements += PresentAdorements;

      // Wrap typeElement with an envoy, so it can survive code changes
      myTypeElementEnvoy = new DeclaredElementEnvoy<ITypeElement>(typeElement);
      MakeModel();
    }


    public ITypeElement TypeElement
    {
      get { return myTypeElementEnvoy.GetValidDeclaredElement(); }
      set
      {
        if (Equals(myTypeElementEnvoy.GetValidDeclaredElement(), value))
          return;
        myTypeElementEnvoy = new DeclaredElementEnvoy<ITypeElement>(value);
        MakeModel();
      }
    }

    public override TreeModel Model
    {
      get { return myModel; }
    }

    public override StructuredPresenter<TreeModelNode, IPresentableItem> Presenter
    {
      get { return myPresenter; }
    }

    private void MakeModel()
    {
      UpdateTitle();

      // Create new model with recursion prevention
      var model = new TypeInterfaceModel(myTypeElementEnvoy, myInstanceOnly)
                    {
                      RecursionPrevention = RecursionPreventionStyle.StopOnOccurence
                    };
      myModel = model;
      // Use our comparer, which sorts by member kind first
      myModel.Comparer = DelegatingComparer<TreeModelNode, object>.Create(source => source.DataValue,
                                                                          new TypeInterfaceModelComparer());

      // Descriptor is finished configuring itself, so request updating visual representation, i.e. tree view
      RequestUpdate(UpdateKind.Structure, true);
    }

    private void UpdateTitle()
    {
      // do not update title if element was lost
      if (TypeElement == null)
        return;

      Title.Value = FormatTypeElement("Type '{0}'");
    }

    private string FormatTypeElement(string format)
    {
      // uses DeclaredElementPresenter to format type element, which is standard way to present code elements
      var style = new DeclaredElementPresenterStyle(NameStyle.SHORT)
                    {
                      ShowTypeParameters = TypeParameterStyle.FULL
                    };
      string typeElementText = DeclaredElementPresenter.Format(PresentationUtil.GetPresentationLanguage(TypeElement),
                                                               style, TypeElement);
      return string.Format(format, typeElementText);
    }

    private void PresentAdorements(object value, IPresentableItem item, TreeModelNode structureElement,
                                   PresentationState state)
    {
      // Emphasize root element
      var element = value as IDeclaredElement;
      if (element == null)
      {
        var envoy = value as DeclaredElementEnvoy<ITypeElement>;
        if (envoy != null)
          element = envoy.GetValidDeclaredElement();
      }
      if (Equals(element, TypeElement))
        item.RichText.SetStyle(FontStyle.Bold);

      // Recursion was stopped, i.e. same type member appeared higher in the chain
/*
      if ((modelNode.Modifiers & TreeModelNodeModifiers.Recursive) != TreeModelNodeModifiers.None)
        item.Images.Add(ourRecursionImage, "Recursive inheritance", ImagePlacement.RIGHT);
*/
    }
  }
}