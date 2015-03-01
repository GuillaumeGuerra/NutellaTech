using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarLauncher.Business
{
    public abstract class AbstractBinaryProcessor<TEntity> : IBinaryProcessor
    {
        public void ProcessBinaries(IList items, IObserver observer)
        {
            foreach (TEntity item in items)
            {
                try
                {
                    if (ShouldProcessItem(item))
                    {
                        observer.PushMessage(GetProgressMessage(item), MessageLevel.Information);
                        ProcessItem(item);
                    }
                }
                catch (Exception e)
                {
                    observer.PushMessage(GetErrorMessage(item), MessageLevel.Error);
                }
            }
        }

        protected virtual bool ShouldProcessItem(TEntity item)
        {
            return true;
        }

        protected abstract void ProcessItem(TEntity item);
        protected abstract string GetErrorMessage(TEntity item);
        protected abstract string GetProgressMessage(TEntity item);
    }
}
