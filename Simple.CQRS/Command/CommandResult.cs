using System;
using System.Collections.Generic;
using System.Linq;

namespace Simple.CQRS.Command
{
    public class CommandResult
    {
        protected internal List<DomainResult> results;

        public CommandResult() : this(new List<DomainResult>())
        {
        }

        public CommandResult(IEnumerable<DomainResult> results)
        {
            this.results = results.ToList();
        }

        public CommandResult(params DomainResult[] results)
        {
            this.results = results.ToList();
        }

        public void AddResultIfSuccessful(Func<DomainResult> callBack)
        {
            if (this.HasErrors())
            {
                return;
            }

            this.results.Add(callBack());
        }

        public void AddResultIfSuccessful(Func<CommandResult> callBack)
        {
            if (this.HasErrors())
            {
                return;
            }

            this.AddResult(callBack().GetAllResults().ToArray());
        }

        public void DoIfSuccessful(Action callBack)
        {
            if (this.WasSuccessful())
            {
                callBack();
            }
        }

        public void AddResult(params DomainResult[] domainResult)
        {
            this.results.AddRange(domainResult);
        }

        public bool WasSuccessful()
        {
            if (!this.results.Any())
            {
                return true;
            }

            return this.results.All(x => x.Successful);
        }

        public bool HasErrors()
        {
            return !this.WasSuccessful();
        }

        public IEnumerable<string> GetErrors()
        {
            return this
                .results
                .Where(x => !x.Successful)
                .Select(x => x.GetMessage())
                .Distinct();
        }

        public IEnumerable<DomainResult> GetAllResults()
        {
            return results;
        }

        public string ToErrorMessage()
        {
            return string.Join(", ", GetErrors());
        }
    }
}
