using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Details
    {
        //new class for query of type of Irequest returning a single activity
        public class Query : IRequest<Activity>
        {
            public Guid Id { get; set; }
        }

        //query needs a handler pass in query type and activity
        //Datacontext tracks changes that are made to all retrived entities
        public class Handler : IRequestHandler<Query, Activity>
        {
            private readonly DataContext _context;

            //initialize field from parameter
            public Handler(DataContext context)
            {
                _context = context;

            }
            //handle method that takes the query object created at the top and cancellationtoken
            public async Task<Activity> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Id);

                //if we can find and activity
                if (activity == null)
                    throw new RestException(HttpStatusCode.NotFound, new { activity = "Not found" });


                return activity;
            }
        }
    }
}