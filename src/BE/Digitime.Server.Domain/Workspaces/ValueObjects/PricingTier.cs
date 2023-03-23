namespace Digitime.Server.Domain.Workspaces.ValueObjects;
public record PricingTier(string Id, string Name, string Description, decimal Price, int MaxProjects, int MaxMembers);

