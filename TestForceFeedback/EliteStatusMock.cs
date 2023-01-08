using EliteAPI.Status.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForceFeedback
{
    public class EliteStatusMock : IStatus
    {
        //private IRawStatus _rawStatus;

        /// <inheritdoc />
        public event EventHandler<IRawStatus> OnChange;

        /// <inheritdoc />
        public string ToJson()
        {
            throw new NotImplementedException();
        }

        void IStatus.TriggerOnChange(IRawStatus status)
        {
            OnChange?.Invoke(this, status);
        }
    }
}
