using System.Collections.Generic;
using System.Linq;

namespace Wiki.Entities.Common
{
    public class ServiceResult
    {
        private bool _succeeded = true;
        public virtual bool Succeeded => _succeeded;

        private IEnumerable<string> _errors;

        public IEnumerable<string> Errors
        {
            get { return _errors; }
            set
            {
                _errors = value;
                if (_errors != null && _errors.Any())
                {
                    _succeeded = false;
                }
            }
        }

        public ServiceResult()
        {
        }

        public ServiceResult(IEnumerable<string> errors)
        {
            Errors = errors;
        }

        public string ErrorMessage
        {
            get
            {
                if (Errors == null || !Errors.Any()) return null;
                return string.Join("; ", Errors);
            }
        }

        public ServiceResult(string error) : this(new[] { error })
        {
        }
    }

    public class ServiceResult<T> : ServiceResult where T : class
    {
        public ServiceResult() { }

        public ServiceResult(T result)
        {
            Result = result;
        }

        public ServiceResult(IEnumerable<string> errors) : base(errors) { }

        public ServiceResult(string error) : base(error) { }

        public T Result { get; set; }

        public override bool Succeeded => Result != null && base.Succeeded;
    }
}
