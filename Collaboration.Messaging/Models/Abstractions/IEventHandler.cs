using System.Threading.Tasks;

namespace Collaboration.Messaging.Models.Abstractions
{
    public interface IEventHandler
    {
        Task Handle();
    }
}
