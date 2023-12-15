using checkinItTwice_BE.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:3000",
                                "http://localhost:7040")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<CheckingItTwiceDbContext>(builder.Configuration["CheckinItTwiceDbConnectionString"]);

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

//Add for Cors git 
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

///////////////////////////               USER ENDPOINTS:      ///////////////////////////////////

//Check user:
app.MapGet("/checkuser/{uid}", (CheckingItTwiceDbContext db, string uid) =>
{
    var userExist = db.Users.Where(user => user.Uid == uid).FirstOrDefault();
    if (userExist == null)
    {
        // return Results.BadRequest("User is not registered");
        return Results.StatusCode(204);
    }
    return Results.Ok(userExist);
});

//Create User:
app.MapPost("/user", (CheckingItTwiceDbContext db, User userPayload) =>
{
    User NewUser = new User()
    {
        FirstName = userPayload.FirstName,
        LastName = userPayload.LastName,
        Uid = userPayload.Uid,
        ImageUrl = userPayload.ImageUrl,
    };

    db.Users.Add(NewUser);
    db.SaveChanges();
    return Results.Created($"/user/{NewUser.Id}", NewUser);
});

//Delete User:
app.MapDelete("/user/{id}", (CheckingItTwiceDbContext db, int id) =>
{
    User userToDelete = db.Users.Where(User => User.Id == id).FirstOrDefault();
    if (userToDelete == null)
    {
        return Results.NotFound();
    }
    db.Users.Remove(userToDelete);
    db.SaveChanges();
    return Results.Ok(db.Users);
});

//Get All Users:
app.MapGet("/users", (CheckingItTwiceDbContext db) =>
{
    return db.Users.Include(u => u.ChristmasYear)
                   .Include(u => u.ChristmasList)
                   .ToList();
});

// Get User by Id:
app.MapGet("/user/{id}", (CheckingItTwiceDbContext db, int id) =>
{
    return db.Users.Where(u => u.Id == id)
                   .Include(u => u.ChristmasYear)
                   .Include(u => u.ChristmasList)
                   .FirstOrDefault();
});

// Get User by uid:
app.MapGet("/userByUid/{uid}", (CheckingItTwiceDbContext db, string uid) =>
{
    return db.Users.Where(u => u.Uid == uid)
                   .Include(u => u.ChristmasYear)
                   .Include(u => u.ChristmasList)
                   .FirstOrDefault();
});

//Update User:
app.MapPut("/updateUser/{id}", (CheckingItTwiceDbContext db, int id, User user) =>
{
    User userToUpdate = db.Users.Include(u => u.ChristmasList)
                                .Include(u => u.ChristmasYear)
                                .SingleOrDefault(u => u.Id == id);
    if (userToUpdate == null)
    {
        return Results.NotFound("user not found");
    }

    userToUpdate.FirstName = user.FirstName;
    userToUpdate.LastName = user.LastName;
    userToUpdate.ImageUrl = user.ImageUrl;

    db.SaveChanges();
    return Results.Ok(userToUpdate);
});

///////////////////////////               GIFTEE ENDPOINTS:      ///////////////////////////////////

//Create Giftee:
app.MapPost("/giftee", (CheckingItTwiceDbContext db, Giftee gifteePayload) =>
{
    Giftee NewGiftee = new Giftee()
    {
        FirstName = gifteePayload.FirstName,
        LastName = gifteePayload.LastName,
        UserId = gifteePayload.UserId,
    };

    db.Giftees.Add(NewGiftee);
    db.SaveChanges();
    return Results.Created($"/giftee/{NewGiftee.Id}", NewGiftee);
});

//Update Giftee:
app.MapPut("/updateGiftee/{id}", (CheckingItTwiceDbContext db, int id, Giftee giftee) =>
{
    Giftee gifteeToUpdate = db.Giftees.Include(g => g.ChristmasLists)
                                      .Include(g => g.User)
                                      .SingleOrDefault(g => g.Id == id);
    if (gifteeToUpdate == null)
    {
        return Results.NotFound("giftee not found");
    }

    gifteeToUpdate.FirstName = giftee.FirstName;
    gifteeToUpdate.LastName = giftee.LastName;

    db.SaveChanges();
    return Results.Ok(gifteeToUpdate);
});

