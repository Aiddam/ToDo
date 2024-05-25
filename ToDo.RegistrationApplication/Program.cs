using Autofac;
using ToDo.Interfaces.Services;
using ToDo.Models.Enum;
using ToDo.Models.Models;
using ToDo.RegistrationApplication;
using Microsoft.Extensions.Configuration;

static async Task Run()
{
    var serviceContainer = GetServiceContainer();

    var userService = serviceContainer.Resolve<IUserService>();
    var organizationService = serviceContainer.Resolve<IOrganizationService>();

    Console.WriteLine("Hello, customer! Please share your information to register ");

    var user = new UserDto
    {
        Role = UserRole.Owner,
    };
    User newUser;

    while (true)
    {
        while (true)
        {
            Console.Write("User name: ");
            var input = Console.ReadLine();
            if (input is null || input.Length == 0)
            {
                Console.WriteLine("Please write name.");
                continue;
            }
            user.Name = input;
            break;
        }

        while (true)
        {
            Console.Write("User surname: ");
            var input = Console.ReadLine();
            if (input is null || input.Length == 0)
            {
                Console.WriteLine("Please write surname.");
                continue;
            }
            user.Surname = input;
            break;
        }

        while (true)
        {
            Console.Write("User login: ");
            var input = Console.ReadLine();
            if (input is null || input.Length == 0)
            {
                Console.WriteLine("Please write login.");
                continue;
            }
            user.Login = input;
            break;
        }

        while (true)
        {
            Console.Write("User password: ");
            var input = Console.ReadLine();
            if (input is null || input.Length == 0)
            {
                Console.WriteLine("Please write password.");
                continue;
            }
            user.Password = input;
            break;
        }

        try
        {
            newUser = await userService.RegistrationAsync(user);
            break;
        }
        catch
        {
            Console.WriteLine("Oh, unable to create, try again :(");
        }
    }
    
    var organization = new Organization 
    {
        UserId = newUser.Id,
    };

    Console.WriteLine("Ok, now");

    while (true)
    {
        while (true)
        {
            Console.Write("Organization name: ");
            var input = Console.ReadLine();
            if (input is null || input.Length == 0)
            {
                Console.WriteLine("Please write password.");
                continue;
            }
            organization.Name = input;
            break;
        }

        while (true)
        {
            Console.Write("Organization tax code: ");
            var input = Console.ReadLine();
            if (input is null || input.Length == 0)
            {
                Console.WriteLine("Please write tax code.");
                continue;
            }
            organization.TaxCode = input;
            break;
        }

        try
        {
            await organizationService.CreateAsync(organization);
            break;
        }
        catch
        {
            Console.WriteLine("Oops, let's try again.");
        }
    }

    Console.WriteLine("All things done!");
    Console.ReadLine();
}

static IContainer GetServiceContainer()
{
    var containerBuilder = new ContainerBuilder();
    containerBuilder
        .RegisterInstance(GetConfiguration())
        .AsImplementedInterfaces();

    DependencyInjector.Load(containerBuilder);

    return containerBuilder.Build();
}

static IConfiguration GetConfiguration()
{
    return new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", true)
        .Build();
}

await Run();