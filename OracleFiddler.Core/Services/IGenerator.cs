using OracleFiddler.Core.Entities;

namespace OracleFiddler.Core.Services
{
    public interface IGenerator
    {
        string Generate(Table table);
    }
}