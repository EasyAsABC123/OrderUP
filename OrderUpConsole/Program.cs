﻿using LineItemLibrary;
using Newtonsoft.Json;
using Spectre.Console;

Console.WriteLine("Welcome to OrderUp");
Console.WriteLine("");

var itemList = new List<LineItem>();
var userWishesToContinue = true;
const string PathToJson = @"D:\\WorkingList.txt";

if (File.Exists(PathToJson))
{
    string loadingString = File.ReadAllText(PathToJson);
    itemList = JsonConvert.DeserializeObject<List<LineItem>>(loadingString);
}

while (userWishesToContinue)
{
    var optionSelected = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("What would you like to do?")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
            .AddChoices(new[] {
                "Create New Item", "View Current List", "Update an Item",
                "Delete an Item", "Save and Exit",
            }));
    
    switch (optionSelected)
    {
        case "Create New Item":
            Console.WriteLine("Enter a Quantity");
            int quantity = int.Parse(Console.ReadLine()!);
            
            var metalColorSelected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a Color")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more colors)[/]")
                    .AddChoices(new[] {
                        "#2 White", "Barn Red", "Black", "Bright Red", "Brown", "Buckskin Tan", "Burnished Slate",
                        "Charcoal", "Clay", "Copper Penny", "Dark Red", "Gallery Blue", "Galvalume", "Gray",
                        "Green", "Hawaiian Blue", "Light Stone", "Sapphire Blue", "Tan", "Plum", "White",
                    }));
            
            Console.WriteLine("Enter a Customer Name");
            string? customerName = Console.ReadLine();


            var priority = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title("Select a Priority Factor")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                    .AddChoices(new[] {
                        1, 2, 3,
                    }));

            itemList!.Add(LineItem.createLineItem(quantity, metalColorSelected, customerName, priority));
            break;

        case "View Current List":
            Console.WriteLine("Current List");
            foreach (var lineItem in itemList!)
            {
                Console.WriteLine(lineItem.quantity);
                Console.WriteLine(lineItem.metalColor);
                Console.WriteLine(lineItem.customerName);
                Console.WriteLine(lineItem.priorityFactor);
                Console.WriteLine(lineItem.isMade);
            }
            break;

        case "Update an Item":
            Console.WriteLine("Which Item Shall Be Updated?");
            int lineNumber = int.Parse(Console.ReadLine()!);

            Console.WriteLine("What do you wish to update?");
            Console.WriteLine("[1] Quantity");
            Console.WriteLine("[2] Metal Color");
            Console.WriteLine("[3] Customer Name");
            Console.WriteLine("[4] Priority");
            int selection = int.Parse(Console.ReadLine()!);

            switch (selection)
            {
                case 1:
                    Console.WriteLine("Enter New Quantity");
                    int newQuantity = int.Parse(Console.ReadLine()!);
                    itemList![lineNumber] = LineItem.updateLineItem(itemList[lineNumber], newQuantity);
                    break;

                case 2:
                    var newMetalColor = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Select a Color")
                            .PageSize(10)
                            .MoreChoicesText("[grey](Move up and down to reveal more colors)[/]")
                            .AddChoices(new[] {
                             "#2 White", "Barn Red", "Black", "Bright Red", "Brown", "Buckskin Tan", "Burnished Slate",
                             "Charcoal", "Clay", "Copper Penny", "Dark Red", "Gallery Blue", "Galvalume", "Gray",
                             "Green", "Hawaiian Blue", "Light Stone", "Sapphire Blue", "Tan", "Plum", "White",

                            }));
                    itemList![lineNumber] = LineItem.updateLineItem(itemList[lineNumber], newMetalColor);
                    break;

                case 3:
                    Console.WriteLine("Enter New Customer Name");
                    string? newCustomerName = Console.ReadLine();
                    itemList![lineNumber] = LineItem.updateLineItem(itemList[lineNumber], newCustomerName, true);
                    break;

                case 4:
                    var newPriorityFactor = AnsiConsole.Prompt(
                        new SelectionPrompt<int>()
                            .Title("Select a Priority Factor")
                            .PageSize(10)
                            .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                            .AddChoices(new[] {
                             1, 2, 3,
                            }));
                    itemList![lineNumber] = LineItem.updateLineItem(itemList[lineNumber], newPriorityFactor, true);
                    break;

                default:
                    break;
            }
            break;

        case "Delete an Item":

            var selectedLineItem = AnsiConsole.Prompt(
                        new SelectionPrompt<LineItem>()
                            .Title("Select an Item to Delete")
                            .PageSize(10)
                            .MoreChoicesText("[grey](Move up and down to reveal more items)[/]")
                            .AddChoices<LineItem>(itemList!)
                            .UseConverter<LineItem>(DisplaySelector));
            
            itemList!.Remove(selectedLineItem);
            break;

        case "Save and Exit":
            Console.WriteLine("Goodbye!");
            string saveState = JsonConvert.SerializeObject(itemList, Formatting.Indented);
            File.WriteAllText(PathToJson, saveState);
            userWishesToContinue = false;
            break;

        default:
            break;
    }
}



string DisplaySelector (LineItem lineitem)
{
    string result;

    result = $"{lineitem.quantity}    {lineitem.metalColor}    {lineitem.customerName}"; 

    return result;
}