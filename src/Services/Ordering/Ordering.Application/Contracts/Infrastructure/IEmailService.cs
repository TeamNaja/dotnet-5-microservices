namespace Ordering.Application.Contracts.Infrastructure
{
    using Ordering.Application.Models;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IEmailService
    {
        Task<bool> SendEmailAsync(Email email, CancellationToken cancellationToken = default);
    }
}
