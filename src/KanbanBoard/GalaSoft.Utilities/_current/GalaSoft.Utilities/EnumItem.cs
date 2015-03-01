using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Reflection;

namespace GalaSoft.Utilities
{
  public class EnumItem<T>
  {
    public T Value
    {
      get;
      private set;
    }
    public string Text
    {
      get;
      private set;
    }
    public object XamlObject
    {
      get;
      private set;
    }

    public override string ToString()
    {
      return (Text == null) ? Value.ToString() : Text;
    }

    public static EnumItem<T>[] MakeItems(ResourceManager resourceManager)
    {
      Type type = typeof(T);
      T[] values = (T[]) Enum.GetValues(type);
      EnumItem<T>[] items = new EnumItem<T>[values.Length];

      for (int index = 0; index < items.Length; index++)
      {
        items[index] = new EnumItem<T>();
        items[index].Value = (T) values[index];

        try
        {
          items[index].Text = resourceManager.GetString(type.Name + values[index].ToString());
        }
        catch (Exception)
        {
          items[index].Text = null;
        }

        try
        {
          string xamlString = resourceManager.GetString(type.Name + values[index].ToString() + "Xaml");
        }
        catch (Exception)
        {
          items[index].XamlObject = null;
        }
      }

      return items;
    }

    public static EnumItem<T>[] MakeItems(string resourcePath, Type typeInResourceAssembly)
    {
      ResourceManager manager = new ResourceManager(resourcePath,
        Assembly.GetAssembly(typeInResourceAssembly));
      return MakeItems(manager);
    }

    public static EnumItem<T>[] MakeItems()
    {
      ResourceManager manager = new ResourceManager(typeof(T));
      return MakeItems(manager);
    }
  }
}
