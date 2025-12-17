using LinoVative.Shared.Dto;

namespace Linovative.Frontend.Services.Interfaces
{
    public interface IGlobalMessage
    {
        public Result? Message { get; }
        public void SetSuccessMessage(string message);
        public void SetFailureMessage(string message);
        public void SetMessage(Result message);

        public void Clear();

    }
}
