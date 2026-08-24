namespace StayNest.Api.Abstractions;

public record ApiResponse<T>(T Data, string Message);
public record ApiResponse(string Message);
