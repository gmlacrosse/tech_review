using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Controllers;

namespace StargateAPI.Business.Commands
{
    public class UpdatePerson : IRequest<UpdatePersonResult>
    {
        public required string CurrentName { get; set; }
        public required string NewName { get; set; } = string.Empty;
    }

    public class UpdatePersonPreProcessor : IRequestPreProcessor<UpdatePerson>
    {
        private readonly StargateContext _context;
        public UpdatePersonPreProcessor(StargateContext context)
        {
            _context = context;
        }
        public Task Process(UpdatePerson request, CancellationToken cancellationToken)
        {
            var currentName = _context.People.AsNoTracking().SingleOrDefault(z => z.Name.ToLower() == request.CurrentName.ToLower());

            if (currentName is null) { throw new BadHttpRequestException($"{request.CurrentName} does not exist"); }

            var newName = _context.People.AsNoTracking().SingleOrDefault(z => z.Name.ToLower() == request.NewName.ToLower());
            if (newName is not null) { throw new BadHttpRequestException($"{request.NewName} already exists"); }

            return Task.CompletedTask;
        }
    }

    public class UpdatePersonHandler : IRequestHandler<UpdatePerson, UpdatePersonResult>
    {
        private readonly StargateContext _context;

        public UpdatePersonHandler(StargateContext context)
        {
            _context = context;
        }
        
        public async Task<UpdatePersonResult> Handle(UpdatePerson request, CancellationToken cancellationToken)
        {
            var person = _context.People.SingleOrDefault(z => z.Name.ToLower() == request.CurrentName.ToLower());

            if (person is null) throw new BadHttpRequestException("Bad Request");

            person.Name = request.NewName;

            await _context.SaveChangesAsync(cancellationToken);

            return new UpdatePersonResult()
            {
                Id = person.Id,
                Name = person.Name
            };
        }
    }

    public class UpdatePersonResult : BaseResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
    }
}
