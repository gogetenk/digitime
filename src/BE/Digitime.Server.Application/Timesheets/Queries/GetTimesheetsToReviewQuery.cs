using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Digitime.Server.Application.Abstractions;
using Digitime.Shared.Contracts.Timesheets;
using MediatR;

namespace Digitime.Server.Application.Timesheets.Queries;
public record GetTimesheetsToReviewQuery(string UserId) : IRequest<TimesheetsToReviewResponse>
{
    public class GetTimesheetsToReviewQueryHandler : IRequestHandler<GetTimesheetsToReviewQuery, TimesheetsToReviewResponse>
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IProjectRepository _projectRepository;

        public GetTimesheetsToReviewQueryHandler(ITimesheetRepository timesheetRepository, IProjectRepository projectRepository)
        {
            _timesheetRepository = timesheetRepository;
            _projectRepository = projectRepository;
        }
        
        public async Task<TimesheetsToReviewResponse> Handle(GetTimesheetsToReviewQuery request, CancellationToken cancellationToken)
        {
            // First we get the projects the user is a reviewer of
            var projects = await _projectRepository.GetProjectsByReviewerId(request.UserId);

            // Then we get all the timesheets waiting for approval in those projects
            var timesheets = await _timesheetRepository.GetTimesheetsFromProjectIds(projects.Select(x => x.Id).ToArray());

            // Then we retrieve it
            return null;
        }
    }
}
