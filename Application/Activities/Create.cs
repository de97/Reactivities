using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;
using FluentValidation;

namespace Application.Activities
{
    public class Create
    {
        //recieve activity inside create command and is then passed to handler
        public class Command : IRequest
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public DateTime Date { get; set; }
            public string city { get; set; }
            public string Venue { get; set; }
        }

        //validate all properties
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                //expression x goes to title
                RuleFor(x => x.Title).NotEmpty();
                RuleFor(x => x.Description).NotEmpty();
                RuleFor(x => x.Category).NotEmpty();
                RuleFor(x => x.Date).NotEmpty();
                RuleFor(x => x.city).NotEmpty();
                RuleFor(x => x.Venue).NotEmpty();
            }

        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;

            }
            //returning a mediatr.unit
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = new Activity 
                {
                    Id = request.Id,
                    Title = request.Title,
                    Description = request.Description,
                    Category = request.Category,
                    Date = request.Date,
                    city = request.city,
                    Venue = request.Venue,
                };
                
                //context tracks acttivities inside database
                _context.Activities.Add(activity);
                //if the save changes returns greater then 0, the consider succsess
                var success = await _context.SaveChangesAsync() > 0;
                //value is empty object
                if (success) return Unit.Value;
                //if unsucsefull 
                throw new Exception("Problem saving changes");
            }
        }
    }
}

