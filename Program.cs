using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RiseOfTheTamagotchiColter.Models;
using System.Diagnostics;
using System.Data.Common;
using System.Runtime.InteropServices;

namespace RiseOfTheTamagotchiColter
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var migrations = await context.Database.GetPendingMigrationsAsync();

                if (migrations.Count() > 0)
                {
                    Console.WriteLine("Starting to migrate database....");
                    try
                    {
                        await context.Database.MigrateAsync();
                        Console.WriteLine("Database is up to date, #party time");
                    }
                    catch (DbException)
                    {
                        Notify("Database Migration FAILED");
                        throw;
                    }
                }
            }

            var task = host.RunAsync();
            Notify("🚀");
            WebHostExtensions.WaitForShutdown(host);
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        public static void Notify(string message)
        {
            var isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var isMac = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

            if (!isWindows && !isMac)
            {
                return;
            }

            // Create a process to launch the nodejs app `notifiy` with our message
            var process = isMac ? new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ".bin/terminal-notifier.app/Contents/MacOS/terminal-notifier",
                    Arguments = $"-message \"{message}\" -title \"RiseOfTheTamagotchiColter\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            } : new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ".bin/snoretoast",
                    Arguments = $"-silent -m \"{message}\" -t \"RiseOfTheTamagotchiColter\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            // Start the message but do not wait for it to end, we don't care about the termination result.
            process.Start();
        }
    }
}

/*


//Problem
Create a CRUD application for a Tamagotchi API.

Create - Create new Tamagotchi pet and store them in a datbase.

Read – Retrieve pets from a database.

Update - Update an entry for a pet in the database.

Delete – Delete an entry fro a pet in the database.


//Examples
new pet = 
{

ID (Int) = 1,
Name(String) = "Jason",
Birthday(DateTime) = 1/26/1995 at 12:00 PM,
Hunger Level(Int) = 1,
Happiness Level(Int) = 5
}


//Data

Int, String, DateTime

Class, Method


//Algorithm

Create a Class for Pet

Connect the Pet Class to a SQL Database called Pets

Ensure that the datbase will save changes made in the code

Create a controller for Get all Pets command

Create a controller to Get a pet with a certain ID

Create a controller for the Post command

Create a controller for Playtime (should find the pet by id and add 5 to its happiness level and 3 to its hungry level)

Create a controller for Feeding (should find the pet by id and subtract 5 from its hungry level and 3 from its happiness level)

Create a controller for Scolding (should find the pet by id and subtract 5 from its happiness level)

Create a controller for Delete using the ID as a guide

Upload the code to Heroku



//Code


*/
