// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace Console_EchoBot
{

    public class EchoBot : IBot
    {
        private static String[] operations = new String[] { "add", "substract", "multiply", "divide" };
        // Every Conversation turn for our EchoBot will call this method. In here
        // the bot checks the <see cref="Activity"/> type to verify it's a <see cref="ActivityTypes.Message"/>
        // message, and then echoes the user's typing back to them.
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            // strip the message into words
            var words = turnContext.Activity.Text.Split(' ');
            // Handle Message activity type, which is the main activity type within a conversational interface
            // Message activities may contain text, speech, interactive cards, and binary or unknown attachments.
            // see https://aka.ms/about-bot-activity-message to learn more about the message and other activity types
            if (turnContext.Activity.Type == ActivityTypes.Message && !string.IsNullOrEmpty(turnContext.Activity.Text))
            {
                // Check to see if the user sent a simple "quit" message.

                if (words[0].Equals("quit", StringComparison.InvariantCultureIgnoreCase))
                {
                    // Send a reply.
                    await turnContext.SendActivityAsync($"Bye!", cancellationToken: cancellationToken);
                    System.Environment.Exit(0);
                }
                // check if user asked for help
                if (words[0].Equals("help", StringComparison.InvariantCultureIgnoreCase))
                {
                    // show the following options 
                    // summation, subtraction, multiplication, division and quit
                    String message = "You can ask me to perform the following operations:\n";
                    // enumerate the operations7
                    int i = 1;
                    foreach (String operation in operations)
                    {
                        message += $"{i++}. {operation} 'number 1' 'number 2'\n";
                    }
                    // help message
                    message += $"{i++}. help\n";
                    // quit message
                    message += $"{i++}. quit";
                    // send the message
                    await turnContext.SendActivityAsync(message, cancellationToken: cancellationToken);
                    return;
                }
                // check if user asked for any of the above (strip first word)
                if (Array.IndexOf(operations, words[0]) > -1)
                {
                    try
                    {
                        if (words.Length == 3)
                        {
                            // convert to int
                            int n1 = Convert.ToInt32(words[1]);
                            int n2 = Convert.ToInt32(words[2]);
                            // perform the operation
                            await doOperation(words[0], n1, n2, turnContext);
                        }
                        else
                        {
                            // ask to provide the numbers
                            await turnContext.SendActivityAsync($"Please provide the numbers to perform the operation {words[0]}.", cancellationToken: cancellationToken);
                        }
                    }
                    catch (System.Exception)
                    {
                        // send the message
                        await turnContext.SendActivityAsync("Please enter two numbers separated by a space.", cancellationToken: cancellationToken);
                        throw;
                    }

                }
                else
                {
                    // send the message
                    await turnContext.SendActivityAsync($"Sorry I did not understand the command {words[0]}, for a list of available commands 'type help'.", cancellationToken: cancellationToken);
                }
            }
            else
            {
                await turnContext.SendActivityAsync($"{turnContext.Activity.Type} event detected", cancellationToken: cancellationToken);
            }
        }
        public async Task doOperation(string operation, int firstNumber, int secondNumber, ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            // check the operation
            try
            {
                switch (operation)
                {
                    case "add":
                        // do the summation
                        await turnContext.SendActivityAsync($"{firstNumber} + {secondNumber} = {firstNumber + secondNumber}", cancellationToken: cancellationToken);
                        break;
                    case "subtract":
                        // do the subtraction
                        await turnContext.SendActivityAsync($"{firstNumber} - {secondNumber} = {firstNumber - secondNumber}", cancellationToken: cancellationToken);
                        break;
                    case "multiply":
                        // do the multiplication
                        await turnContext.SendActivityAsync($"{firstNumber} * {secondNumber} = {firstNumber * secondNumber}", cancellationToken: cancellationToken);
                        break;
                    case "divide":
                        // do the division
                        await turnContext.SendActivityAsync($"{firstNumber} / {secondNumber} = {firstNumber / secondNumber}", cancellationToken: cancellationToken);
                        break;
                    default:
                        // do nothing
                        break;
                }
            }
            catch (Exception)
            {
                // send the message
                await turnContext.SendActivityAsync("Sorry, I could not perform the operation.", cancellationToken: cancellationToken);
            }


        }
    }
}
