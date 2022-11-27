using AutoMapper;
using Digitime.Server.Domain.Models;
using Digitime.Server.Infrastructure.Mapping;
using Digitime.Server.Infrastructure.MongoDb;
using MongoDB.Bson;

namespace Digitime.Server.Infrastructure.Entities;

[BsonCollection("timesheets")]
public class TimesheetEntity : EntityBase, IMapTo<Timesheet>
{
    public List<TimesheetEntryEntity> TimesheetEntries { get; set; } = new();
    public string ProjectId { get; set; }
    public DateTime BeginDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public TimeSpan Period { get; set; }
    public int Hours { get; set; }
    public TimesheetStatusEnum Status { get; set; }
    public string CreatorId { get; set; }
    public string ApproverId { get; set; }
    public DateTime? ApproveDate { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<string, ObjectId>().ConvertUsing(x => ObjectId.Parse(x));
        profile.CreateMap<ObjectId, string>().ConvertUsing(x => x.ToString());
        profile.CreateMap<TimesheetEntity, Timesheet>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TimesheetEntries, opt => opt.MapFrom(src => src.TimesheetEntries))
            .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId))
            .ForMember(dest => dest.BeginDate, opt => opt.MapFrom(src => src.BeginDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
            .ForMember(dest => dest.Period, opt => opt.MapFrom(src => src.Period))
            .ForMember(dest => dest.Hours, opt => opt.MapFrom(src => src.Hours))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.CreatorId))
            .ForMember(dest => dest.ApproverId, opt => opt.MapFrom(src => src.ApproverId))
            .ForMember(dest => dest.ApproveDate, opt => opt.MapFrom(src => src.ApproveDate))
            .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreateDate))
            .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate))
            .ReverseMap();
    }
}

public enum TimesheetStatusEnum
{
    Draft = 0,
    Submitted = 1,
    Approved = 2
}
