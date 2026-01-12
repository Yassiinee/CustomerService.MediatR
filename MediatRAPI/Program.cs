using MediatR;
using MediatRHandlers.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//
// Add controllers (API endpoints)
//
builder.Services.AddControllers();

//
// Register MediatR (scan handlers assembly)
//
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
});

//
// Register Application / Infrastructure dependencies
//
builder.Services.AddApplication();

//
// Swagger (API documentation)
//
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

//
// HTTP request pipeline
//
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();