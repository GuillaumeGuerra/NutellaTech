using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PatchManager.TestFramework.Mock
{
    /// <summary>
    /// Mock wrapper that will always perform a VerifyAll in the Dispose
    /// It avoids forgetting about it in each UT
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StrictMock<T> : Mock<T>, IDisposable
        where T : class
    {
        public StrictMock() : base(MockBehavior.Strict)
        {

        }

        public void Dispose()
        {
            // It's dirty I know...
            // Yet, we should only do this VerifyAll is there is no exception in the current thread
            // Indeed, if there is one, it can be linked for instance to a failed Assert
            // In that case you shouldn't make this VerifyAll, as it would raise another exception that
            // will hide the initial one ...

            bool isInException = Marshal.GetExceptionPointers() != IntPtr.Zero
                        || Marshal.GetExceptionCode() != 0;

            if (!isInException)
                this.VerifyAll();
        }
    }
}
