using Microsoft.AspNetCore.Mvc;

namespace SuperStrong.Types.AspNetCore.Tests;

[ApiController]
public sealed class RouteController<TStrongType> : ControllerBase
    where TStrongType : class
{
    [HttpGet("route/{value}")]
    public TStrongType Get(TStrongType value) => value;
}

[ApiController]
public sealed class QueryController<TStrongType> : ControllerBase
    where TStrongType : class
{
    [HttpGet("query")]
    public TStrongType Get(TStrongType value) => value;
}

[ApiController]
public sealed class BodyController<TStrongType> : ControllerBase
    where TStrongType : class
{
    [HttpPost("body")]
    public TStrongType Post([FromBody] TStrongType value) => value;
}
