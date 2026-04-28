using FCG.Application.Interfaces;

namespace FCG.Infrastructure.Log;

public class CorrelationIdGenerator : ICorrelationIdGenerator
{
    private string _correlationId;
    public CorrelationIdGenerator()
    {

    }
    public string CorrelationId { get => _correlationId; set => _correlationId = value; }
}

