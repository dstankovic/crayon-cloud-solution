﻿namespace CloudSales.Api.Models
{
    public record ErrorResponse(int StatusCode, string Message, string Details);
}
