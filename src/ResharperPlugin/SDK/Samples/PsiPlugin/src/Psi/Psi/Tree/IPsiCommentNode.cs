﻿using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree
{
  public enum CommentType : byte
  {
    EndOfLineComment, // example: //
    MultilineComment, // example: /* */
    DocComment // example: ///
  }

  public interface IPsiCommentNode : ICommentNode
  {
  }
}
