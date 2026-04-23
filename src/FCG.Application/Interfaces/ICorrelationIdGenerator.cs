using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Application.Interfaces
{
    public interface ICorrelationIdGenerator
    {
        public string CorrelationId { get; set; }
    }
}
