using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto;

namespace Linovative.Frontend.Services.FrontendServices
{
    public class GlobalMessageService : IGlobalMessage
    {
        public Result? Message { get; private set;  }

        public void Clear()
        {
            Message = null;
        }

        public void SetFailureMessage(string message)
        {
            Message = Result.Failed(message);
        }

        public void SetSuccessMessage(string message)
        {
            Message = Result.SuccessMessage(message);
        }

        public void SetMessage(Result? message) => Message = message;
    }
}
