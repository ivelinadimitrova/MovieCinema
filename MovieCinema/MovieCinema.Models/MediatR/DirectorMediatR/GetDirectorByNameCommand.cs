﻿using MediatR;
using MovieCinema.Models.Responses.DirectorResponses;

namespace MovieCinema.Models.MediatR.DirectorMediatR
{
    public record GetDirectorByNameCommand(string name) : IRequest<ReceiveDirectorInformationResponse>
    {
    }
}