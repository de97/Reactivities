using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            //? means that datetime is optional
            public DateTime? Date { get; set; }
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
            public async Task<Unit> Handle(Command request,
              CancellationToken cancellationToken)
            {
                //get activity from the database
                var activity = await _context.Activities.FindAsync(request.Id);

                if (activity == null)
                    throw new Exception("Could not find activity");
                
                //if this is null everything to the right of this title will be executed //if request.title is null then activity.title;
                activity.Title = request.Title ?? activity.Title;
                activity.Description = request.Description ?? activity.Description;
                activity.Category = request.Category ?? activity.Category;
                activity.Date = request.Date ?? activity.Date;
                activity.city = request.city ?? activity.city;
                activity.Venue = request.Venue ?? activity.Venue;
                

                //activity is being tracked by context so if we update the property the context is being updated
                //handler logic
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