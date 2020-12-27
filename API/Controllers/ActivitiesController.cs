//any api controller needs a route and an api attribute and also needs to derive from mvc controller based class
//react as the view just going to use .net project as API, view is visible for client 
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Activities;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

//api controller is responsible for recieving request and returning responses
namespace API.Controllers
{
    [Route("api/[controller]")]
    //gives automatic http 400 responses and saves from needing to check a validation error
    [ApiController]

    //inject mediator into activities controller so create contsructor
    //inherit base class for mvc controller without view support
    public class ActivitiesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ActivitiesController(IMediator mediator)
        {
        //use mediator to get data they need to respond toparticular request
            _mediator = mediator;
        }

        //firs endpoint is httpGet get lists of activities
        [HttpGet]
        public async Task<ActionResult<List<Activity>>> List()
        {
            //sending a message to list query
            return await _mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> Details(Guid id)
        {
            return await _mediator.Send(new Details.Query{Id = id});
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Guid id, Edit.Command command)
        {
            command.Id = id; //will be passed across inside command object thats being sent by mediator
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await _mediator.Send(new Delete.Command{Id = id});
        }
    }
}

//need to provide mediator as service in startup class