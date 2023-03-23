using Digitime.Server.Domain.Models;

namespace Digitime.Server.Domain.PricingTier.Entities;

public class PricingTier : Entity<string>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public float Price { get; private set; }
    public int MaxMembers { get; private set; }
    public int MaxProjects { get; private set; }
    public int MaxTasks { get; private set; }
    public int MaxTimesheets { get; private set; }
    public int MaxTimeEntries { get; private set; }
    public int MaxStorage { get; private set; }
    public int MaxWorkspaces { get; private set; }

    public PricingTier(string id, string name, string description, int maxMembers, int maxProjects, int maxTasks, int maxTimesheets, int maxTimeEntries, int maxStorage, int maxWorkspaces)
        : base(id)
    {
        Name = name;
        Description = description;
        MaxMembers = maxMembers;
        MaxProjects = maxProjects;
        MaxTasks = maxTasks;
        MaxTimesheets = maxTimesheets;
        MaxTimeEntries = maxTimeEntries;
        MaxStorage = maxStorage;
        MaxWorkspaces = maxWorkspaces;
    }
}
