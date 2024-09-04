using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using StreamKit.Common.Data.Abstractions;

namespace StreamKit.Server.WebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController(ILogger<ProductController> logger, NpgsqlDataSource source) : ControllerBase
{
    [HttpGet]
    public async IAsyncEnumerable<IProduct> Get()
    {
        yield break;
    }
}

public class ProductDbContext : DbContext
{
    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
}