// Get Giftee by Id:
app.MapGet("/giftee/{id}", (CheckingItTwiceDbContext db, int id) =>
{
    return db.Giftees.Where(g => g.Id == id)
                     .Include(g => g.ChristmasLists)
                     .Include(g => g.User)
                     .FirstOrDefault();
});

// Get user Giftees:
app.MapGet("/userGiftees/{userid}", (CheckingItTwiceDbContext db, int userid) =>
{
    return db.Giftees.Where(g => g.UserId == userid)
                     .Include(g => g.ChristmasLists.Where(l => l.UserId == userid))
                     .ThenInclude(l => l.Gifts)
                     .Include(g => g.User)
                     .ToList();
});

//Delete Giftee:
app.MapDelete("/giftee/{id}", (CheckingItTwiceDbContext db, int id) =>
{
    Giftee gifteeToDelete = db.Giftees.Where(giftee => giftee.Id == id).FirstOrDefault();
    if (gifteeToDelete == null)
    {
        return Results.NotFound();
    }
    db.Giftees.Remove(gifteeToDelete);
    db.SaveChanges();
    return Results.Ok(db.Giftees);
});

//Get All Giftees:
app.MapGet("/giftees", (CheckingItTwiceDbContext db) =>
{
    return db.Giftees.Include(g => g.User)
                     .Include(g => g.ChristmasLists)
                     .ToList();
});

///////////////////////////               CHRISTMAS YEAR ENDPOINTS:      ///////////////////////////////////

//Create Year:
app.MapPost("/year", (CheckingItTwiceDbContext db, ChristmasYear yearPayload) =>
{
    ChristmasYear NewYear = new ChristmasYear()
    {
        ListYear = yearPayload.ListYear,
        YearBudget = yearPayload.YearBudget,
        UserId = yearPayload.UserId,
    };

    db.ChristmasYears.Add(NewYear);
    db.SaveChanges();
    return Results.Created($"/years/{NewYear.Id}", NewYear);
});

//Update Year:
app.MapPut("/updateYear/{id}", (CheckingItTwiceDbContext db, int id, ChristmasYear year) =>
{
    ChristmasYear yearToUpdate = db.ChristmasYears.Include(y => y.ChristmasLists)
                                                  .Include(y => y.User)
                                                  .SingleOrDefault(y => y.Id == id);
    if (yearToUpdate == null)
    {
        return Results.NotFound("year not found");
    }

    yearToUpdate.ListYear = year.ListYear;
    yearToUpdate.YearBudget = year.YearBudget;

    db.SaveChanges();
    return Results.Ok(yearToUpdate);
});

// Get Year by Id:
app.MapGet("/year/{id}", (CheckingItTwiceDbContext db, int id) =>
{
    return db.ChristmasYears.Where(y => y.Id == id)
                     .Include(y => y.ChristmasLists)
                     .Include(y => y.User)
                     .FirstOrDefault();
});

//Years by user id
app.MapGet("/years/{userid}", (CheckingItTwiceDbContext db, int userid) =>
{
    return db.ChristmasYears.Where(y => y.UserId == userid)   
                     .Include(y => y.ChristmasLists)
                     .ThenInclude(l => l.Gifts)
                     .Include(y => y.User)
                     .FirstOrDefault();
});

//Years by uid
app.MapGet("/yearsByUid/{userid}", (CheckingItTwiceDbContext db, string userid) =>
{
    return db.ChristmasYears
        .Where(y => y.User.Uid == userid)
        .Include(y => y.ChristmasLists.Where(l => l.User.Uid == userid)).ThenInclude(l => l.Gifts)
        .Include(y => y.User)
        .ToList();
});

