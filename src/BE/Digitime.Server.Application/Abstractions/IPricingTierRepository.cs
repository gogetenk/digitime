using System.Threading.Tasks;
using Digitime.Server.Domain.PricingTier.Entities;

namespace Digitime.Server.Application.Abstractions;

public interface IPricingTierRepository
{
    Task<PricingTier> GetPricingTierAsync(string id);
}
