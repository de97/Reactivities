using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Delete
    {
        public class Command : IRequest
                {
                    public Guid Id { get; set; }

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
                        var activity = await _context.Activities.FindAsync(request.Id);
                        //if we can find and activity
                        if (activity == null)
                            throw new RestException(HttpStatusCode.NotFound, new{activity = "Not found"});

                        _context.Remove(activity);
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