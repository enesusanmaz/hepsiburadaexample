using HepsiBuradaExample.UI.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HepsiBuradaExample.UI
{
    public class Program
    {
        static void Main(string[] args)
        {
            
            #region Configure Services
            Startup.ConfigureServices();
            
            #endregion
            string command = string.Empty;
            do
            {
                string commandLine = Console.ReadLine();

                var commandArray = commandLine.Split(null).Where(x => x != "").ToArray();

                bool hasCommand = false;

                if (commandArray.Any())
                {
                    command = commandArray[0];
                    Delegate @delegate;
                    hasCommand = DispatcherEngine.CommandSet.TryGetValue(command, out @delegate);

                    if (hasCommand)
                    {
                        var parameters = new List<object>();

                        for (int i = 1; i < commandArray.Length; i++)
                            parameters.Add(commandArray[i]);

                        dynamic result = null;

                        try
                        {
                            result = @delegate.DynamicInvoke(parameters.ToArray());

                            if (result != null)
                            {
                                string objectToInstantiate = result.GetType().AssemblyQualifiedName;
                                var objectType = Type.GetType(objectToInstantiate);
                                dynamic instantiatedObject = Activator.CreateInstance(objectType) as IInfoModel;
                                instantiatedObject = result;

                                Console.WriteLine(instantiatedObject.GetInfo());
                            }
                        }
                        catch
                        {
                            string helpText = string.Empty;
                            DispatcherEngine.HelpMenu.TryGetValue(command, out helpText);
                            Console.WriteLine("Incorrect command usage!");
                            Console.WriteLine(helpText);
                        }
                    }
                    else
                        Console.WriteLine("Command not found!");               
                }

            } while (command != "exit");

            Startup.DisposeServices();

        }
    }
}