//Delete Year:
app.MapDelete("/year/{id}", (CheckingItTwiceDbContext db, int id) =>
{
    ChristmasYear yearToDelete = db.ChristmasYears.Where(y => y.Id == id).FirstOrDefault();
    if (yearToDelete == null)
    {
        return Results.NotFound("year not found");
    }
    db.ChristmasYears.Remove(yearToDelete);
    db.SaveChanges();
    return Results.Ok(db.ChristmasYears);
});

//Get All Years:
app.MapGet("/years", (CheckingItTwiceDbContext db) =>
{
    return db.ChristmasYears.Include(y => y.User)
                     .Include(y => y.ChristmasLists)
                     .ToList();
});

///////////////////////////               GIFT ENDPOINTS:      ///////////////////////////////////

//Create Gift:
app.MapPost("/gift", (CheckingItTwiceDbContext db, Gift giftPayload) =>
{
    Gift NewGift = new Gift()
    {
        GiftName = giftPayload.GiftName,
        Price = giftPayload.Price,
        ImageUrl = giftPayload.ImageUrl,
        UserId = giftPayload.UserId,
        OrderedFrom = giftPayload.OrderedFrom,
    };

    db.Gifts.Add(NewGift);
    db.SaveChanges();
    return Results.Created($"/gifts/{NewGift.Id}", NewGift);
});

//Update Gift:
app.MapPut("/updateGift/{id}", (CheckingItTwiceDbContext db, int id, Gift gift) =>
{
    Gift GiftToUpdate = db.Gifts.Include(g => g.ChristmasLists)
                                .Include(g => g.User)
                                .SingleOrDefault(g => g.Id == id);
    if (GiftToUpdate == null)
    {
        return Results.NotFound("gift not found");
    }

    GiftToUpdate.GiftName = gift.GiftName;
    GiftToUpdate.Price = gift.Price;
    GiftToUpdate.ImageUrl = gift.ImageUrl;
    GiftToUpdate.OrderedFrom = gift.OrderedFrom;

    db.SaveChanges();
    return Results.Ok(GiftToUpdate);
});

// Get Gift by Id:
app.MapGet("/gift/{id}", (CheckingItTwiceDbContext db, int id) =>
{
    return db.Gifts.Where(g => g.Id == id)
                     .Include(g => g.ChristmasLists)
                     .Include(g => g.User)
                     .FirstOrDefault();
});

// Get user Gifts:
app.MapGet("/gifts/{id}", (CheckingItTwiceDbContext db, int id) =>
{
    return db.Gifts.Where(g => g.UserId == id)
                     .Include(g => g.ChristmasLists)
                     .Include(g => g.User)
                     .ToList();
});

//Delete Gift:
app.MapDelete("/gift/{id}", (CheckingItTwiceDbContext db, int id) =>
{
    Gift giftToDelete = db.Gifts.Where(g => g.Id == id).FirstOrDefault();
    if (giftToDelete == null)
    {
        return Results.NotFound("gift not found");
    }
    db.Gifts.Remove(giftToDelete);
    db.SaveChanges();
    return Results.Ok(db.Gifts);
});

//Get All Gifts:
app.MapGet("/gifts", (CheckingItTwiceDbContext db) =>
{
    return db.Gifts.Include(g => g.User)
                   .Include(g => g.ChristmasLists)
                   .ToList();
});

///////////////////////////               LIST ENDPOINTS:      ///////////////////////////////////

//Create List:
app.MapPost("/list", (CheckingItTwiceDbContext db, ChristmasList listPayload) =>
{
    ChristmasList NewList = new ChristmasList()
    {
        ListName = listPayload.ListName,
        ChristmasYearId = listPayload.ChristmasYearId,
        GifteeId = listPayload.GifteeId,
        UserId = listPayload.UserId,
    };

    db.ChristmasLists.Add(NewList);
    db.SaveChanges();
    return Results.Created($"/lists/{NewList.Id}", NewList);
});

