using System;

namespace Simple.CQRS.Command
{
    public abstract class DomainResult
    {
        public abstract bool Successful { get; }

        public abstract string GetMessage();

        public bool HasError()
        {
            return !Successful;
        }

        public DomainResult ReplaceResultIfSuccessful(Func<DomainResult> additionalCheck)
        {
            if (this.HasError())
            {
                return this;
            }

            return additionalCheck();
        }
    }
}
