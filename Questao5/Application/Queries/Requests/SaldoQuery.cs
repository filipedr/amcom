﻿using MediatR;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Queries.Requests
{
    public class SaldoQuery : IRequest<SaldoQueryResultado>
    {
        public Guid IdContaCorrente { get; set; }
    }
}