//Update List:
app.MapPut("/updateList/{id}", (CheckingItTwiceDbContext db, int id, ChristmasList list) =>
{
    ChristmasList listToUpdate = db.ChristmasLists.Include(l => l.ChristmasYear)
                                                  .Include(l => l.User)
                                                  .Include(l => l.Gifts)
                                                  .Include(l => l.Giftee)
                                                  .SingleOrDefault(l => l.Id == id);
    if (listToUpdate == null)
    {
        return Results.NotFound("list not found");
    }

    listToUpdate.ListName = list.ListName;
    listToUpdate.ChristmasYearId = list.ChristmasYearId;

    db.SaveChanges();
    return Results.Ok(listToUpdate);
});

// Get list by Id:
app.MapGet("/list/{id}", (CheckingItTwiceDbContext db, int id) =>
{
    return db.ChristmasLists.Where(l => l.Id == id)
                     .Include(l => l.ChristmasYear)
                     .Include(l => l.Giftee)
                     .Include(l => l.Gifts)
                     .Include(l => l.User)
                     .FirstOrDefault();
});

//Delete List:
app.MapDelete("/list/{id}", (CheckingItTwiceDbContext db, int id) =>
{
    ChristmasList listToDelete = db.ChristmasLists.Where(l => l.Id == id).FirstOrDefault();
    if (listToDelete == null)
    {
        return Results.NotFound("list not found");
    }
    db.ChristmasLists.Remove(listToDelete);
    db.SaveChanges();
    return Results.Ok(db.ChristmasLists);
});

//Get All Lists:
app.MapGet("/lists", (CheckingItTwiceDbContext db) =>
{
    return db.ChristmasLists.Include(l => l.ChristmasYear)
                            .Include(l => l.Giftee)
                            .Include(l => l.Gifts)
                            .Include(l => l.User)
                            .ToList();
});

// Add Gift to List
app.MapPost("/list/{listId}/gifts/{giftId}", (CheckingItTwiceDbContext db, int listId, int giftId) =>
{
    var list = db.ChristmasLists.Include(l => l.Gifts)
                         .FirstOrDefault(l => l.Id == listId);
    if (list == null)
    {
        return Results.NotFound("List not found");
    }

    var giftToAdd = db.Gifts?.Find(giftId);


    if (giftToAdd == null)
    {
        return Results.NotFound("gift not found");
    }

    list?.Gifts?.Add(giftToAdd);
    db.SaveChanges();
    return Results.Ok(list);
});

// remove item from order
app.MapDelete("/lists/{listId}/gifts/{giftId}", (CheckingItTwiceDbContext db, int listId, int giftId) =>
{
    var list = db.ChristmasLists
       .Include(l => l.Gifts)
       .FirstOrDefault(l => l.Id == listId);

    if (list == null)
    {
        return Results.NotFound("List not found");
    }

    var giftToRemove = db.Gifts.Find(giftId);

    if (giftToRemove == null)
    {
        return Results.NotFound("Gift not found");
    }

    list.Gifts.Remove(giftToRemove);
    db.SaveChanges();
    return Results.Ok(list);
});

// SEARCH TEST ENDPOINTS
app.MapGet("/search", (CheckingItTwiceDbContext db, string query) =>
{
     var giftSearchResults = db.Gifts
        .Where(gift => gift.GiftName.Contains(query) || gift.OrderedFrom.Contains(query))
        .ToList();

    var gifteeSearchResults = db.Giftees
        .Where(giftee => giftee.FirstName.Contains(query) || giftee.LastName.Contains(query))
        .ToList();

    var yearSearchResults = db.ChristmasYears
        .Where(year => year.ListYear.Contains(query))
        .ToList();

    var listSearchResults = db.ChristmasLists
    .Where(list => list.ChristmasYear.ListYear.Contains(query) ||
                   list.ListName.Contains(query) || 
                   list.Giftee.FirstName.Contains(query) ||
                   list.Giftee.LastName.Contains(query))
    .ToList();

    var searchResults = new
    {
        Gifts = giftSearchResults,
        Giftees = gifteeSearchResults,
        Years = yearSearchResults,
        Lists = listSearchResults
    };

    return Results.Ok(searchResults);
});


app.Run();
