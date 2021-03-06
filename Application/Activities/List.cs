using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

//handler responsible for getting all activities from the database and returning them.
namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<List<Activity>> { }

        //get access to Datacontext api controllers are going to be dumb no nothing about datacontext or database only responsible for http requests and responses
        public class Handler : IRequestHandler<Query, List<Activity>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }
            public async Task<List<Activity>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activities = await _context.Activities.ToListAsync();

                return activities;
            }
        }
    }
}
