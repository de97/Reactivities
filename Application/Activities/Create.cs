using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
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

