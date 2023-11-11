using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DEFINIÇÃO DO SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SERVICOS SÃO UTILIZADOS ANTES DO BUILD();

// CONTEXTO REGISTRADO COMO SERVIÇO NO CONTAINER DE DEPENDENCIA
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("Tarefas"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/* AQUI POSSUO DOIS ENDPOINTS, COM O "MINÍMO DE CÓDIGO".
 * 
 * Ao utilizar "Minimal API" -> app.MapGet, necessita de dois argumentos: 1) Rota e 2)Expressão Lambda (atua como manipulador de rota)
 */
app.MapGet("/", () => "Olá mundo");

app.MapGet("frases", async () => await new HttpClient().GetStringAsync("https://ron-swanson-quotes.herokuapp.com/v2/quotes")
);


// RETORNA LISTA DE TAREFAS
app.MapGet("/tarefas", async (AppDbContext db) => await db.Tarefas.ToListAsync());

// RETORNA POR ID
app.MapGet("/tarefas/{id}", async (int id, AppDbContext db) =>
await db.Tarefas.FindAsync(id) is Tarefa tarefa ? Results.Ok(tarefa) : Results.NotFound());

// RETORNA TAREFAS CONCLUIDAS
app.MapGet("/tarefas/concluida", async (AppDbContext db) => await db.Tarefas.Where(t => t.IsConcluida).ToListAsync());

// ACRESCENTA TAREFA (UMA A UMA)
app.MapPost("/tarefas", async (Tarefa tarefa, AppDbContext db) =>
{
    db.Tarefas.Add(tarefa);
    await db.SaveChangesAsync(); // para persistir no banco de dados

    return Results.Created($"/tarefas/{tarefa.Id}", tarefa); // classe Results, é estatica e através dela acessamos os códigos de retorno (BadRequest/Created/Ok)
});

// ATUALIZANDO A TAREFA
app.MapPut("/tarefas/{id}", async(int id, Tarefa inputTarefa, AppDbContext db) =>
{
    var tarefa = await db.Tarefas.FindAsync(id);

    if (tarefa is null) return Results.NotFound();

    tarefa.Nome = inputTarefa.Nome;
    tarefa.IsConcluida = inputTarefa.IsConcluida;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

// DELETANDO TAREFA

app.MapDelete("/tarefa/{id}", async (int id, AppDbContext db) =>
{
    if(await db.Tarefas.FindAsync(id) is Tarefa tarefa)
    {
        db.Tarefas.Remove(tarefa);
        await db.SaveChangesAsync();
        return Results.Ok(tarefa);
    }
    return Results.NotFound();
}

);

// DEFINIÇÃO DE MAPEAMENTO, ANTES DO RUN();
app.Run();

class Tarefa
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public bool IsConcluida { get; set; }
}

class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Tarefa> Tarefas => Set<Tarefa>();
}